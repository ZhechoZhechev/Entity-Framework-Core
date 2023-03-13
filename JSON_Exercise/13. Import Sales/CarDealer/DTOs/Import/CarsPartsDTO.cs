using Newtonsoft.Json;

namespace CarDealer.DTOs.Import;

[JsonObject]
public class CarsPartsDTO
{
    [JsonProperty("make")]
    public string Make { get; set; }

    [JsonProperty("model")]
    public string Model { get; set; }

    [JsonProperty("traveledDistance")]
    public int TraveledDistance { get; set; }

    [JsonProperty("partsId")]
    public int[] PartsId { get; set; }
}
