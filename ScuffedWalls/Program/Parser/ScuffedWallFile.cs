using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace ScuffedWalls
{
    class ScuffedWallFile
    {
        public string Path { get; private set; }
        public FileInfo File { get; private set; }
        public List<Parameter> Parameters { get; private set; }
        public FileChangeDetector Detector { get; private set; }
        public List<KeyValuePair<int, string>> Raw { get; private set; }
        public List<KeyValuePair<int, string>> Lines { get; private set; }
        public ScuffedWallFile(string path)
        {
            Path = path;
            File = new FileInfo(path);
            Detector = new FileChangeDetector(File);
            Refresh();
        }
        public void Refresh()
        {

            Raw = GetLines();
            Lines = RemoveEmptyLines(RemoveCommentedAreas(Raw));
            Parameters = Lines.ToParameters();

            if (Raw.Any(line => line.Value.ToLower().Contains("rick roll")))
            {
                var process = new ProcessStartInfo() { FileName = "https://www.youtube.com/watch?v=xvFZjo5PgG0", UseShellExecute = true };
                Process.Start(process);
                Environment.Exit(1);
            }

            if (Utils.ScuffedConfig.IsBackupEnabled)
            {
                System.IO.File.Copy(Path, System.IO.Path.Combine(Utils.ScuffedConfig.BackupPaths.BackupSWFolderPath, $"{DateTime.Now.ToFileString()}.sw"));
            }
        }
        public static List<KeyValuePair<int, string>> RemoveCommentedAreas(IEnumerable<KeyValuePair<int, string>> lines)
        {
            List<KeyValuePair<int, string>> convertedLines = lines.ToList();

            bool isMassComment = false;
            for (int i = 0; i < convertedLines.Count; i++)
            {
                convertedLines[i] = new KeyValuePair<int, string>(convertedLines[i].Key, getCommented(convertedLines[i].Value));
            }

            string getCommented(string line)
            {
                string commented = "";
                for (int i = 0; i < line.Length; i++)
                {
                    if (i < line.Length - 1 && line[i + 1] == '#' && line[i] == '/') isMassComment = true;
                    if (!isMassComment)
                    {
                        if (line[i] == '#') break;
                        commented += line[i];
                    }
                    if (i != 0 && line[i - 1] == '#' && line[i] == '/') isMassComment = false;
                }
                return commented;
            }

            return convertedLines;
        }
        public static List<KeyValuePair<int, string>> RemoveEmptyLines(List<KeyValuePair<int, string>> lines)
        {
            List<KeyValuePair<int, string>> filtered = new List<KeyValuePair<int, string>>();
            foreach (var line in lines) if (!string.IsNullOrEmpty(line.Value.ToLower().RemoveWhiteSpace())) filtered.Add(line);
            return filtered;
        }
        public static List<KeyValuePair<int, string>> GetLinesFromFile(string path)
        {
            List<KeyValuePair<int, string>> raw = new List<KeyValuePair<int, string>>();
            using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using StreamReader FileReader = new StreamReader(stream);
                int i = 1;
                while (!FileReader.EndOfStream)
                {
                    raw.Add(new KeyValuePair<int, string>(i, FileReader.ReadLine()));
                    i++;
                }
            }
            return raw;
        }
        public List<KeyValuePair<int, string>> GetLines() => GetLinesFromFile(Path);

    }



}
