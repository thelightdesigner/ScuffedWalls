using ModChart;
using ModChart.Wall;
using System;
using System.Numerics;
using System.Text.Json;

namespace ScuffedWalls.Functions
{
    [ScuffedFunction("ModelToWall", "ModelToNote", "ModelToBomb", "Model")]
    class ModelToWall : SFunction
    {
        public Parameter Repeat;
        public Parameter Beat;
        public void SetParameters()
        {
            Repeat = new Parameter("repeat","1");
            Beat = new Parameter ("time",Time.ToString());
            Parameters.SetInteralVariables(new Parameter[] { Repeat, Beat });
        }
        public override void Run()
        {
            SetParameters();
            var parsedcustomstuff = Parameters.CustomDataParse(new BeatMap.Obstacle());
            var isNjs = parsedcustomstuff._customData != null && parsedcustomstuff._customData._noteJumpStartBeatOffset != null;
            var isNjspeed = parsedcustomstuff._customData != null && parsedcustomstuff._customData._noteJumpMovementSpeed != null;

            int repeatcount =       GetParam("repeat", DefaultValue: 1, p => int.Parse(p));
            float repeataddtime =   GetParam("repeataddtime", DefaultValue: 0, p => float.Parse(p));
            string Path =           GetParam("path", DefaultValue: string.Empty, p => Utils.ScuffedConfig.MapFolderPath + @"\" + p.RemoveWhiteSpace());
            Path =                  GetParam("fullpath", DefaultValue: Path, p => p);
            int normal =            GetParam("normal", DefaultValue: 0, p => Convert.ToInt32(bool.Parse(p)));
            bool tracks =           GetParam("createtracks", DefaultValue: true, p => bool.Parse(p));
            
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
                    if (isNjs) return Utils.bpmAdjuster.GetPlaceTimeBeats(Time, parsedcustomstuff._customData._noteJumpStartBeatOffset.toFloat());
                    else return Utils.bpmAdjuster.GetPlaceTimeBeats(Time);
                }
                else if (p.ToLower().RemoveWhiteSpace() == "seconds")
                {
                    if (isNjs) return Utils.bpmAdjuster.GetPlaceTimeBeats(Utils.bpmAdjuster.ToBeat(Time), parsedcustomstuff._customData._noteJumpStartBeatOffset.toFloat());
                    else return Utils.bpmAdjuster.GetPlaceTimeBeats(Utils.bpmAdjuster.ToBeat(Time));
                }
                return Time;
            });
            duration =              GetParam("definitedurationseconds", duration, p =>
            {
                if (isNjs) return Utils.bpmAdjuster.GetDefiniteDurationBeats(Utils.bpmAdjuster.ToBeat(p.toFloat()), parsedcustomstuff._customData._noteJumpStartBeatOffset.toFloat());
                return Utils.bpmAdjuster.GetDefiniteDurationBeats(Utils.bpmAdjuster.ToBeat(p.toFloat()));
            });
            duration = GetParam("definitedurationbeats", duration, p =>
            {
                if (isNjs) return Utils.bpmAdjuster.GetDefiniteDurationBeats(p.toFloat(), parsedcustomstuff._customData._noteJumpStartBeatOffset.toFloat());
                return Utils.bpmAdjuster.GetDefiniteDurationBeats(p.toFloat());
            });


            float MapBpm = Utils.Info._beatsPerMinute.toFloat();
            float MapNjs = Utils.InfoDifficulty._noteJumpMovementSpeed.toFloat();
            float MapOffset = Utils.InfoDifficulty._noteJumpStartBeatOffset.toFloat();

            if (isNjs) MapOffset = parsedcustomstuff._customData._noteJumpStartBeatOffset.toFloat();
            if (isNjspeed) MapNjs = parsedcustomstuff._customData._noteJumpMovementSpeed.toFloat();


            int walls = 0;
            int notes = 0;
            int customevents = 0;
            for (int i = 0; i < repeatcount; i++)
            {
                Transformation Delta = new Transformation();
                Delta.Position = GetParam("deltaposition", DefaultValue: new Vector3(0, 0, 0), p => JsonSerializer.Deserialize<float[]>(p).ToVector3());
                Delta.RotationEul = GetParam("deltarotation", DefaultValue: new Vector3(0, 0, 0), p => JsonSerializer.Deserialize<float[]>(p).ToVector3());
                Delta.Scale = GetParam("deltascale", DefaultValue: new Vector3(1, 0, 0), p => new Vector3(float.Parse(p), 0, 0));

                ModelSettings settings = new ModelSettings()
                {
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
                        _time = Time + (i.toFloat() * repeataddtime),
                        _duration = duration
                    }.Append(Parameters.CustomDataParse(new BeatMap.Obstacle()), AppendTechnique.Overwrites)
                };
                var model = new WallModel(settings);

                InstanceWorkspace.Walls.AddRange(model.Output._obstacles);
                InstanceWorkspace.Notes.AddRange(model.Output._notes);
                InstanceWorkspace.CustomEvents.AddRange(model.Output._customData._customEvents);

                walls += model.Output._obstacles.Length;
                notes += model.Output._notes.Length;
                customevents += model.Output._customData._customEvents.Length;

                Repeat.StringData = i.ToString();
                Beat.StringData = (Time + (i * repeataddtime)).ToString();
                Parameter.ExternalVariables.RefreshAllParameters();
            }
            if (walls > 0) ConsoleOut("Wall", walls, Time, "ModelToWall");
            if (notes > 0) ConsoleOut("Note", notes, Time, "ModelToWall");
            if (customevents > 0) ConsoleOut("CustomEvent", customevents, Time, "ModelToWall");
        }
    }


}
