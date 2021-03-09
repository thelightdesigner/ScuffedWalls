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
        public string[] SWFileLines;
        public string[] SWRaw;
        public ScuffedWallFile(string path)
        {
            Path = path;
            Refresh();
        }
        public void Refresh()
        {
            //get all new lines from file
            List<string> lines = new List<string>();
            List<string> raw = new List<string>();
            using (FileStream stream = new FileStream(Path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (StreamReader FileReader = new StreamReader(stream))
                {
                    while (!FileReader.EndOfStream)
                    {
                        string line = FileReader.ReadLine();
                        raw.Add(line);
                        if (!string.IsNullOrEmpty(line.removeWhiteSpace()) && line.removeWhiteSpace()[0] != '#') lines.Add(line);
                    }
                }
            }
            SWRaw = raw.ToArray();
            SWFileLines = lines.ToArray();
            Lines = lines.Select(l => { return new Parameter(l); }).ToArray();

            if (Startup.ScuffedConfig.IsBackupEnabled)
            {
                File.Copy(Path,$"{Startup.ScuffedConfig.BackupPaths.BackupSWFolderPath}\\{DateTime.Now.ToFileString()}.sw");
            }
        }

    }



}
