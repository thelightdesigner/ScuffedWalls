using System;
using System.Collections.Generic;
using System.Text;

namespace ScuffedWalls
{
    static class ScuffedLogger
    {
        public static void Log(string msg) => Console.WriteLine($"[ConsoleLoggerDefault] Main: {msg}");
        
        static public class ScuffedFileParser
        {
            public static void Log(string msg) => Console.WriteLine($"[ConsoleLoggerDefault] ScuffedFileParser: {msg}");
            
        }
        static public class ScuffedWorkspace
        {
            public static void Log(string msg) => Console.WriteLine($"[ConsoleLoggerDefault] ScuffedWorkspace: {msg}");
            
            static public class FunctionParser
            {
                public static void Log(string msg) => Console.WriteLine($"[ConsoleLoggerDefault] ScuffedWorkspace.FunctionParser: {msg}");
                
            }
        }
        static public class ScuffedMapWriter
        {
            public static void Log(string msg) => Console.WriteLine($"[ConsoleLoggerDefault] ScuffedMapWriter: {msg}");
            
        }
        
    }
    static class ConsoleErrorLogger
    {
        public static void Log(string msg) 
        {
            Console.ForegroundColor = ConsoleColor.Red;  
            Console.WriteLine($"[ConsoleErrorLogger] Exception.Log: {msg}");
            Console.ResetColor();
        }

    }
    class ScuffedException : Exception
    {
        public ScuffedException(string Message) : base(Message) {}

    }
}
