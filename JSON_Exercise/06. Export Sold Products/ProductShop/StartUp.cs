using AutoMapper;
using AutoMapper.QueryableExtensions;
using Newtonsoft.Json;
using ProductShop.Data;
using ProductShop.DTOs.Export;
using ProductShop.DTOs.Import;
using ProductShop.Models;
using System.Text.Json;

namespace ProductShop;

public class StartUp 
{
    private static string filePath;
    public static void Main() 
    {
        ProductShopContext context = new ProductShopContext();

        string fileName = "users-sold-products.json";
        InizializeOutputFilePath(fileName);

        string json = GetSoldProducts(context);
        File.WriteAllText(filePath, json);
        //string inputJson = File.ReadAllText("../../../Datasets/categories-products.json");

        //context.Database.EnsureDeleted();
        //context.Database.EnsureCreated();

        //Console.WriteLine("Database was created!");

        //string msg = ImportCategoryProducts(context, inputJson);
        //Console.WriteLine(msg);
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

    public static string ImportCategoryProducts(ProductShopContext context, string inputJson) 
    {
        var mapperConfig = new MapperConfiguration(cfg => 
        {
            cfg .AddProfile<ProductShopProfile>();
        });
        var mapper = new Mapper(mapperConfig);

        ImportCategoryProductDTO[] categoryProductDTO = JsonConvert.DeserializeObject<ImportCategoryProductDTO[]>(inputJson)!;

        ICollection <CategoryProduct> categoryProducts = new List<CategoryProduct>();
        foreach (var cat in categoryProductDTO) 
        {
            CategoryProduct categoryProduct = mapper.Map<CategoryProduct>(cat);
            categoryProducts.Add(categoryProduct);
        }

        context.CategoriesProducts.AddRange(categoryProducts);
        context.SaveChanges();
        return $"Successfully imported {categoryProducts.Count}";
    }

    public static string GetProductsInRange(ProductShopContext context) 
    {
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<ProductShopProfile>();
        });

        var targetedProducts = context.Products
            .Where(p => p.Price >=500 && p.Price <= 1000)
            .OrderBy(p => p.Price)
            .ProjectTo<ExportProductsInRangeDTO>(mapperConfig)
            .ToArray();
        string json = JsonConvert.SerializeObject(targetedProducts, Formatting.Indented);

        return json;
    }

    public static string GetSoldProducts(ProductShopContext context) 
    {
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<ProductShopProfile>();
        });

        var usersQuery = context.Users
            .Where(u => u.ProductsSold.Any(p => p.BuyerId.HasValue))
            .OrderBy(u => u.LastName)
            .ThenBy(u => u.FirstName)
            .ProjectTo<UsersWithSoldItemsDTO>(mapperConfig)
            .ToArray();

        string json = JsonConvert.SerializeObject (usersQuery, Formatting.Indented);
        return json;
    }

    private static void InizializeOutputFilePath(string filename) 
    {
        filePath = Path.Combine(Directory.GetCurrentDirectory(), "../../../Results/", filename);
    }
}