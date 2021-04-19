using ModChart;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace ScuffedWalls.Functions
{
    [ScuffedFunction("CloneFromWorkspace","CloneFromWorkspaceByIndex", "CloneWorkspace")]
    class CloneWorkspace : SFunction
    {
        public void Run()
        {
            int[] Type =  GetParam("type", new int[] { 0, 1, 2, 3 }, p => p.Split(",").Select(a => Convert.ToInt32(a)).ToArray());
            string name = GetParam("name", string.Empty, p => p);
            int Index = GetParam("index", 0, p => int.Parse(p));
            float startbeat = Time;
            float endbeat = GetParam("tobeat", float.PositiveInfinity, p => float.Parse(p));
            float addtime = GetParam("addtime", 0, p => float.Parse(p));

            BeatMap beatMap = null;
            beatMap = FunctionParser.Workspaces[Index].DeepClone().toBeatMap();

            if (name != string.Empty)
            {
                beatMap = FunctionParser.Workspaces.Where(w => w.Name == name).First().DeepClone().toBeatMap();
            }
            beatMap._customData._customEvents ??= new BeatMap.CustomData.CustomEvent[] { };
            if (Type.Any(t => t == 0))
            {
                InstanceWorkspace.Walls.AddRange(beatMap._obstacles.Cast<ITimeable>().GetAllBetween(startbeat, endbeat).Select(o => { o._time = o._time.toFloat() + addtime; return o; }).Cast<BeatMap.Obstacle>());
                ConsoleOut("Wall", beatMap._obstacles.Cast<ITimeable>().GetAllBetween(startbeat, endbeat).Count(), Time, "CloneWorkspace");
            }
            if (Type.Any(t => t == 1))
            {
                InstanceWorkspace.Notes.AddRange(beatMap._notes.Cast<ITimeable>().GetAllBetween(startbeat, endbeat).Select(o => { o._time = o._time.toFloat() + addtime; return o; }).Cast<BeatMap.Note>());
                ConsoleOut("Note", beatMap._notes.Cast<ITimeable>().GetAllBetween(startbeat, endbeat).Count(), Time, "CloneWorkspace");
            }
            if (Type.Any(t => t == 2))
            {
                InstanceWorkspace.Lights.AddRange(beatMap._events.Cast<ITimeable>().GetAllBetween(startbeat, endbeat).Select(o => { o._time = o._time.toFloat() + addtime; return o; }).Cast<BeatMap.Event>());
                ConsoleOut("Light", beatMap._events.Cast<ITimeable>().GetAllBetween(startbeat, endbeat).Count(), Time, "CloneWorkspace");
            }
            if (Type.Any(t => t == 3))
            {
                InstanceWorkspace.CustomEvents.AddRange(beatMap._customData._customEvents.Cast<ITimeable>().GetAllBetween(startbeat, endbeat).Select(o => { o._time = o._time.toFloat() + addtime; return o; }).Cast<BeatMap.CustomData.CustomEvent>());
                ConsoleOut("CustomEvent", beatMap._customData._customEvents.Cast<ITimeable>().GetAllBetween(startbeat, endbeat).Count(), Time, "CloneWorkspace");
            }


            Parameter.ExternalVariables.RefreshAllParameters();
        }

    }
}
