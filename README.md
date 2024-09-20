# GeoLocUtil Utility

## Overview

**GeoLocUtil** is a C# utility designed to fetch geographic coordinates (latitude, longitude) based on user input. The user can input either:

- A city and state in the format `"City, State"`
- A ZIP code

The utility fetches coordinates using the OpenWeatherMap API.

The project also includes unit tests that verify different behaviors, such as handling valid and invalid inputs for both city/state and ZIP codes.

---

## Prerequisites

To run this utility, the following tools and libraries must be installed on your machine:

### Tools
- **.NET 6.0 SDK or later**  
  Download it from [here](https://dotnet.microsoft.com/download/dotnet/6.0).

### Libraries
- **Newtonsoft.Json (v13.0.3)**  
  This is required to handle JSON deserialization from the OpenWeatherMap API responses.
  
- **RestSharp (v112.0.0)**  
  This is used to simplify making HTTP requests to the OpenWeatherMap API.

- **Moq (v3.5.0)**  
  For mocking dependencies in the unit tests.

- **Xunit (v2.5.3)**  
  The testing framework used to create and execute unit tests.

Install these dependencies via NuGet. Run the following commands from the root directory of your project:

```bash
dotnet add package Newtonsoft.Json --version 13.0.3
dotnet add package RestSharp --version 112.0.0
dotnet add package Moq --version 3.5.0
dotnet add package xunit --version 2.5.3
```

## OpenWeatherMap API Setup
This utility uses OpenWeatherMap's geocoding API to fetch geographical coordinates. To use this service, you need an API key.

1. Sign up at OpenWeatherMap and obtain an API key.
2. Replace the placeholder API key in the WeatherApiClient.cs file:

```csharp
private const string ApiKey = "YOUR_API_KEY";
```
Replace "YOUR_API_KEY" with your actual API key.

---

## How to Run
1. Clone the repository or download the code.
2. Open a terminal or command prompt and navigate to the solution directory.
3. Build the solution:

```bash
dotnet build
```
4. Run the utility:
   
```bash
dotnet run --project GeoLocUtil
```

## Usage
After running the utility, you will be prompted to enter city/state pairs or ZIP codes. Enter them in the following format:

- "City, State" (e.g., "Los Angeles, CA")
- "ZipCode" (e.g., "10001")
Make sure to separate each input by a space between quotations.

Example input:
```plaintext
"New York, NY" "Los Angeles, CA" "94103" "10001"
```
---
## Project Structure
The solution contains two projects:

- GeoLocUtil
Contains the main utility code, including the API client and the functionality for fetching geo-coordinates based on city/state or ZIP code.

- GeoLocUtil.Test
Contains unit tests for the GeoLocUtil functionality. These tests use the Moq library to mock the WeatherApiClient dependency, ensuring proper isolation during testing.

## Main Project (GeoLocUtil.csproj)
| File               | Description   |
| -------------      | ------------- |
| GeoLocUtil.cs      |  Main class that validates input and processes city/state or ZIP code lookups. |
| LocationResult.cs  |  Model class representing a location returned from the OpenWeatherMap API.     |
| WeatherApiClient.cs| Class that interacts with the OpenWeatherMap API to fetch geo-coordinates. |
| Program.cs| Entry point of the application, processes user input and calls GeoLocUtil. |

## Test Project (GeoLocUtil.Test.csproj)
| File               | Description   |
| -------------      | ------------- |
| GeoLocUtilIntegrationTests.cs    | Contains integration tests for GeoLocUtil using the Moq library. |

---
## Running Tests
The GeoLocUtil solution includes unit tests written with the xUnit framework and uses Moq for mocking API client dependencies.

## How to Run Tests:
1. Open a terminal or command prompt.
2. Navigate to the solution directory.
3. Run the tests using the following command:
```bash
dotnet test
```

This will execute all test cases located in GeoLocUtil.Test.csproj.

## Available Tests:
1. #### Test_ValidCityStateInput_ShouldReturnCorrectLocation
    Verifies that the utility correctly processes valid city and state input.

2. #### Test_ValidZipCodeInput_ShouldReturnCorrectLocation
    Ensures valid ZIP code inputs return the correct location.

3. #### Test_InvalidInputFormat_ShouldDisplayErrorMessage
    Tests how the utility handles invalid input formats.

4. #### Test_ValidCityStateInput_NoResults_ShouldDisplayNoResultsMessage
    Ensures proper handling when no results are returned for valid city/state inputs.

5. #### Test_ValidZipCodeInput_NotFound_ShouldDisplayZipNotFoundMessage
    Ensures proper handling when no results are returned for valid ZIP code inputs.

6. #### Test_EmptyInput_ShouldDisplayErrorMessage
    Ensures the utility handles empty input appropriately and provides an error message.

---

## Extending the Utility
## Adding More API Endpoints:
If you'd like to extend the functionality to include more endpoints (e.g., to fetch weather data), you can modify the WeatherApiClient.cs file to make additional API requests and return corresponding data. For example, OpenWeatherMap offers a weather API that could be easily integrated in a similar manner.

## Error Handling Improvements:
Currently, the utility catches general exceptions and prints them to the console. You may wish to extend this error handling to include logging frameworks such as Serilog or NLog for production-level logging.

---


License
This project is licensed under the MIT License. See the LICENSE file for more details.


---

## Troubleshooting
1. #### Error: Unauthorized response from OpenWeatherMap API
    Ensure your API key is correct and active. Replace the placeholder key in WeatherApiClient.cs with your actual API key.

2. #### API Rate Limits
    OpenWeatherMap has rate limits for API calls. If you encounter issues, you may have exceeded the rate limit for your free tier.

