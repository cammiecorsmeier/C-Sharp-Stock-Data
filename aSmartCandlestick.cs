using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_2
{
    public class aSmartCandlestick : aCandlestick
    {
        // Constructor that initializes from base class properties
        public aSmartCandlestick(DateTime date, double open, double high, double low, double close, ulong volume)
            : base(date, open, high, low, close, volume) { } // Uses original values from aCandlestick

        // Properties 
        public double Range => Math.Abs(High - Low);        // Range of the whole candlestick
        public double BodyRange => Math.Abs(Close - Open);  // Range of just the body 
        public double TopPrice => Math.Max(Open, Close);    // Maximum price of the body
        public double BottomPrice => Math.Min(Open, Close); // Minimum price of the body
        public double UpperTail => High - TopPrice;         // Length of the tail above the body
        public double LowerTail => BottomPrice - Low;       // Length of the tail below the body

        // Determine if the candlestick is Bullish
        public bool IsBullish => Close > Open;

        // Determine if the candlestick is Bearish
        public bool IsBearish => Close < Open;

        // Determine if the candlestick is Neutral (open and close are almost the same)
        public bool IsNeutral => Math.Abs(Close - Open) < (0.001 * Range);

        // Pattern checks use previous calculations
        public bool IsMarubozu => UpperTail == 0 && LowerTail == 0;
        public bool IsHammer => LowerTail >= 2 * BodyRange && UpperTail < BodyRange;
        public bool IsDoji => IsNeutral && BodyRange < 0.1 * Range;
        public bool IsDragonflyDoji => IsDoji && UpperTail == 0 && LowerTail > 0;
        public bool IsGravestoneDoji => IsDoji && LowerTail == 0 && UpperTail > 0;
    }
}