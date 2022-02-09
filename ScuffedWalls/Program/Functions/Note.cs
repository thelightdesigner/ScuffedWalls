using ModChart;

namespace ScuffedWalls.Functions
{
    [SFunction("Note")]
    class Note : ScuffedFunction
    {
        protected override void Update()
        {

            BeatMap.Note.NoteType type = GetParam("type", BeatMap.Note.NoteType.Right, p => (BeatMap.Note.NoteType)int.Parse(p));
            BeatMap.Note.CutDirection cutdirection = GetParam("notecutdirection", BeatMap.Note.CutDirection.Down, p => (BeatMap.Note.CutDirection)int.Parse(p));
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

            BeatMap.Note note = new BeatMap.Note()
            {
                _time = Time,
                _lineIndex = 0,
                _lineLayer = 0,
                _cutDirection = cutdirection,
                _type = type,
                _customData = njsoffset.HasValue ? new TreeDictionary() { ["_noteJumpStartBeatOffset"] = njsoffset } : null
            };
            BeatMap.Append(note, UnderlyingParameters.CustomDataParse(new BeatMap.Note()), BeatMap.AppendPriority.High);

            InstanceWorkspace.Notes.Add(note);

            RegisterChanges("Note", 1);
        }
    }


}
