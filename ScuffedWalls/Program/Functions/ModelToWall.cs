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
        public Variable Repeat;
        public Variable Beat;
        public void SetParameters()
        {
            Repeat = new Variable { Name = "repeat", Data = "1" };
            Beat = new Variable { Name = "time", Data = Time.ToString() };
            Parameters = Parameters.AddVariables(new Variable[] { Repeat, Beat });
        }
        public void Run()
        {
            SetParameters();
            var customdata = Parameters.CustomDataParse();
            var isNjs = customdata != null && customdata._noteJumpStartBeatOffset != null;

            int repeatcount =       GetParam("repeat", DefaultValue: 1, p => int.Parse(p));
            float repeataddtime =   GetParam("repeataddtime", DefaultValue: 0, p => float.Parse(p));
            string Path =           GetParam("path", DefaultValue: string.Empty, p => Startup.ScuffedConfig.MapFolderPath + @"\" + p.RemoveWhiteSpace());
            Path =                  GetParam("fullpath", DefaultValue: Path, p => p);
            int normal =            GetParam("normal", DefaultValue: 0, p => Convert.ToInt32(bool.Parse(p)));
            bool tracks =           GetParam("createtracks", DefaultValue: true, p => bool.Parse(p));
            float duration =        GetParam("duration", DefaultValue: 0, p => float.Parse(p));
            bool preserveTime =     GetParam("preservetime", DefaultValue: false, p => bool.Parse(p));
            bool hasanimation =     GetParam("hasanimation", DefaultValue: true, p => bool.Parse(p));
            bool assigncamtotrack=  GetParam("cameratoplayer", DefaultValue: true, p => bool.Parse(p));
            bool Notes =            GetParam("createnotes", DefaultValue: true, p => bool.Parse(p));
            bool spline =           GetParam("spline", DefaultValue: false, p => bool.Parse(p));
            float smooth =          GetParam("spreadspawntime", DefaultValue: 0, p => float.Parse(p));
            ModelSettings
            .TypeOverride tpye =    GetParam("type", DefaultValue: ModelSettings.TypeOverride.ModelDefined, p => (ModelSettings.TypeOverride)int.Parse(p));
            float? alpha =          GetParam("alpha", DefaultValue: null, p => (float?)float.Parse(p));
            float? thicc =          GetParam("thicc", DefaultValue: null, p => (float?)float.Parse(p));
            duration =              GetParam("definiteduration", duration, p =>
            {
                if (isNjs) return Startup.bpmAdjuster.GetDefiniteDurationBeats(p.toFloat(), customdata._noteJumpStartBeatOffset.toFloat());
                else return Startup.bpmAdjuster.GetDefiniteDurationBeats(p.toFloat());
            });
            Time =                  GetParam("definitetime", Time, p =>
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
            duration =              GetParam("definitedurationseconds", duration, p =>
            {
                if (isNjs) return Startup.bpmAdjuster.GetDefiniteDurationBeats(Startup.bpmAdjuster.ToBeat(p.toFloat()), customdata._noteJumpStartBeatOffset.toFloat());
                return Startup.bpmAdjuster.GetDefiniteDurationBeats(Startup.bpmAdjuster.ToBeat(p.toFloat()));
            });


            float MapBpm = Startup.Info._beatsPerMinute.toFloat();
            float MapNjs = Startup.InfoDifficulty._noteJumpMovementSpeed.toFloat();

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
                    HasAnimation = hasanimation,
                    ObjectOverride = tpye,
                    BPM = MapBpm,
                    NJS = MapNjs,
                    Offset = Startup.bpmAdjuster.StartBeatOffset,
                    Wall = new BeatMap.Obstacle()
                    {
                        _time = Time + (i.toFloat() * repeataddtime),
                        _duration = duration,
                        _customData = Parameters.CustomDataParse()
                    }
                };
                var model = new WallModel(settings);

                InstanceWorkspace.Walls.AddRange(model.Output._obstacles);
                InstanceWorkspace.Notes.AddRange(model.Output._notes);
                InstanceWorkspace.CustomEvents.AddRange(model.Output._customData._customEvents);

                walls += model.Output._obstacles.Length;
                notes += model.Output._notes.Length;
                customevents += model.Output._customData._customEvents.Length;

                Repeat.Data = i.ToString();
                Beat.Data = (Time + (i * repeataddtime)).ToString();
            }
            if (walls > 0) ConsoleOut("Wall", walls, Time, "ModelToWall");
            if (notes > 0) ConsoleOut("Note", notes, Time, "ModelToWall");
            if (customevents > 0) ConsoleOut("CustomEvent", customevents, Time, "ModelToWall");
        }
    }


}
