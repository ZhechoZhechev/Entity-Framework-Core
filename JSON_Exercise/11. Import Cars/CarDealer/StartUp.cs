using CarDealer.Data;
using CarDealer.DTOs.Import;
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

        string inpitJson = File.ReadAllText("../../../Datasets/cars.json");
        string output = ImportCars(context, inpitJson);
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

    public static string ImportCars(CarDealerContext context, string inputJson) 
    {
        var carsAndParts = JsonConvert.DeserializeObject<List<CarsPartsDTO>>(inputJson);

        List<Car> cars = new List<Car>();
        List<PartCar> partCars = new List<PartCar>();

        foreach (var item in carsAndParts)
        {
            Car car = new Car()
            {
                Make = item.Make,
                Model = item.Model,
                TravelledDistance = item.TraveledDistance
            };
            cars.Add(car);

            foreach (var partId in item.PartsId.Distinct())
            {
                PartCar partCar = new PartCar()
                {
                    PartId = partId,
                    Car = car
                };
                partCars.Add(partCar);
            }
        }

        context.Cars.AddRange(cars);
        context.PartsCars.AddRange(partCars);
        context.SaveChanges();

        return $"Successfully imported {cars.Count}."; ;
    }
}