namespace SoftJail.DataProcessor
{

    using Data;
    using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using SoftJail.Data.Models;
    using SoftJail.DataProcessor.ExportDto;
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    public class Serializer
    {
        public static string ExportPrisonersByCells(SoftJailDbContext context, int[] ids)
        {
            var prisonersByCellsDtos = context.Prisoners
                .Where(p => ids.Contains(p.Id))
                .Select(p => new
                {
                    Id = p.Id,
                    Name = p.FullName,
                    CellNumber = p.Cell.CellNumber,
                    Officers = p.PrisonerOfficers
                    .Select(o => new
                    {
                        OfficerName = o.Officer.FullName,
                        Department = o.Officer.Department.Name
                    })
                    .OrderBy(o => o.OfficerName)
                    .ToArray(),
                    TotalOfficerSalary = decimal.Parse(p.PrisonerOfficers.Sum(o => o.Officer.Salary).ToString("f2"))
                })
                .OrderBy(p => p.Name)
                .ThenBy(p => p.Id)
                .ToArray();

            var json = JsonConvert.SerializeObject(prisonersByCellsDtos, Formatting.Indented);
            return json;
        }

        public static string ExportPrisonersInbox(SoftJailDbContext context, string prisonersNames)
        {
            string[] pNames = prisonersNames.Split(',');

            var pDtos = context.Prisoners
                .Where(p => pNames.Contains(p.FullName))
                .OrderBy (p => p.FullName)
                .ThenBy (p => p.Id)
                .Select(p => new ExportPrisonersInboxDto 
                {
                    PrisonerId = p.Id,
                    Name = p.FullName,
                    IncarcerationDate = p.IncarcerationDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                    EncryptedMessages = p.Mails
                    .Select(m => new Message 
                    {
                        Description = string.Join("", m.Description.Reverse())
                    })
                    .ToArray(),
                })
                .ToArray();

            string xml = Serialize<ExportPrisonersInboxDto[]>(pDtos, "Prisoners");
            return xml;
        }

        private static string Serialize<T>(T dtos, string rootAtributeName)
        {
            StringBuilder sb = new StringBuilder();

            XmlRootAttribute xmlRootAttribute = new XmlRootAttribute(rootAtributeName);
            XmlSerializerNamespaces xmlns = new XmlSerializerNamespaces();
            xmlns.Add(string.Empty, string.Empty);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T), xmlRootAttribute);

            using StringWriter swriter = new StringWriter(sb);

            xmlSerializer.Serialize(swriter, dtos, xmlns);
            return sb.ToString();
        }
    }

}