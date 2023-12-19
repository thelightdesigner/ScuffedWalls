﻿using ModChart;
using ModChart.Wall;

namespace ScuffedWalls.Functions
{
    [ScuffedFunction("ImageToWall","Image","RenderImage")]
    class ImageToWall : SFunction
    {
        public override void Run()
        {
            FunLog();

            var parsedcustomstuff = Parameters.CustomDataParse(new BeatMap.Obstacle());
            var isNjs = parsedcustomstuff._customData != null && parsedcustomstuff._customData["_noteJumpStartBeatOffset"] != null;

            string Path = GetParam("path", DefaultValue: string.Empty, p => System.IO.Path.Combine( Utils.ScuffedConfig.MapFolderPath, p.RemoveWhiteSpace()));
            Path = GetParam("fullpath", DefaultValue: Path, p => p);
            float duration = GetParam("duration", DefaultValue: 0, p => float.Parse(p));
            bool isBlackEmpty = GetParam("isblackempty", DefaultValue: true, p => bool.Parse(p));
            bool centered = GetParam("centered", DefaultValue: true, p => bool.Parse(p));
            float size = GetParam("size", DefaultValue: 1, p => float.Parse(p));
            float shift = GetParam("shift", DefaultValue: 2, p => float.Parse(p));
            float alpha = GetParam("alpha", DefaultValue: 0, p => float.Parse(p));
            float? thicc = GetParam("thicc", DefaultValue: null, p => (float?)float.Parse(p));
            int maxlength = GetParam("maxlinelength", DefaultValue: 100000, p => int.Parse(p));
            float compression = GetParam("compression", DefaultValue: 0, p => float.Parse(p));
            float spreadspawntime = GetParam("spreadspawntime", DefaultValue: 0, p => float.Parse(p));
            Time = GetParam("definitetime", Time, p =>
              {
                  if (p.ToLower().RemoveWhiteSpace() == "beats")
                  {
                      if (isNjs) return Utils.BPMAdjuster.GetPlaceTimeBeats(Time, parsedcustomstuff._customData["_noteJumpStartBeatOffset"].ToFloat());
                      else return Utils.BPMAdjuster.GetPlaceTimeBeats(Time);
                  }
                  else if (p.ToLower().RemoveWhiteSpace() == "seconds")
                  {
                      if (isNjs) return Utils.BPMAdjuster.GetPlaceTimeBeats(Utils.BPMAdjuster.ToBeat(Time), parsedcustomstuff._customData["_noteJumpStartBeatOffset"].ToFloat());
                      else return Utils.BPMAdjuster.GetPlaceTimeBeats(Utils.BPMAdjuster.ToBeat(Time));
                  }
                  return Time;
              });
            duration = GetParam("definitedurationseconds",duration,p =>
            {
                if (isNjs) return Utils.BPMAdjuster.GetDefiniteDurationBeats(Utils.BPMAdjuster.ToBeat(p.ToFloat()), parsedcustomstuff._customData["_noteJumpStartBeatOffset"].ToFloat());
                return Utils.BPMAdjuster.GetDefiniteDurationBeats(Utils.BPMAdjuster.ToBeat(p.ToFloat()));
            });
            duration = GetParam("definitedurationbeats", duration, p =>
            {
                if (isNjs) return Utils.BPMAdjuster.GetDefiniteDurationBeats(p.ToFloat(), parsedcustomstuff._customData["_noteJumpStartBeatOffset"].ToFloat());
                return Utils.BPMAdjuster.GetDefiniteDurationBeats(p.ToFloat());
            });


            WallImage converter = new WallImage(Path,
                new ImageSettings()
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
                    Wall = (BeatMap.Obstacle)new BeatMap.Obstacle()
                    {
                        _time = Time,
                        _duration = duration,
                        _customData = new TreeDictionary()
                    }.Append(Parameters.CustomDataParse(new BeatMap.Obstacle()),  AppendPriority.High)
                });
            BeatMap.Obstacle[] image = converter.Walls;
            InstanceWorkspace.Walls.AddRange(image);
            ConsoleOut("Wall", image.Length, Time, "ImageToWall");
            Parameter.RefreshAllParameters();
        }
    }



}
