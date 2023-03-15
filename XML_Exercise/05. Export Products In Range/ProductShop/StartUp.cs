using ProductShop.Data;
using ProductShop.DTOs.Export;
using ProductShop.DTOs.Import;
using ProductShop.Models;
using System.Text;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace ProductShop;

public class StartUp
{
    public static void Main()
    {
        ProductShopContext context = new ProductShopContext();
        //context.Database.EnsureDeleted();
        //context.Database.EnsureCreated();

        //Console.WriteLine("Database created !");

        string xmlOutput = GetProductsInRange(context);
        File.WriteAllText(@"../../../Results/products-in-range.xml", xmlOutput);
    }

    public static string ImportUsers(ProductShopContext context, string inputXml)
    {
        XmlRootAttribute rootAttribute = new XmlRootAttribute("Users");
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportUserDto[]), rootAttribute);

        using StringReader input = new StringReader(inputXml);

        ImportUserDto[] userDtos = (ImportUserDto[])xmlSerializer.Deserialize(input)!;

        User[] users = userDtos
            .Select(u => new User
            {
                FirstName = u.FirstName,
                LastName = u.LastName,
                Age = u.Age
            })
            .ToArray();

        context.Users.AddRange(users);
        context.SaveChanges();

        return $"Successfully imported {users.Count()}";
    }
    public static string ImportProducts(ProductShopContext context, string inputXml) 
    {
        var productDtos = Deserialize<ImportProductDto[]>(inputXml, "Products");
        var products = productDtos
            .Select(p => new Product
            {
                Name = p.Name,
                Price = p.Price,
                SellerId = p.SellerId,
                BuyerId = p.BuyerId == 0? null : p.BuyerId
            })
            .ToArray();
          
        context.Products.AddRange(products);
        context.SaveChanges();

        return $"Successfully imported {products.Count()}";
    }
    public static string ImportCategories(ProductShopContext context, string inputXml) 
    {
        var productDtos = Deserialize<ImportCategorieDto[]>(inputXml, "Categories").Where(x => x.Name != null);

        var categories = productDtos
            .Select(c => new Category 
            {
                Name = c.Name
            })
            .ToArray();

        context.Categories.AddRange(categories);
        context.SaveChanges();

        return $"Successfully imported {categories.Count()}"; ;
    }
    public static string ImportCategoryProducts(ProductShopContext context, string inputXml) 
    {
        var catProductsDto = Deserialize<ImportCatProductDto[]>(inputXml, "CategoryProducts");
        var catProducts = catProductsDto
            .Where(x => x.ProductId != 0 && x.CategoryId != 0)
            .Select(cp => new CategoryProduct 
            {
                ProductId = cp.ProductId,
                CategoryId  = cp.CategoryId
            })
            .ToArray();

        context.CategoryProducts.AddRange(catProducts);
        context.SaveChanges();

        return $"Successfully imported {catProducts.Count()}";
    }
    public static string GetProductsInRange(ProductShopContext context) 
    {
        var productsDtos = context.Products
            .Where(p => p.Price >= 500 && p.Price <= 1000)
            .OrderBy(p => p.Price)
            .Take(10)
            .Select(p => new ExportProductsInRangeDto 
            {
                Name = p.Name,
                Price = p.Price,
                Buyer = p.Buyer.FirstName + " " + p.Buyer.LastName
            })
            .ToArray();

        StringBuilder   sb = new StringBuilder();

        XmlRootAttribute xmlRootAttribute = new XmlRootAttribute("Products");
        XmlSerializerNamespaces xmlns = new XmlSerializerNamespaces();
        xmlns.Add(string.Empty, string.Empty);
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(ExportProductsInRangeDto[]), xmlRootAttribute);

        using StringWriter swriter = new StringWriter(sb);

        xmlSerializer.Serialize(swriter, productsDtos, xmlns);
        return sb.ToString();
    }

    private static string Serialize<T>(T dtos, string rootAtributeName) 
    {
        StringBuilder sb = new StringBuilder();

        XmlRootAttribute xmlRootAttribute = new XmlRootAttribute(rootAtributeName);
        XmlSerializerNamespaces xmlns = new XmlSerializerNamespaces();
        xmlns.Add(string.Empty, string.Empty);
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(T), xmlRootAttribute);

        using StringWriter swriter = new StringWriter(sb);

        xmlSerializer.Serialize(swriter, dtos, xmlns);
        return sb.ToString();
    }
    private static T Deserialize<T>(string inputXml, string rootName) 
    {
        XmlRootAttribute rootAttribute = new XmlRootAttribute(rootName);
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(T), rootAttribute);

        using StringReader input = new StringReader(inputXml);

        T output = (T)xmlSerializer.Deserialize(input)!;

        return output;
    }
}