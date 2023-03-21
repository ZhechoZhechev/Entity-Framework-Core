namespace Theatre.DataProcessor
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Xml.Serialization;

    using Theatre.Data;
    using Theatre.Data.Models;
    using Theatre.Data.Models.Enums;
    using Theatre.DataProcessor.ImportDto;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfulImportPlay
            = "Successfully imported {0} with genre {1} and a rating of {2}!";

        private const string SuccessfulImportActor
            = "Successfully imported actor {0} as a {1} character!";

        private const string SuccessfulImportTheatre
            = "Successfully imported theatre {0} with #{1} tickets!";

        public static string ImportPlays(TheatreContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();
            var playDtos = Deserialize<ImportPlaysDto[]>(xmlString, "Plays");

            List<Play> validPlays = new List<Play>();

            foreach ( var pDto in playDtos ) 
            {
                if (!IsValid(pDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                bool isTimeSpanValid = TimeSpan.TryParseExact(pDto.Duration, "c", CultureInfo.InvariantCulture, out TimeSpan duration);
                bool isGenreValid = Enum.TryParse<Genre>(pDto.Genre, out Genre genre);

                if (!isTimeSpanValid || !isGenreValid || duration.TotalHours < 1)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Play play = new Play() 
                {
                    Title = pDto.Title,
                    Duration = duration,
                    Rating = pDto.Rating,
                    Genre = genre,
                    Description = pDto.Description,
                    Screenwriter = pDto.Screenwriter
                };
                validPlays.Add(play);
                sb.AppendLine($"Successfully imported {play.Title} with genre {play.Genre} and a rating of {play.Rating}!");
            }
            context.Plays.AddRange(validPlays);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportCasts(TheatreContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();
            var castDtos = Deserialize<ImportCastsDto[]>(xmlString, "Casts");

            List<Cast> validActors = new List<Cast>();
            foreach (var cDto in castDtos)
            {
                if (!IsValid(cDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Cast cast = new Cast() 
                {
                    FullName = cDto.FullName,
                    IsMainCharacter = cDto.IsMainCharacter,
                    PhoneNumber = cDto.PhoneNumber,
                    PlayId = cDto.PlayId
                };
                validActors.Add(cast);
                string actorType = cast.IsMainCharacter ? "main" : "lesser";
                sb.AppendLine(string.Format(SuccessfulImportActor, cast.FullName, actorType));
            }

            context.Casts.AddRange(validActors);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static string ImportTtheatersTickets(TheatreContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            var theaterTicketsDtos = JsonConvert.DeserializeObject<ImportTheatersTicketsDto[]>(jsonString);

            List<Theatre> validTheatres = new List<Theatre>();
            List<Ticket> validTickets = new List<Ticket>();
            foreach (var ttDto in theaterTicketsDtos)
            {
                if (!IsValid(ttDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                Theatre theatre = new Theatre() 
                {
                    Name = ttDto.Name,
                    NumberOfHalls = ttDto.NumberOfHalls,
                    Director = ttDto.Director
                };
                validTheatres.Add(theatre);

                foreach (var tDto in ttDto.Tickets)
                {
                    if (!IsValid(tDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    Ticket ticket = new Ticket() 
                    {
                        Price = tDto.Price,
                        RowNumber = tDto.RowNumber,
                        PlayId = tDto.PlayId
                    };
                    validTickets.Add(ticket);
                    theatre.Tickets.Add(ticket);
                }
                sb.AppendLine(string.Format(SuccessfulImportTheatre, theatre.Name, theatre.Tickets.Count));

            }
            context.Theatres.AddRange(validTheatres);
            context.Tickets.AddRange(validTickets);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }


        private static bool IsValid(object obj)
        {
            var validator = new ValidationContext(obj);
            var validationRes = new List<ValidationResult>();

            var result = Validator.TryValidateObject(obj, validator, validationRes, true);
            return result;
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
