using Newtonsoft.Json;

namespace ProductShop.DTOs.Export;

[JsonObject]
public class CategoriesByProductsCountDTO
{
    [JsonProperty("category")]
    public string Name { get; set; }

    [JsonProperty("productsCount")]
    public int ProductsCount { get; set; }

    [JsonProperty("averagePrice")]
    public double AveragePrice { get; set; }

    [JsonProperty("totalRevenue")]
    public double TotalRevenue { get; set; }
}
