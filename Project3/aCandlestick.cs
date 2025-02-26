using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_2
{
    // Defines a class called `aCandlestick` within the `Project_1` namespace
    // aCandlstick just stores the data in each stock candlestick so all variables are defined.
    public class aCandlestick
    {
        // Property to store the date of the candlestick
        public DateTime Date { get; set; }

        // Property to store the opening price of the candlestick
        public double Open { get; set; }

        // Property to store the highest price reached during the candlestick period
        public double High { get; set; }

        // Property to store the lowest price reached during the candlestick period
        public double Low { get; set; }

        // Property to store the closing price of the candlestick
        public double Close { get; set; }

        // Property to store the trading volume associated with the candlestick
        public ulong Volume { get; set; }

        // Constructor method that initializes an `aCandlestick` object with values for all properties
        public aCandlestick(DateTime date, double open, double high, double low, double close, ulong volume)
        {
            // Sets the `Date` property to the provided `date` parameter
            Date = date;

            // Sets the `Open` property to the provided `open` parameter
            Open = open;

            // Sets the `High` property to the provided `high` parameter
            High = high;

            // Sets the `Low` property to the provided `low` parameter
            Low = low;

            // Sets the `Close` property to the provided `close` parameter
            Close = close;

            // Sets the `Volume` property to the provided `volume` parameter
            Volume = volume;
        }
    }
}

