namespace VaporStore.DataProcessor
{ 
    using Data;
    using Newtonsoft.Json;
    using System.Globalization;
    using System.Text;
    using System.Text.Json.Nodes;
    using System.Xml.Serialization;
    using VaporStore.Data.Models;
    using VaporStore.DataProcessor.ExportDto;

    public static class Serializer
    {
        public static string ExportGamesByGenres(VaporStoreDbContext context, string[] genreNames)
        {

            var outputDtos = context.Genres
                .Where(g => genreNames.Contains(g.Name))
                .ToArray()
                .Select(g => new
                {
                    Id = g.Id,
                    Genre = g.Name,
                    Games = g.Games
                    .Where(ga => ga.Purchases.Any())
                    .Select(ga => new
                    {
                        Id = ga.Id,
                        Title = ga.Name,
                        Developer = ga.Developer.Name,
                        Tags = String.Join(", ", ga.GameTags.Select(gt => gt.Tag.Name)),
                        Players = ga.Purchases.Count()
                    })
                    .OrderByDescending(ga => ga.Players)
                    .ThenBy(ga => ga.Id)
                    .ToArray(),
                    TotalPlayers = g.Games.Sum(g => g.Purchases.Count)
                })
                .OrderByDescending(x => x.TotalPlayers)
                .ThenBy(g => g.Id)
                .ToArray();

            var output = JsonConvert.SerializeObject(outputDtos, Formatting.Indented);  
            return output;
        }

        public static string ExportUserPurchasesByType(VaporStoreDbContext context, string purchaseType)
        {
            var usersByTypeDtos = context.Users
                .ToArray()
                .Where(u => u.Cards.Any(x => x.Purchases.Any(p => p.Type.ToString() == purchaseType)))
                .Select(u => new ExportUserPurchasesDto()
                {
                    UserName = u.Username,
                    Purchases = u.Cards
                    .SelectMany(x => x.Purchases)
                    .Where(p => p.Type.ToString() == purchaseType)
                    .Select(p => new PurchasesDto()
                    {
                        CardNumber = p.Card.Number,
                        Cvc = p.Card.Cvc,
                        Date = p.Date.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
                        Game = new GameDto()
                        {
                            Title = p.Game.Name,
                            Genre = p.Game.Genre.Name,
                            Price = p.Game.Price
                        }
                    })
                    .OrderBy(p => p.Date)
                    .ToArray(),
                    TotalSpent = u.Cards.Sum(p => p.Purchases.Where(p => p.Type.ToString() == purchaseType).Sum(x => x.Game.Price))
                })
                .OrderByDescending(u => u.TotalSpent)
                .ThenBy(u => u.UserName)
                .ToArray();
            var output = Serialize<ExportUserPurchasesDto[]>(usersByTypeDtos, "Users");
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