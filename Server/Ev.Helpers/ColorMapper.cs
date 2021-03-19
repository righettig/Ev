using Ev.Common.Core;
using System;

namespace Ev.Helpers
{
    static class ColorMapper
    {
        public static ConsoleColor MapColor(Color color) => color switch
        {
            Color.Black       => ConsoleColor.Black,
            Color.DarkBlue    => ConsoleColor.DarkBlue,
            Color.DarkGreen   => ConsoleColor.DarkGreen,
            Color.DarkCyan    => ConsoleColor.DarkCyan,
            Color.DarkRed     => ConsoleColor.DarkRed,
            Color.DarkMagenta => ConsoleColor.DarkMagenta,
            Color.DarkYellow  => ConsoleColor.DarkYellow,
            Color.Gray        => ConsoleColor.Gray,
            Color.DarkGray    => ConsoleColor.DarkGray,
            Color.Blue        => ConsoleColor.Blue,
            Color.Green       => ConsoleColor.Green,
            Color.Cyan        => ConsoleColor.Cyan,
            Color.Red         => ConsoleColor.Red,
            Color.Magenta     => ConsoleColor.Magenta,
            Color.Yellow      => ConsoleColor.Yellow,
            Color.White       => ConsoleColor.White,
            _ => throw new ArgumentException("Unsupported color.")
        };
    }
}
