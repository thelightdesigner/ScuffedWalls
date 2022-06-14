using ModChart;
using ModChart.Wall;

namespace ScuffedWalls.Functions
{
    
    [SFunction("ImageToWall", "Image", "RenderImage")]
    class ImageToWall : ScuffedFunction
    {
        ICustomDataMapObject parsedcustomstuff;
        bool isNjs;
        float duration;
        bool isBlackEmpty;
        bool centered;
        float size;
        float shift;
        float alpha;
        float? thicc;
        int maxlength;
        float compression;
        float spreadspawntime;
        string Path;
        protected override void Init()
        {
            parsedcustomstuff = UnderlyingParameters.CustomDataParse(new BeatMap.Obstacle());
            isNjs = parsedcustomstuff._customData != null && parsedcustomstuff._customData["_noteJumpStartBeatOffset"] != null;
            duration = GetParam("duration", DefaultValue: 0, p => float.Parse(p));
            isBlackEmpty = GetParam("isblackempty", DefaultValue: true, p => bool.Parse(p));
            centered = GetParam("centered", DefaultValue: true, p => bool.Parse(p));
            size = GetParam("size", DefaultValue: 1, p => float.Parse(p));
            shift = GetParam("shift", DefaultValue: 2, p => float.Parse(p));
            alpha = GetParam("alpha", DefaultValue: 0, p => float.Parse(p));
            thicc = GetParam("thicc", DefaultValue: null, p => (float?)float.Parse(p));
            maxlength = GetParam("maxlinelength", DefaultValue: 100000, p => int.Parse(p));
            compression = GetParam("compression", DefaultValue: 0, p => float.Parse(p));
            spreadspawntime = GetParam("spreadspawntime", DefaultValue: 0, p => float.Parse(p));

            Time = GetParam("definitetime", Time, p =>
            {
                if (p.ToLower().RemoveWhiteSpace() == "beats")
                {
                    if (isNjs) return ScuffedWallsContainer.BPMAdjuster.GetPlaceTimeBeats(Time, parsedcustomstuff._customData["_noteJumpStartBeatOffset"].ToFloat());
                    else return ScuffedWallsContainer.BPMAdjuster.GetPlaceTimeBeats(Time);
                }
                else if (p.ToLower().RemoveWhiteSpace() == "seconds")
                {
                    if (isNjs) return ScuffedWallsContainer.BPMAdjuster.GetPlaceTimeBeats(ScuffedWallsContainer.BPMAdjuster.ToBeat(Time), parsedcustomstuff._customData["_noteJumpStartBeatOffset"].ToFloat());
                    else return ScuffedWallsContainer.BPMAdjuster.GetPlaceTimeBeats(ScuffedWallsContainer.BPMAdjuster.ToBeat(Time));
                }
                return Time;
            });
            duration = GetParam("definitedurationseconds", duration, p =>
            {
                if (isNjs) return ScuffedWallsContainer.BPMAdjuster.GetDefiniteDurationBeats(ScuffedWallsContainer.BPMAdjuster.ToBeat(p.ToFloat()), parsedcustomstuff._customData["_noteJumpStartBeatOffset"].ToFloat());
                return ScuffedWallsContainer.BPMAdjuster.GetDefiniteDurationBeats(ScuffedWallsContainer.BPMAdjuster.ToBeat(p.ToFloat()));
            });
            duration = GetParam("definitedurationbeats", duration, p =>
            {
                if (isNjs) return ScuffedWallsContainer.BPMAdjuster.GetDefiniteDurationBeats(p.ToFloat(), parsedcustomstuff._customData["_noteJumpStartBeatOffset"].ToFloat());
                return ScuffedWallsContainer.BPMAdjuster.GetDefiniteDurationBeats(p.ToFloat());
            });
        }
        protected override void Update()
        {
            Path = GetParam("path", DefaultValue: string.Empty, p => System.IO.Path.Combine(ScuffedWallsContainer.ScuffedConfig.MapFolderPath, p.RemoveWhiteSpace()));
            Path = GetParam("fullpath", DefaultValue: Path, p => p);
            AddRefresh(Path);

            BeatMap.Obstacle wall = new BeatMap.Obstacle()
            {
                _time = Time,
                _duration = duration,
                _customData = new TreeDictionary()
            };
            
            wall._customData ??= new TreeDictionary();
        
            // by default make walls fake and uninteractable
            wall._customData["_fake"] = true;
            wall._customData["_interactable"] = false;

            BeatMap.Append(wall, UnderlyingParameters.CustomDataParse(new BeatMap.Obstacle()), BeatMap.AppendPriority.High);

            WallImage converter = new WallImage(Path, new ImageSettings()
            {
                maxPixelLength = maxlength,
                isBlackEmpty = isBlackEmpty,
                scale = size,
                thicc = thicc,
                shift = shift,
                centered = centered,
                PCOptimizerPro = spreadspawntime,
                alfa = alpha,
                tolerance = compression,
                Wall = wall
            });

            InstanceWorkspace.Walls.AddRange(converter.Walls);

            RegisterChanges("Wall", converter.Walls.Length);
        }
    }
    
}
