using ModChart;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScuffedWalls.Functions
{
    [ScuffedFunction("Note")]
    class Note : SFunction
    {
        public Parameter Repeat;
        public Parameter Beat;
        public void SetParameters()
        {
            Repeat = new Parameter ("repeat", "1");
            Beat = new Parameter ("time",Time.ToString());
            Parameters.SetInteralVariables(new Parameter[] { Repeat, Beat });
        }
        public void Run()
        {
            SetParameters();


            int repeatcount = GetParam("repeat", 1, p => int.Parse(p));
            float repeatTime = GetParam("repeataddtime", 0, p => float.Parse(p));
            int type = GetParam("type", 1, p => int.Parse(p));
            int cutdirection = GetParam("cutdirection", 0, p => int.Parse(p));
            float? njsoffset = GetParam("definiteduration", null, p => (float?)Utils.bpmAdjuster.GetDefiniteDurationBeats(p.toFloat()));


            var parsedcustomstuff = Parameters.CustomDataParse(new BeatMap.Obstacle());
            var isNjs = parsedcustomstuff._customData != null && parsedcustomstuff._customData._noteJumpStartBeatOffset != null && njsoffset != null;

            Time = GetParam("definitetime", Time, p =>
            {
                if (p.ToLower().RemoveWhiteSpace() == "beats")
                {
                    if (isNjs) return Utils.bpmAdjuster.GetPlaceTimeBeats(Time, parsedcustomstuff._customData._noteJumpStartBeatOffset.toFloat()) + (njsoffset ?? parsedcustomstuff._customData._noteJumpStartBeatOffset.toFloat());
                    else return Utils.bpmAdjuster.GetPlaceTimeBeats(Time);
                }
                else if (p.ToLower().RemoveWhiteSpace() == "seconds")
                {
                    if (isNjs) return Utils.bpmAdjuster.GetPlaceTimeBeats(Utils.bpmAdjuster.ToBeat(Time), parsedcustomstuff._customData._noteJumpStartBeatOffset.toFloat()) + (njsoffset ?? parsedcustomstuff._customData._noteJumpStartBeatOffset.toFloat());
                    else return Utils.bpmAdjuster.GetPlaceTimeBeats(Utils.bpmAdjuster.ToBeat(Time));
                }
                return Time;
            });
            njsoffset = GetParam("definitedurationseconds", njsoffset, p =>
            {
                if (isNjs) return Utils.bpmAdjuster.GetDefiniteDurationBeats(Utils.bpmAdjuster.ToBeat(p.toFloat()), parsedcustomstuff._customData._noteJumpStartBeatOffset.toFloat());
                return Utils.bpmAdjuster.GetDefiniteDurationBeats(Utils.bpmAdjuster.ToBeat(p.toFloat()));
            });
            njsoffset = GetParam("definitedurationbeats", njsoffset, p =>
            {
                if (isNjs) return Utils.bpmAdjuster.GetDefiniteDurationBeats(p.toFloat(), parsedcustomstuff._customData._noteJumpStartBeatOffset.toFloat());
                return Utils.bpmAdjuster.GetDefiniteDurationBeats(p.toFloat());
            });

            //parse special parameters
            for (float i = 0; i < repeatcount; i++)
            {
                InstanceWorkspace.Notes.Add((BeatMap.Note)new BeatMap.Note()
                {
                    _time = Time + (i * repeatTime),
                    _lineIndex = 0,
                    _lineLayer = 0,
                    _cutDirection = cutdirection,
                    _type = type
                }.Append(Parameters.CustomDataParse(new BeatMap.Note()), AppendTechnique.Overwrites));

                Repeat.StringData = i.ToString();
                Beat.StringData = (Time + (i * repeatTime)).ToString();
                Parameter.ExternalVariables.RefreshAllParameters();
            }
            ConsoleOut("Note", repeatcount, Time, "Note");
        }
    }


}
