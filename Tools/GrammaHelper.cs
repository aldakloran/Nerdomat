using System;

namespace Nerdomat.Tools
{
    public static class GrammaHelper
    {
        public static string FlaskGrammaVariety(this int count)
        {
            var number = Math.Abs(count);
            var lastNumber = number % 10;
            
            if (number == 1)
                return "flaszke";

            if((number > 20 || number < 10) && (lastNumber == 2 || lastNumber == 3 || lastNumber == 4))
                return "flaszki";

            return "flaszek";
        }
    }
}