using Newtonsoft.Json;

namespace ProductShop.DTOs.Import;

[JsonObject]
public class ImportUserDTO 
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public int? Age { get; set; }
}
