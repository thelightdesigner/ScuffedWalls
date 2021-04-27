using System;
using System.IO;
using System.Linq;
using System.Text.Json;

using static ScuffedWalls.ScuffedLogger.Default;



namespace ScuffedWalls
{
    static class ScuffedWalls
    {
        public static string ver = "v1.0.2";
        static void Main(string[] args)
        {
            var helper = new Utils(args);

            Log($"ScuffedWalls {ver}");
            var rpc = new RPC();
            var Ts = new TimeKeeper();
            var scuffedFile = new ScuffedWallFile(Utils.ScuffedConfig.SWFilePath);
            var change = new Change(scuffedFile);

            Log(Utils.ScuffedConfig.MapFolderPath);

            while (true)
            {
               Log("Changes detected, running...");

                Ts.Start();

                //Create request
                ScuffedRequest Request = null;
                try
                {
                    Request = new ScuffedRequest(scuffedFile.Lines);
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

                Ts.Complete();

                rpc.currentMap = Parser.BeatMap;
                rpc.workspace = FunctionParser.Workspaces.Count();

                //om nom nom
                GC.Collect();

                //Warn the user
                helper.Check(Parser.BeatMap);

                //Wait for changes
                Log($"Waiting for changes to {new FileInfo(Utils.ScuffedConfig.SWFilePath).Name}");
                change.Detect();

            }
        }

    }
}
