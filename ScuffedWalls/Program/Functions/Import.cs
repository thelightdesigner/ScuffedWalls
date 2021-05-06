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
        public override void Run()
        {
            string Path = GetParam("path", string.Empty, p => Utils.ScuffedConfig.MapFolderPath + @"\" + p);
            Path = GetParam("fullpath", DefaultValue: Path, p => p);
            int[] Type = GetParam("type", new int[] { 0, 1, 2, 3, 4 }, p => p.Split(",").Select(a => Convert.ToInt32(a)).ToArray());
            float startbeat = Time;
            float addtime = GetParam("addtime", 0, p => float.Parse(p));
            float endbeat = GetParam("tobeat", float.PositiveInfinity, p => float.Parse(p));


            BeatMap beatMap = JsonSerializer.Deserialize<BeatMap>(File.ReadAllText(Path));
            beatMap._customData ??= new BeatMap.CustomData();
            beatMap._customData._customEvents ??= new BeatMap.CustomData.CustomEvent[] { };
            if (Type.Any(t => t == 0))
            {
                InstanceWorkspace.Walls.AddRange(beatMap._obstacles.Cast<ITimeable>().GetAllBetween(startbeat, endbeat).Select(o => { o._time = o._time.toFloat() + addtime; return o; }).Cast<BeatMap.Obstacle>());
                ConsoleOut("Wall", beatMap._obstacles.GetAllBetween(startbeat, endbeat).Count(), Time, "Import");
            }
            if (Type.Any(t => t == 1))
            {
                InstanceWorkspace.Notes.AddRange(beatMap._notes.Cast<ITimeable>().GetAllBetween(startbeat, endbeat).Select(o => { o._time = o._time.toFloat() + addtime; return o; }).Cast<BeatMap.Note>());
                ConsoleOut("Note", beatMap._notes.GetAllBetween(startbeat, endbeat).Count(), Time, "Import");
            }
            if (Type.Any(t => t == 2))
            {
                InstanceWorkspace.Lights.AddRange(beatMap._events.Cast<ITimeable>().GetAllBetween(startbeat, endbeat).Select(o => { o._time = o._time.toFloat() + addtime; return o; }).Cast<BeatMap.Event>());
                ConsoleOut("Light", beatMap._events.GetAllBetween(startbeat, endbeat).Count(), Time, "Import");
            }
            if (Type.Any(t => t == 3) && beatMap._customData != null)
            {
                if (beatMap._customData._customEvents != null)
                {
                    InstanceWorkspace.CustomEvents.AddRange(beatMap._customData._customEvents.Cast<ITimeable>().GetAllBetween(startbeat, endbeat).Select(o => { o._time = o._time.toFloat() + addtime; return o; }).Cast<BeatMap.CustomData.CustomEvent>());
                    ConsoleOut("CustomEvent", beatMap._customData._customEvents.GetAllBetween(startbeat, endbeat).Count(), Time, "Import");
                }
                if (beatMap._customData._pointDefinitions != null)
                {
                    InstanceWorkspace.PointDefinitions.AddRange(beatMap._customData._pointDefinitions);
                    ConsoleOut("PointDefinition", beatMap._customData._pointDefinitions.Length, Time, "Import");
                }
                if (beatMap._customData._environment != null)
                {
                    InstanceWorkspace.Environment.AddRange(beatMap._customData._environment);
                    ConsoleOut("Environment", beatMap._customData._environment.Length, Time, "Import");
                }
            }
            if (Type.Any(t => t == 4) && beatMap._customData != null && beatMap._customData._bookmarks != null)
            {
                InstanceWorkspace.Bookmarks.AddRange(beatMap._customData._bookmarks);
                ConsoleOut("Bookmark", beatMap._customData._bookmarks.Length, Time, "Import");
            }
            Parameter.ExternalVariables.RefreshAllParameters();
        }


    }
}
