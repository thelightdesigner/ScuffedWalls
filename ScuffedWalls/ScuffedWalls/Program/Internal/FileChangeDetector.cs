using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ScuffedWalls
{
    class FileChangeDetector
    {
        public FileChangeDetector(FileInfo file)
        {
            _lastModifiedTime = File.GetLastWriteTime(file.FullName);
            _file = file;
        }
        private FileInfo _file;
        public DateTime _lastModifiedTime { get; set; }
        public bool HasChanged()
        {
            var thisModifiedTime = File.GetLastWriteTime(_file.FullName);
            if (thisModifiedTime != _lastModifiedTime)
            {
                _lastModifiedTime = thisModifiedTime;
                return true;
            }
            return false;
        }
        public static void WaitForChange(IEnumerable<FileChangeDetector> files)
        {
            while (files.All(file => !file.HasChanged())) { Task.Delay(500); }
        }
    }
}
