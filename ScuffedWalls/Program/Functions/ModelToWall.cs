using ModChart;
using ModChart.Wall;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text.Json;

namespace ScuffedWalls.Functions
{
    [SFunction("ModelToWall", "ModelToNote", "ModelToBomb", "Model")]
    class ModelToWall : ScuffedFunction
    {
        public Parameter Repeat;
        public Parameter Beat;
        public void SetParameters()
        {
            Repeat = new Parameter("repeat","0");
            Beat = new Parameter ("time",Time.ToString());
            UnderlyingParameters.SetInteralVariables(new Parameter[] { Repeat, Beat });
        }
        public override void Run()
        {
            SetParameters();
            var parsedcustomstuff = UnderlyingParameters.CustomDataParse(new BeatMap.Obstacle());
            var isNjs = parsedcustomstuff._customData != null && parsedcustomstuff._customData["_noteJumpStartBeatOffset"] != null;
            var isNjspeed = parsedcustomstuff._customData != null && parsedcustomstuff._customData["_noteJumpMovementSpeed"] != null;

            int repeatcount =       GetParam("repeat", DefaultValue: 1, p => int.Parse(p));
            float repeataddtime =   GetParam("repeataddtime", DefaultValue: 0, p => float.Parse(p));
            
            int normal =            GetParam("normal", DefaultValue: 0, p => Convert.ToInt32(bool.Parse(p)));
            bool tracks =           GetParam("createtracks", DefaultValue: true, p => bool.Parse(p));
            string defaulttrack =    GetParam("defaulttrack", null, p => p);
            bool preserveTime =     GetParam("preservetime", DefaultValue: false, p => bool.Parse(p));
            bool hasanimation =     GetParam("hasanimation", DefaultValue: true, p => bool.Parse(p));
            bool assigncamtotrack=  GetParam("cameratoplayer", DefaultValue: true, p => bool.Parse(p));
            float colormult =       GetParam("colormult",1,p => float.Parse(p));
            bool Notes =            GetParam("createnotes", DefaultValue: true, p => bool.Parse(p));
            bool spline =           GetParam("spline", DefaultValue: false, p => bool.Parse(p));
            float smooth =          GetParam("spreadspawntime", DefaultValue: 0, p => float.Parse(p));
            ModelSettings
            .TypeOverride tpye =    GetParam("type", DefaultValue: ModelSettings.TypeOverride.ModelDefined, p => (ModelSettings.TypeOverride)int.Parse(p));
            float? alpha =          GetParam("alpha", DefaultValue: null, p => (float?)float.Parse(p));
            float? thicc =          GetParam("thicc", DefaultValue: null, p => (float?)float.Parse(p));
            bool setdeltapos =      GetParam("setdeltaposition", false, p => bool.Parse(p));
            bool setdeltascale =    GetParam("setdeltascale", false, p => bool.Parse(p));
            float duration =        GetParam("duration", DefaultValue: 0, p => float.Parse(p));
            Time =                  GetParam("definitetime", Time, p =>
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
            duration =              GetParam("definitedurationseconds", duration, p =>
            {
                if (isNjs) return Utils.BPMAdjuster.GetDefiniteDurationBeats(Utils.BPMAdjuster.ToBeat(p.ToFloat()), parsedcustomstuff._customData["_noteJumpStartBeatOffset"].ToFloat());
                return Utils.BPMAdjuster.GetDefiniteDurationBeats(Utils.BPMAdjuster.ToBeat(p.ToFloat()));
            });
            duration = GetParam("definitedurationbeats", duration, p =>
            {
                if (isNjs) return Utils.BPMAdjuster.GetDefiniteDurationBeats(p.ToFloat(), parsedcustomstuff._customData["_noteJumpStartBeatOffset"].ToFloat());
                return Utils.BPMAdjuster.GetDefiniteDurationBeats(p.ToFloat());
            });


            float MapBpm = Utils.Info["_beatsPerMinute"].ToFloat();
            float MapNjs = Utils.InfoDifficulty["_noteJumpMovementSpeed"].ToFloat();
            float MapOffset = Utils.InfoDifficulty["_noteJumpStartBeatOffset"].ToFloat();

            if (isNjs) MapOffset = parsedcustomstuff._customData["_noteJumpStartBeatOffset"].ToFloat();
            if (isNjspeed) MapNjs = parsedcustomstuff._customData["_noteJumpMovementSpeed"].ToFloat();

            BeatMap output = new BeatMap();
            for (int i = 0; i < repeatcount; i++)
            {
                Repeat.StringData = i.ToString();
                Beat.StringData = (Time + (i * repeataddtime)).ToString();


                FunLog();

                string Path = GetParam("path", DefaultValue: string.Empty, p => System.IO.Path.Combine(Utils.ScuffedConfig.MapFolderPath, p.RemoveWhiteSpace()));
                Path = GetParam("fullpath", DefaultValue: Path, p => p);
                AddRefresh(Path);



                Transformation Delta = new Transformation
                {
                    Position = GetParam("deltaposition", DefaultValue: new Vector3(0, 0, 0), p => JsonSerializer.Deserialize<float[]>(p).ToVector3()),
                    RotationEul = GetParam("deltarotation", DefaultValue: new Vector3(0, 0, 0), p => JsonSerializer.Deserialize<float[]>(p).ToVector3()),
                    Scale = GetParam("deltascale", DefaultValue: new Vector3(1, 0, 0), p => new Vector3(float.Parse(p), 0, 0))
                };
                
                ModelSettings settings = new ModelSettings()
                {
                    DefaultTrack = defaulttrack,
                    PCOptimizerPro = smooth,
                    Path = Path,
                    Thicc = thicc,
                    CreateNotes = Notes,
                    DeltaTransformation = Delta,
                    PreserveTime = preserveTime,
                    Alpha = alpha,
                    technique = (ModelSettings.Technique)normal,
                    AssignCameraToPlayerTrack = assigncamtotrack,
                    CreateTracks = tracks,
                    Spline = spline,
                    ColorMult = colormult,
                    HasAnimation = hasanimation,
                    ObjectOverride = tpye,
                    BPM = MapBpm,
                    NJS = MapNjs,
                    Offset = MapOffset,
                    SetDeltaScale = setdeltascale,
                    SetDeltaPos = setdeltapos,
                    ScaleDuration = true,
                    Wall = (BeatMap.Obstacle)new BeatMap.Obstacle()
                    {
                        _time = Time + (i.ToFloat() * repeataddtime),
                        _duration = duration
                    }.Append(UnderlyingParameters.CustomDataParse(new BeatMap.Obstacle()), AppendPriority.High)
                };
                var model = new WallModel(settings);

                output.AddMap(model.Output);

                Parameter.ExternalVariables.RefreshAllParameters();
            }
            foreach (var stat in output.Stats) ConsoleOut(stat.Key,stat.Value,Time,"Model");
        }
    }


}
