using ModChart;
using System;
using System.Text.Json;

namespace ScuffedWalls.Functions
{
    [SFunction("Note")]
    class Note : ScuffedFunction
    {
        public Parameter Repeat;
        public Parameter Beat;
        public void SetParameters()
        {
            Repeat = new Parameter("repeat", "0");
            Beat = new Parameter("time", Time.ToString());
            UnderlyingParameters.SetInteralVariables(new Parameter[] { Repeat, Beat });
        }
        public override void Run()
        {
            SetParameters();


            int repeatcount = GetParam("repeat", 1, p => int.Parse(p));
            float repeatTime = GetParam("repeataddtime", 0, p => float.Parse(p));
            BeatMap.Note.NoteType type = GetParam("type", BeatMap.Note.NoteType.Right, p => (BeatMap.Note.NoteType)int.Parse(p));
            BeatMap.Note.CutDirection cutdirection = GetParam("notecutdirection", BeatMap.Note.CutDirection.Down, p => (BeatMap.Note.CutDirection)int.Parse(p));
            float? njsoffset = GetParam("definitedurationseconds", null, p =>
            {
                return (float?)Utils.BPMAdjuster.GetDefiniteNjsOffsetBeats(Utils.BPMAdjuster.ToBeat(p.ToFloat()));
            });
            njsoffset = GetParam("definitedurationbeats", njsoffset, p =>
            {
                return (float?)Utils.BPMAdjuster.GetDefiniteNjsOffsetBeats(p.ToFloat());
            });

            Time = GetParam("definitetime", Time, p =>
            {
                if (p.ToLower().RemoveWhiteSpace() == "beats")
                {
                    if (njsoffset.HasValue) return Utils.BPMAdjuster.GetPlaceTimeBeats(Time, njsoffset.Value);
                    else return Utils.BPMAdjuster.GetPlaceTimeBeats(Time);
                }
                else if (p.ToLower().RemoveWhiteSpace() == "seconds")
                {
                    if (njsoffset.HasValue) return Utils.BPMAdjuster.GetPlaceTimeBeats(Utils.BPMAdjuster.ToBeat(Time), njsoffset.Value);
                    else return Utils.BPMAdjuster.GetPlaceTimeBeats(Utils.BPMAdjuster.ToBeat(Time));
                }
                return Time;
            });
            //parse special parameters
            for (float i = 0; i < repeatcount; i++)
            {
                Repeat.StringData = i.ToString();
                Beat.StringData = (Time + (i * repeatTime)).ToString();

                FunLog();


                InstanceWorkspace.Notes.Add((BeatMap.Note)new BeatMap.Note()
                {
                    _time = Time + (i * repeatTime),
                    _lineIndex = 0,
                    _lineLayer = 0,
                    _cutDirection = cutdirection,
                    _type = type,
                    _customData = njsoffset.HasValue ? new TreeDictionary() { ["_noteJumpStartBeatOffset"] = njsoffset } : null
                }.Append(UnderlyingParameters.CustomDataParse(new BeatMap.Note()), AppendPriority.High));

                Parameter.ExternalVariables.RefreshAllParameters();
            }
            ConsoleOut("Note", repeatcount, Time, "Note");
        }
    }


}
