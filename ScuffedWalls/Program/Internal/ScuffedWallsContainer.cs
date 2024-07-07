﻿using ModChart;
using Octokit;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;

namespace ScuffedWalls
{
    internal static class ScuffedWallsContainer
    {
        public static string[] args;
        
        public static JsonSerializerOptions DefaultJsonConverterSettings { get; private set; }
        public static string ConfigFileName { get; private set; }
        public static Config ScuffedConfig { get; private set; }
        public static SDictionary Info { get; private set; }
        public static SDictionary InfoDifficulty { get; private set; }
        public static BpmAdjuster BPMAdjuster { get; private set; }
        public static ScuffedWallFile ScuffedWallFile { get; private set; }
        public static List<FileChangeDetector> FilesToChange { get; set; }
        public static RPC DiscordRPCManager { get; private set; }
        /// <summary>
        /// These events are erased after a single invoke
        /// </summary>

        public static event Action OnProgramComplete;
        /// <summary>
        /// These events are erased after a single invoke
        /// </summary>

        public static event Action OnChangeDetected;
        //Airscrach: Adds Code Correction For Some Code Editors, (This Is The Only Code Edit That I Wanted To Make, The Rest Will Just Be Docs, love your work <3)
        public static string SWText =>
@$"# ScuffedWalls {ScuffedWalls.Version}

# Documentation on functions can be found at
# https://github.com/thelightdesigner/ScuffedWalls/blob/main/Functions.md

# Using this tool requires an understanding of Noodle Extensions.
# https://github.com/Aeroluna/Heck/wiki

# If the documentation is not sufficient
# DM thelightdesigner#0832 or iswimfly#0556 for help (Discord)
# Noodle Extension Community Discord https://discord.gg/ZDC3pG3xB8

Workspace
";
        public static void ResetAwaitingFiles()
        {
            FilesToChange = new List<FileChangeDetector>() { ScuffedWallFile.Detector };
        }
        public static void Initialize(string[] argss)
        {
            JsonSerializerOptions SerializerOptions = new JsonSerializerOptions() { IgnoreNullValues = true };
            SerializerOptions.Converters.Add(new SDictionaryJsonConverter());
            DefaultJsonConverterSettings = SerializerOptions;

            Console.Title = $"ScuffedWalls {ScuffedWalls.Version}";
            args = argss;
            ConfigFileName = $"{AppDomain.CurrentDomain.BaseDirectory}ScuffedWalls.json";
            Console.WriteLine(ConfigFileName);
            ScuffedConfig = GetConfig();

            VerifyOld();
            VerifySW();
            VerifyBackups();

            
            Info = GetInfo();
            try
            {
                InfoDifficulty = Info.at<IEnumerable<object>>("_difficultyBeatmapSets").Cast<SDictionary>()
                         .Where(set => set.at<IEnumerable<object>>("_difficultyBeatmaps").Cast<SDictionary>().Any(dif => dif["_beatmapFilename"].ToString() == new FileInfo(ScuffedConfig.MapFilePath).Name))
                         .First().at<IEnumerable<object>>("_difficultyBeatmaps").Cast<SDictionary>()
                         .Where(dif => dif["_beatmapFilename"].ToString() == new FileInfo(ScuffedConfig.MapFilePath).Name).First();
            }
            catch(Exception e)
            {
                ScuffedWalls.Print($"Error in info.dat! {e}", ScuffedWalls.LogSeverity.Critical);
                Console.ReadLine();
                Environment.Exit(1);
            }

            BPMAdjuster = new BpmAdjuster(Info["_beatsPerMinute"].ToFloat(), InfoDifficulty["_noteJumpMovementSpeed"].ToFloat(), InfoDifficulty["_noteJumpStartBeatOffset"].ToFloat());
            ScuffedWalls.Print($"Njs: {BPMAdjuster.Njs} Offset: {BPMAdjuster.StartBeatOffset} HalfJump: {BPMAdjuster.HalfJumpBeats}");

            ScuffedWallFile = new ScuffedWallFile(ScuffedConfig.SWFilePath);
            ResetAwaitingFiles();

            DiscordRPCManager = new RPC();
            var releasething = CheckReleases();

        }

        public static void InvokeOnProgramComplete()
        {
            OnProgramComplete?.Invoke();
            if (OnProgramComplete != null)
            {
                var subs = OnProgramComplete.GetInvocationList();
                foreach (var sub in subs) OnProgramComplete -= (Action)sub;
            }
        }
        public static void InvokeOnChangeDetected()
        {
            OnChangeDetected?.Invoke();
            if (OnChangeDetected != null)
            {
                var subs = OnChangeDetected.GetInvocationList();
                foreach (var sub in subs) OnChangeDetected -= (Action)sub;
            }
        }
        public static void BackupMap()
        {
            File.Copy(ScuffedConfig.MapFilePath, Path.Combine(ScuffedConfig.BackupPaths.BackupMAPFolderPath, $"{DateTime.Now.ToFileString()}.dat"));
        }

