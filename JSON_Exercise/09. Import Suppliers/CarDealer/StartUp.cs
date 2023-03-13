using CarDealer.Data;
using CarDealer.Models;
using Newtonsoft.Json;

namespace CarDealer;

public class StartUp
{
    public static void Main()
    {
        CarDealerContext context = new CarDealerContext();

        //context.Database.EnsureDeleted();
        //context.Database.EnsureCreated();

        //Console.WriteLine("Successfully created!");

        string inpitJson = File.ReadAllText("../../../Datasets/suppliers.json");
        string output = ImportSuppliers(context, inpitJson);
        Console.WriteLine(output);
    }

    public static string ImportSuppliers(CarDealerContext context, string inputJson) 
    {
        ICollection<Supplier> supliersInfo;

        supliersInfo = JsonConvert.DeserializeObject<List<Supplier>>(inputJson)!;

        context.Suppliers.AddRange(supliersInfo);
        context.SaveChanges();

        return $"Successfully imported {supliersInfo.Count}.";
    }
}