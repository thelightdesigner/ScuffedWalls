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
        public const string Version = "v2.1.1";
        static void Main(string[] args)
        {
            ScuffedWallsContainer.Initialize(args);

            Print($"ScuffedWalls {Version}");
            Print(ScuffedWallsContainer.ScuffedConfig.MapFolderPath);


            while (true)
            {
                if (ScuffedWallsContainer.ScuffedConfig.ClearConsoleOnRefresh) Console.Clear();
                ScuffedWallsContainer.ScuffedWallFile.Refresh();
                Print($"{FileChangeDetector.LatestMessage}, running...");
                var StartTime = DateTime.Now;
                ScuffedWallsContainer.InvokeOnChangeDetected();
                ExecuteRequest();
                GC.Collect();
                ScuffedWallsContainer.InvokeOnProgramComplete();
                printStats();
                Print($"Completed in {(DateTime.Now - StartTime).TotalSeconds} Seconds");
                Print($"Waiting for changes to {string.Join(", ", ScuffedWallsContainer.FilesToChange.Select(file => file.File.Name))}");
                FileChangeDetector.WaitForChange(ScuffedWallsContainer.FilesToChange);
                ScuffedWallsContainer.ResetAwaitingFiles();
            }
        }
        static void ExecuteRequest()
        {
            ScuffedRequest Request = null;
            Debug.TryAction(() => 
            {
                Request = (ScuffedRequest)new ScuffedRequest().SetupFromLines(ScuffedWallsContainer.ScuffedWallFile.Parameters);
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
                Print($"Writing to {new FileInfo(ScuffedWallsContainer.ScuffedConfig.MapFilePath).Name}", ShowStackFrame: false);
                File.WriteAllText(ScuffedWallsContainer.ScuffedConfig.MapFilePath, JsonSerializer.Serialize(Parser.Result, new JsonSerializerOptions() { IgnoreNullValues = true, WriteIndented = ScuffedWallsContainer.ScuffedConfig.PrettyPrintJson }));
                
                ScuffedWallsContainer.DiscordRPCManager.CurrentMap = Parser.Result;
                ScuffedWallsContainer.DiscordRPCManager.Workspaces = Parser.Workspaces.Count();

                Print(string.Join(' ', Parser.Result.Stats.Select(st => $"[{st.Value} {st.Key.MakePlural(st.Value)}]")), ShowStackFrame: false);
            }, e =>
            {
                Print($"Error saving to map file ERROR: {(e.InnerException ?? e).Message}", LogSeverity.Critical);
            }); 
            Print("Saving Config");
            File.WriteAllText(ScuffedWallsContainer.ConfigFileName, JsonSerializer.Serialize(ScuffedWallsContainer.ScuffedConfig, new JsonSerializerOptions() { IgnoreNullValues = true, WriteIndented = true }));
           
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
            Error, //Provide information about a non-critical failure
            Critical //Provide information about a critical failure
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
