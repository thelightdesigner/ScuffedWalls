using System;
using System.Collections;
using System.Linq;

namespace ScuffedWalls
{
    class Rainbow
    {
        readonly ConsoleColor[] colors = new ConsoleColor[]
        {
            ConsoleColor.Red,
            ConsoleColor.Yellow,
            ConsoleColor.Green,
            ConsoleColor.Cyan,
            ConsoleColor.Blue,
            ConsoleColor.Magenta
        };
        readonly IEnumerator colorenum; 
        
        public Rainbow()
        {
            colorenum = colors.GetEnumerator();
        }
        public void Next()
        {
            if (!colorenum.MoveNext())
            {
                colorenum.Reset();
                colorenum.MoveNext();
            }

            Console.ForegroundColor = (ConsoleColor)colorenum.Current;
        }
        public void PrintRainbow(string s)
        {
            foreach(var letter in s)
            {
                Next();
                Console.Write(letter);
            }
            Console.Write("\n");
            Console.ResetColor();
        }
    }



}
