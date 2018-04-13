using AT.MIN;
using DFHack;
using System;

namespace FortressToMinecraftConverter
{
    class ColorConsoleStream : IDFStream
    {
        public void AddText(ColorValue color, string text)
        {
            switch (color)
            {
                case ColorValue.ColorReset:
                    Console.ResetColor();
                    break;
                case ColorValue.ColorBlack:
                    Console.ForegroundColor = ConsoleColor.Black;
                    break;
                case ColorValue.ColorBlue:
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    break;
                case ColorValue.ColorGreen:
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    break;
                case ColorValue.ColorCyan:
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    break;
                case ColorValue.ColorRed:
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    break;
                case ColorValue.ColorMagenta:
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    break;
                case ColorValue.ColorBrown:
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    break;
                case ColorValue.ColorGrey:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                case ColorValue.ColorDarkgrey:
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    break;
                case ColorValue.ColorLightblue:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;
                case ColorValue.ColorLightgreen:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case ColorValue.ColorLightcyan:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;
                case ColorValue.ColorLightred:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case ColorValue.ColorLightmagenta:
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    break;
                case ColorValue.ColorYellow:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case ColorValue.ColorWhite:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                default:
                    break;
            }
            Console.Write(text);
        }

        public void BeginBatch()
        {
        }

        public void EndBatch()
        {
            Console.Write(Environment.NewLine);
        }

        public void Print(string format, params object[] parameters)
        {
            Console.WriteLine(Tools.Sprintf(format, parameters).TrimEnd('\r', '\n'));
        }

        public void PrintErr(string format, params object[] parameters)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(Tools.Sprintf(format, parameters).TrimEnd('\r', '\n'));
            Console.ResetColor();
        }
    }
}
