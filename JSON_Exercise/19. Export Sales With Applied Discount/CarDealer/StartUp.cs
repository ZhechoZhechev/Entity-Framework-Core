using CarDealer.Data;
using CarDealer.DTOs.Import;
using CarDealer.Models;
using Castle.Core.Resource;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Globalization;
using System.IO;

namespace CarDealer;

public class StartUp
{
    private static string filePath;
    public static void Main()
    {
        CarDealerContext context = new CarDealerContext();

        //context.Database.EnsureDeleted();
        //context.Database.EnsureCreated();
        //Console.WriteLine("Successfully created!");

        //string inpitJson = File.ReadAllText("../../../Datasets/sales.json");
        //Console.WriteLine(output);

        string fileName = "sales-discounts.json";
        InizializeOutputFilePath(fileName);
        string json = GetSalesWithAppliedDiscount(context);
        File.WriteAllText(filePath, json);

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
        context.SaveChanges();

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
                TraveledDistance = item.TraveledDistance
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
    public static string ImportCustomers(CarDealerContext context, string inputJson)
    {
        var customers = JsonConvert.DeserializeObject<List<Customer>>(inputJson);

        context.Customers.AddRange(customers);
        context.SaveChanges();

        return $"Successfully imported {customers.Count}.";
    }
    public static string ImportSales(CarDealerContext context, string inputJson)
    {
        var sales = JsonConvert.DeserializeObject<List<Sale>>(inputJson);

        context.Sales.AddRange(sales);
        context.SaveChanges();

        return $"Successfully imported {sales.Count}.";
    }
    public static string GetOrderedCustomers(CarDealerContext context)
    {
        var customersOrderd = context.Customers
             .OrderBy(b => b.BirthDate)
             .ThenBy(b => b.IsYoungDriver)
             .Select(c => new
             {
                 Name = c.Name,
                 BirthDate = c.BirthDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                 IsYoungDriver = c.IsYoungDriver
             })
             .ToArray();

        string json = JsonConvert.SerializeObject(customersOrderd, Formatting.Indented);

        return json;
    }
    public static string GetCarsFromMakeToyota(CarDealerContext context)
    {
        var toytasOrdered = context.Cars
            .Where(c => c.Make == "Toyota")
            .OrderBy(c => c.Model)
            .ThenByDescending(c => c.TraveledDistance)
            .Select(c => new
            {
                Id = c.Id,
                Make = c.Make,
                Model = c.Model,
                TraveledDistance = c.TraveledDistance
            })
            .ToList();

        string json = JsonConvert.SerializeObject(toytasOrdered, Formatting.Indented);
        return json;
    }
    public static string GetLocalSuppliers(CarDealerContext context)
    {
        var localSupliers = context.Suppliers
            .Where(s => s.IsImporter == false)
            .Select(s => new
            {
                Id = s.Id,
                Name = s.Name,
                PartsCount = s.Parts.Count
            })
            .ToArray();

        string json = JsonConvert.SerializeObject(localSupliers, Formatting.Indented);

        return json;
    }
    public static string GetCarsWithTheirListOfParts(CarDealerContext context) 
    {
        var carsAndParts = context.Cars
            .Select(c => new
            {
                car = new
                {
                    Make = c.Make,
                    Model = c.Model,
                    c.TraveledDistance
                },
                parts = c.PartsCars
                .Select(p => new
                {
                    Name = p.Part.Name,
                    Price = p.Part.Price.ToString("f2")
                })
            })
            .ToArray();

        string json = JsonConvert.SerializeObject(carsAndParts, Formatting.Indented);
        return json;
    }
    public static string GetTotalSalesByCustomer(CarDealerContext context) 
    {
        var customersWithCars  = context.Customers
            .Where(c => c.Sales.Count > 0)
            .Select(c => new 
            {
                fullName = c.Name,
                boughtCars = c.Sales.Count,
                spentMoney = c.Sales.SelectMany(c => c.Car.PartsCars.Select(p => p.Part.Price))
            })
            .ToArray();

        var finalInfo = customersWithCars
            .Select(x => new 
            {
                x.fullName,
                x.boughtCars,
                spentMoney = x.spentMoney.Sum()
            })
            .OrderByDescending(x => x.spentMoney)
            .ThenByDescending(x => x.boughtCars)
            .ToArray();

        string json = JsonConvert.SerializeObject (finalInfo, Formatting.Indented);

        return json;
    }

    public static string GetSalesWithAppliedDiscount(CarDealerContext context) 
    {
        var infoSalesAndCustomers = context.Sales
            .Take(10)
            .Select(c => new 
            {
                car = new 
                {
                    Make = c.Car.Make,
                    Model = c.Car.Model,
                    TraveledDistance = c.Car.TraveledDistance
                },
                customerName = c.Customer.Name,
                discount = c.Discount.ToString("f2"),
                price = (c.Car.PartsCars.Sum(p => p.Part.Price)).ToString("f2"),
                priceWithDiscount = (c.Car.PartsCars.Sum(p => p.Part.Price) * (1 - c.Discount/100 )).ToString("f2")
            })
            .ToArray();

        var json = JsonConvert.SerializeObject(infoSalesAndCustomers, Formatting.Indented);
        return json;
    }

    private static void InizializeOutputFilePath(string filename)
    {
        filePath = Path.Combine(Directory.GetCurrentDirectory(), "../../../Results/", filename);
    }
}