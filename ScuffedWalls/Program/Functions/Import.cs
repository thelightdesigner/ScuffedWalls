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
            FunLog();


            string Path = GetParam("path", string.Empty, p => System.IO.Path.Combine(Utils.ScuffedConfig.MapFolderPath, p));
            Path = GetParam("fullpath", DefaultValue: Path, p => p);
            int[] Type = GetParam("type", new int[] { 0, 1, 2, 3, 4, 5 }, p => p.Split(",").Select(a => Convert.ToInt32(a)).ToArray());
            float startbeat = Time;
            float addtime = GetParam("addtime", 0, p => float.Parse(p));
            float endbeat = GetParam("tobeat", float.PositiveInfinity, p => float.Parse(p));

            
            BeatMap beatMap = JsonSerializer.Deserialize<BeatMap>(File.ReadAllText(Path), Utils.DefaultJsonConverterSettings);
            if (Type.Any(t => t == 0))
            {
                var filtered = beatMap._obstacles.Cast<ITimeable>().GetAllBetween(startbeat, endbeat).Select(o => { o._time = o._time.ToFloat() + addtime; return o; }).Cast<BeatMap.Obstacle>();
                InstanceWorkspace.Walls.AddRange(filtered);
                if (filtered.Count() > 0) ConsoleOut("_obstacle", filtered.Count(), Time, "Import");
            }
            if (Type.Any(t => t == 1))
            {
                var filtered = beatMap._notes.Cast<ITimeable>().GetAllBetween(startbeat, endbeat).Select(o => { o._time = o._time.ToFloat() + addtime; return o; }).Cast<BeatMap.Note>();
                InstanceWorkspace.Notes.AddRange(filtered);
                if (filtered.Count() > 0) ConsoleOut("_note", filtered.Count(), Time, "Import");
            }
            if (Type.Any(t => t == 2))
            {
                var filtered = beatMap._events.Cast<ITimeable>().GetAllBetween(startbeat, endbeat).Select(o => { o._time = o._time.ToFloat() + addtime; return o; }).Cast<BeatMap.Event>();
                InstanceWorkspace.Lights.AddRange(filtered);
                if (filtered.Count() > 0) ConsoleOut("_event", filtered.Count(), Time, "Import");
            }
            if (Type.Any(t => t == 3))
            {
                InstanceWorkspace.CustomData = (TreeDictionary)TreeDictionary.Merge(InstanceWorkspace.CustomData, beatMap._customData, TreeDictionary.MergeType.Arrays, TreeDictionary.MergeBindingFlags.HasValue);

                foreach (var item in beatMap._customData) if (item.Value is IEnumerable<object> array && array.Count() > 0) ConsoleOut($"_customData.{item.Key}", array.Count(), Time, "Import");
            }

            Parameter.ExternalVariables.RefreshAllParameters();
        }


    }
}
