using System;

namespace Ev.Helpers
{
    static class ColorMapper
    {
        public static ConsoleColor MapColor(Domain.Utils.Color color) => color switch
        {
            Domain.Utils.Color.Black       => ConsoleColor.Black,
            Domain.Utils.Color.DarkBlue    => ConsoleColor.DarkBlue,
            Domain.Utils.Color.DarkGreen   => ConsoleColor.DarkGreen,
            Domain.Utils.Color.DarkCyan    => ConsoleColor.DarkCyan,
            Domain.Utils.Color.DarkRed     => ConsoleColor.DarkRed,
            Domain.Utils.Color.DarkMagenta => ConsoleColor.DarkMagenta,
            Domain.Utils.Color.DarkYellow  => ConsoleColor.DarkYellow,
            Domain.Utils.Color.Gray        => ConsoleColor.Gray,
            Domain.Utils.Color.DarkGray    => ConsoleColor.DarkGray,
            Domain.Utils.Color.Blue        => ConsoleColor.Blue,
            Domain.Utils.Color.Green       => ConsoleColor.Green,
            Domain.Utils.Color.Cyan        => ConsoleColor.Cyan,
            Domain.Utils.Color.Red         => ConsoleColor.Red,
            Domain.Utils.Color.Magenta     => ConsoleColor.Magenta,
            Domain.Utils.Color.Yellow      => ConsoleColor.Yellow,
            Domain.Utils.Color.White       => ConsoleColor.White,
            _ => throw new ArgumentException("Unsupported color.")
        };
    }
}
