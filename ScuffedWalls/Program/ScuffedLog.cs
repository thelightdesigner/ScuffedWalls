using System;
using System.Collections.Generic;
using System.Text;

namespace ScuffedWalls
{
    static class ConsoleLogger
    {
        public static void Log(string msg)
        {
            Console.WriteLine($"[ConsoleLoggerDefault] Main: {msg}");
        }
        static public class ScuffedFileParser
        {
            public static void Log(string msg)
            {
                Console.WriteLine($"[ConsoleLoggerDefault] ScuffedFileParser: {msg}");
            }
        }
        static public class ScuffedWorkspace
        {
            public static void Log(string msg)
            {
                Console.WriteLine($"[ConsoleLoggerDefault] ScuffedWorkspace: {msg}");
            }
            static public class FunctionParser
            {
                public static void Log(string msg)
                {
                    Console.WriteLine($"[ConsoleLoggerDefault] ScuffedWorkspace.FunctionParser: {msg}");
                }
            }
        }
        static public class ScuffedMapWriter
        {
            public static void Log(string msg)
            {
                Console.WriteLine($"[ConsoleLoggerDefault] ScuffedMapWriter: {msg}");
            }
        }
        
    }
    static public class ConsoleErrorLogger
    {
        public static void Log(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine($"[ConsoleErrorLogger] Main: {msg}"); Console.ResetColor();
        }
        static public class ScuffedFileParser
        {
            public static void Log(string msg)
            {
                Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine($"[ConsoleErrorLogger] ScuffedFileParser: {msg}"); Console.ResetColor();
            }
        }
        static public class ScuffedWorkspace
        {
            public static void Log(string msg)
            {
                Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine($"[ConsoleErrorLogger] ScuffedWorkspace: {msg}"); Console.ResetColor();
            }
            static public class FunctionParser
            {
                public static void Log(string msg)
                {
                    Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine($"[ConsoleErrorLogger] ScuffedWorkspace.FunctionParser: {msg}"); Console.ResetColor();
                }
            }
        }
        static public class ScuffedMapWriter
        {
            public static void Log(string msg)
            {
                Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine($"[ConsoleErrorLogger] ScuffedMapWriter: {msg}"); Console.ResetColor();
            }
        }
    }
}
