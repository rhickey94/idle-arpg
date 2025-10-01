using System;

namespace IdleARPG.Utilities
{
    public static class NumberFormatter
    {
        // Suffixes for number magnitudes
        private static readonly string[] suffixes = new string[]
        {
        "", "K", "M", "B", "T", "Qd", "Qt", "Sx", "Sv", "O", "N", "D"
        };

        // Implementation goes here
        public static string Format(float value)
        {
            if (value < 0) return $"{value}";
            if (value == 0) return "0";

            if (value < 1000)
                return value.ToString("F2");

            // Determine the magnitude (how many thousands)
            int magnitude = 0;
            double workingValue = value;

            while (workingValue >= 1000 && magnitude < suffixes.Length - 1)
            {
                workingValue /= 1000;
                magnitude++;
            }

            // Format the number with appropriate decimal places
            string formatted;

            // Has decimals
            formatted = workingValue.ToString("F2");
            

            // Return number with suffix
            return formatted + suffixes[magnitude];
        }
    }
}
