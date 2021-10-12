using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace ModChart.Wall
{
    class WallModel
    {
        public BeatMap Output { get; private set; } = BeatMap.Empty;
        public Model Model { get; private set; }
        public float NJS { get; private set; }
        public float Offset { get; private set; }
        public BpmAdjuster BPMAdjuster { get; private set; }

        ModelSettings _settings;
        public WallModel(ModelSettings settings)
        {
            this._settings = settings;
            Model = new Model(settings.Path);
            NJS = settings.NJS;
            Offset = settings.Offset;
            BPMAdjuster = new BpmAdjuster(settings.BPM, NJS, Offset);
            GenerateMapObjectsFromModel();
        }
        public WallModel(Model model, ModelSettings settings)
        {
            this._settings = settings;
            Model = model;
            NJS = settings.NJS;
            Offset = settings.Offset;
            BPMAdjuster = new BpmAdjuster(settings.BPM, NJS, Offset);
            GenerateMapObjectsFromModel();
        }
        public WallModel(Cube[] cubes, ModelSettings settings)
        {
            this._settings = settings;
            Model = new Model(cubes);
            NJS = settings.NJS;
            Offset = settings.Offset;
            BPMAdjuster = new BpmAdjuster(settings.BPM, NJS, Offset);
            GenerateMapObjectsFromModel();
        }
        void GenerateMapObjectsFromModel()
        {
            Random rnd = new Random();


            _settings.Wall ??= new BeatMap.Obstacle();
            _settings.Wall._customData ??= new TreeDictionary();
            _settings.Wall._customData["_animation"] ??= new TreeDictionary();
            Output._customData["_customEvents"] ??= new object[] { };

            if (_settings.Wall != null && _settings.Wall._customData != null && _settings.Wall._customData["_noteJumpMovementSpeed"] != null) NJS = _settings.Wall._customData["_noteJumpMovementSpeed"].ToFloat();
            if (_settings.Wall != null && _settings.Wall._customData != null && _settings.Wall._customData["_noteJumpStartBeatOffset"] != null) Offset = _settings.Wall._customData["_noteJumpStartBeatOffset"].ToFloat();

            if (_settings.ObjectOverride == ModelSettings.TypeOverride.AllWalls)
            {
                foreach (var s in Model.Cubes)
                {
                    s.isBomb = false;
                    s.isNote = false;
                }
            }
            else if (_settings.ObjectOverride == ModelSettings.TypeOverride.AllNotes)
            {
                foreach (var s in Model.Cubes)
                {
                    s.isBomb = false;
                    s.isNote = true;
                }
            }
            else if (_settings.ObjectOverride == ModelSettings.TypeOverride.AllBombs)
            {
                foreach (var s in Model.Cubes)
                {
                    s.isBomb = true;
                    s.isNote = false;
                }

            }

            if (_settings.DeltaTransformation != null) Model.Cubes = Cube.TransformCollection(new DeltaTransformOptions()
            {
                cubes = Model.Cubes,
                Position = _settings.DeltaTransformation.Position,
                Rotation = _settings.DeltaTransformation.RotationEul,
                Scale = _settings.DeltaTransformation.Scale.X,
                SetPos = _settings.SetDeltaPos,
                SetScale = _settings.SetDeltaScale
            }).ToArray();

            if (_settings.ColorMult != 1f) foreach (var cube in Model.Cubes) cube.Color *= _settings.ColorMult;

            float realduration = BPMAdjuster.GetRealDuration(_settings.Wall._duration.ToFloat());
            float realstarttime = BPMAdjuster.GetRealTime(_settings.Wall._time.ToFloat());


            //camera
            if (_settings.AssignCameraToPlayerTrack && Model.Cubes.Any(c => c.isCamera && c.Frames != null))
            {
                var camera = Model.Cubes.Where(c => c.isCamera).First();
                camera.Frames = camera.Frames.Select(f =>
                {
                    f.Matrix = f.Matrix.Value.TransformLoc(new Vector3(0, -3, 0));
                    return f;
                }).ToArray();
                camera.Decompose();

                Output._customData["_customEvents"] = Output._customData.at<IEnumerable<object>>("_customEvents").Append(new TreeDictionary()
                {
                    ["_time"] = BPMAdjuster.GetRealTime(_settings.Wall.GetTime()),
                    ["_type"] = "AnimateTrack",
                    ["_data"] = new TreeDictionary()
                    {
                        ["_track"] = camera.Name,
                        ["_duration"] = BPMAdjuster.GetRealDuration(_settings.Wall._duration.ToFloat()),
                        ["_position"] = camera.Frames.Select(f => new object[] { f.Transformation.Position.X * -1, f.Transformation.Position.Y, f.Transformation.Position.Z, f.Number.ToFloat() / camera.Count.ToFloat() }),
                        ["_localRotation"] = camera.Frames.Select(f => new object[] { f.Transformation.RotationEul.X, f.Transformation.RotationEul.Y * -1, f.Transformation.RotationEul.Z * -1, f.Number.ToFloat() / camera.Count.ToFloat() })
                    }
                }).ToArray();
                Output._customData["_customEvents"] = Output._customData.at<IEnumerable<object>>("_customEvents").Append(new TreeDictionary()
                {
                    ["_time"] = 0,
                    ["_type"] = "AssignPlayerToTrack",
                    ["_data"] = new TreeDictionary()
                    {
                        ["_track"] = camera.Name
                    }
                }).ToArray();
            }


            //walls
            List<BeatMap.Obstacle> walls = new List<BeatMap.Obstacle>();


            foreach (var cube in Model.Cubes.Where(c => !c.isBomb && !c.isCamera && !c.isNote))
            {
                var wall = new BeatMap.Obstacle()
                {
                    _time = _settings.Wall.GetTime() + (Convert.ToSingle(rnd.Next(-100, 100)) / 100) * _settings.PCOptimizerPro,
                    _duration = _settings.Wall._duration.ToFloat(),
                    _lineIndex = 0,
                    _width = 0,
                    _type = 0,
                    _customData = new TreeDictionary()
                    {
                        ["_animation"] = new TreeDictionary()
                    }
                };


                switch (_settings.technique)
                {
                    case ModelSettings.Technique.Definite:
                        {
                            wall._time = _settings.Wall._time;
                            if (_settings.Thicc.HasValue)
                            {
                                float thiccoffset = cube.OffsetTransformation.Scale.X - (1f / _settings.Thicc.Value / 2f);
                                wall._customData["_scale"] = new object[] { 1f / _settings.Thicc.Value, 1f / _settings.Thicc.Value, 1f / _settings.Thicc.Value };
                                wall._customData["_animation._definitePosition"] = new object[][] { new object[] { cube.OffsetTransformation.Position.X * -1f + thiccoffset, cube.OffsetTransformation.Position.Y, cube.OffsetTransformation.Position.Z, 0 } };
                                wall._customData["_animation._scale"] = new object[][] { new object[] { cube.OffsetTransformation.Scale.X * 2f * _settings.Thicc.Value, cube.OffsetTransformation.Scale.Y * 2f * _settings.Thicc.Value, cube.OffsetTransformation.Scale.Z * 2f * _settings.Thicc.Value, 0 } };
                            }
                            else
                            {
                                wall._customData["_scale"] = new object[] { cube.OffsetTransformation.Scale.X * 2f, cube.OffsetTransformation.Scale.Y * 2f, cube.OffsetTransformation.Scale.Z * 2f };
                                wall._customData["_animation._definitePosition"] = new object[][] { new object[] { cube.OffsetTransformation.Position.X * -1f, cube.OffsetTransformation.Position.Y, cube.OffsetTransformation.Position.Z, 0 } };
                               // wall._customData["_animation._scale"] = new object[][] { new object[] { 1, 1, 1, 0 } };
                            }

                            if (_settings.HasAnimation && cube.Frames != null && cube.Frames.Any())
                            {
                                wall._duration = BPMAdjuster.GetDefiniteDurationBeats(realduration * ((cube.FrameSpan.Val2.ToFloat() - cube.FrameSpan.Val1.ToFloat()) / cube.Count.Value.ToFloat()));
                                wall._time = BPMAdjuster.GetPlaceTimeBeats(realstarttime + (realduration * (cube.FrameSpan.Val1.ToFloat() / cube.Count.Value.ToFloat())));
                                List<object[]> positionN = new List<object[]>();
                                List<object[]> rotationN = new List<object[]>();
                                List<object[]> scaleN = new List<object[]>();
                                List<object[]> colorN = new List<object[]>();
                                for (int i = 0; i < cube.Frames.Length; i++)
                                {
                                    float TimeStamp = i.ToFloat() / cube.Frames.Length.ToFloat();
                                    if (cube.Frames[i].OffsetTransformation != null)
                                    {
                                        var framerot = new object[] { cube.Frames[i].OffsetTransformation.RotationEul.X, cube.Frames[i].OffsetTransformation.RotationEul.Y * -1, cube.Frames[i].OffsetTransformation.RotationEul.Z * -1, TimeStamp };

                                        if (_settings.Spline) framerot = framerot.Append("splineCatmullRom").ToArray();

                                        rotationN.Add(framerot);
                                        if (_settings.Thicc.HasValue)
                                        {
                                            float thiccoffset = cube.Frames[i].OffsetTransformation.Scale.X - (1f / _settings.Thicc.Value / 2f);
                                            var framescale = new object[] { cube.Frames[i].OffsetTransformation.Scale.X * 2f * _settings.Thicc.Value, cube.Frames[i].OffsetTransformation.Scale.Y * 2f * _settings.Thicc.Value, cube.Frames[i].OffsetTransformation.Scale.Z * 2f * _settings.Thicc.Value, TimeStamp };
                                            var framepos = new object[] { (cube.Frames[i].OffsetTransformation.Position.X * -1f) + thiccoffset, cube.Frames[i].OffsetTransformation.Position.Y - (_settings.Thicc.Value / 2f), cube.Frames[i].OffsetTransformation.Position.Z - (_settings.Thicc.Value / 2f), TimeStamp };

                                            if (_settings.Spline)
                                            {
                                                framescale = framescale.Append("splineCatmullRom").ToArray();
                                                framepos = framepos.Append("splineCatmullRom").ToArray();
                                            }

                                            scaleN.Add(framescale);
                                            positionN.Add(framepos);
                                        }
                                        else
                                        {
                                            var framescale = new object[] { cube.Frames[i].OffsetTransformation.Scale.X / cube.OffsetTransformation.Scale.X, cube.Frames[i].OffsetTransformation.Scale.Y / cube.OffsetTransformation.Scale.Y, cube.Frames[i].OffsetTransformation.Scale.Z / cube.OffsetTransformation.Scale.Z, TimeStamp };
                                            var framepos = new object[] { cube.Frames[i].OffsetTransformation.Position.X * -1f, cube.Frames[i].OffsetTransformation.Position.Y, cube.Frames[i].OffsetTransformation.Position.Z, TimeStamp };

                                            if (_settings.Spline)
                                            {
                                                framescale = framescale.Append("splineCatmullRom").ToArray();
                                                framepos = framepos.Append("splineCatmullRom").ToArray();
                                            }

                                            scaleN.Add(framescale);
                                            positionN.Add(framepos);
                                        }

                                    }
                                    if (cube.Frames[i].Color != null)
                                    {
                                        colorN.Add(new object[] { cube.Frames[i].Color.R, cube.Frames[i].Color.G, cube.Frames[i].Color.B, cube.Frames[i].Color.A, TimeStamp });
                                    }
                                }
                                if (positionN.Any()) wall._customData["_animation._definitePosition"] = positionN.ToArray();
                                if (rotationN.Any()) wall._customData["_animation._localRotation"] = rotationN.ToArray();
                                if (scaleN.Any()) wall._customData["_animation._scale"] = scaleN.ToArray();
                                if (colorN.Any()) wall._customData["_animation._color"] = colorN.ToArray();
                            }
                            else
                            {
                                wall._customData["_localRotation"] = new object[] { cube.OffsetTransformation.RotationEul.X, cube.OffsetTransformation.RotationEul.Y * -1, cube.OffsetTransformation.RotationEul.Z * -1 };
                            }
                        }
                        break;
                    case ModelSettings.Technique.Normal:
                        {

                            wall._customData["_localRotation"] = new object[] { cube.OffsetTransformation.RotationEul.X, cube.OffsetTransformation.RotationEul.Y * -1f, cube.OffsetTransformation.RotationEul.Z * -1f };

                            float beatlength = (5f / 3f * (60f / _settings.BPM) * NJS);

                            Vector2 pos = new Vector2(0);
                            if (_settings.Wall._customData != null && _settings.Wall._customData["_position"] != null && _settings.Wall._customData.at<IEnumerable<object>>("_position").Any()) pos = _settings.Wall._customData.at<IEnumerable<object>>("_position").ToArray().ToVector2();

                            if (_settings.Thicc.HasValue)
                            {
                                float thiccoffset = cube.OffsetTransformation.Scale.X - (1f / _settings.Thicc.Value / 2f);
                                wall._customData["_position"] = new object[] { (((cube.OffsetTransformation.Position.X * -1f) - 2f) + thiccoffset) + pos.X, cube.OffsetTransformation.Position.Y + pos.Y };
                                wall._customData["_scale"] = new object[] { 1 / _settings.Thicc.Value, 1 / _settings.Thicc.Value, 1 / _settings.Thicc.Value };
                                wall._customData["_animation._scale"] = new object[][] { new object[] { cube.OffsetTransformation.Scale.X * 2f * _settings.Thicc.Value, cube.OffsetTransformation.Scale.Y * 2f * _settings.Thicc.Value, cube.OffsetTransformation.Scale.Z * 2f * _settings.Thicc.Value, 0 } };
                            }
                            else
                            {
                                wall._customData["_position"] = new object[] { ((cube.OffsetTransformation.Position.X * -1f) - 2f) + pos.X, cube.OffsetTransformation.Position.Y + pos.Y };
                                wall._customData["_scale"] = new object[] { cube.OffsetTransformation.Scale.X * 2f, cube.OffsetTransformation.Scale.Y * 2f, cube.OffsetTransformation.Scale.Z * 2f };
                            }


                            if (!_settings.PreserveTime)
                            {
                                wall._duration = (cube.OffsetTransformation.Scale.Z * 2f) / beatlength;
                                wall._time = (cube.OffsetTransformation.Position.Z / beatlength) + _settings.Wall.GetTime() + (Convert.ToSingle(rnd.Next(-100, 100)) / 100) * _settings.PCOptimizerPro;
                            }
                            else
                            {
                                wall._duration = cube.OffsetTransformation.Scale.Z * 2f;
                                wall._time = cube.OffsetTransformation.Position.Z + (Convert.ToSingle(rnd.Next(-100, 100)) / 100) * _settings.PCOptimizerPro;
                            }

                            if(!_settings.ScaleDuration)
                            {
                                wall._duration = _settings.Wall._duration;
                            }

                        }
                        break;
                }

                if (cube.Color != null)
                {
                    float alpha = cube.Color.A;
                    if (_settings.Alpha.HasValue) alpha = _settings.Alpha.Value;
                    wall._customData["_color"] = new object[] { cube.Color.R, cube.Color.G, cube.Color.B, alpha };
                }
                if (_settings.Wall._customData["_color"] != null) wall._customData["_color"] = _settings.Wall._customData["_color"];
                if (_settings.CreateTracks && !string.IsNullOrEmpty(cube.Track)) wall._customData["_track"] = cube.Track;
                if (_settings.DefaultTrack != null && _settings.DefaultTrack != "") wall._customData[BeatMap._track] = _settings.DefaultTrack; 
                walls.Add((BeatMap.Obstacle)wall.Append(_settings.Wall, AppendPriority.Low));

            }
            Output._obstacles = walls.ToList();

            //notes and bombs
            List<BeatMap.Note> notes = new List<BeatMap.Note>();
            foreach (var cube in Model.Cubes.Where(c => c.isBomb || c.isNote))
            {
                BeatMap.Note.NoteType type = BeatMap.Note.NoteType.Right;
                if (cube.isBomb) type = BeatMap.Note.NoteType.Bomb;

                var note = new BeatMap.Note()
                {
                    _time = _settings.Wall.GetTime() + (Convert.ToSingle(rnd.Next(-100, 100)) / 100) * _settings.PCOptimizerPro,
                    _lineIndex = 0,
                    _cutDirection = BeatMap.Note.CutDirection.Down,
                    _lineLayer = 0,
                    _type = type,
                    _customData = new TreeDictionary()
                    {
                        ["_animation"] = new TreeDictionary()
                    }
                };


                float notesizefactor = 2.25f;
                switch (_settings.technique)
                {
                    case ModelSettings.Technique.Definite:
                        {
                            note._time = realduration/2f + realstarttime;
                            note._customData["_noteJumpStartBeatOffset"] = BPMAdjuster.GetDefiniteNjsOffsetBeats(realduration);
                            note._customData["_animation._definitePosition"] = new object[][] { new object[] { cube.Transformation.Position.X * -1f, cube.Transformation.Position.Y, cube.Transformation.Position.Z, 0 } };
                            note._customData["_animation._scale"] = new object[][] { new object[] { cube.Transformation.Scale.X * notesizefactor, cube.Transformation.Scale.Y * notesizefactor, cube.Transformation.Scale.Z * notesizefactor, 0 } };
                            note._customData["_animation._localRotation"] = new object[][] { new object[] { cube.Transformation.RotationEul.X, cube.Transformation.RotationEul.Y * -1, cube.Transformation.RotationEul.Z * -1, 0 } };
                            if (_settings.HasAnimation && cube.Frames != null && cube.Frames.Any())
                            {
                                float defnjsoffset = BPMAdjuster.GetDefiniteNjsOffsetBeats((realduration * ((cube.FrameSpan.Val2.ToFloat() - cube.FrameSpan.Val1.ToFloat()) / cube.Count.Value.ToFloat())));

                                note._time = (realstarttime + (realduration * (cube.FrameSpan.Val1.ToFloat() / cube.Count.Value.ToFloat()))) + BPMAdjuster.GetJumps(defnjsoffset);
                                note._customData["_noteJumpStartBeatOffset"] = defnjsoffset;

                                List<object[]> positionN = new List<object[]>();
                                List<object[]> rotationN = new List<object[]>();
                                List<object[]> scaleN = new List<object[]>();
                                List<object[]> colorN = new List<object[]>();
                                for (int i = 0; i < cube.Frames.Length; i++)
                                {
                                    float TimeStamp = i.ToFloat() / cube.Frames.Length.ToFloat();
                                    if (cube.Frames[i].Transformation != null)
                                    {
                                        positionN.Add(new object[] { cube.Frames[i].Transformation.Position.X * -1f, cube.Frames[i].Transformation.Position.Y, cube.Frames[i].Transformation.Position.Z, TimeStamp });
                                        rotationN.Add(new object[] { cube.Frames[i].Transformation.RotationEul.X, cube.Frames[i].Transformation.RotationEul.Y * -1, cube.Frames[i].Transformation.RotationEul.Z * -1, TimeStamp });
                                        scaleN.Add(new object[] { cube.Frames[i].Transformation.Scale.X * notesizefactor, cube.Frames[i].Transformation.Scale.Y * notesizefactor, cube.Frames[i].Transformation.Scale.Z * notesizefactor, TimeStamp });
                                    }
                                    if (cube.Frames[i].Color != null)
                                    {
                                        colorN.Add(new object[] { cube.Frames[i].Color.R, cube.Frames[i].Color.G, cube.Frames[i].Color.B, cube.Frames[i].Color.A, TimeStamp });
                                    }
                                }
                                if (positionN.Any()) note._customData["_animation._definitePosition"] = positionN.ToArray();
                                if (rotationN.Any()) note._customData["_animation._localRotation"] = rotationN.ToArray();
                                if (scaleN.Any()) note._customData["_animation._scale"] = scaleN.ToArray();
                                if (colorN.Any()) note._customData["_animation._color"] = colorN.ToArray();
                            }
                        }
                        break;
                    case ModelSettings.Technique.Normal:
                        {
                            note._customData["_position"] = new object[] { (cube.Transformation.Position.X * -1f) - 2f, cube.Transformation.Position.Y };
                            note._customData["_localRotation"] = new object[] { cube.Transformation.RotationEul.X, cube.Transformation.RotationEul.Y * -1f, cube.Transformation.RotationEul.Z * -1f };
                            note._customData["_animation._scale"] = new object[][] { new object[] { cube.Transformation.Scale.X * notesizefactor, cube.Transformation.Scale.Y * notesizefactor, cube.Transformation.Scale.Z * notesizefactor, 0 } };
                            float beatlength = (5f / 3f * (60f / _settings.BPM) * NJS);

                            if (!_settings.PreserveTime)
                            {
                                note._time = (cube.Transformation.Position.Z / beatlength) + _settings.Wall.GetTime();
                            }
                        }
                        break;
                }

                if (cube.Color != null) note._customData["_color"] = new object[] { cube.Color.R, cube.Color.G, cube.Color.B, cube.Color.A };
                if (_settings.Wall._customData["_color"] != null) note._customData["_color"] = _settings.Wall._customData["_color"];
                if (_settings.CreateTracks && string.IsNullOrEmpty(cube.Track)) note._customData["_track"] = cube.Track;
                if (_settings.DefaultTrack != null && _settings.DefaultTrack != "") note._customData[BeatMap._track] = _settings.DefaultTrack;

                notes.Add((BeatMap.Note)note.Append(_settings.Wall, AppendPriority.Low));
            }
            Output._notes = notes;
        }
    }
    public class ModelSettings
    {
        public string DefaultTrack { get; set; }
        public string Path { get; set; }
        /// <summary>
        /// (spreadspawntime)
        /// </summary>
        public float PCOptimizerPro { get; set; }
        public Technique technique { get; set; }
        public bool HasAnimation { get; set; }
        public BeatMap.Obstacle Wall { get; set; }
        public float NJS { get; set; }
        public float BPM { get; set; }
        public float Offset { get; set; }
        public float? Thicc { get; set; }
        public float ColorMult { get; set; } = 1;
        public TypeOverride ObjectOverride { get; set; }
        public bool PreserveTime { get; set; }
        public Transformation DeltaTransformation { get; set; }
        public bool SetDeltaPos { get; set; }
        public bool SetDeltaScale { get; set; }
        public bool Spline { get; set; }
        public bool AssignCameraToPlayerTrack { get; set; }
        public bool CreateTracks { get; set; }
        public float? Alpha { get; set; }
        public bool CreateNotes { get; set; }
        public bool ScaleDuration { get; set; } = true;
        public enum Technique
        {
            Definite,
            Normal
        }
        public enum TypeOverride
        {
            ModelDefined,
            AllWalls,
            AllBombs,
            AllNotes
        }

    }

}