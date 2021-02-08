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
        public static string ver = "v0.8.2";
        static void Main(string[] args)
        {
            ScuffedLogger.Log($"ScuffedWalls {ver}");
            Rainbow rnb = new Rainbow();
            RPC rpc = new RPC();
            new Startup(args);
            Change change = new Change();
            ScuffedFile scuffedFile = new ScuffedFile(Startup.ScuffedConfig.SWFilePath);


            ScuffedLogger.Log(Startup.ScuffedConfig.MapFolderPath);


            do
            {
                var StartTime = DateTime.Now;

                ScuffedLogger.Log("Changes detected, running...");

                change._LastModifiedTime = File.GetLastWriteTime(Startup.ScuffedConfig.SWFilePath);

                scuffedFile.Refresh();


                List<Workspace> workspaces = new List<Workspace>();
                for (int i = 0; i < scuffedFile.SWFileLines.Length; i++)
                {
                    if (scuffedFile.SWFileLines[i].ToLower().removeWhiteSpace().StartsWith("workspace"))
                    {
                        try
                        {
                            var workspaceparam = scuffedFile.SWFileLines[i].TryGetParameter();

                            rnb.Next();
                            if(workspaceparam.argument != string.Empty) ScuffedLogger.ScuffedWorkspace.Log($"Workspace {workspaces.Count} : \"{workspaceparam.argument}\"");
                            else ScuffedLogger.ScuffedWorkspace.Log($"Workspace {workspaces.Count}");
                            Console.ResetColor();

                            if (workspaces.Any(w => w.Name == workspaceparam.argument && w.Name != string.Empty)) ConsoleErrorLogger.Log($"Workspaces should not have the same name. The first will be used on Clone");

                            workspaces.Add(FunctionParser.parseWorkspace(scuffedFile.getLinesUntilNextWorkspace(i), workspaces.ToArray()).toWorkspace(workspaceparam.argument));
                        }
                        catch (Exception e)
                        {
                            ConsoleErrorLogger.Log(e.Message);
                        }
                    }
                }
                ScuffedLogger.ScuffedMapWriter.Log($"Writing to {new FileInfo(Startup.ScuffedConfig.MapFilePath).Name}");

                //write to json file
                BeatMap generate = FunctionParser.toBeatMap(workspaces.ToArray());
                File.WriteAllText(Startup.ScuffedConfig.MapFilePath, JsonSerializer.Serialize(generate, new JsonSerializerOptions() { IgnoreNullValues = true }));

                ScuffedLogger.ScuffedMapWriter.Log($"Completed in {(DateTime.Now - StartTime).TotalSeconds} Seconds");

                rpc.currentMap = generate;

                //collect the trash
                GC.Collect();

                if (!Startup.InfoDifficulty._customData._requirements.Any(r => r.ToString() == "Noodle Extensions") && generate.needsNoodleExtensions())
                {
                    ScuffedLogger.ScuffedMapWriter.Log("Info.dat does not contain required field Noodle Extensions");
                }
                if (!(Startup.InfoDifficulty._customData._requirements.Any(r => r.ToString() == "Chroma") || Startup.InfoDifficulty._customData._suggestions.Any(s => s.ToString() == "Chroma")) && generate.needsChroma())
                {
                    ScuffedLogger.ScuffedMapWriter.Log("Info.dat does not contain required/suggested field Chroma");
                }
                ScuffedLogger.Log($"Waiting for changes to {new FileInfo(Startup.ScuffedConfig.SWFilePath).Name}");
                change.Detect();

            } while (true);

        }

    }

}
