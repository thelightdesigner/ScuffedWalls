namespace ScuffedWalls
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text.Json;
    using static ScuffedLogger.Default;


    static class ScuffedWalls
    {
        public static string ver => "v1.3.3";
        static void Main(string[] args)
        {
            Utils.Initialize(args);

            Log($"ScuffedWalls {ver}");
            Log(Utils.ScuffedConfig.MapFolderPath);


            while (true)
            {
                Log("Changes detected, running...");
                var StartTime = DateTime.Now;
                Utils.InvokeOnChangeDetected();
                ExecuteRequest();
                GC.Collect();
                Utils.InvokeOnProgramComplete();
                Log($"Completed in {(DateTime.Now - StartTime).TotalSeconds} Seconds");
                Log($"Waiting for changes to {new FileInfo(Utils.ScuffedConfig.SWFilePath).Name}");
                Utils.SWFileChangeDetector.Detect();
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
                ScuffedLogger.Error.Log($"Error parsing ScuffedWall file ERR: {(e.InnerException ?? e).Message}");
            }

            //Do request
            FunctionParser Parser = null;
            try
            {
                Parser = new FunctionParser(Request);
            }
            catch (Exception e)
            {
                ScuffedLogger.Error.Log($"Error executing ScuffedRequest ERR: {(e.InnerException ?? e).Message}");
            }

            //write to json file
            ScuffedMapWriter.Log($"Writing to {new FileInfo(Utils.ScuffedConfig.MapFilePath).Name}");
            File.WriteAllText(Utils.ScuffedConfig.MapFilePath, JsonSerializer.Serialize(Parser.BeatMap, new JsonSerializerOptions() { IgnoreNullValues = true, WriteIndented = Utils.ScuffedConfig.PrettyPrintJson }));

            //add in requirements
            Utils.Check(Parser.BeatMap);

            ScuffedMapWriter.Log($"Writing to Info.dat");
            File.WriteAllText(Utils.ScuffedConfig.InfoPath, JsonSerializer.Serialize(Utils.Info, new JsonSerializerOptions() { IgnoreNullValues = true, WriteIndented = true }));


            Utils.DiscordRPCManager.CurrentMap = Parser.BeatMap;
            Utils.DiscordRPCManager.Workspaces = FunctionParser.Workspaces.Count();
        }

    }
}
