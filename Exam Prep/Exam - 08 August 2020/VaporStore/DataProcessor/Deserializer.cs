namespace VaporStore.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;
    using System.Globalization;
    using System.Text;
    using System.Linq;
    using Newtonsoft.Json;

    using Data;
    using ImportDto;
    using Data.Models;
    using VaporStore.Data.Models.Enums;

    public static class Deserializer
    {
        public const string ErrorMessage = "Invalid Data";

        public const string SuccessfullyImportedGame = "Added {0} ({1}) with {2} tags";

        public const string SuccessfullyImportedUser = "Imported {0} with {1} cards";

        public const string SuccessfullyImportedPurchase = "Imported {0} for {1}";

        public static string ImportGames(VaporStoreDbContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            var desDtos = JsonConvert.DeserializeObject<GamesDevsGenresTagsDto[]>(jsonString);

            List<Game> validGames = new List<Game>();
            List<Developer> validDevelopers = new List<Developer>();
            List<Genre> validGenres = new List<Genre>();
            List<Tag> validTags = new List<Tag>();

            foreach (var gDto in desDtos)
            {
                var isReleaseDateValid = DateTime.TryParseExact
                    (gDto.ReleaseDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var validRD);

                if (!isReleaseDateValid || !IsValid(gDto) || gDto.Tags.Length == 0)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Game game = new Game()
                {
                    Name = gDto.Name,
                    Price = gDto.Price,
                    ReleaseDate = validRD
                };

                var genre = validGenres.FirstOrDefault(x => x.Name == gDto.Genre);
                if (genre == null)
                {
                    genre = new Genre()
                    {
                        Name = gDto.Genre
                    };
                    validGenres.Add(genre);

                }
                game.Genre = genre;

                var developer = validDevelopers.FirstOrDefault(x => x.Name == gDto.Developer);
                if (developer == null)
                {
                    developer = new Developer()
                    {
                        Name = gDto.Developer
                    };
                    validDevelopers.Add(developer);
                }
                game.Developer = developer;

                foreach (string name in gDto.Tags)
                {
                    if (String.IsNullOrEmpty(name))
                    {
                        continue;
                    }

                    var tag = validTags.FirstOrDefault(x => x.Name == name);
                    if (tag == null)
                    {
                        tag = new Tag()
                        {
                            Name = name
                        };
                        validTags.Add(tag);

                    }
                    GameTag gameTag = new GameTag()
                    {
                        Game = game,
                        Tag = tag
                    };
                    game.GameTags.Add(gameTag);
                }

                validGames.Add(game);
                sb.AppendLine(string.Format(SuccessfullyImportedGame, game.Name, game.Genre.Name, game.GameTags.Count()));
            }

            context.Games.AddRange(validGames);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static string ImportUsers(VaporStoreDbContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            var desDtos = JsonConvert.DeserializeObject<ImportUsersCardsDto[]>(jsonString);

            List<User> validUsers = new List<User>();
            List<Card> validCards = new List<Card>();

            
            foreach (var uDto in desDtos)
            {
                bool hasInvalidCard = uDto.Cards.Any(x => Enum.TryParse<CardType>(x.Type, out var result));
                if (!IsValid(uDto) || !uDto.Cards.Any() || !hasInvalidCard)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                User user = new User() 
                {
                    FullName = uDto.FullName,
                    Username = uDto.Username,
                    Email = uDto.Email,
                    Age = uDto.Age
                };

                foreach (var c in uDto.Cards)
                {
                    if (!IsValid(c)) 
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Card card = new Card() 
                    {
                        Number = c.Number,
                        Cvc = c.CVC,
                        Type = Enum.Parse<CardType>(c.Type)
                    };
                    user.Cards.Add(card);
                }

                validUsers.Add(user);
                sb.AppendLine(string.Format(SuccessfullyImportedUser, user.Username, user.Cards.Count()));
            }
            context.AddRange(validUsers);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportPurchases(VaporStoreDbContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();
            var desDtos = Deserialize<ImportPurchasesDto[]>(xmlString, "Purchases");

            List<Purchase> purchases = new List<Purchase>();
            foreach (var pDto in desDtos) 
            {
                Game game = context.Games.FirstOrDefault(x => x.Name == pDto.GameName);
                Card card = context.Cards.FirstOrDefault(x => x.Number == pDto.CardNumber);
                bool isTypeValid = Enum.TryParse<PurchaseType>(pDto.Type, out var validType);
                bool isDateValid = DateTime.TryParseExact
                    (pDto.Date, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out var validDate);

                if (!IsValid(pDto) || game == null || card == null
                    || !isTypeValid || !isDateValid) 
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                
                Purchase purchase = new Purchase() 
                {
                    Type = validType,
                    ProductKey = pDto.ProductKey,
                    Date = validDate,
                    Card = card,
                    Game = game
                };
                purchases.Add(purchase);
                sb.AppendLine(string.Format(SuccessfullyImportedPurchase, purchase.Game.Name, purchase.Card.User.Username));
            }
            context.AddRange(purchases);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
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