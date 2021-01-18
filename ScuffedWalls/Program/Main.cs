using ModChart;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace ScuffedWalls
{
    static class ScuffedWalls
    {
        public static string ver = "v4.2.0.6.9";
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
                change.Detect();
                change._LastModifiedTime = File.GetLastWriteTime(ScuffedConfig.SWFilePath);
                scuffedFile.Refresh();
                ScuffedLogger.ScuffedFileParser.Log("ScuffedWall File Parsed");
                List<Workspace> workspaces = new List<Workspace>();

                for (int i = 0; i < scuffedFile.SWFileLines.Length; i++)
                {
                    if (scuffedFile.SWFileLines[i].ToLower().StartsWith("workspace"))
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

                //write to json file
                JsonSerializerOptions jso = new JsonSerializerOptions(); jso.IgnoreNullValues = true;
                BeatMap generate = FunctionParser.toBeatMap(workspaces.ToArray());
                File.WriteAllText(ScuffedConfig.MapFilePath, JsonSerializer.Serialize(generate, jso));

                rpc.currentMap = new MapObj()
                {
                    Walls = generate._obstacles.Length,
                    Notes = generate._notes.Length,
                    CustomEvents = generate._customData._customEvents.Length,
                    MapName = new FileInfo(ScuffedConfig.MapFolderPath).Name
                };

                //collect the trash
                GC.Collect();

            }

        }

    }

}
