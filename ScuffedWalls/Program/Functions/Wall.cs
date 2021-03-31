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
            var parsedthing = Parameters.CustomDataParse(new BeatMap.Obstacle());
            bool isNjs = parsedthing != null && parsedthing._customData._noteJumpStartBeatOffset != null;

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
                        if (isNjs) Startup.bpmAdjuster.GetDefiniteDurationBeats(p.Data.toFloat(), parsedthing._customData._noteJumpStartBeatOffset.toFloat());
                        break;
                    case "definitetime":
                        if (p.Data.ToLower().RemoveWhiteSpace() == "beats")
                        {
                            if (isNjs) Time = Startup.bpmAdjuster.GetPlaceTimeBeats(Time, parsedthing._customData._noteJumpStartBeatOffset.toFloat());
                            else Time = Startup.bpmAdjuster.GetPlaceTimeBeats(Time);
                        }
                        else if (p.Data.ToLower().RemoveWhiteSpace() == "seconds")
                        {
                            if (isNjs) Time = Startup.bpmAdjuster.GetPlaceTimeBeats(Startup.bpmAdjuster.ToBeat(Time), parsedthing._customData._noteJumpStartBeatOffset.toFloat());
                            else Time = Startup.bpmAdjuster.GetPlaceTimeBeats(Startup.bpmAdjuster.ToBeat(Time));
                        }
                        break;
                    case "definitedurationseconds":
                        duration = Startup.bpmAdjuster.GetDefiniteDurationBeats(Startup.bpmAdjuster.ToBeat(p.Data.toFloat()));
                        if (isNjs) duration = Startup.bpmAdjuster.GetDefiniteDurationBeats(Startup.bpmAdjuster.ToBeat(p.Data.toFloat()), parsedthing._customData._noteJumpStartBeatOffset.toFloat());
                        break;
                }
            }
            for (float i = 0; i < repeatcount; i++)
            {
                InstanceWorkspace.Walls.Add((BeatMap.Obstacle)new BeatMap.Obstacle()
                {
                    _time = Time + (i * repeatTime),
                    _duration = duration,
                    _lineIndex = 0,
                    _width = 0,
                    _type = 0
                }.Append(Parameters.CustomDataParse(new BeatMap.Obstacle()),AppendTechnique.Overwrites));
                Repeat.Data = i.ToString();
                Beat.Data = (Time + (i * repeatTime)).ToString();
            }

            ConsoleOut("Wall", repeatcount, Time, "Wall");
        }
    }


}
