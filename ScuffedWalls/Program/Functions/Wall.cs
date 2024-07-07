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
            parsedthing = UnderlyingParameters.CustomDataParse(new DifficultyV2.Obstacle());
            isNjs = parsedthing != null && parsedthing._customData != null && parsedthing._customData["_noteJumpStartBeatOffset"] != null;
        }
        protected override void Update()
        {
            float duration = GetParam("duration", 0, p => float.Parse(p));
            int lineindex = GetParam("lineindex", 0, p => int.Parse(p));


            duration = GetParam("definiteduration", duration, p =>
            {
                if (isNjs) return ScuffedWallsContainer.BPMAdjuster.GetDefiniteDurationBeats(p.ToFloat(), parsedthing._customData["_noteJumpStartBeatOffset"].ToFloat());
                else return ScuffedWallsContainer.BPMAdjuster.GetDefiniteDurationBeats(p.ToFloat());
            });
            Time = GetParam("definitetime", Time, p =>
            {
                if (p.ToLower().RemoveWhiteSpace() == "beats")
                {
                    if (isNjs) return ScuffedWallsContainer.BPMAdjuster.GetPlaceTimeBeats(Time, parsedthing._customData["_noteJumpStartBeatOffset"].ToFloat());
                    else return ScuffedWallsContainer.BPMAdjuster.GetPlaceTimeBeats(Time);
                }
                else if (p.ToLower().RemoveWhiteSpace() == "seconds")
                {
                    if (isNjs) return ScuffedWallsContainer.BPMAdjuster.GetPlaceTimeBeats(ScuffedWallsContainer.BPMAdjuster.ToBeat(Time), parsedthing._customData["_noteJumpStartBeatOffset"].ToFloat());
                    else return ScuffedWallsContainer.BPMAdjuster.GetPlaceTimeBeats(ScuffedWallsContainer.BPMAdjuster.ToBeat(Time));
                }
                return Time;
            });
            duration = GetParam("definitedurationseconds", duration, p =>
            {
                if (isNjs) return ScuffedWallsContainer.BPMAdjuster.GetDefiniteDurationBeats(ScuffedWallsContainer.BPMAdjuster.ToBeat(p.ToFloat()), parsedthing._customData["_noteJumpStartBeatOffset"].ToFloat());
                return ScuffedWallsContainer.BPMAdjuster.GetDefiniteDurationBeats(ScuffedWallsContainer.BPMAdjuster.ToBeat(p.ToFloat()));
            });
            duration = GetParam("definitedurationbeats", duration, p =>
            {
                if (isNjs) return ScuffedWallsContainer.BPMAdjuster.GetDefiniteDurationBeats(p.ToFloat(), parsedthing._customData["_noteJumpStartBeatOffset"].ToFloat());
                return ScuffedWallsContainer.BPMAdjuster.GetDefiniteDurationBeats(p.ToFloat());
            });

            DifficultyV2.Obstacle wall = new DifficultyV2.Obstacle()
            {
                _time = Time,
                _duration = duration,
                _lineIndex = GetParam("lineindex", 0, p => int.Parse(p)),
                _width = GetParam("width", 0, p => int.Parse(p)),
                _type = (DifficultyV2.Obstacle.Type)GetParam("type", 0, p => int.Parse(p))
            };
            
            wall._customData ??= new TreeDictionary();
        
            // by default make walls fake and uninteractable
            wall._customData["_fake"] = true;
            wall._customData["_interactable"] = false;

            DifficultyV2.Obstacle append = (DifficultyV2.Obstacle)UnderlyingParameters.CustomDataParse(new DifficultyV2.Obstacle());

            DifficultyV2.Append(wall, append, DifficultyV2.AppendPriority.High);

            InstanceWorkspace.Walls.Add(wall);


            RegisterChanges("Wall", 1);
        }
    }


}
