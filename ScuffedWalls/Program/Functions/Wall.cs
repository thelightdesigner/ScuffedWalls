using ModChart;

namespace ScuffedWalls.Functions
{
    [SFunction("Wall")]
    class Wall : ScuffedFunction
    {
        ICustomDataMapObject parsedthing;
        bool isNjs;
        protected override void Init()
        {
            parsedthing = UnderlyingParameters.CustomDataParse(new BeatMap.Obstacle());
            isNjs = parsedthing != null && parsedthing._customData != null && parsedthing._customData["_noteJumpStartBeatOffset"] != null;
        }
        protected override void Update()
        {
            float duration = GetParam("duration", 0, p => float.Parse(p));
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

            BeatMap.Obstacle wall = new BeatMap.Obstacle()
            {
                _time = Time,
                _duration = duration,
                _lineIndex = GetParam("lineindex", 0, p => int.Parse(p)),
                _width = 0,
                _type = 0
            };

            BeatMap.Obstacle append = (BeatMap.Obstacle)UnderlyingParameters.CustomDataParse(new BeatMap.Obstacle());

            BeatMap.Append(wall, append, BeatMap.AppendPriority.High);

            InstanceWorkspace.Walls.Add(wall);


            RegisterChanges("Wall", 1);
        }
    }


}
