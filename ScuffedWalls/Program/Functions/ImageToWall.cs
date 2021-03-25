using ModChart;
using ModChart.Wall;

namespace ScuffedWalls.Functions
{
    [ScuffedFunction("ImageToWall")]
    class ImageToWall : SFunction
    {
        public void Run()
        {
            var customdata = Parameters.CustomDataParse();
            var isNjs = customdata != null && customdata._noteJumpStartBeatOffset != null;

            string Path = GetParam("path", DefaultValue: string.Empty, p => Startup.ScuffedConfig.MapFolderPath + @"\" + p.RemoveWhiteSpace());
            Path = GetParam("fullpath", DefaultValue: Path, p => p);
            float duration = GetParam("duration", DefaultValue: 0, p => float.Parse(p));
            bool isBlackEmpty = GetParam("isblackempty", DefaultValue: true, p => bool.Parse(p));
            bool centered = GetParam("centered", DefaultValue: true, p => bool.Parse(p));
            float size = GetParam("size", DefaultValue: 1, p => float.Parse(p));
            float shift = GetParam("shift", DefaultValue: 2, p => float.Parse(p));
            float alpha = GetParam("alpha", DefaultValue: 0, p => float.Parse(p));
            float thicc = GetParam("thicc", DefaultValue: 12, p => float.Parse(p));
            int maxlength = GetParam("maxlinelength", DefaultValue: 100000, p => int.Parse(p));
            float compression = GetParam("compression", DefaultValue: 0, p => float.Parse(p));
            float spreadspawntime = GetParam("spreadspawntime", DefaultValue: 0, p => float.Parse(p));
            duration = GetParam("definiteduration", duration, p =>
              {
                  if (isNjs) return Startup.bpmAdjuster.GetDefiniteDurationBeats(p.toFloat(), customdata._noteJumpStartBeatOffset.toFloat());
                  else return Startup.bpmAdjuster.GetDefiniteDurationBeats(p.toFloat());
              });
            Time = GetParam("definitetime", Time, p =>
              {
                  if (p.ToLower().RemoveWhiteSpace() == "beats")
                  {
                      if (isNjs) return Startup.bpmAdjuster.GetPlaceTimeBeats(Time, customdata._noteJumpStartBeatOffset.toFloat());
                      else return Startup.bpmAdjuster.GetPlaceTimeBeats(Time);
                  }
                  else if (p.ToLower().RemoveWhiteSpace() == "seconds")
                  {
                      if (isNjs) return Startup.bpmAdjuster.GetPlaceTimeBeats(Startup.bpmAdjuster.ToBeat(Time), customdata._noteJumpStartBeatOffset.toFloat());
                      else return Startup.bpmAdjuster.GetPlaceTimeBeats(Startup.bpmAdjuster.ToBeat(Time));
                  }
                  return Time;
              });
            duration = GetParam("definitedurationseconds",duration,p =>
            {
                if (isNjs) return Startup.bpmAdjuster.GetDefiniteDurationBeats(Startup.bpmAdjuster.ToBeat(p.toFloat()), customdata._noteJumpStartBeatOffset.toFloat());
                return Startup.bpmAdjuster.GetDefiniteDurationBeats(Startup.bpmAdjuster.ToBeat(p.toFloat()));
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
                    Wall = new BeatMap.Obstacle()
                    {
                        _time = Time,
                        _duration = duration,
                        _customData = Parameters.CustomDataParse()
                    }
                });
            BeatMap.Obstacle[] image = converter.Walls;
            InstanceWorkspace.Walls.AddRange(image);
            ConsoleOut("Wall", image.Length, Time, "ImageToWall");
        }
    }



}
