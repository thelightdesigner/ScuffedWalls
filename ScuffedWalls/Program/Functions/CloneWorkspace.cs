using ModChart;
using System;
using System.Linq;
using System.Collections.Generic;

namespace ScuffedWalls.Functions
{
    [ScuffedFunction("CloneFromWorkspace","CloneFromWorkspaceByIndex", "CloneWorkspace")]
    class CloneWorkspace : SFunction
    {
        public override void Run()
        {
            int[] Type =  GetParam("type", new int[] { 0, 1, 2, 3 }, p => p.Split(",").Select(a => Convert.ToInt32(a)).ToArray());
            string name = GetParam("name", string.Empty, p => p);
            int Index = GetParam("index", 0, p => int.Parse(p));
            float startbeat = Time;
            float endbeat = GetParam("tobeat", float.PositiveInfinity, p => float.Parse(p));
            float addtime = GetParam("addtime", 0, p => float.Parse(p));

            Workspace cloned = null;

            cloned = (Workspace)FunctionParser.Workspaces[Index].Clone();
            if (name != string.Empty) cloned = (Workspace)FunctionParser.Workspaces.Where(w => w.Name == name).First().Clone();
            
            if (Type.Any(t => t == 0))
            {
                InstanceWorkspace.Walls.AddRange(cloned.Walls.Cast<ITimeable>().GetAllBetween(startbeat, endbeat).Select(o => { o._time = o._time.ToFloat() + addtime; return o; }).Cast<BeatMap.Obstacle>());
                ConsoleOut("_obstacle", cloned.Walls.Cast<ITimeable>().GetAllBetween(startbeat, endbeat).Count(), Time, "CloneWorkspace");
            }
            if (Type.Any(t => t == 1))
            {
                InstanceWorkspace.Notes.AddRange(cloned.Notes.Cast<ITimeable>().GetAllBetween(startbeat, endbeat).Select(o => { o._time = o._time.ToFloat() + addtime; return o; }).Cast<BeatMap.Note>());
                ConsoleOut("_note", cloned.Notes.Cast<ITimeable>().GetAllBetween(startbeat, endbeat).Count(), Time, "CloneWorkspace");
            }
            if (Type.Any(t => t == 2))
            {
                InstanceWorkspace.Lights.AddRange(cloned.Lights.Cast<ITimeable>().GetAllBetween(startbeat, endbeat).Select(o => { o._time = o._time.ToFloat() + addtime; return o; }).Cast<BeatMap.Event>());
                ConsoleOut("_event", cloned.Lights.Cast<ITimeable>().GetAllBetween(startbeat, endbeat).Count(), Time, "CloneWorkspace");
            }
            if (Type.Any(t => t == 3))
            {
                InstanceWorkspace.CustomData = (TreeDictionary)TreeDictionary.Merge(InstanceWorkspace.CustomData, cloned.CustomData, TreeDictionary.MergeType.Arrays, TreeDictionary.MergeBindingFlags.HasValue);
                
                foreach (var item in cloned.CustomData) if (item.Value is IEnumerable<object> array) ConsoleOut($"_customData.{item.Key}", array.Count(), Time, "CloneWorkspace");
            }


            Parameter.ExternalVariables.RefreshAllParameters();
        }

    }
}
