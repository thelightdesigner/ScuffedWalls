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
        public List<Parameter> Lines { get; private set; }
        public FileChangeDetector Detector { get; private set; }
        public KeyValuePair<int, string>[] SWFileLines { get; private set; }
        public KeyValuePair<int, string>[] SWRaw { get; private set; }
        public ScuffedWallFile(string path)
        {
            Path = path;
            File = new FileInfo(path);
            Detector = new FileChangeDetector(File);
            Refresh();
        }
        public void Refresh()
        {

            //get all new lines from file
            List<KeyValuePair<int, string>> lines = new List<KeyValuePair<int, string>>();
            List<KeyValuePair<int, string>> raw = new List<KeyValuePair<int, string>>();
            using (FileStream stream = new FileStream(Path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (StreamReader FileReader = new StreamReader(stream))
                {
                    int i = 1;
                    while (!FileReader.EndOfStream)
                    {
                        string line = FileReader.ReadLine();
                        raw.Add(new KeyValuePair<int, string>(i, line));
                        if (!string.IsNullOrEmpty(line.RemoveWhiteSpace()) && line.RemoveWhiteSpace()[0] != '#') lines.Add(new KeyValuePair<int, string>(i, line));
                        i++;
                    }
                }
            }
            SWRaw = raw.ToArray();
            SWFileLines = lines.ToArray();
            Lines = SWFileLines.Select(l => new Parameter(l.Value, l.Key)).ToList();

            if (SWRaw.Any(line => line.Value.ToLower().Contains("rick roll")))
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

    }



}
