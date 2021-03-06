using ModChart;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScuffedWalls.Functions
{
    [ScuffedFunction("Wall")]
    class Wall : SFunction
    {
        public Variable Repeat; 
        public Variable Beat;
        public void SetParameters()
        {
            Repeat = new Variable { Name = "repeat", Data = "1" };
            Beat = new Variable { Name = "time", Data = Time.ToString() };
            Parameters = Parameters.AddVariables(new Variable[] { Repeat, Beat });
        }
        public void Run()
        {
            SetParameters();
            float duration = 1;
            int repeatcount = 1;
            float repeatTime = 0;
            var customdata = Parameters.CustomDataParse();
            bool isNjs = customdata != null && customdata._noteJumpStartBeatOffset != null;

            foreach (var p in Parameters)
            {
                switch (p.Name)
                {
                    case "repeat":
                        repeatcount = Convert.ToInt32(p.Data);
                        break;
                    case "repeataddtime":
                        repeatTime = Convert.ToSingle(p.Data);
                        break;
                    case "duration":
                        duration = Convert.ToSingle(p.Data);
                        break;
                    case "definiteduration":
                        duration = Startup.bpmAdjuster.GetDefiniteDurationBeats(p.Data.toFloat());
                        if (isNjs) Startup.bpmAdjuster.GetDefiniteDurationBeats(p.Data.toFloat(), customdata._noteJumpStartBeatOffset.toFloat());
                        break;
                    case "definitetime":
                        if (p.Data.ToLower().removeWhiteSpace() == "beats")
                        {
                            if (isNjs) Time = Startup.bpmAdjuster.GetPlaceTimeBeats(Time, customdata._noteJumpStartBeatOffset.toFloat());
                            else Time = Startup.bpmAdjuster.GetPlaceTimeBeats(Time);
                        }
                        else if (p.Data.ToLower().removeWhiteSpace() == "seconds")
                        {
                            if (isNjs) Time = Startup.bpmAdjuster.GetPlaceTimeBeats(Startup.bpmAdjuster.ToBeat(Time), customdata._noteJumpStartBeatOffset.toFloat());
                            else Time = Startup.bpmAdjuster.GetPlaceTimeBeats(Startup.bpmAdjuster.ToBeat(Time));
                        }
                        break;
                    case "definitedurationseconds":
                        duration = Startup.bpmAdjuster.GetDefiniteDurationBeats(Startup.bpmAdjuster.ToBeat(p.Data.toFloat()));
                        if (isNjs) duration = Startup.bpmAdjuster.GetDefiniteDurationBeats(Startup.bpmAdjuster.ToBeat(p.Data.toFloat()), customdata._noteJumpStartBeatOffset.toFloat());
                        break;
                }
            }
            for (float i = 0; i < repeatcount; i++)
            {
                InstanceWorkspace.Walls.Add(new BeatMap.Obstacle()
                {
                    _time = Time + (i * repeatTime),
                    _duration = duration,
                    _lineIndex = 0,
                    _width = 0,
                    _type = 0,
                    _customData = Parameters.CustomDataParse()
                });
                Repeat.Data = i.ToString();
                Beat.Data = (Time + (i * repeatTime)).ToString();
            }

            ConsoleOut("Wall", repeatcount, Time, "Wall");
        }
    }


}
