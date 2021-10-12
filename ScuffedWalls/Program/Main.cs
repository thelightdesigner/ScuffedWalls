namespace ScuffedWalls
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text.Json;

    static class ScuffedWalls
    {

        public static string ver => "v1.5.2-dev";
        static void Main(string[] args)
        {
            Utils.Initialize(args);

            Print($"ScuffedWalls {ver}");
            Print(Utils.ScuffedConfig.MapFolderPath);


            while (true)
            {
                if (Utils.ScuffedConfig.ClearConsoleOnRefresh) Console.Clear();
                Utils.ScuffedWallFile.Refresh();
                Print($"{FileChangeDetector.LatestMessage}, running...");
                var StartTime = DateTime.Now;
                Utils.InvokeOnChangeDetected();
                ExecuteRequest();
                GC.Collect();
                Utils.InvokeOnProgramComplete();
                printStats();
                Print($"Completed in {(DateTime.Now - StartTime).TotalSeconds} Seconds");
                Print($"Waiting for changes to {string.Join(", ", Utils.FilesToChange.Select(file => file.File.Name))}");
                FileChangeDetector.WaitForChange(Utils.FilesToChange);
                Utils.ResetAwaitingFiles();
            }
        }
        static void ExecuteRequest()
        {
            ScuffedRequest Request = null;
            Debug.TryAction(() => 
            {
                Request = (ScuffedRequest)new ScuffedRequest().Setup(Utils.ScuffedWallFile.Lines);
            },e => 
            {
                Print($"Error parsing ScuffedWall file ERR: {(e.InnerException ?? e).Message}", LogSeverity.Critical);
            });

            ScuffedRequestParser Parser = null;
            Debug.TryAction(() => 
            {
                Parser = new ScuffedRequestParser(Request);
                Parser.GetResult();
            },e => 
            {
                Print($"Error executing ScuffedRequest ERR: {(e.InnerException ?? e).Message}", LogSeverity.Critical);
            });

            //write to json file
            Print($"Writing to {new FileInfo(Utils.ScuffedConfig.MapFilePath).Name}");
            File.WriteAllText(Utils.ScuffedConfig.MapFilePath, JsonSerializer.Serialize(Parser.Result, new JsonSerializerOptions() { IgnoreNullValues = true, WriteIndented = Utils.ScuffedConfig.PrettyPrintJson }));

            //add in requirements
            Utils.Check(Parser.Result);

            Print($"Writing to Info.dat");
            File.WriteAllText(Utils.ScuffedConfig.InfoPath, JsonSerializer.Serialize(Utils.Info, new JsonSerializerOptions() { IgnoreNullValues = true, WriteIndented = true }));


            Utils.DiscordRPCManager.CurrentMap = Parser.Result;
            Utils.DiscordRPCManager.Workspaces = Parser.Workspaces.Count();
        }
        public static void Print(string Message, LogSeverity Severity = LogSeverity.Info, ConsoleColor? Color = null, StackFrame StackFrame = null, bool ShowStackFrame = true)
        {
            if (Color.HasValue) Console.ForegroundColor = Color.Value;
            else Console.ForegroundColor = sevColor[(int)Severity];

            var methodInfo = (StackFrame ?? new StackTrace().GetFrame(1)).GetMethod();
            string stack = methodInfo.DeclaringType.Name.Replace("ScuffedWalls", "Main");

            Console.WriteLine($"[{Severity}]{(ShowStackFrame && Severity != LogSeverity.Error ? " " + stack : "")} - {Message}");

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
                if (debugStats[i] > 0) stat.Add($"[{debugStats[i]} {((LogSeverity)i).ToString().MakePlural(debugStats[i])}]");
            }
            Print(string.Join(' ', stat), Color: getHighestSev());
            debugStats = new int[logSevCount];

            ConsoleColor getHighestSev()
            {
                LogSeverity highest = LogSeverity.Info;
                for (int i = 0; i < logSevCount; i++)
                {
                    if (debugStats[i] > 0) highest = (LogSeverity)i;
                }
                return sevColor[(int)highest];
            }
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
        private static int[] debugStats = new int[logSevCount];
    }
}
