using System;
using System.Collections.Generic;
using System.Text;

namespace ScuffedWalls
{
    static class ScuffedLogger
    {
        public static class Default
        {
            public static void Log(string msg) => Console.WriteLine($"[Default] Main - {msg}");

            static public class ScuffedFileParser
            {
                public static void Log(string msg) => Console.WriteLine($"[Default] MapFileParser - {msg}");

            }
            static public class ScuffedWorkspace
            {
                public static void Log(string msg) => Console.WriteLine($"[Default] Parser.Workspace - {msg}");

                static public class FunctionParser
                {
                    public static void Log(string msg) => Console.WriteLine($"[Default] Parser.Workspace.Function - {msg}");

                }
            }
            static public class ScuffedMapWriter
            {
                public static void Log(string msg) => Console.WriteLine($"[Default] MapFileWriter - {msg}");

            }
            static public class BpmAdjuster
            {
                public static void Log(string msg) => Console.WriteLine($"[Default] BpmAdjuster - {msg}");
            }
        }
        public static class Error
        {
            public static void Log(string msg)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[Error] Exception.Log - {msg}");
                Console.ResetColor();
            }
            public static void Log(Exception msg)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[Error] Exception.Log - {msg.Message}");
                Console.ResetColor();
            }
        }
        public static class Warning
        {
            public static void Log(string msg)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"[Warning] Warning.Log - {msg}");
                Console.ResetColor();
            }
        }
        
    }

}
