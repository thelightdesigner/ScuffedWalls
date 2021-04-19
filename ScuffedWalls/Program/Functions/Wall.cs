using ModChart;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScuffedWalls.Functions
{
    [ScuffedFunction("Wall")]
    class Wall : SFunction
    {
        public Parameter Repeat; 
        public Parameter Beat;
        public void SetParameters()
        {
            Repeat = new Parameter ("repeat", "1");
            Beat = new Parameter ("time", Time.ToString());
            Parameters.SetInteralVariables(new Parameter[] { Repeat, Beat });
        }
        public void Run()
        {
            SetParameters();

            var parsedthing = Parameters.CustomDataParse(new BeatMap.Obstacle());
            bool isNjs = parsedthing != null && parsedthing._customData != null && parsedthing._customData._noteJumpStartBeatOffset != null;

            float duration = GetParam("duration", 0, p => float.Parse(p));
            int repeatcount = GetParam("repeat", 1, p => int.Parse(p));
            float repeatTime = GetParam("repeataddtime", 0, p => float.Parse(p));

            duration = GetParam("definiteduration", duration, p =>
            {
                if (isNjs) return Utils.bpmAdjuster.GetDefiniteDurationBeats(p.toFloat(), parsedthing._customData._noteJumpStartBeatOffset.toFloat());
                else return Utils.bpmAdjuster.GetDefiniteDurationBeats(p.toFloat());
            });
            Time = GetParam("definitetime", Time, p =>
            {
                if (p.ToLower().RemoveWhiteSpace() == "beats")
                {
                    if (isNjs) return Utils.bpmAdjuster.GetPlaceTimeBeats(Time, parsedthing._customData._noteJumpStartBeatOffset.toFloat());
                    else return Utils.bpmAdjuster.GetPlaceTimeBeats(Time);
                }
                else if (p.ToLower().RemoveWhiteSpace() == "seconds")
                {
                    if (isNjs) return Utils.bpmAdjuster.GetPlaceTimeBeats(Utils.bpmAdjuster.ToBeat(Time), parsedthing._customData._noteJumpStartBeatOffset.toFloat());
                    else return Utils.bpmAdjuster.GetPlaceTimeBeats(Utils.bpmAdjuster.ToBeat(Time));
                }
                return Time;
            });
            duration = GetParam("definitedurationseconds", duration, p =>
            {
                if (isNjs) return Utils.bpmAdjuster.GetDefiniteDurationBeats(Utils.bpmAdjuster.ToBeat(p.toFloat()), parsedthing._customData._noteJumpStartBeatOffset.toFloat());
                return Utils.bpmAdjuster.GetDefiniteDurationBeats(Utils.bpmAdjuster.ToBeat(p.toFloat()));
            });
            duration = GetParam("definitedurationbeats", duration, p =>
            {
                if (isNjs) return Utils.bpmAdjuster.GetDefiniteDurationBeats(p.toFloat(), parsedthing._customData._noteJumpStartBeatOffset.toFloat());
                return Utils.bpmAdjuster.GetDefiniteDurationBeats(p.toFloat());
            });

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
                Repeat.StringData = i.ToString();
                Beat.StringData = (Time + (i * repeatTime)).ToString();
                Parameter.ExternalVariables.RefreshAllParameters();
            }

            ConsoleOut("Wall", repeatcount, Time, "Wall");
        }
    }


}
