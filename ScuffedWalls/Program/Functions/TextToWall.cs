using ModChart;
using ModChart.Wall;
using System.Collections.Generic;
using System.IO;

namespace ScuffedWalls.Functions
{
    [ScuffedFunction("TextToWall")]
    class TextToWall : SFunction
    {
        public void Run()
        {
            var parsedshit = Parameters.CustomDataParse(new BeatMap.Obstacle());
            var isNjs = parsedshit._customData != null && parsedshit._customData._noteJumpStartBeatOffset != null;
            List<string> lines = new List<string>();

            float letting =         GetParam("letting", 1, p => float.Parse(p));
            float leading =         GetParam("leading", 1, p => float.Parse(p));
            float size =            GetParam("size", 1, p => float.Parse(p));
            float thicc =           GetParam("thicc", 1, p => float.Parse(p));
            float compression =     GetParam("compression", 0.1f, p => float.Parse(p));
            float shift =           GetParam("shift", 1, p => float.Parse(p));
            int linelength =        GetParam("maxlinelength", 1000000, p => int.Parse(p));
            bool isblackempty =     GetParam("isblackempty", true, p => bool.Parse(p));
            float alpha =           GetParam("alpha", 1, p => float.Parse(p));
            float smooth =          GetParam("spreadspawntime", 0, p => float.Parse(p));
            string Path =           GetParam("path", DefaultValue: string.Empty, p => Startup.ScuffedConfig.MapFolderPath + @"\" + p.RemoveWhiteSpace());
            Path =                  GetParam("fullpath", DefaultValue: Path, p => p);
            float duration =        GetParam("duration", DefaultValue: 0, p => float.Parse(p));
            duration =              GetParam("definiteduration", duration, p =>
            {
                if (isNjs) return Startup.bpmAdjuster.GetDefiniteDurationBeats(p.toFloat(), parsedshit._customData._noteJumpStartBeatOffset.toFloat());
                else return Startup.bpmAdjuster.GetDefiniteDurationBeats(p.toFloat());
            });
            ModelSettings
            .TypeOverride tpye =    GetParam("type", DefaultValue: ModelSettings.TypeOverride.ModelDefined, p => (ModelSettings.TypeOverride)int.Parse(p));
            Time =                  GetParam("definitetime", Time, p =>
            {
                if (p.ToLower().RemoveWhiteSpace() == "beats")
                {
                    if (isNjs) return Startup.bpmAdjuster.GetPlaceTimeBeats(Time, parsedshit._customData._noteJumpStartBeatOffset.toFloat());
                    else return Startup.bpmAdjuster.GetPlaceTimeBeats(Time);
                }
                else if (p.ToLower().RemoveWhiteSpace() == "seconds")
                {
                    if (isNjs) return Startup.bpmAdjuster.GetPlaceTimeBeats(Startup.bpmAdjuster.ToBeat(Time), parsedshit._customData._noteJumpStartBeatOffset.toFloat());
                    else return Startup.bpmAdjuster.GetPlaceTimeBeats(Startup.bpmAdjuster.ToBeat(Time));
                }
                return Time;
            });
            duration =              GetParam("definitedurationseconds", duration, p =>
            {
                if (isNjs) return Startup.bpmAdjuster.GetDefiniteDurationBeats(Startup.bpmAdjuster.ToBeat(p.toFloat()), parsedshit._customData._noteJumpStartBeatOffset.toFloat());
                return Startup.bpmAdjuster.GetDefiniteDurationBeats(Startup.bpmAdjuster.ToBeat(p.toFloat()));
            });
            

            float MapBpm = Startup.Info._beatsPerMinute.toFloat();
            float MapNjs = Startup.InfoDifficulty._noteJumpMovementSpeed.toFloat();

            foreach (var p in Parameters)
            {
                if (p.Name == "line")
                {
                    lines.Add(p.Data);
                }
            }


            bool isModel = false;
            if (new FileInfo(Path).Extension.ToLower() == ".dae")isModel = true;
            

            lines.Reverse();
            WallText text = new WallText(new TextSettings()
            {
                ModelEnabled = isModel,
                Centered = true,
                Leading = leading,
                Letting = letting,
                Path = Path,
                Text = lines.ToArray(),
                ImageSettings = new ImageSettings()
                {
                    scale = size,
                    shift = shift,
                    PCOptimizerPro = smooth,
                    alfa = alpha,
                    centered = false,
                    isBlackEmpty = isblackempty,
                    maxPixelLength = linelength,
                    thicc = thicc,
                    tolerance = compression,
                    Wall = (BeatMap.Obstacle)new BeatMap.Obstacle()
                    {
                        _time = Time,
                        _duration = duration
                    }.Append(Parameters.CustomDataParse(new BeatMap.Obstacle()), AppendTechnique.Overwrites)
                },
                ModelSettings = new ModelSettings()
                {
                    PCOptimizerPro = smooth,
                    Path = Path,
                    Thicc = thicc,
                    CreateNotes = false,
                    DeltaTransformation = null,
                    PreserveTime = false,
                    Alpha = alpha,
                    technique = ModelSettings.Technique.Normal,
                    AssignCameraToPlayerTrack = false,
                    CreateTracks = false,
                    Spline = false,
                    HasAnimation = false,
                    ObjectOverride = tpye,
                    BPM = MapBpm,
                    NJS = MapNjs,
                    Offset = Startup.bpmAdjuster.StartBeatOffset,
                    Wall = (BeatMap.Obstacle)new BeatMap.Obstacle()
                    {
                        _time = Time,
                        _duration = duration
                    }.Append(Parameters.CustomDataParse(new BeatMap.Obstacle()), AppendTechnique.Overwrites)
                }
            });
            InstanceWorkspace.Walls.AddRange(text.Walls);
            ConsoleOut("Wall", text.Walls.Length, Time, "TextToWall");
        }
    }


}
