using ModChart;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScuffedWalls.Functions
{
    [ScuffedFunction("Blackout")]
    class Blackout : SFunction
    {
        public override void Run()
        {
            ConsoleOut("Light", 1, Time, "Blackout");
            InstanceWorkspace.Lights.Add(new BeatMap.Event() { _time = Time, _type = 0, _value = 0 });
            Parameter.ExternalVariables.RefreshAllParameters();
        }
    }


}
