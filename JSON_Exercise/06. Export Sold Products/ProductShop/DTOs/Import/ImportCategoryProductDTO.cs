using Newtonsoft.Json;

namespace ProductShop.DTOs.Import;

[JsonObject]
internal class ImportCategoryProductDTO
{
    public int CategoryId { get; set; }
    public int ProductId { get; set; }
}
