using ModChart;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace ScuffedWalls
{
    static class ScuffedWalls
    {
        public static string ver = "v0.8.0";
        static void Main(string[] args)
        {
            ScuffedLogger.Log($"ScuffedWalls {ver}");
            Rainbow rnb = new Rainbow();
            RPC rpc = new RPC();
            Startup startup = new Startup(args);
            Config ScuffedConfig = startup.ScuffedConfig;
            Change change = new Change(ScuffedConfig);

            ScuffedFile scuffedFile = new ScuffedFile(ScuffedConfig.SWFilePath);


            ScuffedLogger.Log(ScuffedConfig.MapFolderPath);
            //loop through 
            while (true)
            {
                ScuffedLogger.Log($"Waiting for changes to {new FileInfo(ScuffedConfig.SWFilePath).Name}");
                change.Detect();
                var StartTime = DateTime.Now;
                ScuffedLogger.Log("Changes detected, running...");
                change._LastModifiedTime = File.GetLastWriteTime(ScuffedConfig.SWFilePath);
                scuffedFile.Refresh();
                List<Workspace> workspaces = new List<Workspace>();

                for (int i = 0; i < scuffedFile.SWFileLines.Length; i++)
                {
                    if (scuffedFile.SWFileLines[i].ToLower().removeWhiteSpace().StartsWith("workspace"))
                    {
                        try
                        {
                            rnb.Next(); ScuffedLogger.ScuffedWorkspace.Log($"Workspace {workspaces.Count}"); Console.ResetColor();
                            workspaces.Add(FunctionParser.parseWorkspace(scuffedFile.getLinesUntilNextWorkspace(i), ScuffedConfig, startup.Info, workspaces.ToArray()).toWorkspace());
                        }
                        catch (Exception e)
                        {
                            ConsoleErrorLogger.Log(e.Message);
                        }
                    }
                }
                ScuffedLogger.ScuffedMapWriter.Log($"Writing to {new FileInfo(ScuffedConfig.MapFilePath).Name}");
                //write to json file
                JsonSerializerOptions jso = new JsonSerializerOptions(); jso.IgnoreNullValues = true;
                BeatMap generate = FunctionParser.toBeatMap(workspaces.ToArray());
                File.WriteAllText(ScuffedConfig.MapFilePath, JsonSerializer.Serialize(generate, jso));
                ScuffedLogger.ScuffedMapWriter.Log($"Completed in {(DateTime.Now - StartTime).TotalSeconds} Seconds");

                rpc.currentMap = new MapObj()
                {
                    Walls = generate._obstacles.Length,
                    Notes = generate._notes.Length,
                    CustomEvents = generate._customData._customEvents.Length,
                    MapName = startup.Info._songName.ToString()
                };

                //collect the trash
                GC.Collect();

            }

        }

    }

}
