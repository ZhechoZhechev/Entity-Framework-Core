namespace CarDealer.DTOs.Export;

using System.Xml.Serialization;

[XmlType("customer")]
public class ExportTotalSalesByCustomer
{

    [XmlAttribute("full-name")]
    public string FullName { get; set;}

    [XmlAttribute("bought-cars")]
    public int BoughtCarsCount { get; set; }

    [XmlIgnore]
    public bool IsYoungDriver { get; set; }

    [XmlAttribute("spent-money")]
    public string TotalMoneySpent { get; set; }

}
