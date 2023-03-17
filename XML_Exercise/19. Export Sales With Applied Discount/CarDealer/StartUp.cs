namespace CarDealer;

using System.Globalization;
using System.Text;
using System.Xml.Serialization;

using CarDealer.Data;
using CarDealer.DTOs.Export;
using CarDealer.DTOs.Import;
using CarDealer.Models;

public class StartUp
{
    private static string filePath;
    public static void Main()
    {
        CarDealerContext context = new CarDealerContext();

        //context.Database.EnsureDeleted();
        //context.Database.EnsureCreated();
        //Console.WriteLine("Database Creation Successfull");

        //string inputXml = File.ReadAllText("../../../Datasets/cars.xml");
        //string output = GetCarsWithDistance(context);
        //Console.WriteLine(output);

        string fileName = "sales-discounts.xml";
        OutoutFilePatth(fileName);
        string xml = GetSalesWithAppliedDiscount(context);
        File.WriteAllText(filePath, xml);
        
    }

    public static string ImportSuppliers(CarDealerContext context, string inputXml)
    {
        var suppliersDtos = Deserialize<ImportSuppliersDto[]>(inputXml, "Suppliers");

        Supplier[] suppliers = suppliersDtos
            .Select(s => new Supplier
            {
                Name = s.Name,
                IsImporter = s.IsImporter
            })
            .ToArray();

        context.Suppliers.AddRange(suppliers);
        context.SaveChanges();

        return $"Successfully imported {suppliers.Count()}"; ;
    }
    public static string ImportParts(CarDealerContext context, string inputXml)
    {
        var supplierIds = context.Suppliers
            .Select(s => s.Id)
            .ToArray();
        var partDtos = Deserialize<ImportPartsDto[]>(inputXml, "Parts");

        var parts = partDtos
            .Where(x => supplierIds.Contains(x.SupplierId))
            .Select(p => new Part
            {
                Name = p.Name,
                Price = p.Price,
                Quantity = p.Quantity,
                SupplierId = p.SupplierId
            })
            .ToArray();

        context.Parts.AddRange(parts);
        context.SaveChanges();


        return $"Successfully imported {parts.Count()}";
    }
    public static string ImportCars(CarDealerContext context, string inputXml)
    {
        ImportCarsDto[] carsDtos = Deserialize<ImportCarsDto[]>(inputXml, "Cars");

        List<Car> cars = new List<Car>();
        List<PartCar> partCars = new List<PartCar>();
        int[] allPartIds = context.Parts.Select(p => p.Id).ToArray();
        int carId  = 1;

        foreach (var dto in carsDtos)
        {
            Car car = new Car()
            {
                Make = dto.Make,
                Model = dto.Model,
                TraveledDistance = dto.TravellDistance
            };

            cars.Add(car);

            foreach (int partId in dto.PartIds
                .Where(p => allPartIds.Contains(p.PartId))
                .Select(p => p.PartId)
                .Distinct())
            {
                PartCar partCar = new PartCar()
                {
                    PartId = partId,
                    CarId = carId
                };
                partCars.Add(partCar);
            }
            carId ++;
        }

        context.Cars.AddRange(cars);
        context.PartsCars.AddRange(partCars);
        context.SaveChanges();

        return $"Successfully imported {cars.Count}";
    }
    public static string ImportCustomers(CarDealerContext context, string inputXml)
    {
        var customersDtos = Deserialize<ImportCustomersDto[]>(inputXml, "Customers");

        var customers = customersDtos
            .Select(x => new Customer
            {
                Name = x.Name,
                BirthDate = DateTime.Parse(x.BirthDate, CultureInfo.InvariantCulture),
                IsYoungDriver = x.isYoungDriver
            })
            .ToArray();
        context.Customers.AddRange(customers);
        context.SaveChanges();

        return $"Successfully imported {customers.Count()}";
    }
    public static string ImportSales(CarDealerContext context, string inputXml) 
    {
        var salesDtos = Deserialize<ImportSalesDto[]>(inputXml, "Sales");
        var CarIds = context.Cars
            .Select(c => c.Id)
            .ToArray();

        var sales = salesDtos
            .Where(s => CarIds.Contains(s.CarId))
            .Select(s => new Sale 
            {
                Discount = s.Discount,
                CustomerId = s.CustomerId,
                CarId = s.CarId
            })
            .ToArray();

        context.Sales.AddRange(sales);
        context.SaveChanges();

       return $"Successfully imported {sales.Count()}";
    }
    public static string GetCarsWithDistance(CarDealerContext context) 
    {
        var carsWithDistance = context.Cars
            .Where(c => c.TraveledDistance > 2000000)
            .OrderBy(c => c.Make)
            .ThenBy(c => c.Model)
            .Take(10)
            .Select(c => new ExportCarsWithDIstance 
            {
                Make = c.Make,
                Model = c.Model,
                TraveledDistance = c.TraveledDistance,
            })
            .ToArray();

        string output = Serialize<ExportCarsWithDIstance[]>(carsWithDistance, "cars");
        return output;
    }
    public static string GetCarsFromMakeBmw(CarDealerContext context) 
    {
        var bmvCar = context.Cars
            .Where(c => c.Make == "BMW")
            .OrderBy(c => c.Model)
            .ThenByDescending(c => c.TraveledDistance)
            .Select(c => new ExportCarsFromMakeBmw 
            {
                Id = c.Id,
                Model = c.Model,
                TraveledDistance = c.TraveledDistance
            })
            .ToArray();

        var outout = Serialize<ExportCarsFromMakeBmw[]>(bmvCar, "cars");

        return outout;
    }
    public static string GetLocalSuppliers(CarDealerContext context) 
    {
        var supplierNotImporters = context.Suppliers
            .Where(s => s.IsImporter == false)
            .Select(s => new ExportLocalSuppliers 
            {
                Id = s.Id,
                Name = s.Name,
                PartsCount = s.Parts.Count()
            })
            .ToArray();

        var output = Serialize<ExportLocalSuppliers[]>(supplierNotImporters, "suppliers");
        return output;
    }
    public static string GetCarsWithTheirListOfParts(CarDealerContext context) 
    {
        var carsWithPartsDtos = context.Cars
            .Select(c => new ExportCarsWithTheirListOfParts 
            {
                Make = c.Make,
                Model = c.Model,
                TraveledDistance = c.TraveledDistance,
                ListParts = c.PartsCars
                .Select(p => new PartExpDto 
                {
                    Name = p.Part.Name,
                    Price = p.Part.Price,
                })
                .OrderByDescending(p => p.Price)
                .ToArray()
            })
            .OrderByDescending(c => c.TraveledDistance)
            .ThenBy(c => c.Model)
            .Take(5)
            .ToArray();

        var output = Serialize<ExportCarsWithTheirListOfParts[]>(carsWithPartsDtos, "cars");

        return output;
    }
    public static string GetTotalSalesByCustomer(CarDealerContext context) 
    {
        var customerDtosTemp = context.Customers
            .Where(c => c.Sales.Any())
            .Select(c => new
            {
                FullName = c.Name,
                BoughtCarsCount = c.Sales.Count(),
                SalesPricesDiscount = c.Sales
                .Select(x => new
                {
                    Prices = c.IsYoungDriver ?
                    x.Car.PartsCars.Sum(p => Math.Round((double)p.Part.Price * 0.95, 2)) :
                    x.Car.PartsCars.Sum(p => (double)p.Part.Price)
                })
            })
            .ToArray();

        var customerDtos = customerDtosTemp
            .OrderByDescending(t => t.SalesPricesDiscount.Sum(s => s.Prices))
            .Select(d => new ExportTotalSalesByCustomer
            {
                FullName = d.FullName,
                BoughtCarsCount = d.BoughtCarsCount,
                TotalMoneySpent = d.SalesPricesDiscount.Sum(x => x.Prices).ToString("f2")
            })
            .ToArray ();



        var output = Serialize<ExportTotalSalesByCustomer[]>(customerDtos, "customers");
        return output;
    }
    public static string GetSalesWithAppliedDiscount(CarDealerContext context) 
    {
        var infoDtos = context.Sales
            .Select(s => new ExportSalesWithAppliedDiscount()
            {
                Car = new SingleCarDto 
                {
                    Make = s.Car.Make,
                    Model = s.Car.Model,
                    TraveledDistance = s.Car.TraveledDistance,
                },
                Discount = s.Discount,
                CustomerName= s.Customer.Name,
                Price = s.Car.PartsCars.Sum(p => p.Part.Price),
            })
            .ToArray ();

        var output = Serialize<ExportSalesWithAppliedDiscount[]>(infoDtos, "sales");
        return output;
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
    private static void OutoutFilePatth(string FileName)
    {
        filePath = Path.Combine(Directory.GetCurrentDirectory(),"../../../Results", FileName);
    }
}