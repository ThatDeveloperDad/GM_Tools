using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GameTools.Framework.Converters
{
    public static class UnitConverters
    {
        public static string AsFeetInches(this int lengthCm)
        {
            int lengthMm = lengthCm * 10;
            int totalInches = lengthMm / 25;
            int feet = totalInches / 12;
            int inches = totalInches%12;

            return $"{feet}'{inches}";
        }

        public static string AsMeters(this int lengthCm)
        {
            double lengthMeters = (double)lengthCm / 100;
            return $"{lengthMeters} meters";
        }

        public static string AsPounds(this int weightKg)
        {
            double pounds = (double)weightKg * 2.2;
            return $"{(int)pounds} lbs";
        }
    }
}
