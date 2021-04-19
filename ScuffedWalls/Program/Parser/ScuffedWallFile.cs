using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ScuffedWalls
{
    class ScuffedWallFile
    {
        public string Path;
        public Parameter[] Lines;
        public KeyValuePair<int, string>[] SWFileLines;
        public KeyValuePair<int, string>[] SWRaw;
        public ScuffedWallFile(string path)
        {
            Path = path;
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
            Lines = SWFileLines.Select(l => new Parameter(l.Value) { GlobalIndex = l.Key }).ToArray();

            if (Utils.ScuffedConfig.IsBackupEnabled)
            {
                File.Copy(Path, $"{Utils.ScuffedConfig.BackupPaths.BackupSWFolderPath}\\{DateTime.Now.ToFileString()}.sw");
            }
        }

    }



}
