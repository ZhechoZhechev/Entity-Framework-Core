namespace Boardgames.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    using System.Xml.Serialization;
    using Boardgames.Data;
    using Boardgames.Data.Models;
    using Boardgames.Data.Models.Enums;
    using Boardgames.DataProcessor.ImportDto;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedCreator
            = "Successfully imported creator – {0} {1} with {2} boardgames.";

        private const string SuccessfullyImportedSeller
            = "Successfully imported seller - {0} with {1} boardgames.";

        public static string ImportCreators(BoardgamesContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();
            var creatorDtos = Deserialize<ImportCreatorsDto[]>(xmlString, "Creators");

            List<Creator> validCreators = new List<Creator>();

            foreach (var cDto in creatorDtos) 
            {
                if (!IsValid(cDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Creator creator = new Creator() 
                {
                    FirstName = cDto.FirstName,
                    LastName = cDto.LastName
                };
                foreach (var bDto in cDto.Boardgames)
                {
                    if (!IsValid(bDto) || (bDto.CategoryType < 0 && bDto.CategoryType > 4))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Boardgame boardgame = new Boardgame()
                    {
                        Name = bDto.Name,
                        Rating = bDto.Rating,
                        YearPublished = bDto.YearPublished,
                        CategoryType = (CategoryType)bDto.CategoryType,
                        Mechanics = bDto.Mechanics
                    };
                    creator.Boardgames.Add(boardgame);
                }
                validCreators.Add(creator);
                sb.AppendLine(string.Format(SuccessfullyImportedCreator, creator.FirstName, creator.LastName, creator.Boardgames.Count));
            }
            context.Creators.AddRange(validCreators);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static string ImportSellers(BoardgamesContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            var sellerDtos = JsonConvert.DeserializeObject<ImportSellersDto[]>(jsonString);

            List<Seller> validSellers = new List<Seller>();

            foreach (var sDto in sellerDtos)
            {
                if (!IsValid(sDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                Seller seller = new Seller() 
                {
                    Name = sDto.Name,
                    Address = sDto.Address,
                    Country = sDto.Country,
                    Website = sDto.Website,
                };

                foreach (int bgameId in sDto.Boardgames.Distinct())
                {
                    var boardgame = context.Boardgames.FirstOrDefault(x => x.Id == bgameId);
                    if (boardgame == null)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    BoardgameSeller boardgameSeller = new BoardgameSeller() 
                    {
                        Boardgame = boardgame,
                        Seller = seller
                    };

                    seller.BoardgamesSellers.Add(boardgameSeller);
                }
                validSellers.Add(seller);
                sb.AppendLine(string.Format(SuccessfullyImportedSeller, seller.Name, seller.BoardgamesSellers.Count));
            }
            context.Sellers.AddRange(validSellers);
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
