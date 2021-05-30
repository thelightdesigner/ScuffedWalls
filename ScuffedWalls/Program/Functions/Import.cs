using ModChart;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace ScuffedWalls.Functions
{
    [ScuffedFunction("Import")]
    class Import : SFunction
    {
        public override void Run()
        {
            string Path = GetParam("path", string.Empty, p => System.IO.Path.Combine(Utils.ScuffedConfig.MapFolderPath, p));
            Path = GetParam("fullpath", DefaultValue: Path, p => p);
            int[] Type = GetParam("type", new int[] { 0, 1, 2, 3, 4, 5 }, p => p.Split(",").Select(a => Convert.ToInt32(a)).ToArray());
            float startbeat = Time;
            float addtime = GetParam("addtime", 0, p => float.Parse(p));
            float endbeat = GetParam("tobeat", float.PositiveInfinity, p => float.Parse(p));

            
            BeatMap beatMap = JsonSerializer.Deserialize<BeatMap>(File.ReadAllText(Path), Utils.DefaultJsonConverterSettings);
            if (Type.Any(t => t == 0))
            {
                InstanceWorkspace.Walls.AddRange(beatMap._obstacles.Cast<ITimeable>().GetAllBetween(startbeat, endbeat).Select(o => { o._time = o._time.ToFloat() + addtime; return o; }).Cast<BeatMap.Obstacle>());
                ConsoleOut("_obstacle", beatMap._obstacles.GetAllBetween(startbeat, endbeat).Count(), Time, "Import");
            }
            if (Type.Any(t => t == 1))
            {
                InstanceWorkspace.Notes.AddRange(beatMap._notes.Cast<ITimeable>().GetAllBetween(startbeat, endbeat).Select(o => { o._time = o._time.ToFloat() + addtime; return o; }).Cast<BeatMap.Note>());
                ConsoleOut("_note", beatMap._notes.GetAllBetween(startbeat, endbeat).Count(), Time, "Import");
            }
            if (Type.Any(t => t == 2))
            {
                InstanceWorkspace.Lights.AddRange(beatMap._events.Cast<ITimeable>().GetAllBetween(startbeat, endbeat).Select(o => { o._time = o._time.ToFloat() + addtime; return o; }).Cast<BeatMap.Event>());
                ConsoleOut("_event", beatMap._events.GetAllBetween(startbeat, endbeat).Count(), Time, "Import");
            }
            if (Type.Any(t => t == 3) && beatMap._customData != null)
            {
                InstanceWorkspace.CustomData = (TreeDictionary)TreeDictionary.Merge(InstanceWorkspace.CustomData, beatMap._customData, TreeDictionary.MergeType.Arrays, TreeDictionary.MergeBindingFlags.HasValue);

                foreach (var item in beatMap._customData) if (item.Value is IEnumerable<object> array) ConsoleOut($"_customData.{item.Key}", array.Count(), Time, "Import");
                
            }

            Parameter.ExternalVariables.RefreshAllParameters();
        }


    }
}
