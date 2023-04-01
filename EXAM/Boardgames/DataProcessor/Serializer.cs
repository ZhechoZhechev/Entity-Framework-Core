namespace Boardgames.DataProcessor
{
    using Boardgames.Data;
    using Boardgames.DataProcessor.ExportDto;
    using Newtonsoft.Json;
    using System.Text;
    using System.Xml.Linq;
    using System.Xml.Serialization;

    public class Serializer
    {
        public static string ExportCreatorsWithTheirBoardgames(BoardgamesContext context)
        {
            var creatorAndBoardGamesDtos = context.Creators
                .ToArray()
                .Where(c => c.Boardgames.Any())
                .Select(c => new ExportCreatorsAndBoardgamesDto() 
                {
                    BoardgamesCount = c.Boardgames.Count,
                    CreatorName = $"{c.FirstName} {c.LastName}",
                    Boardgames = c.Boardgames
                    .Select(b => new EportBoardgamesDto() 
                    {
                        BoardgameName = b.Name,
                        BoardgameYearPublished = b.YearPublished,
                    })
                    .OrderBy(b => b.BoardgameName)
                    .ToArray()
                })
                .OrderByDescending(c => c.BoardgamesCount)
                .ThenBy(c => c.CreatorName)
                .ToArray();

            var output = Serialize<ExportCreatorsAndBoardgamesDto[]>(creatorAndBoardGamesDtos, "Creators");
            return output;
        }

        public static string ExportSellersWithMostBoardgames(BoardgamesContext context, int year, double rating)
        {
            var sellersGamesDtos = context.Sellers
                //.Where(s => s.BoardgamesSellers.Any())
                .Where(s => s.BoardgamesSellers.Any(g => g.Boardgame.YearPublished >= year 
                && g.Boardgame.Rating <= rating))
                .Select(s => new 
                {
                    Name = s.Name,
                    Website = s.Website,
                    Boardgames = s.BoardgamesSellers
                    .Where(g => g.Boardgame.YearPublished >= year && g.Boardgame.Rating <= rating)
                    .Select(g => new 
                    {
                        Name = g.Boardgame.Name,
                        Rating = g.Boardgame.Rating,
                        Mechanics = g.Boardgame.Mechanics,
                        Category = g.Boardgame.CategoryType.ToString()
                    })
                    .OrderByDescending(g => g.Rating)
                    .ThenBy(g => g.Name)
                    .ToArray()
                })
                .OrderByDescending(s => s.Boardgames.Count())
                .ThenBy(s => s.Name)
                .Take(5)
                .ToArray();

            var output = JsonConvert.SerializeObject(sellersGamesDtos, Formatting.Indented);
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