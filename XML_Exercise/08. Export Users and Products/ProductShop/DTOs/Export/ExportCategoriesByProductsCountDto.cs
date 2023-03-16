namespace ProductShop.DTOs.Export;

using System.Xml.Serialization;

[XmlType("Category")]
public class ExportCategoriesByProductsCountDto
{
    [XmlElement("name")]
    public string Name { get; set; }

    [XmlElement("count")]
    public int ProductsCount { get; set; }

    [XmlElement("averagePrice")]
    public decimal AveragePrice { get; set; }

    [XmlElement("totalRevenue")]
    public decimal TotalRevenue { get; set; }
}
