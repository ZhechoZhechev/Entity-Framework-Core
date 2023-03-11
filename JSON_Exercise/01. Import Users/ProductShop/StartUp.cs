using AutoMapper;
using Newtonsoft.Json;
using ProductShop.Data;
using ProductShop.DTOs.Import;
using ProductShop.Models;

namespace ProductShop;

public class StartUp 
{
    public static void Main() 
    {
        ProductShopContext context = new ProductShopContext();

        string inputJson = File.ReadAllText("../../../Datasets/users.json");

        //context.Database.EnsureDeleted();
        //context.Database.EnsureCreated();

        //Console.WriteLine("Database was created!");

        string msg = ImportUsers(context, inputJson);
        Console.WriteLine(msg);
    }

    public static string ImportUsers(ProductShopContext context, string inputJson)
    {
        var mapConfig = new MapperConfiguration(cfg => 
        {
            cfg.AddProfile<ProductShopProfile>();
        });

        var mapper = new Mapper(mapConfig);

        ImportUserDTO[] userDTOs = JsonConvert.DeserializeObject<ImportUserDTO[]>(inputJson);

        ICollection<User> users = new List<User>();
        foreach (var userDTO in userDTOs!) 
        {
            User user = mapper.Map<User>(userDTO);
            users.Add(user);
        }

        context.Users.AddRange(users);
        context.SaveChanges();

        return $"Successfully imported {users.Count}";

    }
}