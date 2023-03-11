using AutoMapper;
using Newtonsoft.Json;
using ProductShop.Data;
using ProductShop.DTOs.Import;
using ProductShop.Models;
using System.Text.Json;

namespace ProductShop;

public class StartUp 
{
    public static void Main() 
    {
        ProductShopContext context = new ProductShopContext();

        string inputJson = File.ReadAllText("../../../Datasets/categories.json");

        //context.Database.EnsureDeleted();
        //context.Database.EnsureCreated();

        //Console.WriteLine("Database was created!");

        string msg = ImportCategories(context, inputJson);
        Console.WriteLine(msg);
    }

    public static string ImportUsers(ProductShopContext context, string inputJson)
    {
        var mapConfig = new MapperConfiguration(cfg => 
        {
            cfg.AddProfile<ProductShopProfile>();
        });

        var mapper = new Mapper(mapConfig);

        ImportUserDTO[] userDTOs = JsonConvert.DeserializeObject<ImportUserDTO[]>(inputJson)!;

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

    public static string ImportProducts(ProductShopContext context, string inputJson) 
    {
        var mapConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<ProductShopProfile>();
        });
        var mapper = new Mapper(mapConfig);

        ImportProductDTO[] importProductDTOs = JsonConvert.DeserializeObject<ImportProductDTO[]>(inputJson)!;
        ICollection<Product> products = new List<Product>();

        foreach (var productDTO in importProductDTOs!)
        {
            Product product = mapper.Map<Product>(productDTO);
            products.Add(product);
        }
        context.Products.AddRange(products);
        context.SaveChanges();

        return $"Successfully imported {products.Count}";
    }

    public static string ImportCategories(ProductShopContext context, string inputJson) 
    {
        var mapConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<ProductShopProfile>();
        });
        var mapper = new Mapper(mapConfig);

        ImportCategorieDTO[] categorieDTOs = JsonConvert.DeserializeObject<ImportCategorieDTO[]>(inputJson)!
            .Where(x => x.Name != null).ToArray()!;
        ICollection<Category> categories = new List<Category>();

        foreach (var cat in categorieDTOs!)
        {
            Category categorie = mapper.Map<Category>(cat);
            
            categories.Add(categorie);
        }
        context.Categories.AddRange(categories);
        context.SaveChanges();
        return $"Successfully imported {categories.Count}"; ;
    }
}