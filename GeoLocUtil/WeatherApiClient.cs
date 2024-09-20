using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class WeatherApiClient
{
    private readonly HttpClient _httpClient;
    private const string ApiKey = "f897a99d971b5eef57be6fafa0d83239";

    public WeatherApiClient()
    {
        _httpClient = new HttpClient();
    }
     // Handle city and state input
    public virtual async Task<LocationResult[]> GetCoordinatesByLocationAsync(string location)
    {
        var url = $"http://api.openweathermap.org/geo/1.0/direct?q={location},US&limit=1&appid={ApiKey}";


        try
        {
            var response = await _httpClient.GetStringAsync(url);
            return JsonConvert.DeserializeObject<LocationResult[]>(response);
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"HTTP error while fetching data for location '{location}': {ex.Message}");
            return null; // Handle network or HTTP-related errors
        }
        catch (JsonSerializationException ex)
        {
            Console.WriteLine($"Error deserializing data for location '{location}': {ex.Message}");
            return null; // Handle JSON parsing errors
        }
        catch (TaskCanceledException ex)
        {
            Console.WriteLine($"Request for location '{location}' timed out: {ex.Message}");
            return null; // Handle timeout or canceled task
        }
        catch (Exception ex)
        {
            // Catch any other unforeseen errors
            Console.WriteLine($"An unexpected error occurred while fetching data for location '{location}': {ex.Message}");
            return null;
        }

    }
    
       
    

    // Handle zip code input
    public virtual async Task<LocationResult> GetCoordinatesByZipAsync(string zip)
    {
        var url = $"http://api.openweathermap.org/geo/1.0/zip?zip={zip},US&appid={ApiKey}";

        try
        {
            var response = await _httpClient.GetStringAsync(url);
            return JsonConvert.DeserializeObject<LocationResult>(response);
        }
        catch (HttpRequestException ex)
        {
            // Handle other potential HTTP errors
            Console.WriteLine($"Error fetching data for ZIP code {zip}: {ex.Message}");
            return null; // Return null for other errors as well
        }
        catch (JsonSerializationException ex)
        {
            // Handle issues with JSON deserialization
            Console.WriteLine($"Error deserializing data for ZIP code {zip}: {ex.Message}");
            return null;
        }
        catch (TaskCanceledException ex)
        {
            // Handle timeout or request cancellation
            Console.WriteLine($"Request for ZIP code {zip} timed out: {ex.Message}");
            return null;
        }
        catch (Exception ex)
        {
            // Catch all other unforeseen errors
            Console.WriteLine($"An unexpected error occurred while fetching data for ZIP code {zip}: {ex.Message}");
            return null;
        }
    }
}

