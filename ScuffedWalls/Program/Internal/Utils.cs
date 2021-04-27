using ModChart;
using Octokit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace ScuffedWalls
{
    class Utils
    {
        private string[] args;

        public static string ConfigFileName { get; private set; }
        public static Config ScuffedConfig { get; private set; }
        public static Info Info { get; private set; }
        public static Info.DifficultySet.Difficulty InfoDifficulty { get; private set; }
        public static BpmAdjuster bpmAdjuster { get; private set; }

        static string SWText = 
@$"# ScuffedWalls {ScuffedWalls.ver}

# Documentation on functions can be found at
# https://github.com/thelightdesigner/ScuffedWalls/blob/main/Functions.md
            
# DM @thelightdesigner#1337 for more help?

# Using this tool requires an understanding of Noodle Extensions.
# https://github.com/Aeroluna/NoodleExtensions/blob/master/Documentation/AnimationDocs.md

# Playtest your maps

Workspace:Default";

        public Utils(string[] args)
        {
            Console.Title = $"ScuffedWalls {ScuffedWalls.ver}";
            this.args = args;
            ConfigFileName = $"{AppDomain.CurrentDomain.BaseDirectory}ScuffedWalls.json";
            Console.WriteLine(ConfigFileName);
            ScuffedConfig = GetConfig();
            Info = GetInfo();
            InfoDifficulty = Info._difficultyBeatmapSets
                        .Where(set => set._difficultyBeatmaps.Any(dif => dif._beatmapFilename.ToString() == new FileInfo(Utils.ScuffedConfig.MapFilePath).Name))
                        .First()._difficultyBeatmaps
                        .Where(dif => dif._beatmapFilename.ToString() == new FileInfo(Utils.ScuffedConfig.MapFilePath).Name).First();

            bpmAdjuster = new BpmAdjuster(Info._beatsPerMinute.toFloat(), InfoDifficulty._noteJumpMovementSpeed.toFloat(), InfoDifficulty._noteJumpStartBeatOffset.toFloat());
            VerifyOld();
            VerifySW();
            VerifyBackups();
            ScuffedLogger.Default.BpmAdjuster.Log($"Njs: {bpmAdjuster.Njs} Offset: {bpmAdjuster.StartBeatOffset} HalfJump: {bpmAdjuster.HalfJumpBeats}");
            var releasething = CheckReleases();
            
        }

        void BackupMap()
        {
            File.Copy(ScuffedConfig.MapFilePath, ScuffedConfig.BackupPaths.BackupMAPFolderPath + $"\\{DateTime.Now.ToFileString()}.dat");
        }

        public void Check(BeatMap map)
        {
            try
            {
                if (InfoDifficulty._customData._requirements.Any(r => r.ToString() == "Mapping Extensions") && map.needsNoodleExtensions())
                {
                    ScuffedLogger.Warning.Log("Info.dat CANNOT contain Mapping Extensions as a requirement if the map requires Noodle Extensions");
                }
                if (!InfoDifficulty._customData._requirements.Any(r => r.ToString() == "Noodle Extensions") && map.needsNoodleExtensions())
                {
                    ScuffedLogger.Warning.Log("Info.dat does not contain required field Noodle Extensions");
                }
                if (!(InfoDifficulty._customData._requirements.Any(r => r.ToString() == "Chroma") || InfoDifficulty._customData._suggestions.Any(s => s.ToString() == "Chroma")) && map.needsChroma())
                {
                    ScuffedLogger.Warning.Log("Info.dat does not contain required/suggested field Chroma");
                }
            }
            catch
            {
                //void
            }
        }
        void VerifyBackups()
        {
            if (!ScuffedConfig.IsBackupEnabled) return;
            //check for
            if (!Directory.Exists(ScuffedConfig.BackupPaths.BackupFolderPath))
            {
                Directory.CreateDirectory(ScuffedConfig.BackupPaths.BackupFolderPath);
            }
            if (!Directory.Exists(ScuffedConfig.BackupPaths.BackupSWFolderPath))
            {
                Directory.CreateDirectory(ScuffedConfig.BackupPaths.BackupSWFolderPath);
            }
            if (!Directory.Exists(ScuffedConfig.BackupPaths.BackupMAPFolderPath))
            {
                Directory.CreateDirectory(ScuffedConfig.BackupPaths.BackupMAPFolderPath);
            }
            BackupMap();
        }

        public Config GetConfig()
        {
            VerifyConfig();
            return JsonSerializer.Deserialize<Config>(File.ReadAllText(ConfigFileName));
        }
        public void VerifySW()
        {
            if (!File.Exists(ScuffedConfig.SWFilePath))
            {
                using (StreamWriter file = new StreamWriter(ScuffedConfig.SWFilePath))
                {
                    file.WriteLine(SWText);

                    if (ScuffedConfig.IsAutoImportEnabled)
                    {
                        file.WriteLine(
@$"
0: Import
   Path:{new FileInfo(ScuffedConfig.OldMapPath).Name}");
                    }
                }
                Console.Write("[Default] Main: ");
                new Rainbow().PrintRainbow("New Scuffed Wall File Created");
            }
        }
        public async Task CheckReleases()
        {
            GitHubClient client = new GitHubClient(new ProductHeaderValue("ScuffedWalls"));
            var releases = await client.Repository.Release.GetAll("thelightdesigner", "ScuffedWalls");
            var latest = releases.OrderByDescending(r => r.PublishedAt).First();
            if (latest.TagName != ScuffedWalls.ver)
            {
                ScuffedLogger.Default.Log($"Update Available! Latest Ver: {latest.Name} ({latest.HtmlUrl})");
            }
        }
        public Info GetInfo()
        {
            Info info = null;
            if (File.Exists(ScuffedConfig.InfoPath)) info = JsonSerializer.Deserialize<Info>(File.ReadAllText(ScuffedConfig.InfoPath));
            return info;
        }
        public void VerifyOld()
        {
            if (!ScuffedConfig.IsAutoImportEnabled) return;

            if ((!File.Exists(ScuffedConfig.OldMapPath)))
            {
                using (StreamWriter file = new StreamWriter(ScuffedConfig.OldMapPath))
                {
                    file.Write(File.ReadAllText(ScuffedConfig.MapFilePath));
                }
                ScuffedLogger.Default.Log("Created New Old Map File");
            }
        }
        public void VerifyConfig()
        {
            if (args.Length != 0 || !File.Exists(ConfigFileName))
            {
                Config reConfig = ConfigureSW();


                if (File.Exists(ConfigFileName)) File.Delete(ConfigFileName);


                using (StreamWriter file = new StreamWriter(ConfigFileName))
                {
                    file.Write(JsonSerializer.Serialize(reConfig, new JsonSerializerOptions() { WriteIndented = true }));
                }
                File.SetAttributes(ConfigFileName, FileAttributes.Hidden);
            }
        }




        Config ConfigureSW()
        {
            Config config = new Config() { IsBackupEnabled = true, IsAutoImportEnabled = false };

            if ((args.Length == 0 || args == null))
            {
                Console.WriteLine("No file was dragged into the exe!");
                Console.ReadLine();
                Environment.Exit(1);
            }

            // get beatmap option
            DirectoryInfo mapFolder = new DirectoryInfo(args[0]);
            FileInfo[] mapDataFiles = mapFolder.GetFiles("*.dat");

            int j = 0;
            List<int> indexoption = new List<int>();
            foreach (var filename in mapDataFiles)
            {
                if (filename.Name != "info.dat" && filename.Name != "Info.dat")
                {
                    Console.WriteLine(j + ": " + filename.Name.Split('.')[0]);
                    indexoption.Add(j);
                }
                else
                {
                    config.InfoPath = filename.FullName;
                }
                j++;
            }

            Console.Write($"Difficulty To Work On (overwrites everything in this difficulty) ({string.Join(",",indexoption)}):");
            int option = Convert.ToInt32(Console.ReadLine());

            Console.Write("AutoImport Map? (writes an import statement for created backup file) (y/n):");
            {
                char answer = Convert.ToChar(Console.ReadLine().ToLower());
                if (answer == 'y') config.IsAutoImportEnabled = true;
            }

            Console.Write("Create a Backup of SW History? (backs up .SW file on each save) (y/n):");
            {
                char answer = Convert.ToChar(Console.ReadLine().ToLower());
                if (answer == 'n') config.IsBackupEnabled = false;
            }

            //path of the sw file by difficulty name
            config.SWFilePath = mapFolder.FullName + @"\" + mapDataFiles[option].Name.Split('.')[0] + "_ScuffedWalls.sw";

            config.OldMapPath = mapFolder.FullName + @"\" + mapDataFiles[option].Name.Split('.')[0] + "_Old.dat";

            config.BackupPaths = new Config.Backup();

            config.MapFilePath = mapDataFiles[option].FullName;

            config.MapFolderPath = args[0];

            config.BackupPaths.BackupFolderPath = mapFolder.FullName + @"\" + mapDataFiles[option].Name.Split('.')[0] + "Backup";

            config.BackupPaths.BackupSWFolderPath = config.BackupPaths.BackupFolderPath + @"\" + "SW_History";

            config.BackupPaths.BackupMAPFolderPath = config.BackupPaths.BackupFolderPath + @"\" + "Map_History";

            config.IsAutoSimplifyPointDefinitionsEnabled = true;

            config.PrettyPrintJson = false;

            return config;
        }
    }




}
