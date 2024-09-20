using Newtonsoft.Json;

public class LocationResult
{
    [JsonProperty("lat")]
    public double Lat { get; set; }

    [JsonProperty("lon")]
    public double Lon { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("state")]
    public string State { get; set; } // This may be null for zip code results

    [JsonProperty("country")]
    public string Country { get; set; }


}
