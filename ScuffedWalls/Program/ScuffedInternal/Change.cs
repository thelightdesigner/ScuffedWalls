using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ScuffedWalls
{
    class Change
    {
        public Change()
        {
            _LastModifiedTime = File.GetLastWriteTime(Startup.ScuffedConfig.SWFilePath);
        }
        public DateTime _LastModifiedTime { get; set; }
        public void Detect()
        {
            while (File.GetLastWriteTime(Startup.ScuffedConfig.SWFilePath) == _LastModifiedTime) 
            {
                if (Console.KeyAvailable) if (Console.ReadKey().Key == ConsoleKey.R) break;
                Task.Delay(20);
            }
        }
    }
}
