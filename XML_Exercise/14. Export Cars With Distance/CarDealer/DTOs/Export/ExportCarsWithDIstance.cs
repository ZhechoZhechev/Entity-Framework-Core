namespace CarDealer.DTOs.Export;

using System.Xml.Serialization;

[XmlType("car")]
public class ExportCarsWithDIstance
{
    [XmlElement("make")]
    public string Make { get; set; }

    [XmlElement("model")]
    public string Model { get; set; }

    [XmlElement("traveled-distance")]
    public long TraveledDistance { get; set; }
}
