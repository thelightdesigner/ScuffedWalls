using ModChart;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace ScuffedWalls
{
    static class ScuffedWalls
    {
        public static string ver = "v0.4.0";
        static void Main(string[] args)
        {
            
            Console.Title = $"ScuffedWalls {ver}";
            ScuffedLogger.Log($"ScuffedWalls {ver}");
            Rainbow rnb = new Rainbow();
            Random rnd = new Random();
            RPC rpc = new RPC();
            string ConfigFileName = $"{AppDomain.CurrentDomain.BaseDirectory}ScuffedWalls.json";

            if (args.Length != 0 || !File.Exists(ConfigFileName))
            {
                Config reConfig = ConfigureSW();
                if (File.Exists(ConfigFileName)) File.Delete(ConfigFileName);
                using (StreamWriter file = new StreamWriter(ConfigFileName))
                {
                    file.Write(JsonSerializer.Serialize(reConfig));
                }
            }

            FileInfo config = new FileInfo(ConfigFileName);
            File.SetAttributes(config.FullName, FileAttributes.Hidden);

            Config ScuffedConfig = JsonSerializer.Deserialize<Config>(File.ReadAllText(config.FullName));

            //check for old notes/bloqs file
            if ((!File.Exists(ScuffedConfig.OldMapPath)) && ScuffedConfig.AutoImport)
            {
                using (StreamWriter file = new StreamWriter(ScuffedConfig.OldMapPath))
                {
                    file.Write(File.ReadAllText(ScuffedConfig.MapFilePath));
                }
                ScuffedLogger.Log("Created New Old Map File");
            }
            //check for old sw file
            if (!File.Exists(ScuffedConfig.SWFilePath))
            {
                using (StreamWriter file = new StreamWriter(ScuffedConfig.SWFilePath))
                {
                    file.WriteLine($"#ScuffedWalls {ver}");
                    file.WriteLine("#New SWFile Created");
                    file.WriteLine("#DM @thelightdesigner#1337 for help?");
                    file.WriteLine("");
                    file.WriteLine("Workspace");
                    if (ScuffedConfig.AutoImport)
                    {
                        file.WriteLine("");
                        file.WriteLine("0: Import");
                        file.WriteLine($" Path:{new FileInfo(ScuffedConfig.OldMapPath).Name}");
                    }
                }
                Console.Write("[ConsoleLoggerDefault] Main: "); Console.ForegroundColor = ConsoleColor.Red; Console.Write("N"); Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("e"); Console.ForegroundColor = ConsoleColor.Green; Console.Write("w"); Console.Write(" "); Console.ForegroundColor = ConsoleColor.Cyan; Console.Write("S"); Console.ForegroundColor = ConsoleColor.Blue; Console.Write("W"); Console.ForegroundColor = ConsoleColor.Magenta; Console.Write("F"); Console.ForegroundColor = ConsoleColor.Red; Console.Write("i"); Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("l"); Console.ForegroundColor = ConsoleColor.Green; Console.Write("e"); Console.Write(" "); Console.ForegroundColor = ConsoleColor.Cyan; Console.Write("C"); Console.ForegroundColor = ConsoleColor.Blue; Console.Write("r"); Console.ForegroundColor = ConsoleColor.Magenta; Console.Write("e"); Console.ForegroundColor = ConsoleColor.Red; Console.Write("a"); Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("t"); Console.ForegroundColor = ConsoleColor.Green; Console.Write("e"); Console.ForegroundColor = ConsoleColor.Cyan; Console.Write("d"); Console.ForegroundColor = ConsoleColor.Blue; Console.WriteLine("!"); Console.ResetColor();
            }


            bool ov = true;
            ScuffedFile scuffedFile = new ScuffedFile(ScuffedConfig.SWFilePath);
            DateTime lastModifiedTime = File.GetLastWriteTime(ScuffedConfig.SWFilePath);

            //loop through 
            while (true)
            {
                if (File.GetLastWriteTime(ScuffedConfig.SWFilePath) > lastModifiedTime || ov)
                {
                    scuffedFile.Refresh(); ov = false;
                    ScuffedLogger.ScuffedFileParser.Log("ScuffedWall File Parsed");
                    lastModifiedTime = File.GetLastWriteTime(ScuffedConfig.SWFilePath);
                    List<Workspace> workspaces = new List<Workspace>();

                    for (int i = 0; i < scuffedFile.SWFileLines.Length; i++)
                    {
                        if (scuffedFile.SWFileLines[i].ToLower().StartsWith("workspace"))
                        {
                            try
                            {
                                rnb.Next(); ScuffedLogger.ScuffedWorkspace.Log($"Workspace {workspaces.Count}"); Console.ResetColor();
                                workspaces.Add(FunctionParser.parseWorkspace(scuffedFile.getLinesUntilNextWorkspace(i), ScuffedConfig.MapFolderPath, workspaces.ToArray()).toWorkspace());
                            }
                            catch (Exception e)
                            {
                                ConsoleErrorLogger.Log(e.InnerException.Message);
                            }
                        }
                    }

                    //write to json file
                    BeatMap generate = FunctionParser.toBeatMap(workspaces.ToArray());
                    Map.GenerateToJson(ScuffedConfig.MapFilePath, generate);

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
                if (Console.KeyAvailable) { if (Console.ReadKey(true).Key == ConsoleKey.R) ov = true; }

                Task.Delay(15);
            }

            Config ConfigureSW()
            {
                if ((args.Length == 0 || args == null))
                {
                    ConsoleErrorLogger.Log("No file was dragged into the exe!");
                    Console.ReadLine();
                    Environment.Exit(1);
                }

                // get beatmap option
                DirectoryInfo mapFolder = new DirectoryInfo(args[0]);
                FileInfo[] mapDataFiles = mapFolder.GetFiles("*.dat");

                int j = 0;
                foreach (var filename in mapDataFiles)
                {
                    if (filename.Name != "info.dat" && filename.Name != "Info.dat")
                    {
                        Console.WriteLine(j + ": " + filename.Name.Split('.')[0]);
                    }
                    j++;
                }

                Console.Write("Input Difficulty Number:");
                int option = Convert.ToInt32(Console.ReadLine());

                Console.Write("AutoImport Map? (y/n):");
                char answer = Convert.ToChar(Console.ReadLine());
                bool AutoImportMap = false;
                if (answer == 'y') AutoImportMap = true;

                //path of the sw file by difficulty name
                string SWFilePath = mapFolder.FullName + @"\" + mapDataFiles[option].Name.Split('.')[0] + "_ScuffedWalls.sw";

                string OldMapPath = mapFolder.FullName + @"\" + mapDataFiles[option].Name.Split('.')[0] + "_OldMap.dat";

                return new Config() { SWFilePath = SWFilePath, MapFilePath = mapDataFiles[option].FullName, MapFolderPath = args[0], OldMapPath = OldMapPath, AutoImport = AutoImportMap };
            }

        }


    }


    class ScuffedFile
    {
        string SWFilePath;
        public string[] SWFileLines;
        public ScuffedFile(string path)
        {
            SWFilePath = path;
            Refresh();
        }
        public void Refresh()
        {
            //get all new lines from file
            List<string> lines = new List<string>();
            FileStream FileStream = new FileStream(SWFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            StreamReader FileReader = new StreamReader(FileStream);
            while (!FileReader.EndOfStream)
            {
                string line = new string(FileReader.ReadLine().removeWhiteSpace());
                if (!string.IsNullOrEmpty(line)) if (line[0] != '#') lines.Add(line);
            }
            //clean up
            FileReader.Close();
            FileStream.Close();
            SWFileLines = lines.ToArray();
        }
        public string[] getLinesUntilNextWorkspace(int index)
        {
            List<string> lines = new List<string>();
            for (int i = index + 1; i < SWFileLines.Length; i++)
            {
                if (SWFileLines[i].ToLower().StartsWith("workspace")) return lines.ToArray();
                lines.Add(SWFileLines[i]);
            }
            return lines.ToArray();
        }
        public static string[] getLinesUntilNextFunction(int index, string[] args)
        {
            List<string> lines = new List<string>();
            for (int i = index + 1; i < args.Length; i++)
            {
                if (Char.IsNumber(args[i][0])) return lines.ToArray();
                lines.Add(args[i]);
            }
            return lines.ToArray();
        }

    }
}
