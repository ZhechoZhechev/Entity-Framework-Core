namespace CarDealer.DTOs.Import;

using System.Xml.Serialization;

[XmlType("Supplier")]
public class ImportSuppliersDto
{
    [XmlElement("name")]
    public string Name { get; set; }

    [XmlElement("isImporter")]
    public bool IsImporter { get; set; }
}
