namespace BookShop
{
    using BookShop.Models;
    using BookShop.Models.Enums;
    using Data;
    using Initializer;
    using Microsoft.EntityFrameworkCore.Metadata;
    using System.Globalization;
    using System.Text;

    public class StartUp
    {
        public static void Main()
        {
            //int input = int.Parse(Console.ReadLine()!);


            using var context = new BookShopContext();
            //DbInitializer.ResetDatabase(context);

            var result = RemoveBooks(context);

            Console.WriteLine(result);
        }

        //02. Age Restriction
        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {

            AgeRestriction ageRestriction;

            bool hasParsed = Enum.TryParse<AgeRestriction>(command, true, out ageRestriction);

            if (!hasParsed)
            {
                return string.Empty;
            }

            var bookNames = context.Books
                .Where(b => b.AgeRestriction == ageRestriction)
                .Select(b => b.Title)
                .OrderBy(t => t)
                .ToArray();

            return string.Join(Environment.NewLine, bookNames);
        }

        //03. Golden Books
        public static string GetGoldenBooks(BookShopContext context)
        {
            var bookNames = context.Books
                .Where(b => b.EditionType == EditionType.Gold &&
                b.Copies < 5000)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title)
                .ToArray();

            return string.Join(Environment.NewLine, bookNames);
        }

        //04. Books by Price
        public static string GetBooksByPrice(BookShopContext context)
        {
            var booksInfo = context.Books
                .Where(b => b.Price > 40)
                .OrderByDescending(b => b.Price)
                .Select(b => new { b.Title, Price = b.Price.ToString("f2") })
                .ToArray();
            StringBuilder sb = new StringBuilder();

            foreach (var b in booksInfo)
            {
                sb.AppendLine($"{b.Title} - ${b.Price}");
            }

            return sb.ToString().TrimEnd();
        }

        //05. Not Released In
        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            var bookNames = context.Books
                .Where(b => b.ReleaseDate!.Value.Year != year)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title)
                .ToArray();

            return string.Join(Environment.NewLine, bookNames);
        }

        //06. Book Titles by Category
        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            string[] categories = input
                .ToLower()
                .Split(' ', StringSplitOptions.RemoveEmptyEntries);

            List<string> bookNames = new List<string>();

            foreach (string category in categories)
            {
                var books = context.BooksCategories
                    .Where(bc => bc.Category.Name.ToLower() == category)
                    .Select(b => b.Book.Title)
                    .ToList();

                bookNames.AddRange(books);

            }

            bookNames.Sort();
            return string.Join(Environment.NewLine, bookNames);
        }

        //07. Released Before Date
        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            var resultDate = DateTime.ParseExact(date, "dd-MM-yyyy", null);

            var booksInfo = context.Books
                .Where(b => b.ReleaseDate < resultDate)
                .OrderByDescending(b => b.ReleaseDate)
                .Select(b => new
                {
                    b.Title,
                    EditionType = b.EditionType.ToString(),
                    Price = b.Price.ToString("f2")
                })
                .ToArray();
            StringBuilder sb = new StringBuilder();
            foreach (var book in booksInfo)
            {
                sb.AppendLine($"{book.Title} - {book.EditionType} - ${book.Price}");
            }
            return sb.ToString().TrimEnd();
        }

        //08. Author Search
        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            var authorNames = context.Authors
                .Where(a => a.FirstName.EndsWith(input))
                .Select(a => new
                {
                    FullName = string.Concat(a.FirstName, " ", a.LastName)
                })
                .OrderBy(n => n.FullName)
                .ToArray();
            StringBuilder sb = new StringBuilder();

            foreach (var a in authorNames)
            {
                sb.AppendLine($"{a.FullName}");
            }

            return sb.ToString().TrimEnd();
        }

        //09. Book Search
        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            var bookTitles = context.Books
                .Where(b => b.Title.ToLower().Contains(input.ToLower()))
                .Select(b => b.Title)
                .OrderBy(t => t)
                .ToArray();

            return string.Join(Environment.NewLine, bookTitles);
        }

        //10. Book Search by Author
        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            var bookInfo = context.Books
                .Where(b => b.Author.LastName.
                ToLower()
                .StartsWith(input.ToLower()))
                .OrderBy(b => b.BookId)
                .Select(b => new
                {
                    b.Title,
                    AuthorFullName = string.Concat(b.Author.FirstName, " ", b.Author.LastName)
                })
                .ToArray();

            StringBuilder sb = new StringBuilder();

            foreach (var b in bookInfo)
            {
                sb.AppendLine($"{b.Title} ({b.AuthorFullName})");
            }

            return sb.ToString().TrimEnd();
        }

        //11. Count Books
        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            int booksCount = context.Books
                .Where(b => b.Title.Length > lengthCheck)
                .Count();

            return booksCount;
        }

        //12. Total Book Copies
        public static string CountCopiesByAuthor(BookShopContext context)
        {
            var info = context.Authors
                .Select(a => new
                {
                    CopiesTotal = a.Books.Sum(b => b.Copies),
                    FullName = string.Concat(a.FirstName, " ", a.LastName)
                })
                .OrderByDescending(c => c.CopiesTotal);
            StringBuilder sb = new StringBuilder();
            foreach (var a in info)
            {
                sb.AppendLine($"{a.FullName} - {a.CopiesTotal}");
            }

            return sb.ToString().TrimEnd();
        }

        //13. Profit by Category
        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            var info = context.Categories
                .Select(c => new
                {
                    c.Name,
                    TotalProfit = c.CategoryBooks.Sum(b => (b.Book.Copies * b.Book.Price))
                })
                .OrderByDescending(c => c.TotalProfit)
                .ThenBy(c => c.Name)
                .ToArray();


            StringBuilder sb = new StringBuilder();

            foreach (var b in info)
            {
                sb.AppendLine($"{b.Name} ${b.TotalProfit:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        //14. Most Recent Books
        public static string GetMostRecentBooks(BookShopContext context)
        {
            var info = context.Categories
                .OrderBy(c => c.Name)
                .Select(c => new
                {
                    c.Name,
                    Books = c.CategoryBooks
                    .OrderByDescending(c => c.Book.ReleaseDate)
                    .Select(b => new
                    {
                        BookName = b.Book.Title,
                        BookReleaseDate = b.Book.ReleaseDate!.Value.Year,
                    })
                    .Take(3)
                    .ToArray()
                })
                .ToArray();

            StringBuilder sb = new StringBuilder();
            foreach (var c in info)
            {
                sb.AppendLine($"--{c.Name}");

                foreach (var b in c.Books)
                {
                    sb.AppendLine($"{b.BookName} ({b.BookReleaseDate})");
                }
            }

            return sb.ToString().TrimEnd();
        }

        //15. Increase Prices
        public static void IncreasePrices(BookShopContext context)
        {
            var bookToIncreasePrices = context.Books
                .Where(b => b.ReleaseDate.HasValue &&
                            b.ReleaseDate.Value.Year < 2010);

            foreach (var book in bookToIncreasePrices)
            {
                book.Price += 5;
            }

            context.SaveChanges();
        }

        //16. Remove Books
        public static int RemoveBooks(BookShopContext context)
        {
            var booksToRemove = context.Books
                .Where(b => b.Copies < 4200)
                .ToArray();

            context.Books.RemoveRange(booksToRemove);

            context.SaveChanges();

            return booksToRemove.Count();
        }
    }
}


