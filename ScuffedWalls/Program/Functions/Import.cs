using ModChart;
using System;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace ScuffedWalls.Functions
{
    [SFunction("Import")]
    class Import : ScuffedFunction
    {
        string Path;
        int[] Type;
        float startbeat;
        float addtime;
        float endbeat;
        protected override void Init()
        {
            Path = GetParam("path", string.Empty, p => System.IO.Path.Combine(Utils.ScuffedConfig.MapFolderPath, p));
            Path = GetParam("fullpath", DefaultValue: Path, p => p);
            AddRefresh(Path); 
            Type = GetParam("type", new int[] { 0, 1, 2, 3, 4, 5 }, p => p.Split(",").Select(a => Convert.ToInt32(a)).ToArray()); 
            startbeat = Time;
            addtime = GetParam("addtime", 0, p => float.Parse(p));
            endbeat = GetParam("tobeat", float.PositiveInfinity, p => float.Parse(p));

            BeatMap beatMap = JsonSerializer.Deserialize<BeatMap>(File.ReadAllText(Path), Utils.DefaultJsonConverterSettings);
            BeatMap filtered = new BeatMap();

            if (beatMap._obstacles != null && beatMap._obstacles.Any() && Type.Any(t => t == 0))
            {
                filtered._obstacles.AddRange(beatMap._obstacles.Cast<ITimeable>().GetAllBetween(startbeat, endbeat).Select(o => { o._time = o._time.ToFloat() + addtime; return o; }).Cast<BeatMap.Obstacle>());

            }
            if (beatMap._notes != null && beatMap._notes.Any() && Type.Any(t => t == 1))
            {
                filtered._notes.AddRange(beatMap._notes.Cast<ITimeable>().GetAllBetween(startbeat, endbeat).Select(o => { o._time = o._time.ToFloat() + addtime; return o; }).Cast<BeatMap.Note>());

            }
            if (beatMap._events != null && beatMap._events.Any() && Type.Any(t => t == 2))
            {
                filtered._events.AddRange(beatMap._events.Cast<ITimeable>().GetAllBetween(startbeat, endbeat).Select(o => { o._time = o._time.ToFloat() + addtime; return o; }).Cast<BeatMap.Event>());

            }
            if (beatMap._customData != null && Type.Any(t => t == 3))
            {
                TreeDictionary.Merge(InstanceWorkspace.CustomData, beatMap._customData, TreeDictionary.MergeType.Arrays, TreeDictionary.MergeBindingFlags.HasValue);
            }
            Stats.AddStats(filtered.Stats);
            InstanceWorkspace.Add(filtered);
        }
    }
}
