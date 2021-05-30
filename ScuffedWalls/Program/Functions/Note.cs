using ModChart;

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
        public override void Run()
        {
            SetParameters();


            int repeatcount = GetParam("repeat", 1, p => int.Parse(p));
            float repeatTime = GetParam("repeataddtime", 0, p => float.Parse(p));
            int type = GetParam("type", 1, p => int.Parse(p));
            int cutdirection = GetParam("cutdirection", 0, p => int.Parse(p));
            float? njsoffset = GetParam("definitedurationseconds", null, p =>
            {
                return (float?)Utils.bpmAdjuster.GetDefiniteNjsOffsetBeats(Utils.bpmAdjuster.ToBeat(p.ToFloat()));
            });
            njsoffset = GetParam("definitedurationbeats", njsoffset, p =>
            {
                return (float?)Utils.bpmAdjuster.GetDefiniteNjsOffsetBeats(p.ToFloat());
            });

            Time = GetParam("definitetime", Time, p =>
            {
                if (p.ToLower().RemoveWhiteSpace() == "beats")
                {
                    if (njsoffset.HasValue) return Utils.bpmAdjuster.GetPlaceTimeBeats(Time, njsoffset.Value);
                    else return Utils.bpmAdjuster.GetPlaceTimeBeats(Time);
                }
                else if (p.ToLower().RemoveWhiteSpace() == "seconds")
                {
                    if (njsoffset.HasValue) return Utils.bpmAdjuster.GetPlaceTimeBeats(Utils.bpmAdjuster.ToBeat(Time), njsoffset.Value);
                    else return Utils.bpmAdjuster.GetPlaceTimeBeats(Utils.bpmAdjuster.ToBeat(Time));
                }
                return Time;
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
                    _type = type,
                    _customData = njsoffset.HasValue ? new TreeDictionary() { ["_noteJumpStartBeatOffset"] = njsoffset} : null
                }.Append(Parameters.CustomDataParse(new BeatMap.Note()), AppendPriority.High));

                Repeat.StringData = i.ToString();
                Beat.StringData = (Time + (i * repeatTime)).ToString();
                Parameter.ExternalVariables.RefreshAllParameters();
            }
            ConsoleOut("Note", repeatcount, Time, "Note");
        }
    }


}
