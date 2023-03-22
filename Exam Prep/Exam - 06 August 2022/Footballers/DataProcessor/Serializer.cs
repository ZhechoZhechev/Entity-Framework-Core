namespace Footballers.DataProcessor
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Data;
    using Footballers.DataProcessor.ExportDto;
    using Newtonsoft.Json;
    using Formatting = Newtonsoft.Json.Formatting;

    public class Serializer
    {
        public static string ExportCoachesWithTheirFootballers(FootballersContext context)
        {
            var coachesFootballersDto =
                context.Coaches
                .Where(c => c.Footballers.Any())
                .ToArray()
                .Select(c => new ExportCoachesWithTheirFootballersDto()
                {
                    CoachFootballersCount = c.Footballers.Count(),
                    CoachName = c.Name,
                    Footballers = c.Footballers
                    .Select(f => new FootballerDto()
                    {
                        Name = f.Name,
                        Position = f.PositionType.ToString()
                    })
                    .OrderBy(f => f.Name)
                    .ToArray()
                })
                .OrderByDescending(c => c.CoachFootballersCount)
                .ThenBy(c => c.CoachName)
                .ToArray();

            var output = Serialize<ExportCoachesWithTheirFootballersDto[]>(coachesFootballersDto, "Coaches");
            return output;
        }

        public static string ExportTeamsWithMostFootballers(FootballersContext context, DateTime date)
        {
            var teamsInfoDto = context.Teams
                .Where(t => t.TeamsFootballers.Any(f => f.Footballer.ContractStartDate >= date))
                .ToArray()
                .Select(t => new
                {
                    Name = t.Name,
                    Footballers = t.TeamsFootballers
                    .Where(f => f.Footballer.ContractStartDate >= date)
                    .OrderByDescending(x => x.Footballer.ContractEndDate)
                    .ThenBy(x => x.Footballer.Name)
                    .Select(f => new
                    {
                        FootballerName = f.Footballer.Name,
                        ContractStartDate = f.Footballer.ContractStartDate.ToString("d", CultureInfo.InvariantCulture),
                        ContractEndDate = f.Footballer.ContractEndDate.ToString("d", CultureInfo.InvariantCulture),
                        BestSkillType = f.Footballer.BestSkillType.ToString(),
                        PositionType = f.Footballer.PositionType.ToString()
                    })
                    .ToArray()
                })
                .OrderByDescending(t => t.Footballers.Count())
                .ThenBy(t => t.Name)
                .Take(5)
                .ToArray();

            var output = JsonConvert.SerializeObject(teamsInfoDto, Formatting.Indented);
            return output;
        }

        private static string Serialize<T>(T dataTransferObjects, string xmlRootAttributeName)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T), new XmlRootAttribute(xmlRootAttributeName));

            StringBuilder sb = new StringBuilder();
            using var write = new StringWriter(sb);

            XmlSerializerNamespaces xmlNamespaces = new XmlSerializerNamespaces();
            xmlNamespaces.Add(string.Empty, string.Empty);

            serializer.Serialize(write, dataTransferObjects, xmlNamespaces);

            return sb.ToString();
        }
    }
}
