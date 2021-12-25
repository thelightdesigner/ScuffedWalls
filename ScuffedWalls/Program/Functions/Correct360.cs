using ModChart;
using System;
using System.Collections.Generic;
using System.Linq;
using static ModChart.BeatMap;

// _value _rotation
// 0      -60
// 1      -45
// 2      -30
// 3      -15
// 4      15
// 5      30
// 6      45
// 7      60

namespace ScuffedWalls.Functions
{
    [SFunction("Correct360")]
    class Append360 : ScuffedFunction
    {
        public AssignableInlineVariable RotationEvent;
        float endbeat;
        protected override void Init()
        {
            RotationEvent = new AssignableInlineVariable("index", "0");
            Variables.Add(RotationEvent);

            endbeat = GetParam("tobeat", float.PositiveInfinity, p => float.Parse(p));
            float starttime = Time;
            VariablePopulator internalvars = new VariablePopulator();
            Variables.Register(internalvars.Properties);



        }

    }
}