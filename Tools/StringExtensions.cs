using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Nerdomat.Tools
{
    public static class StringExtensions
    {
        //trim beggining and end of string and return it as ref string
        public static string DiscordTrimmer(this string value, ref string beginTrim, ref string endTrim)
        {
            var output = value.TrimBegining(ref beginTrim);
            var valueRevert = output.RevertString();
            valueRevert = valueRevert.TrimBegining(ref endTrim);
            output = valueRevert.RevertString();

            return output;
        }

        //gets trimmed value from begging of string
        public static string TrimBegining(this string value, ref string trimmedValue)
        {
            var output = string.Empty;
            var firstCharFound = false;
            foreach (var c in value)
            {
                if (char.IsWhiteSpace(c) && !firstCharFound)
                {
                    trimmedValue += c;
                    continue;
                }

                output += c;
                firstCharFound = true;
            }

            return output;
        }

        // revert string
        public static string RevertString(this string value) => value.ToCharArray().Reverse().Aggregate(string.Empty, (current, c) => current + c);

        //generate empty string with certain length
        public static string Empty(int length)
        {
            var o = string.Empty;
            for (int i = 0; i < length; i++)
                o += " ";

            return o;
        }

        //algin text (add empty string to right)
        public static string AlginText(this string text, int? le = null)
        {
            int length = le ?? 40;
            var LengthEmpty = length - text.Length; ;

            var empty = LengthEmpty <= 0
                ? string.Empty
                : Empty(LengthEmpty);

            return $"{text}{empty}";
        }

        public static IEnumerable<string> DiscordMessageSplit(this string message)
        {
            const int maxMessageLength = 2000;
            if (message.Length >= maxMessageLength)
            {
                var sb = new StringBuilder();
                foreach (var sentence in Regex.Split(message, Environment.NewLine))
                {
                    if (sb.Length + sentence.Length < maxMessageLength)
                    {
                        sb.AppendLine(sentence);
                    }
                    else
                    {
                        yield return sb.ToString();
                        sb.Clear();
                        sb.AppendLine(sentence);
                    }
                }
                yield return sb.ToString();
            }
            else
            {
                yield return message;
            }
        }
    }
}