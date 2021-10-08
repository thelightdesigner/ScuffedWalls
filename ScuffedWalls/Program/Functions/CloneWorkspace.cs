using ModChart;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ScuffedWalls.Functions
{
    /*
    [SFunction("CloneFromWorkspace", "CloneFromWorkspaceByIndex", "CloneWorkspace")]
    class CloneWorkspace : ScuffedFunction
    {
        public override void Run()
        {
            FunLog();


            int[] Type = GetParam("type", new int[] { 0, 1, 2, 3 }, p => p.Split(",").Select(a => Convert.ToInt32(a)).ToArray());
            string name = GetParam("name", string.Empty, p => p);
            int Index = GetParam("index", 0, p => int.Parse(p));
            float startbeat = Time;
            float endbeat = GetParam("tobeat", float.PositiveInfinity, p => float.Parse(p));
            float addtime = GetParam("addtime", 0, p => float.Parse(p));

            Workspace cloned = null;

            cloned = (Workspace)InstanceWorkspaceRequest.Workspaces[Index].Clone();
            if (name != string.Empty) cloned = (Workspace)InstanceWorkspaceRequest.Workspaces.Where(w => w.Name == name).First().Clone();

            if (Type.Any(t => t == 0))
            {
                var filtered = cloned.Walls.Cast<ITimeable>().GetAllBetween(startbeat, endbeat).Select(o => { o._time = o._time.ToFloat() + addtime; return o; }).Cast<BeatMap.Obstacle>();
                InstanceWorkspace.Walls.AddRange(filtered);
                if (filtered.Count() > 0) ConsoleOut("_obstacle", filtered.Count(), Time, "CloneWorkspace");
            }
            if (Type.Any(t => t == 1))
            {
                var filtered = cloned.Notes.Cast<ITimeable>().GetAllBetween(startbeat, endbeat).Select(o => { o._time = o._time.ToFloat() + addtime; return o; }).Cast<BeatMap.Note>();
                InstanceWorkspace.Notes.AddRange(filtered);
                if (filtered.Count() > 0) ConsoleOut("_note", filtered.Count(), Time, "CloneWorkspace");
            }
            if (Type.Any(t => t == 2))
            {
                var filtered = cloned.Lights.Cast<ITimeable>().GetAllBetween(startbeat, endbeat).Select(o => { o._time = o._time.ToFloat() + addtime; return o; }).Cast<BeatMap.Event>();
                InstanceWorkspace.Lights.AddRange(filtered);
                if (filtered.Count() > 0) ConsoleOut("_event", filtered.Count(), Time, "CloneWorkspace");
            }
            if (Type.Any(t => t == 3))
            {
                InstanceWorkspace.CustomData = (TreeDictionary)
                    TreeDictionary.Merge(
                    InstanceWorkspace.CustomData,
                    cloned.CustomData,
                    TreeDictionary.MergeType.Arrays,
                    TreeDictionary.MergeBindingFlags.HasValue);
                

                foreach (var item in cloned.CustomData) if (item.Value is IEnumerable<object> array && array.Count() > 0) ConsoleOut($"_customData.{item.Key}", array.Count(), Time, "CloneWorkspace");
            }


            Parameter.ExternalVariables.RefreshAllParameters();
        }

    }
    */
}
