using System;
using System.Threading.Tasks;
using Moq;
using Xunit;


public class GeoLocUtilIntegrationTests
{

    [Fact]
    public async Task Test_ValidCityStateInput_ShouldReturnCorrectLocation()
    {
        // Arrange
        var mockApiClient = new Mock<WeatherApiClient>();
        var geoLocUtil = new GeoLocUtil(mockApiClient.Object); // Pass mock API client to GeoLocUtil

        var mockCityStateResponse = new LocationResult[]
        {
            new LocationResult { Name = "Los Angeles", Lat = 34.0522, Lon = -118.2437, State = "CA", Country = "US" }
        };

        mockApiClient.Setup(client => client.GetCoordinatesByLocationAsync(It.IsAny<string>()))
            .ReturnsAsync(mockCityStateResponse);

        var stringWriter = new StringWriter();
        var originalConsoleOut = Console.Out;
        Console.SetOut(stringWriter);

        try
        {
            // Act
            string[] locations = { "Los Angeles, CA" };
            await geoLocUtil.RunAsync(locations);

            // Assert
            stringWriter.Flush();
            string output = stringWriter.ToString();
            Assert.Contains("Place Name: Los Angeles, State: CA, Country: US, Latitude: 34.0522, Longitude: -118.2437", output);
        }
        finally
        {
            Console.SetOut(originalConsoleOut); // Reset Console output
        }
    }

    [Fact]
    public async Task Test_ValidZipCodeInput_ShouldReturnCorrectLocation()
    {
        // Arrange
        var mockApiClient = new Mock<WeatherApiClient>();
        var geoLocUtil = new GeoLocUtil(mockApiClient.Object);

        var mockZipResponse = new LocationResult
        {
            Name = "New York",
            Lat = 40.7128,
            Lon = -74.0060,
            Country = "US"
        };

        mockApiClient.Setup(client => client.GetCoordinatesByZipAsync(It.IsAny<string>()))
            .ReturnsAsync(mockZipResponse);

        var stringWriter = new StringWriter();
        var originalConsoleOut = Console.Out;
        Console.SetOut(stringWriter);

        try
        {
            // Act
            string[] locations = { "10001" };
            await geoLocUtil.RunAsync(locations);

            // Assert
            stringWriter.Flush();

            string output = stringWriter.ToString().Trim(); // Trim output to avoid whitespace issues
            Console.WriteLine($"Captured Output: {output}");                // Log the captured output for debugging

            Assert.Contains("ZIP: 10001, Place Name: New York, Country: US, Latitude: 40.7128, Longitude: -74.006", output);
        }
        finally
        {
            Console.SetOut(originalConsoleOut);
        }
    }

    [Fact]
    public async Task Test_InvalidInputFormat_ShouldDisplayErrorMessage()
    {
        // Arrange
        var geoLocUtil = new GeoLocUtil(); // No need for a mock API in this case
        var stringWriter = new StringWriter();
        var originalConsoleOut = Console.Out;
        Console.SetOut(stringWriter);

        try
        {
            // Act
            string[] locations = { "InvalidLocationFormat" };
            await geoLocUtil.RunAsync(locations);

            // Assert
            stringWriter.Flush();
            string output = stringWriter.ToString();
            Assert.Contains("Invalid input format: InvalidLocationFormat", output);
        }
        finally
        {
            Console.SetOut(originalConsoleOut);
        }
    }

    [Fact]
    public async Task Test_ValidCityStateInput_NoResults_ShouldDisplayNoResultsMessage()
    {
        // Arrange
        var mockApiClient = new Mock<WeatherApiClient>();
        var geoLocUtil = new GeoLocUtil(mockApiClient.Object);

        var mockEmptyCityStateResponse = new LocationResult[0];

        mockApiClient.Setup(client => client.GetCoordinatesByLocationAsync(It.IsAny<string>()))
            .ReturnsAsync(mockEmptyCityStateResponse);

        var stringWriter = new StringWriter();
        var originalConsoleOut = Console.Out;
        Console.SetOut(stringWriter);

        try
        {
            // Act
            string[] locations = { "Los Angeles, CA" };
            await geoLocUtil.RunAsync(locations);

            // Assert
            stringWriter.Flush();
            string output = stringWriter.ToString();
            Assert.Contains("No results found for location: Los Angeles, CA", output);
        }
        finally
        {
            Console.SetOut(originalConsoleOut);
        }
    }

    [Fact]
    public async Task Test_ValidZipCodeInput_NotFound_ShouldDisplayZipNotFoundMessage()
    {
        // Arrange
        var mockApiClient = new Mock<WeatherApiClient>();
        var geoLocUtil = new GeoLocUtil(mockApiClient.Object);

        mockApiClient.Setup(client => client.GetCoordinatesByZipAsync(It.IsAny<string>()))
            .ReturnsAsync((LocationResult)null);

        var stringWriter = new StringWriter();
        var originalConsoleOut = Console.Out;
        Console.SetOut(stringWriter);

        try
        {
            // Act
            string[] locations = { "99999" };
            await geoLocUtil.RunAsync(locations);

            // Assert
            stringWriter.Flush();
            string output = stringWriter.ToString();
            Assert.Contains("ZIP code 99999 not found", output);
        }
        finally
        {
            Console.SetOut(originalConsoleOut);
        }
    }

    [Fact]
    public async Task Test_EmptyInput_ShouldDisplayErrorMessage()
    {
        // Arrange
        var geoLocUtil = new GeoLocUtil(); // No need for a mock API
        var stringWriter = new StringWriter();
        var originalConsoleOut = Console.Out;
        Console.SetOut(stringWriter);

        try
        {
            // Act
            await geoLocUtil.RunAsync(new string[] {});

            // Assert
            stringWriter.Flush();
            string output = stringWriter.ToString();
            Assert.Contains("No valid input provided or invalid format. Please use \"City, State\" or \"ZipCode\" format.", output);
        }
        finally
        {
            Console.SetOut(originalConsoleOut);
        }
    }
}