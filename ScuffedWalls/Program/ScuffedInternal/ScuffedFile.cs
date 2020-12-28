using System;
using System.Collections.Generic;
using System.IO;

namespace ScuffedWalls
{
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
