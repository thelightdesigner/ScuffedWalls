using System;
using System.Collections.Generic;
using System.IO;

namespace ScuffedWalls
{
    class ScuffedFile
    {
        string SWFilePath;
        public string[] SWFileLines;
        public string[] SWRaw;
        public ScuffedFile(string path)
        {
            SWFilePath = path;
        }
        public void Refresh()
        {
            //get all new lines from file
            List<string> lines = new List<string>();
            List<string> raw = new List<string>();

            using (StreamReader FileReader = new StreamReader(new FileStream(SWFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)))
            {
                while (!FileReader.EndOfStream)
                {
                    string line = FileReader.ReadLine();
                    raw.Add(line);
                    if (!string.IsNullOrEmpty(line.removeWhiteSpace())) if (line.removeWhiteSpace()[0] != '#') lines.Add(line);
                }
            }
            SWRaw = raw.ToArray();
            SWFileLines = lines.ToArray();

            if (Startup.ScuffedConfig.IsBackupEnabled)
            {
                File.Copy(Startup.ScuffedConfig.SWFilePath,$"{Startup.ScuffedConfig.BackupPaths.BackupSWFolderPath}\\{DateTime.Now.ToFileString()}.sw");
            }
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
