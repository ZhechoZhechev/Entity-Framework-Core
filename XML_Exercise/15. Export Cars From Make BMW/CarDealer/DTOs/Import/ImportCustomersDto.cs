using System.Xml.Serialization;

namespace CarDealer.DTOs.Import;

[XmlType("Customer")]
public class ImportCustomersDto
{
    [XmlElement("name")]
    public string Name { get; set; }

    [XmlElement("birthDate")]
    public string BirthDate { get; set; }

    [XmlElement("isYoungDriver")]
    public bool isYoungDriver { get; set; }
}
