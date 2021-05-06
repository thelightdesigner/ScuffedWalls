using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ScuffedWalls.Functions
{
    //By rizthesnugie
    [ScuffedFunction("Uwu")]
    class uwu : SFunction
    {
        public override void Run()
        {
            Console.Title = "Skynoot";
            string OWO;


            Console.WriteLine("uuuuuuuuWuuuuuuuu (say OWO plz)");


            OWO = Console.ReadLine();

            if (OWO.ToUpper() == "OWO")
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("Are you a cat?[y/n]");
                Console.ForegroundColor = ConsoleColor.Gray;
                string y = Console.ReadLine();
                if (y.ToUpper() == "Y")
                {
                    Console.WriteLine("POGGERS");
                }
                else if (y.ToUpper() == "N")
                {
                    Console.WriteLine("That is not poggers");
                }
                else
                {
                    Console.WriteLine("Thats so sad");
                    Task.Delay(5000);
                    Environment.Exit(1);
                }
            }
            else
            {
                Console.WriteLine("you didn't say OWO");
            }

            Console.ReadKey();

        }

    }
    //end rizthesnuggie




}
