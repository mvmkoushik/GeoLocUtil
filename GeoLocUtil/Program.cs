using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

public class Program
{
    public static async Task Main(string[] args)
    {
        try
        {
            var geoLocUtil = new GeoLocUtil();

            Console.WriteLine("Enter city/state pairs or zip codes (format: \"City, State\" or \"ZipCode\"), separated by spaces between quotations:");
            var input = Console.ReadLine();

            // Attempt to extract inputs
            List<string> locations = ExtractQuotedInputs(input);

            if (locations.Count == 0)
            {
                Console.WriteLine("No valid input provided or invalid format. Please use \"City, State\" or \"ZipCode\" format.");
                return;
            }

            // Run async function to fetch geo locations
            await geoLocUtil.RunAsync(locations.ToArray());
        }
        catch (ArgumentNullException ex)
        {
            Console.WriteLine($"Input was null: {ex.Message}");
        }
        catch (RegexMatchTimeoutException ex)
        {
            Console.WriteLine($"Regex match timed out: {ex.Message}");
        }
        catch (FormatException ex)
        {
            Console.WriteLine($"Invalid format encountered: {ex.Message}");
        }
        catch (Exception ex) // General catch-all for any unforeseen errors
        {
            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
        }
    }

    // Helper method to extract quoted strings from input
    public static List<string> ExtractQuotedInputs(string input)
    {
        List<string> results = new List<string>();

        try
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return results;
            }

            // Use regex to extract values inside double quotes
            var matches = Regex.Matches(input, "\"([^\"]*)\"");

            foreach (Match match in matches)
            {
                var extracted = match.Groups[1].Value.Trim();
                if (!string.IsNullOrWhiteSpace(extracted) && (IsCityState(extracted) || IsZipCode(extracted)))
                {
                    results.Add(extracted);
                }
            }
        }
        catch (RegexMatchTimeoutException ex)
        {
            Console.WriteLine($"Regex match timed out while extracting inputs: {ex.Message}");
            throw; // Rethrow to allow upper levels to handle if needed
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while extracting inputs: {ex.Message}");
            throw; // Rethrow the exception to handle in higher-level code
        }

        return results;
    }

    // Helper method to validate if the input is a valid city, state format
    public static bool IsCityState(string input)
    {
        try
        {
            // Basic validation for city/state format, expecting a comma between city and state
            return input.Contains(",") && input.Split(',').Length == 2;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred during city/state validation: {ex.Message}");
            return false;
        }
    }

    // Helper method to validate if the input is a valid zip code (5 digits)
    public static bool IsZipCode(string input)
    {
        try
        {
            // Basic validation for zip code format (assuming 5 digits)
            return Regex.IsMatch(input, @"^\d{5}$");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred during zip code validation: {ex.Message}");
            return false;
        }
    }
}

