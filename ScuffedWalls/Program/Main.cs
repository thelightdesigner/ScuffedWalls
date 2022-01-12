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
        public const string Version = "v2.0.6-dev (\"i promise its better this time\" version)";
        static void Main(string[] args)
        {
            Utils.Initialize(args);

            Print($"ScuffedWalls {Version}");
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
                Request = (ScuffedRequest)new ScuffedRequest().SetupFromLines(Utils.ScuffedWallFile.Parameters);
            },e => 
            {
                Print($"Error parsing ScuffedWall file ERROR: {(e.InnerException ?? e).Message}", LogSeverity.Critical);
            });

            ScuffedRequestParser Parser = null;
            Debug.TryAction(() => 
            {
                Parser = new ScuffedRequestParser(Request);
                Parser.GetResult();
            },e => 
            {
                Print($"Error executing ScuffedRequest ERROR: {(e.InnerException ?? e).Message}", LogSeverity.Critical);
            });
            Debug.TryAction(() =>
            {
                Print($"Writing to {new FileInfo(Utils.ScuffedConfig.MapFilePath).Name}", ShowStackFrame: false);
                File.WriteAllText(Utils.ScuffedConfig.MapFilePath, JsonSerializer.Serialize(Parser.Result, new JsonSerializerOptions() { IgnoreNullValues = true, WriteIndented = Utils.ScuffedConfig.PrettyPrintJson }));
                
                Utils.DiscordRPCManager.CurrentMap = Parser.Result;
                Utils.DiscordRPCManager.Workspaces = Parser.Workspaces.Count();

                Print(string.Join(' ', Parser.Result.Stats.Select(st => $"[{st.Value} {st.Key.MakePlural(st.Value)}]")), ShowStackFrame: false);
            }, e =>
            {
                Print($"Error saving to map file ERROR: {(e.InnerException ?? e).Message}", LogSeverity.Critical);
            }); 
            Print("Saving Config");
            File.WriteAllText(Utils.ConfigFileName, JsonSerializer.Serialize(Utils.ScuffedConfig, new JsonSerializerOptions() { IgnoreNullValues = true, WriteIndented = true }));
           
        }
        public static void Print(string Message, LogSeverity Severity = LogSeverity.Info, ConsoleColor? Color = null, StackFrame StackFrame = null, bool ShowStackFrame = true, string OverrideStackFrame = null)
        {
            if (string.IsNullOrEmpty(Message)) return;

            if (Color.HasValue) Console.ForegroundColor = Color.Value;
            else Console.ForegroundColor = sevColor[(int)Severity];

            var methodInfo = (StackFrame ?? new StackTrace().GetFrame(1)).GetMethod();
            string stack = methodInfo.DeclaringType.Name.Replace("ScuffedWalls", "Main");
            string message = 
                OverrideStackFrame != null ? 
                $"{OverrideStackFrame} - {Message}" :
                (ShowStackFrame && Severity < LogSeverity.Error ?
                $"{stack} - {Message}" :
                Message);

            Console.WriteLine($"[{Severity}] {message}");

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
            Print(string.Join(' ', stat), Color: getHighestSev(), ShowStackFrame: false);
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
