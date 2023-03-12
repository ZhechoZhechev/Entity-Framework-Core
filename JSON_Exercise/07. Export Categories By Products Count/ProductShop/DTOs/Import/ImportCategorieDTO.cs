using Newtonsoft.Json;

namespace ProductShop.DTOs.Import;

[JsonObject]
internal class ImportCategorieDTO
{
    public string Name { get; set; } = null!;
}
