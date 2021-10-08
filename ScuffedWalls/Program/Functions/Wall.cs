using ModChart;
using System;
using System.Text.Json;

namespace ScuffedWalls.Functions
{
    [SFunction("Wall")]
    class Wall : ScuffedFunction
    {
        public Parameter Repeat; 
        public Parameter Beat;
        public void SetParameters()
        {
            Repeat = new Parameter ("repeat", "0");
            Beat = new Parameter ("time", Time.ToString());
            UnderlyingParameters.SetInteralVariables(new Parameter[] { Repeat, Beat });
        }
        public override void Run()
        {
            SetParameters();

            var parsedthing = UnderlyingParameters.CustomDataParse(new BeatMap.Obstacle());
            bool isNjs = parsedthing != null && parsedthing._customData != null && parsedthing._customData["_noteJumpStartBeatOffset"] != null;

            float duration = GetParam("duration", 0, p => float.Parse(p));
            int repeatcount = GetParam("repeat", 1, p => int.Parse(p));
            float repeatTime = GetParam("repeataddtime", 0, p => float.Parse(p));
            int lineindex = GetParam("lineindex", 0, p => int.Parse(p));
            

            duration = GetParam("definiteduration", duration, p =>
            {
                if (isNjs) return Utils.BPMAdjuster.GetDefiniteDurationBeats(p.ToFloat(), parsedthing._customData["_noteJumpStartBeatOffset"].ToFloat());
                else return Utils.BPMAdjuster.GetDefiniteDurationBeats(p.ToFloat());
            });
            Time = GetParam("definitetime", Time, p =>
            {
                if (p.ToLower().RemoveWhiteSpace() == "beats")
                {
                    if (isNjs) return Utils.BPMAdjuster.GetPlaceTimeBeats(Time, parsedthing._customData["_noteJumpStartBeatOffset"].ToFloat());
                    else return Utils.BPMAdjuster.GetPlaceTimeBeats(Time);
                }
                else if (p.ToLower().RemoveWhiteSpace() == "seconds")
                {
                    if (isNjs) return Utils.BPMAdjuster.GetPlaceTimeBeats(Utils.BPMAdjuster.ToBeat(Time), parsedthing._customData["_noteJumpStartBeatOffset"].ToFloat());
                    else return Utils.BPMAdjuster.GetPlaceTimeBeats(Utils.BPMAdjuster.ToBeat(Time));
                }
                return Time;
            });
            duration = GetParam("definitedurationseconds", duration, p =>
            {
                if (isNjs) return Utils.BPMAdjuster.GetDefiniteDurationBeats(Utils.BPMAdjuster.ToBeat(p.ToFloat()), parsedthing._customData["_noteJumpStartBeatOffset"].ToFloat());
                return Utils.BPMAdjuster.GetDefiniteDurationBeats(Utils.BPMAdjuster.ToBeat(p.ToFloat()));
            });
            duration = GetParam("definitedurationbeats", duration, p =>
            {
                if (isNjs) return Utils.BPMAdjuster.GetDefiniteDurationBeats(p.ToFloat(), parsedthing._customData["_noteJumpStartBeatOffset"].ToFloat());
                return Utils.BPMAdjuster.GetDefiniteDurationBeats(p.ToFloat());
            });

            for (float i = 0; i < repeatcount; i++)
            {
                Repeat.StringData = i.ToString();
                Beat.StringData = (Time + (i * repeatTime)).ToString();

                FunLog();

                InstanceWorkspace.Walls.Add((BeatMap.Obstacle)new BeatMap.Obstacle()
                {
                    _time = Time + (i * repeatTime),
                    _duration = duration,
                    _lineIndex = GetParam("lineindex", 0, p => int.Parse(p)),
                    _width = 0,
                    _type = 0
                }.Append(UnderlyingParameters.CustomDataParse(new BeatMap.Obstacle()),AppendPriority.High));
                Parameter.ExternalVariables.RefreshAllParameters();
            }

            ConsoleOut("Wall", repeatcount, Time, "Wall");
        }
    }


}
