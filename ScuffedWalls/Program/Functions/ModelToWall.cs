using ModChart;
using ModChart.Wall;
using System;
using System.Text.Json;

namespace ScuffedWalls.Functions
{
    [ScuffedFunction("ModelToWall")]
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
            int repeatcount = 1;
            float repeataddtime = 0;
            string Path = string.Empty;
            bool hasanimation = true;
            float[] colormult = null;
            int normal = 0;
            bool tracks = true;
            float? thicc = null;
            float duration = 1;
            bool preserveTime = false;
            bool assigncamtotrack = true;
            bool Notes = true;
            bool spline = false;
            float smooth = 0;
            float MapBpm = Startup.Info._beatsPerMinute.toFloat();
            float MapNjs = Startup.InfoDifficulty._noteJumpMovementSpeed.toFloat();
            var customdata = Parameters.CustomDataParse();
            var isNjs = customdata != null && customdata._noteJumpStartBeatOffset != null;


            foreach (var p in Parameters)
            {
                switch (p.Name)
                {
                    case "repeat":
                        repeatcount = Convert.ToInt32(p.Data);
                        break;
                    case "repeataddtime":
                        repeataddtime = Convert.ToSingle(p.Data);
                        break;
                    case "normal":
                        normal = Convert.ToInt32(bool.Parse(p.Data));
                        break;
                    case "path":
                        Path = Startup.ScuffedConfig.MapFolderPath + @"\" + p.Data.removeWhiteSpace();
                        break;
                    case "fullpath":
                        Path = p.Data;
                        break;
                    case "thicc":
                        thicc = float.Parse(p.Data);
                        break;
                    case "colormult":
                        colormult = JsonSerializer.Deserialize<float[]>(p.Data);
                        break;
                    case "hasanimation":
                        hasanimation = Convert.ToBoolean(p.Data);
                        break;
                    case "duration":
                        duration = Convert.ToSingle(p.Data);
                        break;
                    case "spreadspawntime":
                        smooth = Convert.ToSingle(p.Data);
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
                        if (isNjs)
                        {
                            duration = Startup.bpmAdjuster.GetDefiniteDurationBeats(Startup.bpmAdjuster.ToBeat(p.Data.toFloat()), customdata._noteJumpStartBeatOffset.toFloat());
                        }
                        break;
                }
            }
            int walls = 0;
            int notes = 0;
            int customevents = 0;
            for (int i = 0; i < repeatcount; i++)
            {
                ModelSettings settings = new ModelSettings() { spread = smooth, Path = Path, Thicc = thicc, CreateNotes = Notes, DeltaTransformation = null, PreserveTime = preserveTime, technique = (ModelTechnique)normal, AssignCameraToTrack = assigncamtotrack, CreateTracks = tracks, Spline = spline, HasAnimation = hasanimation, BPM = MapBpm, NJS = MapNjs, Offset = Startup.bpmAdjuster.StartBeatOffset, Wall = new BeatMap.Obstacle() { _time = Time + (i.toFloat() * repeataddtime), _duration = duration, _customData = Parameters.CustomDataParse() } };
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
