namespace ScuffedWalls
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Text.Json;

    static class ScuffedWalls
    {
        public static string ver => "v1.5.0";
        static void Main(string[] args)
        {
            Utils.Initialize(args);

            Print($"ScuffedWalls {ver}");
            Print(Utils.ScuffedConfig.MapFolderPath);

            while (true)
            {
                Print("Changes detected, running...");
                var StartTime = DateTime.Now;
                Utils.InvokeOnChangeDetected();
                ExecuteRequest();
                GC.Collect();
                Utils.InvokeOnProgramComplete();
                Print($"Completed in {(DateTime.Now - StartTime).TotalSeconds} Seconds");
                Print($"Waiting for changes to {new FileInfo(Utils.ScuffedConfig.SWFilePath).Name}");
                
            }
        }
        static void ExecuteRequest()
        {
            ScuffedRequest Request = null;
            try
            {
                Request = new ScuffedRequest(Utils.ScuffedWallFile.Lines);
            }
            catch (Exception e)
            {
                Print($"Error parsing ScuffedWall file ERR: {(e.InnerException ?? e).Message}", LogSeverity.Critical);
            }

            //Do request
            FunctionParser Parser = null;
            try
            {
                Parser = new FunctionParser(Request);
            }
            catch (Exception e)
            {
                Print($"Error executing ScuffedRequest ERR: {(e.InnerException ?? e).Message}", LogSeverity.Critical);
            }

            //write to json file
            Print($"Writing to {new FileInfo(Utils.ScuffedConfig.MapFilePath).Name}");
            File.WriteAllText(Utils.ScuffedConfig.MapFilePath, JsonSerializer.Serialize(Parser.BeatMap, new JsonSerializerOptions() { IgnoreNullValues = true, WriteIndented = Utils.ScuffedConfig.PrettyPrintJson }));

            //add in requirements
            Utils.Check(Parser.BeatMap);

            Print($"Writing to Info.dat");
            File.WriteAllText(Utils.ScuffedConfig.InfoPath, JsonSerializer.Serialize(Utils.Info, new JsonSerializerOptions() { IgnoreNullValues = true, WriteIndented = true }));

            printStats();

            Utils.DiscordRPCManager.CurrentMap = Parser.BeatMap;
            Utils.DiscordRPCManager.Workspaces = FunctionParser.Workspaces.Count();
        }
        public static void Print(string Message, LogSeverity Severity = LogSeverity.Info, ConsoleColor? Color = null, StackFrame StackFrame = null)
        {
            if (Color.HasValue) Console.ForegroundColor = Color.Value;
            else Console.ForegroundColor = sevColor[(int)Severity];

            var methodInfo = (StackFrame ?? new StackTrace().GetFrame(1)).GetMethod();
            string stack = methodInfo.DeclaringType.Name.Replace("ScuffedWalls","Main");

            Console.WriteLine($"[{Severity}] {stack} - {Message}");

            Console.ResetColor();

            debugStats[(int)Severity]++;
        }
        public enum LogSeverity
        {
            Info, //Provide information about normal operations
            Notice, //Provide information about the important
            Warning, //Provide information about the unexpected
            Error, //Provide information about a non-crucial failure
            Critical //Provide information about a crucial failure
        }
        private static void printStats()
        {
            List<string> stat = new List<string>();
            for (int i = 0; i < logSevCount; i++)
            {
                if (debugStats[i] > 0) stat.Add($"[{debugStats[i]} {Extensions.MakePlural(((LogSeverity)i).ToString(), debugStats[i])}]");
                debugStats[i] = 0;
            }
            Print(string.Join(' ',stat));
            
        }
        private static readonly int logSevCount = Enum.GetNames(typeof(LogSeverity)).Length;

        private static readonly ConsoleColor[] sevColor = new ConsoleColor[]
        {
            ConsoleColor.Gray,
            ConsoleColor.Cyan,
            ConsoleColor.Yellow,
            ConsoleColor.Red,
            ConsoleColor.Magenta
        };
        private static int[] debugStats = new int[5];
    }
}
