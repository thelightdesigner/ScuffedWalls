using ModChart;
using System;

namespace ScuffedWalls.Functions
{
    [SFunction("Note")]
    class Note : ScuffedFunction
    {
        protected override void Update()
        {

            DifficultyV2.Note.NoteType type = GetParam("type", DifficultyV2.Note.NoteType.Right, p => (DifficultyV2.Note.NoteType)int.Parse(p));
            DifficultyV2.Note.CutDirection cutdirection = GetParam("notecutdirection", DifficultyV2.Note.CutDirection.Down, p => (DifficultyV2.Note.CutDirection)int.Parse(p));
            float? njsoffset = GetParam("definitedurationseconds", null, p =>
            {
                return (float?)ScuffedWallsContainer.BPMAdjuster.GetDefiniteNjsOffsetBeats(ScuffedWallsContainer.BPMAdjuster.ToBeat(p.ToFloat()));
            });
            njsoffset = GetParam("definitedurationbeats", njsoffset, p =>
            {
                return (float?)ScuffedWallsContainer.BPMAdjuster.GetDefiniteNjsOffsetBeats(p.ToFloat());
            });

            Time = GetParam("definitetime", Time, p =>
            {
                if (p.ToLower().RemoveWhiteSpace() == "beats")
                {
                    if (njsoffset.HasValue) return ScuffedWallsContainer.BPMAdjuster.GetPlaceTimeBeats(Time, njsoffset.Value);
                    else return ScuffedWallsContainer.BPMAdjuster.GetPlaceTimeBeats(Time);
                }
                else if (p.ToLower().RemoveWhiteSpace() == "seconds")
                {
                    if (njsoffset.HasValue) return ScuffedWallsContainer.BPMAdjuster.GetPlaceTimeBeats(ScuffedWallsContainer.BPMAdjuster.ToBeat(Time), njsoffset.Value);
                    else return ScuffedWallsContainer.BPMAdjuster.GetPlaceTimeBeats(ScuffedWallsContainer.BPMAdjuster.ToBeat(Time));
                }
                return Time;
            });
            //parse special parameters

            DifficultyV2.Note note = new DifficultyV2.Note()
            {
                _time = Time,
                _lineIndex = GetParam("lineindex",0,p => int.Parse(p)),
                _lineLayer = GetParam("linelayer", 0, p => int.Parse(p)),
                _cutDirection = cutdirection,
                _type = type,
                _customData = new TreeDictionary()
            };
            DifficultyV2.Append(note, UnderlyingParameters.CustomDataParse(new DifficultyV2.Note()), DifficultyV2.AppendPriority.High);
            note._customData[DifficultyV2._noteJumpStartBeatOffset] ??= njsoffset;
           // Console.WriteLine(note._customData[BeatMap._noteJumpStartBeatOffset]);

            InstanceWorkspace.Notes.Add(note);

            RegisterChanges("Note", 1);
        }
    }


}
