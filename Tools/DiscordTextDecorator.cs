using System;

namespace Nerdomat.Tools
{
    public static class DiscordTextDecorator
    {
        public static string Decorate(this string input, Decorator decorator, bool fullDecorate = false)
        {
            if (string.IsNullOrEmpty(input)) return input;

            var begin = string.Empty;
            var end = string.Empty;
            var inputReady = fullDecorate
                ? input
                : input.DiscordTrimmer(ref begin, ref end);

            switch (decorator)
            {
                case Decorator.Italics:
                    return $"{begin}*{inputReady}*{end}";
                case Decorator.Bold:
                    return $"{begin}**{inputReady}**{end}";
                case Decorator.Bold_Italics:
                    return $"{begin}***{inputReady}***{end}";
                case Decorator.Underline:
                    return $"{begin}__{inputReady}__{end}";
                case Decorator.Underline_italics:
                    return $"{begin}__*{inputReady}*__{end}";
                case Decorator.Underline_bold:
                    return $"{begin}__**{inputReady}**__{end}";
                case Decorator.underline_bold_italics:
                    return $"{begin}__***{inputReady}***__{end}";
                case Decorator.Strikethrough:
                    return $"{begin}~~{inputReady}~~{end}";
                case Decorator.Inline_code:
                    return $"{begin}`{inputReady}`{end}";
                case Decorator.Block_code:
                    return $"{begin}```\n{inputReady}\n```{end}";
                default:
                    throw new ArgumentOutOfRangeException(nameof(decorator), decorator, null);
            }
        }
    }

    public enum Decorator
    {
        Italics,
        Bold,
        Bold_Italics,
        Underline,
        Underline_italics,
        Underline_bold,
        underline_bold_italics,
        Strikethrough,
        Inline_code,
        Block_code
    }
}