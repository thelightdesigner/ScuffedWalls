using ModChart;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace ScuffedWalls
{
    static class ScuffedWalls
    {
        public static string ver = "v1.0.0";
        static void Main(string[] args)
        {
            new Startup(args);

            ScuffedLogger.Log($"ScuffedWalls {ver}");
            var rnb = new Rainbow();
            var rpc = new RPC();
            var scuffedFile = new ScuffedWallFile(Startup.ScuffedConfig.SWFilePath);
            var change = new Change(scuffedFile);


            ScuffedLogger.Log(Startup.ScuffedConfig.MapFolderPath);

            do {
                var StartTime = DateTime.Now;

                ScuffedLogger.Log("Changes detected, running...");

                //Create request
                ScuffedRequest Request = new ScuffedRequest(scuffedFile.Lines);
                try
                {
                    Request = new ScuffedRequest(scuffedFile.Lines);
                }
                catch(Exception e)
                {
                    ConsoleErrorLogger.Log($"Error parsing ScuffedWall file ERR: {e.InnerException.Message}");
                }

                //Do request
                FunctionParser Parser = new FunctionParser(Request);
                try
                {
                    Parser = new FunctionParser(Request);
                }
                catch (Exception e)
                {
                    ConsoleErrorLogger.Log($"Error executing ScuffedRequest ERR: {e.InnerException.Message}");
                }

                //write to json file
                ScuffedLogger.ScuffedMapWriter.Log($"Writing to {new FileInfo(Startup.ScuffedConfig.MapFilePath).Name}");
                File.WriteAllText(Startup.ScuffedConfig.MapFilePath, JsonSerializer.Serialize(Parser.BeatMap, new JsonSerializerOptions() { IgnoreNullValues = true, WriteIndented = Startup.ScuffedConfig.PrettyPrintJson }));

                ScuffedLogger.ScuffedMapWriter.Log($"Completed in {(DateTime.Now - StartTime).TotalSeconds} Seconds");

                rpc.currentMap = Parser.BeatMap;
                rpc.workspace = FunctionParser.Workspaces.Count();

                //collect the trash
                GC.Collect();

                //Warn the user
                if (Startup.InfoDifficulty._customData._requirements.Any(r => r.ToString() == "Mapping Extensions") && Parser.BeatMap.needsNoodleExtensions())
                {
                    ScuffedLogger.ScuffedMapWriter.Log("Info.dat CANNOT contain Mapping Extensions as a requirement if the map requires Noodle Extensions");
                }
                if (!Startup.InfoDifficulty._customData._requirements.Any(r => r.ToString() == "Noodle Extensions") && Parser.BeatMap.needsNoodleExtensions())
                {
                    ScuffedLogger.ScuffedMapWriter.Log("Info.dat does not contain required field Noodle Extensions");
                }
                if (!(Startup.InfoDifficulty._customData._requirements.Any(r => r.ToString() == "Chroma") || Startup.InfoDifficulty._customData._suggestions.Any(s => s.ToString() == "Chroma")) && Parser.BeatMap.needsChroma())
                {
                    ScuffedLogger.ScuffedMapWriter.Log("Info.dat does not contain required/suggested field Chroma");
                }
                ScuffedLogger.Log($"Waiting for changes to {new FileInfo(Startup.ScuffedConfig.SWFilePath).Name}");

                //Wait for changes
                change.Detect();

            } while (true);

        }

    }

}