       /* public static void Check(DifficultyV3 map)
        {
            try
            {
                InfoDifficulty["_customData"] ??= new TreeDictionary();
                InfoDifficulty["_customData._requirements"] ??= new List<object>();
                InfoDifficulty["_customData._suggestions"] ??= new List<object>();

                var requirements = (IList<object>)InfoDifficulty["_customData._requirements"];
                var suggestions = (IList<object>)InfoDifficulty["_customData._suggestions"];


                if (requirements.Any(r => r.ToString() == "Mapping Extensions") && map.needsNoodleExtensions())
                {
                    requirements.Remove("Mapping Extensions");
                }
                if (!requirements.Any(r => r.ToString() == "Noodle Extensions") && map.needsNoodleExtensions())
                {
                    requirements.Add("Noodle Extensions");
                }
                if (!(requirements.Any(r => r.ToString() == "Chroma") || suggestions.Any(s => s.ToString() == "Chroma")) && map.needsChroma())
                {
                    requirements.Add("Chroma");
                }
            }
            catch
            {
                //void
            }
        }*/

        public static void VerifyBackups()
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

        public static Config GetConfig()
        {
            VerifyConfig();
            return JsonSerializer.Deserialize<Config>(File.ReadAllText(ConfigFileName), DefaultJsonConverterSettings);
        }
        public static void VerifySW()
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
        public static async Task CheckReleases()
        {
            GitHubClient client = new GitHubClient(new ProductHeaderValue("ScuffedWalls"));
            var releases = await client.Repository.Release.GetAll("thelightdesigner", "ScuffedWalls");
            var latest = releases.OrderBy(r => r.PublishedAt).Last();
            if (latest.TagName != ScuffedWalls.Version)
            {
                ScuffedWalls.Print($"Update Available! Latest Ver: {latest.Name} ({latest.HtmlUrl})", ScuffedWalls.LogSeverity.Notice, ShowStackFrame: false);
            }
        }
        public static SDictionary GetInfo()
        {
            SDictionary info = null;
            if (File.Exists(ScuffedConfig.InfoPath)) info = JsonSerializer.Deserialize<SDictionary>(File.ReadAllText(ScuffedConfig.InfoPath), DefaultJsonConverterSettings);
            else
            {
                Console.WriteLine("No Info.dat/info.dat found!");
                Console.ReadLine();
                Environment.Exit(1);
            }
            info["_customData"] ??= new SDictionary();
            info["_customData._editors"] ??= new SDictionary();
            info["_customData._editors._lastEditedBy"] = "ScuffedWalls";
            info["_customData._editors.ScuffedWalls"] ??= new SDictionary();
            info["_customData._editors.ScuffedWalls.version"] = ScuffedWalls.Version;

            return info;
        }
        public static void VerifyOld()
        {
            if (!ScuffedConfig.IsAutoImportEnabled) return;

            if ((!File.Exists(ScuffedConfig.OldMapPath)))
            {
                using (StreamWriter file = new StreamWriter(ScuffedConfig.OldMapPath))
                {
                    file.Write(File.ReadAllText(ScuffedConfig.MapFilePath));
                }
                ScuffedWalls.Print("Created New Old Map File");
            }
        }
        public static void VerifyConfig()
        {
            if (args.Any() || !File.Exists(ConfigFileName))
            {
                Config reConfig = ConfigureSW();

                if (File.Exists(ConfigFileName)) File.Delete(ConfigFileName);

                using (StreamWriter file = new StreamWriter(ConfigFileName))
                {
                    file.Write(JsonSerializer.Serialize(reConfig, new JsonSerializerOptions() { WriteIndented = true }));
                    file.Close();
                }
                File.SetAttributes(ConfigFileName, FileAttributes.Normal);
            }
        }




        public static Config ConfigureSW()
        {
            Config config = new Config() { IsBackupEnabled = true, IsAutoImportEnabled = false };

            string mapfolderpath = args != null && args.Any() ? args[0] : new FileInfo(Process.GetCurrentProcess().MainModule.FileName).DirectoryName;
            // get beatmap option
            DirectoryInfo mapFolder = new DirectoryInfo(mapfolderpath);
            FileInfo[] mapDataFiles = mapFolder.GetFiles("*.dat");

            if(mapDataFiles.Count() < 1)
            {
                ScuffedWalls.Print("No map files (*.dat) detected!", ScuffedWalls.LogSeverity.Critical);
                Console.ReadLine();
                Environment.Exit(1);
            }

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

            Console.Write($"Difficulty To Work On (overwrites everything in this difficulty) ({string.Join(",", indexoption)}):");
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
            Console.Write("Clear Console on Refresh? (y/n):");
            {
                char answer = Convert.ToChar(Console.ReadLine().ToLower());
                if (answer == 'y') config.ClearConsoleOnRefresh = true;
            }


            //path of the sw file by difficulty name
            config.SWFilePath = Path.Combine(mapFolder.FullName, mapDataFiles[option].Name.Split('.')[0] + "_ScuffedWalls.sw");

            config.OldMapPath = Path.Combine(mapFolder.FullName, mapDataFiles[option].Name.Split('.')[0] + "_Old.dat");

            config.BackupPaths = new Config.Backup();

            config.MapFilePath = mapDataFiles[option].FullName;

            config.MapFolderPath = mapfolderpath;

            config.BackupPaths.BackupFolderPath = Path.Combine(mapFolder.FullName, mapDataFiles[option].Name.Split('.')[0] + "Backup");

            config.BackupPaths.BackupSWFolderPath = Path.Combine(config.BackupPaths.BackupFolderPath, "SW_History");

            config.BackupPaths.BackupMAPFolderPath = Path.Combine(config.BackupPaths.BackupFolderPath, "Map_History");

            config.IsAutoSimplifyPointDefinitionsEnabled = true;

            config.PrettyPrintJson = false;

            return config;
        }
    }




}
