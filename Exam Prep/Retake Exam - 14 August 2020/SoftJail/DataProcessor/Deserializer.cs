namespace SoftJail.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Microsoft.EntityFrameworkCore.Internal;
    using Newtonsoft.Json;

    using Data;
    using SoftJail.Data.Models;
    using SoftJail.DataProcessor.ImportDto;
    using SoftJail.Data.Models.Enums;

    public class Deserializer
    {
        public static string ImportDepartmentsCells(SoftJailDbContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            var DepsWithCellsDtos = JsonConvert.DeserializeObject<ImportDepartmentsCellsDto[]>(jsonString);

            List<Department> departments = new List<Department>();

            foreach (var DepDto in DepsWithCellsDtos)
            {
                if (!IsValid(DepDto))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                if (!DepDto.Cells.Any()) 
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                if (DepDto.Cells.Any(c => !IsValid(c)))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }
                Department department = new Department() 
                {
                    Name = DepDto.Name
                };

                List<Cell> cells = new List<Cell>();
                foreach (var c in DepDto.Cells) 
                {
                    Cell cell = new Cell() 
                    {
                        CellNumber = c.CellNumber,
                        HasWindow = c.HasWindow
                    };
                    cells.Add(cell);
                }
                department.Cells = cells;
                departments.Add(department);
                sb.AppendLine($"Imported {department.Name} with {cells.Count} cells");
            }

            context.Departments.AddRange( departments );
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportPrisonersMails(SoftJailDbContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            var prisonerMailDtos = JsonConvert.DeserializeObject<ImportPrisonersMailsDto[]>(jsonString);

            List<Prisoner> validPrisoners = new List<Prisoner>();
            foreach (var pDto in prisonerMailDtos)
            {
                if (!IsValid(pDto))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                if (pDto.Mails.Any(m => !IsValid(m)))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                DateTime incarcerationDate;
                bool IsIncDateValid = DateTime.TryParseExact
                    (pDto.IncarcerationDate, "dd/MM/yyyy",CultureInfo.InvariantCulture, DateTimeStyles.None, out incarcerationDate);
                if (!IsIncDateValid) 
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                DateTime? releaseDate = null;
                if (!string.IsNullOrEmpty(pDto.ReleaseDate))
                {
                    bool IsReleaseDateValid = DateTime.TryParseExact
                        (pDto.ReleaseDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime value);

                    if (!IsReleaseDateValid)
                    {
                        sb.AppendLine("Invalid Data");
                        continue;
                    }

                    releaseDate = value;
                }

                Prisoner prisoner = new Prisoner()
                {
                    FullName = pDto.FullName,
                    Nickname = pDto.Nickname,
                    Age = pDto.Age,
                    IncarcerationDate = incarcerationDate,
                    ReleaseDate = releaseDate,
                    Bail = pDto.Bail,
                    CellId = pDto.CellId
                };

                List<Mail> mails = new List<Mail>();
                foreach (var m in pDto.Mails)
                {
                    Mail mail = new Mail() 
                    {
                        Description = m.Description,
                        Sender = m.Sender,
                        Address = m.Address
                    };
                    mails.Add(mail);
                }
                prisoner.Mails = mails;
                validPrisoners.Add(prisoner);

                sb.AppendLine($"Imported {prisoner.FullName} {prisoner.Age} years old");
            }
            context.Prisoners.AddRange(validPrisoners);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportOfficersPrisoners(SoftJailDbContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();
            var officerPrisonerDtos = Deserialize<ImportOfficersPrisonersDto[]>(xmlString, "Officers");

            List<Officer> validOfficers = new List<Officer>();
            foreach (var opDto in officerPrisonerDtos)
            {
                if (!IsValid(opDto))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                bool isPositionValid = Enum.TryParse(typeof(Position), opDto.Position, out object position);
                if (!isPositionValid) 
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                bool isWeaponValid = Enum.TryParse(typeof(Weapon), opDto.Weapon, out object weapon);
                if (!isWeaponValid) 
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                Officer officer = new Officer() 
                {
                    FullName = opDto.Name,
                    Salary = opDto.Money,
                    Position = (Position)position,
                    Weapon = (Weapon)weapon,
                    DepartmentId = opDto.DepartmentId
                };

                foreach (var pid in opDto.Prisoners)
                {
                    officer.OfficerPrisoners.Add(new OfficerPrisoner() 
                    {
                        Officer = officer,
                        PrisonerId = pid.PrisonerId
                    });
                }
                validOfficers.Add(officer);
                sb.AppendLine($"Imported {officer.FullName} ({officer.OfficerPrisoners.Count} prisoners)");
            }
            context.Officers.AddRange(validOfficers);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object obj)
        {
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(obj);
            var validationResult = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(obj, validationContext, validationResult, true);
            return isValid;
        }

        private static T Deserialize<T>(string inputXml, string rootName)
        {
            XmlRootAttribute rootAttribute = new XmlRootAttribute(rootName);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T), rootAttribute);

            using StringReader input = new StringReader(inputXml);

            T output = (T)xmlSerializer.Deserialize(input)!;

            return output;
        }
    }
}