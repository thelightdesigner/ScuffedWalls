using System;

namespace ScuffedWalls
{
    class Rainbow
    {
        int color;
        public Rainbow()
        {
            color = 0;
        }
        public void Next()
        {
            Console.ForegroundColor = Rainbow.toConsoleColor((Color)color);
            color++;
            if (color == Enum.GetValues(typeof(Color)).Length) color = 0;
        }
        static ConsoleColor toConsoleColor(Color c)
        {
            foreach (var color in Enum.GetValues(typeof(ConsoleColor)))
            {
                if (color.ToString() == c.ToString()) return (ConsoleColor)color;
            }
            return ConsoleColor.Red;
        }
    }
    enum Color
    {
        Red, Yellow, Green, Cyan, Blue, Magenta
    }
}
