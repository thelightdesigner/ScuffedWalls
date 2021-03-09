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
        public static string ver = "v1.0.0-unreleased";
        static void Main(string[] args)
        {
            var helper = new Startup(args);

            ScuffedLogger.Log($"ScuffedWalls {ver}");
            var rpc = new RPC();
            var rainbow = new Rainbow();
            var Ts = new TimeKeeper();
            var scuffedFile = new ScuffedWallFile(Startup.ScuffedConfig.SWFilePath);
            var change = new Change(scuffedFile);

            ScuffedLogger.Log(Startup.ScuffedConfig.MapFolderPath);

            do
            {
                ScuffedLogger.Log("Changes detected, running...");

                Ts.Start();

                //Create request
                ScuffedRequest Request = null;
                try
                {
                    Request = new ScuffedRequest(scuffedFile.Lines);
                }
                catch (Exception e)
                {
                    ConsoleErrorLogger.Log($"Error parsing ScuffedWall file ERR: {e.InnerException.Message}");
                }

                //Do request
                FunctionParser Parser = null;
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

                Ts.Complete();

                rpc.currentMap = Parser.BeatMap;
                rpc.workspace = FunctionParser.Workspaces.Count();

                //collect the trash
                GC.Collect();

                //Warn the user
                helper.Check(Parser.BeatMap);
                
                //Wait for changes
                ScuffedLogger.Log($"Waiting for changes to {new FileInfo(Startup.ScuffedConfig.SWFilePath).Name}");
                change.Detect();

            } while (true);

        }

    }




}
