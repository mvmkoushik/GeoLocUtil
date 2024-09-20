using System.Text.RegularExpressions;

public class GeoLocUtil
{
    public readonly WeatherApiClient _apiClient;

    // Default constructor for production
    public GeoLocUtil()
    {
        _apiClient = new WeatherApiClient();
    }

    // Overloaded constructor for testability (dependency injection)
    public GeoLocUtil(WeatherApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task RunAsync(string[] locations)
    {
        if (locations == null || locations.Length == 0)
        {
            Console.WriteLine("No valid input provided or invalid format. Please use \"City, State\" or \"ZipCode\" format.");
            return;
        }

        foreach (var location in locations)
        {
            if (IsZipCode(location))
            {
                await HandleZipCodeAsync(location); // Process valid ZIP codes
            }
            else if (IsCityState(location))
            {
                await HandleCityStateAsync(location); // Process valid City/State entries
            }
            else
            {
                Console.WriteLine($"Invalid input format: {location}"); // Log invalid entries
            }
        }
    }

    private async Task HandleZipCodeAsync(string zip)
    {
        try
        {
            var result = await _apiClient.GetCoordinatesByZipAsync(zip);
            if (result != null)
            {
                Console.WriteLine($"ZIP: {zip}, Place Name: {result.Name}, Country: {result.Country}, Latitude: {result.Lat}, Longitude: {result.Lon}");
            }
            else
            {
                Console.WriteLine($"ZIP code {zip} not found");
            }
            
        }
        catch (Exception ex)
        {
            // Log error for this particular ZIP code (if not found or other issues)
            Console.WriteLine($"Error handling ZIP code {zip}: {ex.Message}");
        }
    }

    private async Task HandleCityStateAsync(string location)
    {
        try
        {
            var results = await _apiClient.GetCoordinatesByLocationAsync(location);

            if (results.Any())
            {
                var result = results.First(); // Use the first result if multiple are returned
                Console.WriteLine($"Place Name: {result.Name}, State: {result.State}, Country: {result.Country}, Latitude: {result.Lat}, Longitude: {result.Lon}");
            }
            else
            {
                Console.WriteLine($"No results found for location: {location}");
            }
        }
        catch (Exception ex)
        {
            // Log error for this particular City/State entry
            Console.WriteLine($"Error handling city/state '{location}': {ex.Message}");
        }
    }

    private bool IsZipCode(string input)
    {
        // Basic validation for ZIP code format (5 digits)
        return Regex.IsMatch(input, @"^\d{5}$");
    }

    private bool IsCityState(string input)
    {
        // Basic validation for City, State format (must have a comma between city and state)
        return input.Contains(",") && input.Split(',').Length == 2;
    }
}
