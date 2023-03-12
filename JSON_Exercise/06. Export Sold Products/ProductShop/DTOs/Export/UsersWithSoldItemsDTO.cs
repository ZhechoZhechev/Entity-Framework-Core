using Newtonsoft.Json;

namespace ProductShop.DTOs.Export;

[JsonObject]
public class UsersWithSoldItemsDTO
{
    [JsonProperty("firstName")]
    public string FirstName { get; set; }

    [JsonProperty("lastName")]
    public string LastName { get; set; }

    [JsonProperty("soldProducts")]
    public ICollection<UsersSoldProductsDTO> ProductInfo { get; set; }
}
