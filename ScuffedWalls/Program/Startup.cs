using ModChart;
using Octokit;
using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace ScuffedWalls
{
    class Startup
    {
        private string[] args;

        public static string ConfigFileName { get; private set; }
        public static Config ScuffedConfig { get; private set; }
        public static Info Info { get; private set; }
        public static Info.DifficultySet.Difficulty InfoDifficulty { get; private set; }

        static string[] SWText = {
            $"# ScuffedWalls {ScuffedWalls.ver}",
            "",
            @"# Documentation on functions can be found at",
            @"# https://github.com/thelightdesigner/ScuffedWalls/blob/main/Functions.md",
            "",
            @"# An example SW File can be found at",
            @"# https://github.com/thelightdesigner/ScuffedWalls/blob/main/ScuffedWalls%20Documentation%20Map/ExpertPlusStandard_ScuffedWalls.txt",
            "",
            "# DM @thelightdesigner#1337 for more help?",
            "",
            "Workspace" };

        public Startup(string[] args)
        {
            Console.Title = $"ScuffedWalls {ScuffedWalls.ver}";
            this.args = args;
            ConfigFileName = $"{AppDomain.CurrentDomain.BaseDirectory}ScuffedWalls.json";
            Console.WriteLine(ConfigFileName);
            ScuffedConfig = GetConfig();
            Info = GetInfo();
            InfoDifficulty = Info._difficultyBeatmapSets
                        .Where(set => set._difficultyBeatmaps.Any(dif => dif._beatmapFilename.ToString() == new FileInfo(Startup.ScuffedConfig.MapFilePath).Name))
                        .First()._difficultyBeatmaps
                        .Where(dif => dif._beatmapFilename.ToString() == new FileInfo(Startup.ScuffedConfig.MapFilePath).Name).First();
            VerifyOld();
            VerifySW();
            VerifyBackups();
            _ = CheckReleases();
        }

        void BackupMap()
        {
            File.Copy(ScuffedConfig.MapFilePath, ScuffedConfig.BackupPaths.BackupMAPFolderPath + $"\\{DateTime.Now.ToFileString()}.dat");
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
                    SWText.ToList().ForEach(line => { file.WriteLine(line); });

                    if (ScuffedConfig.IsAutoImportEnabled)
                    {
                        file.WriteLine("");
                        file.WriteLine("0: Import");
                        file.WriteLine($"   Path:{new FileInfo(ScuffedConfig.OldMapPath).Name}");
                    }
                }
                Console.Write("[ConsoleLoggerDefault] Main: "); Console.ForegroundColor = ConsoleColor.Red; Console.Write("N"); Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("e"); Console.ForegroundColor = ConsoleColor.Green; Console.Write("w"); Console.Write(" "); Console.ForegroundColor = ConsoleColor.Cyan; Console.Write("S"); Console.ForegroundColor = ConsoleColor.Blue; Console.Write("W"); Console.ForegroundColor = ConsoleColor.Magenta; Console.Write("F"); Console.ForegroundColor = ConsoleColor.Red; Console.Write("i"); Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("l"); Console.ForegroundColor = ConsoleColor.Green; Console.Write("e"); Console.Write(" "); Console.ForegroundColor = ConsoleColor.Cyan; Console.Write("C"); Console.ForegroundColor = ConsoleColor.Blue; Console.Write("r"); Console.ForegroundColor = ConsoleColor.Magenta; Console.Write("e"); Console.ForegroundColor = ConsoleColor.Red; Console.Write("a"); Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("t"); Console.ForegroundColor = ConsoleColor.Green; Console.Write("e"); Console.ForegroundColor = ConsoleColor.Cyan; Console.Write("d"); Console.ForegroundColor = ConsoleColor.Blue; Console.WriteLine("!"); Console.ResetColor();
            }
        }
        public async Task CheckReleases()
        {
            GitHubClient client = new GitHubClient(new ProductHeaderValue("ScuffedWalls"));
            var releases = await client.Repository.Release.GetAll("thelightdesigner", "ScuffedWalls");
            var latest = releases.OrderByDescending(r => r.PublishedAt).First();
            if (latest.TagName != ScuffedWalls.ver)
            {
                ScuffedLogger.Log($"Update Available! Latest Ver: {latest.Name} ({latest.HtmlUrl})");
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
                ScuffedLogger.Log("Created New Old Map File");
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
            foreach (var filename in mapDataFiles)
            {
                if (filename.Name != "info.dat" && filename.Name != "Info.dat")
                {
                    Console.WriteLine(j + ": " + filename.Name.Split('.')[0]);
                }
                else
                {
                    config.InfoPath = filename.FullName;
                }
                j++;
            }

            Console.Write("Input Difficulty Number:");
            int option = Convert.ToInt32(Console.ReadLine());

            Console.Write("AutoImport Map? (y/n):");
            {
                char answer = Convert.ToChar(Console.ReadLine());
                if (answer == 'y') config.IsAutoImportEnabled = true;
            }

            Console.Write("Create a Backup of SW History? (y/n):");
            {
                char answer = Convert.ToChar(Console.ReadLine());
                if (answer == 'n') config.IsBackupEnabled = false;
            }

            //path of the sw file by difficulty name
            config.SWFilePath = mapFolder.FullName + @"\" + mapDataFiles[option].Name.Split('.')[0] + "_ScuffedWalls.sw";

            config.OldMapPath = mapFolder.FullName + @"\" + mapDataFiles[option].Name.Split('.')[0] + "_OldMap.dat";

            config.BackupPaths = new Config.Backup();

            config.MapFilePath = mapDataFiles[option].FullName;

            config.MapFolderPath = args[0];

            config.BackupPaths.BackupFolderPath = mapFolder.FullName + @"\" + mapDataFiles[option].Name.Split('.')[0] + "_Backup";

            config.BackupPaths.BackupSWFolderPath = config.BackupPaths.BackupFolderPath + @"\" + "SW_History";

            config.BackupPaths.BackupMAPFolderPath = config.BackupPaths.BackupFolderPath + @"\" + "Map_History";

            config.IsAutoSimplifyPointDefinitionsEnabled = true;

            return config;
        }
    }
}
