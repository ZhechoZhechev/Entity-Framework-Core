namespace BookShop.DataProcessor
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using BookShop.DataProcessor.ExportDto;
    using Data;
    using Newtonsoft.Json;
    using Formatting = Newtonsoft.Json.Formatting;

    public class Serializer
    {
        public static string ExportMostCraziestAuthors(BookShopContext context)
        {
            var authorBookDtos = context.Authors
                .ToArray()
                .Select(a => new 
                {
                    AuthorName = $"{a.FirstName} {a.LastName}",
                    Books = a.AuthorsBooks
                    .OrderByDescending(b => b.Book.Price)
                    .Select(b => new 
                    {
                        BookName = b.Book.Name,
                        BookPrice = b.Book.Price.ToString("f2")
                    })
                    .ToArray()
                })
                .ToArray()
                .OrderByDescending(a => a.Books.Length)
                .OrderBy(a => a.AuthorName)
                .ToArray();

            var output = JsonConvert.SerializeObject(authorBookDtos, Formatting.Indented);
            return output;
        }

        public static string ExportOldestBooks(BookShopContext context, DateTime date)
        {
            var booksDto = context.Books
                .ToArray()
                .Where(b => b.PublishedOn < date && b.Genre.ToString() == "Science")
                .OrderByDescending(b => b.Pages)
                .ThenByDescending(b => b.PublishedOn)
                .Select(b => new ExportOldBooksDto
                {
                    Pages = b.Pages,
                    BookName = b.Name,
                    Date = b.PublishedOn.ToString("d", CultureInfo.InvariantCulture)

                })
                .Take(10)
                .ToArray();

            var output = Serialize<ExportOldBooksDto[]>(booksDto, "Books");
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