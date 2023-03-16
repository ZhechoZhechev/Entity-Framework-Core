namespace CarDealer;

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

        string xmlFile = File.ReadAllText("../../../Datasets/suppliers.xml");

        string output = ImportSuppliers(context, xmlFile);
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