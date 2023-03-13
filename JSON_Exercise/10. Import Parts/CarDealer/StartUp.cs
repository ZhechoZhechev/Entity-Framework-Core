using CarDealer.Data;
using CarDealer.Models;
using Newtonsoft.Json;
using System.IO;

namespace CarDealer;

public class StartUp
{
    public static void Main()
    {
        CarDealerContext context = new CarDealerContext();

        //context.Database.EnsureDeleted();
        //context.Database.EnsureCreated();

        //Console.WriteLine("Successfully created!");

        string inpitJson = File.ReadAllText("../../../Datasets/parts.json");
        string output = ImportParts(context, inpitJson);
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
    public static string ImportParts(CarDealerContext context, string inputJson)
    {
        var supliersId = context.Suppliers
            .Select(s => s.Id)
            .ToArray();

        ICollection<Part> partsInfo;
        partsInfo = JsonConvert.DeserializeObject<List<Part>>(inputJson)!
            .Where(x => supliersId.Contains(x.SupplierId))
            .ToList();

        context.Parts.AddRange(partsInfo);
        context.SaveChanges ();

        return $"Successfully imported {partsInfo.Count}.";
    }
}