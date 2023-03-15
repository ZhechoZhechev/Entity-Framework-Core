using ProductShop.Data;
using ProductShop.DTOs.Import;
using ProductShop.Models;
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

        string inputXml = File.ReadAllText("../../../Datasets/products.xml");

        string result = ImportProducts(context, inputXml);
        Console.WriteLine(result);
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

    private static T Deserialize<T>(string inputXml, string rootName) 
    {
        XmlRootAttribute rootAttribute = new XmlRootAttribute(rootName);
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(T), rootAttribute);

        using StringReader input = new StringReader(inputXml);

        T output = (T)xmlSerializer.Deserialize(input)!;

        return output;
    }
}