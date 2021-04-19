using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ScuffedWalls
{
    class Change
    {
        public Change(ScuffedWallFile file)
        {
            _LastModifiedTime = File.GetLastWriteTime(file.Path);
            SWFile = file;
        }
        ScuffedWallFile SWFile;
        public DateTime _LastModifiedTime { get; set; }
        public void Detect()
        {
            while (File.GetLastWriteTime(Utils.ScuffedConfig.SWFilePath) == _LastModifiedTime) 
            {
                if (Console.KeyAvailable) if (Console.ReadKey().Key == ConsoleKey.R) break;
                Task.Delay(20);
            }
            _LastModifiedTime = File.GetLastWriteTime(Utils.ScuffedConfig.SWFilePath);

            SWFile.Refresh();
        }
        public static bool isFileLocked(string path)
        {
            try
            {
                using (Stream stream = new FileStream("MyFilename.txt", FileMode.Open))
                {
                    //void
                }
                return false;
            }
            catch
            {
                return true;
            }
        }
    }


}
