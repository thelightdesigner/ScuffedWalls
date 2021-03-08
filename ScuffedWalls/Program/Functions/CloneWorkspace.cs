using ModChart;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace ScuffedWalls.Functions
{
    [ScuffedFunction("CloneFromWorkspace")]
    class CloneWorkspace : SFunction
    {
        public void Run()
        {
            int[] Type = { 0, 1, 2, 3 };
            string name = string.Empty;
            int Index = 0;
            float startbeat = Time;
            float endbeat = 1000000;
            float addtime = 0;

            foreach (var p in Parameters)
            {
                switch (p.Name)
                {
                    case "index":
                        Index = Convert.ToInt32(p.Data);
                        break;
                    case "name":
                        name = p.Data;
                        break;
                    case "type":
                        Type = p.Data.Split(",").Select(a => Convert.ToInt32(a)).ToArray();
                        break;
                    case "tobeat":
                        endbeat = Convert.ToSingle(p.Data);
                        break;
                    case "addtime":
                        addtime = Convert.ToSingle(p.Data);
                        break;
                }
            }
            BeatMap beatMap = null;
            beatMap = FunctionParser.Workspaces[Index].DeepClone().toBeatMap();

            if (name != string.Empty)
            {
                beatMap = FunctionParser.Workspaces.Where(w => w.Name == name).First().DeepClone().toBeatMap();
            }
            beatMap._customData._customEvents ??= new BeatMap.CustomData.CustomEvents[] { };
            if (Type.Any(t => t == 0))
            {
                InstanceWorkspace.Walls.AddRange(beatMap._obstacles.GetAllBetween(startbeat, endbeat).Select(o => { o._time = o._time.toFloat() + addtime; return o; }));
                ConsoleOut("Wall", beatMap._obstacles.GetAllBetween(startbeat, endbeat).Length, Time, "CloneWorkspace");
            }
            if (Type.Any(t => t == 1))
            {
                InstanceWorkspace.Notes.AddRange(beatMap._notes.GetAllBetween(startbeat, endbeat).Select(o => { o._time = o._time.toFloat() + addtime; return o; }));
                ConsoleOut("Note", beatMap._notes.GetAllBetween(startbeat, endbeat).Length, Time, "CloneWorkspace");
            }
            if (Type.Any(t => t == 2))
            {
                InstanceWorkspace.Lights.AddRange(beatMap._events.GetAllBetween(startbeat, endbeat).Select(o => { o._time = o._time.toFloat() + addtime; return o; }));
                ConsoleOut("Light", beatMap._events.GetAllBetween(startbeat, endbeat).Length, Time, "CloneWorkspace");
            }
            if (Type.Any(t => t == 3))
            {
                InstanceWorkspace.CustomEvents.AddRange(beatMap._customData._customEvents.GetAllBetween(startbeat, endbeat).Select(o => { o._time = o._time.toFloat() + addtime; return o; }));
                ConsoleOut("CustomEvent", beatMap._customData._customEvents.GetAllBetween(startbeat, endbeat).Length, Time, "CloneWorkspace");
            }
        }



    }
}
