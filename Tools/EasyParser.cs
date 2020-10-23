using System;
using System.Globalization;

namespace Nerdomat.Tools
{
    public static class EasyParser
    {
        public static int GetInt(this string value) {
            const int defaultValue = 0;

            if (!int.TryParse(value.DigitsOnly(), NumberStyles.Any, CultureInfo.CurrentCulture, out var result) &&
                !int.TryParse(value.DigitsOnly(), NumberStyles.Any, CultureInfo.GetCultureInfo("en-US"), out result) &&
                !int.TryParse(value.DigitsOnly(), NumberStyles.Any, CultureInfo.InvariantCulture, out result)) {

                result = defaultValue;
            }

            return result;
        }

        public static bool CanGetInt(this string value) {
            if (!int.TryParse(value.DigitsOnly(), NumberStyles.Any, CultureInfo.CurrentCulture, out _) &&
                !int.TryParse(value.DigitsOnly(), NumberStyles.Any, CultureInfo.GetCultureInfo("en-US"), out _) &&
                !int.TryParse(value.DigitsOnly(), NumberStyles.Any, CultureInfo.InvariantCulture, out _)) {

                return false;
            }

            return true;
        }

        public static short GetShort(this string value) {
            const short defaultValue = 0;

            if (!short.TryParse(value.DigitsOnly(), NumberStyles.Any, CultureInfo.CurrentCulture, out var result) &&
                !short.TryParse(value.DigitsOnly(), NumberStyles.Any, CultureInfo.GetCultureInfo("en-US"), out result) &&
                !short.TryParse(value.DigitsOnly(), NumberStyles.Any, CultureInfo.InvariantCulture, out result)) {

                result = defaultValue;
            }

            return result;
        }
        
        public static bool CanGetShort(this string value) {
            if (!short.TryParse(value.DigitsOnly(), NumberStyles.Any, CultureInfo.CurrentCulture, out _) &&
                !short.TryParse(value.DigitsOnly(), NumberStyles.Any, CultureInfo.GetCultureInfo("en-US"), out _) &&
                !short.TryParse(value.DigitsOnly(), NumberStyles.Any, CultureInfo.InvariantCulture, out _)) {

                return false;
            }

            return true;
        }

        public static float GetFloat(this string value) {
            const float defaultValue = 0;

            if (!float.TryParse(value.DigitsOnly(), NumberStyles.Any, CultureInfo.CurrentCulture, out var result) &&
                !float.TryParse(value.DigitsOnly(), NumberStyles.Any, CultureInfo.GetCultureInfo("en-US"), out result) &&
                !float.TryParse(value.DigitsOnly(), NumberStyles.Any, CultureInfo.InvariantCulture, out result)) {

                result = defaultValue;
            }

            return result;
        }
        
        public static bool CanGetFloat(this string value) {
            if (!float.TryParse(value.DigitsOnly(), NumberStyles.Any, CultureInfo.CurrentCulture, out _) &&
                !float.TryParse(value.DigitsOnly(), NumberStyles.Any, CultureInfo.GetCultureInfo("en-US"), out _) &&
                !float.TryParse(value.DigitsOnly(), NumberStyles.Any, CultureInfo.InvariantCulture, out _)) {

                return false;
            }

            return true;
        }

        public static double GetDouble(this string value) {
            const double defaultValue = 0;

            if (!double.TryParse(value.DigitsOnly(), NumberStyles.Any, CultureInfo.CurrentCulture, out var result) &&
                !double.TryParse(value.DigitsOnly(), NumberStyles.Any, CultureInfo.GetCultureInfo("en-US"), out result) &&
                !double.TryParse(value.DigitsOnly(), NumberStyles.Any, CultureInfo.InvariantCulture, out result)) {

                result = defaultValue;
            }

            return result;
        }

        public static bool CanGetDouble(this string value) {
            if (!double.TryParse(value, NumberStyles.Any, CultureInfo.CurrentCulture, out _) &&
                !double.TryParse(value, NumberStyles.Any, CultureInfo.GetCultureInfo("en-US"), out _) &&
                !double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out _)) {

                return false;
            }

            return true;
        }

        public static decimal GetDecimal(this string value) {
            const decimal defaultValue = 0;

            if (!decimal.TryParse(value.DigitsOnly(), NumberStyles.Any, CultureInfo.CurrentCulture, out var result) &&
                !decimal.TryParse(value.DigitsOnly(), NumberStyles.Any, CultureInfo.GetCultureInfo("en-US"), out result) &&
                !decimal.TryParse(value.DigitsOnly(), NumberStyles.Any, CultureInfo.InvariantCulture, out result)) {

                result = defaultValue;
            }

            return result;
        }
        
        public static bool CanGetDecimal(this string value) {
            if (!decimal.TryParse(value, NumberStyles.Any, CultureInfo.CurrentCulture, out _) &&
                !decimal.TryParse(value, NumberStyles.Any, CultureInfo.GetCultureInfo("en-US"), out _) &&
                !decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out _)) {

                return false;
            }

            return true;
        }
    }
}