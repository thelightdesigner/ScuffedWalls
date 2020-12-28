using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ScuffedWalls
{
    class Change
    {
        public Change(Config config)
        {
            _ScuffedConfig = config;
            _Override = true;
            _LastModifiedTime = File.GetLastWriteTime(_ScuffedConfig.SWFilePath);
        }
        public Config _ScuffedConfig { get; set; }
        public DateTime _LastModifiedTime { get; set; }
        public bool _Override { get; set; }
        public void Override() => _Override = true;
        

        public void Detect()
        {
            while (File.GetLastWriteTime(_ScuffedConfig.SWFilePath) == _LastModifiedTime && !_Override) 
            {
                if (Console.KeyAvailable) if (Console.ReadKey().Key == ConsoleKey.R) break;
                Task.Delay(20);
            }
            _Override = false;
        }
    }
}
