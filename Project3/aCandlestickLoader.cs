using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_2
{
    /// <summary>
    /// aCandlestickLoader is used to load the data from the CSV file chosen from the user. It reads each line
    /// of data and splits it based on the delimiters and then breaks apart each piece of data into variables date,
    /// open, high, low, close, volume and rounds the data if needed. It then casts these numbers to doubles and 
    /// returns the data into a candlestick object to be used when displaying data on the form. It returns each line
    /// as a candlestick.
    /// </summary>
    public class aCandlestickLoader
    {
        // Method to load a list of `aCandlestick` objects from a CSV file
        public List<aCandlestick> LoadFromCsv(string filePath)
        {
            // Creates a new list to hold the candlestick data as `aCandlestick` objects
            var candlesticks = new List<aCandlestick>();

            // Opens a file stream for reading the file at `filePath` and disposes of it once finished
            using (var reader = new StreamReader(filePath))
            {
                // Defines characters to be used as delimiters for splitting data values
                char[] delimiters = { ',', '"', ' ' };

                // Reads and skips the first line, typically used for headers in CSV files
                string line = reader.ReadLine();

                // Loops through each line in the CSV file, starting from the second line
                while ((line = reader.ReadLine()) != null)
                {
                    // Splits the line into an array of values, removing empty entries
                    var values = line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

                    // Parses and converts the date value from the first element (assumes "yyyy-MM-dd" format)
                    var date = DateTime.ParseExact(values[0], "yyyy-MM-dd", CultureInfo.InvariantCulture);

                    // Parses and rounds the "open" value to two decimal places
                    var open = (double)Math.Round(100 * decimal.Parse(values[1], CultureInfo.InvariantCulture)) / 100;

                    // Parses and rounds the "high" value to two decimal places
                    var high = (double)Math.Round(100 * decimal.Parse(values[2], CultureInfo.InvariantCulture)) / 100;

                    // Parses and rounds the "low" value to two decimal places
                    var low = (double)Math.Round(100 * decimal.Parse(values[3], CultureInfo.InvariantCulture)) / 100;

                    // Parses and rounds the "close" value to two decimal places
                    var close = (double)Math.Round(100 * decimal.Parse(values[4], CultureInfo.InvariantCulture)) / 100;

                    // Parses the "volume" value, converting it to an unsigned long integer
                    var volume = ulong.Parse(values[5], CultureInfo.InvariantCulture);

                    // Creates a new `aCandlestick` object with parsed values for date, open, high, low, close, and volume
                    var candlestick = new aCandlestick(date, open, high, low, close, volume);

                    // Adds the newly created candlestick object to the list of candlesticks
                    candlesticks.Add(candlestick);
                }
            }

            // Returns the list of candlesticks loaded from the CSV file
            return candlesticks;
        }
    }
}



