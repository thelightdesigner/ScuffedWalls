using ModChart;
using ModChart.Wall;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScuffedWalls.Functions
{
    [ScuffedFunction("ImageToWall")]
    class ImageToWall : SFunction
    {
        public void Run()
        {
            string Path = string.Empty;
            float duration = 1;
            bool isBlackEmpty = false;
            bool centered = false;
            float size = 1;
            float shift = 2;
            float alpha = 1;
            float thicc = 1;
            int maxlength = 100000;
            float compression = 0;
            float spreadspawntime = 0;
            var customdata = Parameters.CustomDataParse();
            var isNjs = customdata != null && customdata._noteJumpStartBeatOffset != null;
            foreach (var p in Parameters)
            {
                switch (p.Name)
                {
                    case "path":
                        Path = Startup.ScuffedConfig.MapFolderPath + @"\" + p.Data.removeWhiteSpace();
                        break;
                    case "fullpath":
                        Path = p.Data;
                        break;
                    case "duration":
                        duration = Convert.ToSingle(p.Data);
                        break;
                    case "maxlinelength":
                        maxlength = Convert.ToInt32(p.Data);
                        break;
                    case "isblackempty":
                        isBlackEmpty = Convert.ToBoolean(p.Data);
                        break;
                    case "size":
                        size = Convert.ToSingle(p.Data);
                        break;
                    case "spreadspawntime":
                        spreadspawntime = Convert.ToSingle(p.Data);
                        break;
                    case "alpha":
                        alpha = Convert.ToSingle(p.Data);
                        break;
                    case "thicc":
                        thicc = Convert.ToSingle(p.Data);
                        break;
                    case "shift":
                        shift = Convert.ToSingle(p.Data);
                        break;
                    case "centered":
                        centered = Convert.ToBoolean(p.Data);
                        break;
                    case "compression":
                        compression = Convert.ToSingle(p.Data);
                        break;
                    case "definiteduration":
                        duration = Startup.bpmAdjuster.GetDefiniteDurationBeats(p.Data.toFloat());
                        if (isNjs) Startup.bpmAdjuster.GetDefiniteDurationBeats(p.Data.toFloat(), customdata._noteJumpStartBeatOffset.toFloat());
                        break;
                    case "definitetime":
                        if (p.Data.ToLower().removeWhiteSpace() == "beats")
                        {
                            if (isNjs) Time = Startup.bpmAdjuster.GetPlaceTimeBeats(Time, customdata._noteJumpStartBeatOffset.toFloat());
                            else Time = Startup.bpmAdjuster.GetPlaceTimeBeats(Time);
                        }
                        else if (p.Data.ToLower().removeWhiteSpace() == "seconds")
                        {
                            if (isNjs) Time = Startup.bpmAdjuster.GetPlaceTimeBeats(Startup.bpmAdjuster.ToBeat(Time), customdata._noteJumpStartBeatOffset.toFloat());
                            else Time = Startup.bpmAdjuster.GetPlaceTimeBeats(Startup.bpmAdjuster.ToBeat(Time));
                        }
                        break;
                    case "definitedurationseconds":
                        duration = Startup.bpmAdjuster.GetDefiniteDurationBeats(Startup.bpmAdjuster.ToBeat(p.Data.toFloat()));
                        if (isNjs) duration = Startup.bpmAdjuster.GetDefiniteDurationBeats(Startup.bpmAdjuster.ToBeat(p.Data.toFloat()), customdata._noteJumpStartBeatOffset.toFloat());
                        break;
                }
            }
            WallImage converter = new WallImage(Path, new ImageSettings() { maxPixelLength = maxlength, isBlackEmpty = isBlackEmpty, scale = size, thicc = thicc, shift = shift, centered = centered, spread = spreadspawntime, alfa = alpha, tolerance = compression, Wall = new BeatMap.Obstacle() { _time = Time, _duration = duration, _customData = Parameters.CustomDataParse() } });
            BeatMap.Obstacle[] image = converter.Walls;
            InstanceWorkspace.Walls.AddRange(image);
            ConsoleOut("Wall", image.Length, Time, "ImageToWall");
        }
    }
}
