using ModChart;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace ScuffedWalls.Functions
{
    [ScuffedFunction("Import")]
    class Import : SFunction
    {
        public void Run()
        {
            string Path = string.Empty;
            int[] Type = { 0, 1, 2, 3, 4 };
            float startbeat = 0;
            float addtime = 0;
            float endbeat = 1000000;

            foreach (var p in Parameters)
            {
                switch (p.Name)
                {
                    case "path":
                        Path = Startup.ScuffedConfig.MapFolderPath + @"\" + p.Data.RemoveWhiteSpace();
                        break;
                    case "fullpath":
                        Path = p.Data;
                        break;
                    case "type":
                        Type = p.Data.Split(",").Select(a => Convert.ToInt32(a)).ToArray();
                        break;
                    case "frombeat":
                        startbeat = Convert.ToSingle(p.Data);
                        break;
                    case "addtime":
                        addtime = Convert.ToSingle(p.Data);
                        break;
                    case "tobeat":
                        endbeat = Convert.ToSingle(p.Data);
                        break;
                }
            }

            BeatMap beatMap = JsonSerializer.Deserialize<BeatMap>(File.ReadAllText(Path));
            beatMap._customData ??= new BeatMap.CustomData();
            beatMap._customData._customEvents ??= new BeatMap.CustomData.CustomEvents[] { };
            if (Type.Any(t => t == 0))
            {
                InstanceWorkspace.Walls.AddRange(beatMap._obstacles.GetAllBetween(startbeat, endbeat).Select(o => { o._time = o._time.toFloat() + addtime; return o; }));
                ConsoleOut("Wall", beatMap._obstacles.GetAllBetween(startbeat, endbeat).Length, Time, "Import");
            }
            if (Type.Any(t => t == 1))
            {
                InstanceWorkspace.Notes.AddRange(beatMap._notes.GetAllBetween(startbeat, endbeat).Select(o => { o._time = o._time.toFloat() + addtime; return o; }));
                ConsoleOut("Note", beatMap._notes.GetAllBetween(startbeat, endbeat).Length, Time, "Import");
            }
            if (Type.Any(t => t == 2))
            {
                InstanceWorkspace.Lights.AddRange(beatMap._events.GetAllBetween(startbeat, endbeat).Select(o => { o._time = o._time.toFloat() + addtime; return o; }));
                ConsoleOut("Light", beatMap._events.GetAllBetween(startbeat, endbeat).Length, Time, "Import");
            }
            if (Type.Any(t => t == 3) && beatMap._customData != null)
            {
                if (beatMap._customData._customEvents != null)
                {
                    InstanceWorkspace.CustomEvents.AddRange(beatMap._customData._customEvents.GetAllBetween(startbeat, endbeat).Select(o => { o._time = o._time.toFloat() + addtime; return o; }));
                    ConsoleOut("CustomEvent", beatMap._customData._customEvents.GetAllBetween(startbeat, endbeat).Length, Time, "Import");
                }
                if (beatMap._customData._pointDefinitions != null)
                {
                    InstanceWorkspace.PointDefinitions.AddRange(beatMap._customData._pointDefinitions);
                    ConsoleOut("PointDefinition", beatMap._customData._pointDefinitions.Length, Time, "Import");
                }
            }
            if (Type.Any(t => t == 4) && beatMap._customData != null && beatMap._customData._bookmarks != null)
            {
                InstanceWorkspace.Bookmarks.AddRange(beatMap._customData._bookmarks);
                ConsoleOut("Bookmark", beatMap._customData._bookmarks.Length, Time, "Import");
            }
        }


    }
}
