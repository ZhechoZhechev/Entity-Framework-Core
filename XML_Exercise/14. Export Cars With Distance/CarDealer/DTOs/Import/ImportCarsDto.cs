namespace CarDealer.DTOs.Import;

using System.Xml.Serialization;

[XmlType("Car")]
public class ImportCarsDto
{
    [XmlElement("make")]
    public string Make { get; set; }

    [XmlElement("model")]
    public string Model { get; set; }

    [XmlElement("traveledDistance")]
    public long TravellDistance { get; set; }

    [XmlArray("parts")]
    public PartDto[] PartIds { get; set; }
}

[XmlType("partId")]
public class PartDto 
{
    [XmlAttribute("id")]
    public int PartId { get; set; }
}