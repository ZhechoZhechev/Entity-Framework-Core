namespace CarDealer.DTOs.Import;

using System.Xml.Serialization;

[XmlType("Sale")]
public class ImportSalesDto
{
    [XmlElement("carId")]
    public int CarId { get; set; }

    [XmlElement("customerId")]
    public int CustomerId { get; set; }

    [XmlElement("discount")]
    public int Discount { get; set; }
}
