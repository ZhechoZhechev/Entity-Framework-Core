namespace CarDealer.DTOs.Export;

using System.Xml.Serialization;

[XmlType("sale")]
public class ExportSalesWithAppliedDiscount
{
    [XmlElement("car")]
    public SingleCarDto Car { get; set; }

    [XmlElement("discount")]
    public decimal Discount { get; set; }

    [XmlElement("customer-name")]
    public string CustomerName { get; set; }

    [XmlElement("price")]
    public decimal Price { get; set; }

    [XmlElement("price-with-discount")]
    public double PriceWithDiscount
    {
        get
        {
            return Math.Round((double)(this.Price * (1-this.Discount / 100)), 4);
        }
        set
        {
        }
    }


}

[XmlType("car")]
public class SingleCarDto
{
    [XmlAttribute("make")]
    public string Make { get; set; }

    [XmlAttribute("model")]
    public string Model { get; set; }

    [XmlAttribute("traveled-distance")]
    public long TraveledDistance { get; set; }
}
