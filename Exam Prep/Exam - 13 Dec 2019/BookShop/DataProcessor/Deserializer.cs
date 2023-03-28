namespace BookShop.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Runtime.Intrinsics.X86;
    using System.Runtime.Serialization;
    using System.Text;
    using System.Text.Json.Nodes;
    using System.Xml.Serialization;
    using BookShop.Data.Models;
    using BookShop.Data.Models.Enums;
    using BookShop.DataProcessor.ImportDto;
    using Data;
    using Newtonsoft.Json;
    using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedBook
            = "Successfully imported book {0} for {1:F2}.";

        private const string SuccessfullyImportedAuthor
            = "Successfully imported author - {0} with {1} books.";

        public static string ImportBooks(BookShopContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();
            var booksDtos = Deserialize<ImportBooksDto[]>(xmlString, "Books");

            List<Book> validBooks = new List<Book>();

            foreach (var book in booksDtos) 
            {
                var isPublishedDateValid = DateTime.TryParseExact
                    (book.PublishedOn, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var validPDate);

                if (!IsValid(book) || !isPublishedDateValid 
                    || (book.Genre < 1 || book.Genre >3))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Book bookToAdd = new Book() 
                {
                    Name = book.Name,
                    Genre = (Genre)book.Genre,
                    Price = book.Price,
                    Pages = book.Pages,
                    PublishedOn = validPDate
                };
                validBooks.Add(bookToAdd);
                sb.AppendLine(string.Format(SuccessfullyImportedBook, book.Name, book.Price));
            }
            
            context.Books.AddRange(validBooks);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static string ImportAuthors(BookShopContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            var authorDtos = JsonConvert.DeserializeObject<ImportAuthorsDto[]>(jsonString);

            List<Author> validAuthors = new List<Author>();

            foreach (var aDto in authorDtos) 
            {
                if (!IsValid(aDto) || aDto.Books.Length == 0)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (validAuthors.Any(x => x.Email == aDto.Email))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                Author author = new Author() 
                {
                    FirstName = aDto.FirstName,
                    LastName = aDto.LastName,
                    Phone = aDto.Phone,
                    Email = aDto.Email

                };

                foreach (var bDto in aDto.Books)
                {
                    var book = context.Books.FirstOrDefault(b => b.Id == bDto.Id);
                    if (book == null) 
                    {
                        continue;
                    }
                    AuthorBook authorBook = new AuthorBook() 
                    {
                        Author = author,
                        Book = book
                    };
                    
                    author.AuthorsBooks.Add(authorBook);
                }
                if (author.AuthorsBooks.Count() == 0) 
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                validAuthors.Add(author);
                sb.AppendLine
                    (string.Format(SuccessfullyImportedAuthor, $"{author.FirstName} {author.LastName}", author.AuthorsBooks.Count()));
            }
            context.Authors.AddRange(validAuthors);
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