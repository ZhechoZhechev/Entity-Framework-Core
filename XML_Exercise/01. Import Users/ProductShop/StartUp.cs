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

        string inputXml = File.ReadAllText("../../../Datasets/users.xml");

        string result = ImportUsers(context, inputXml);
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

        return $"Successfully imported {users.Count()}";
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