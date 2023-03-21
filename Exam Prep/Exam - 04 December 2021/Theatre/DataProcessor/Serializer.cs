namespace Theatre.DataProcessor
{
    using Newtonsoft.Json;
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Theatre.Data;
    using Theatre.DataProcessor.ExportDto;

    public class Serializer
    {
        public static string ExportTheatres(TheatreContext context, int numbersOfHalls)
        {
            var theatersDtos = context.Theatres
                .ToArray()
                .Where(t => t.NumberOfHalls >= numbersOfHalls &&
                t.Tickets.Count >= 20)
                .Select(t => new 
                {
                    Name = t.Name,
                    Halls = t.NumberOfHalls,
                    TotalIncome = t.Tickets
                    .Where(t => t.RowNumber >= 1 &&
                    t.RowNumber <= 5).Sum(tp => tp.Price),
                    Tickets = t.Tickets
                    .Select(ti => new 
                    {
                        Price = ti.Price,
                        RowNumber = ti.RowNumber
                    })
                    .Where(t => t.RowNumber >= 1 &&
                    t.RowNumber <= 5)
                    .OrderByDescending(tp => tp.Price)
                    .ToArray()
                })
                .OrderByDescending(t => t.Halls)
                .ThenBy(t => t.Name)
                .ToArray();

            var output = JsonConvert.SerializeObject(theatersDtos, Formatting.Indented);
            return output;
        }

        public static string ExportPlays(TheatreContext context, double rating)
        {
            var playActorsDtos = context.Plays
                .ToArray()
                .Where(t => t.Rating <= rating)
                .Select(p => new ExportPlaysDto()
                {
                    Title = p.Title,
                    Duration = p.Duration.ToString("c"),
                    Rating = p.Rating == 0 ? "Premier" : p.Rating.ToString(),
                    Genre = p.Genre.ToString(),
                    Actors = p.Casts
                    .Where(a => a.IsMainCharacter == true)
                    .Select(a => new ActorDto()
                    {
                        FullName = a.FullName,
                        MainCharacter = $"Plays main character in '{a.Play.Title}'."
                    })
                    .OrderByDescending(a=> a.FullName)
                    .ToArray()
                })
                .OrderBy(p => p.Title)
                .ThenByDescending(p => p.Genre)
                .ToArray();

            var output = Serialize<ExportPlaysDto[]>(playActorsDtos, "Plays");
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
