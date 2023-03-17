namespace CarDealer;

using System.Globalization;
using System.Text;
using System.Xml.Serialization;

using CarDealer.Data;
using CarDealer.DTOs.Import;
using CarDealer.Models;

public class StartUp
{
    public static void Main()
    {
        CarDealerContext context = new CarDealerContext();

        //context.Database.EnsureDeleted();
        //context.Database.EnsureCreated();
        //Console.WriteLine("Database Creation Successfull");

        string inputXml = File.ReadAllText("../../../Datasets/cars.xml");

        string output = ImportCars(context, inputXml);
        Console.WriteLine(output);
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

        foreach (var dto in carsDtos)
        {
            Car car = new Car()
            {
                Make = dto.Make,
                Model = dto.Model,
                TravelledDistance = dto.TravellDistance
            };

            cars.Add(car);

            foreach (int partId in dto.PartIds
                .Where(p => allPartIds.Contains(p.PartId))
                .Select(p => p.PartId)
                .Distinct())
            {
                PartCar partCar = new PartCar()
                {
                    PartId = partId
                };
                partCars.Add(partCar);
            }
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
}