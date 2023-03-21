namespace Footballers.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Xml.Serialization;
    using Data;
    using Footballers.Data.Models;
    using Footballers.Data.Models.Enums;
    using Footballers.DataProcessor.ImportDto;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedCoach
            = "Successfully imported coach - {0} with {1} footballers.";

        private const string SuccessfullyImportedTeam
            = "Successfully imported team - {0} with {1} footballers.";

        public static string ImportCoaches(FootballersContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();
            var coachesPlayersDtos = Deserialize<ImportCoachesDto[]>(xmlString, "Coaches");

            List<Coach> validCoaches = new List<Coach>();
            List<Footballer> validFootballers = new List<Footballer>();

            foreach (var cDto in coachesPlayersDtos)
            {
                if (!IsValid(cDto) || string.IsNullOrEmpty(cDto.Nationality))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                
                Coach coach = new Coach() 
                {
                    Name = cDto.Name,
                    Nationality = cDto.Nationality
                };
                validCoaches.Add(coach);

                foreach (var fDto in cDto.Footballers)
                {
                    if (!IsValid(fDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    bool isStartDateValid = DateTime.TryParseExact
                        (fDto.ContractStartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var validStartDate);
                    bool isEndDateValid = DateTime.TryParseExact
                        (fDto.ContractEndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var validEndDate);
                    bool endDateBiggerThanStart = validEndDate > validStartDate;

                    if (!isStartDateValid || !isEndDateValid || !endDateBiggerThanStart) 
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Footballer footballer = new Footballer()
                    {
                        Name = fDto.Name,
                        ContractStartDate = validStartDate,
                        ContractEndDate = validEndDate,
                        BestSkillType = (BestSkillType)fDto.BestSkillType,
                        PositionType = (PositionType)fDto.PositionType
                    };
                    validFootballers.Add(footballer);
                    coach.Footballers.Add(footballer);
                }
                sb.AppendLine(string.Format(SuccessfullyImportedCoach, coach.Name, coach.Footballers.Count));
            }
            context.Coaches.AddRange(validCoaches);
            context.Footballers.AddRange(validFootballers);
            context.SaveChanges();
            
            return sb.ToString().TrimEnd();
        }
        public static string ImportTeams(FootballersContext context, string jsonString)
        {
            throw new NotImplementedException();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }

        private static T Deserialize<T>(string inputXml, string rootName)
        {
            XmlRootAttribute root = new XmlRootAttribute(rootName);
            XmlSerializer serializer = new XmlSerializer(typeof(T), root);

            using StringReader reader = new StringReader(inputXml);

            T dtos = (T)serializer.Deserialize(reader);
            return dtos;
        }
    }
}
