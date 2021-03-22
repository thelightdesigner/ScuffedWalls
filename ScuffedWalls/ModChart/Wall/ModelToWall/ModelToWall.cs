using ModChart.Note;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace ModChart.Wall
{
    class WallModel
    {
        /// <summary>
        /// yeaaaaaaaaaaaa.....
        /// </summary>
        public BeatMap Output { get; private set; } = BeatMap.Empty;
        public Model Model { get; private set; }
        public float NJS { get; private set; }
        public float Offset { get; private set; }
        public BpmAdjuster BPMAdjuster { get; private set; }

        ModelSettings settings;
        public WallModel(ModelSettings settings)
        {
            this.settings = settings;
            Model = new Model(settings.Path);
            NJS = settings.NJS;
            Offset = settings.Offset;
            BPMAdjuster = new BpmAdjuster(settings.BPM, NJS, Offset);
            SetWalls();
        }
        void SetWalls()
        {
            Random rnd = new Random();

            if (settings.Wall._customData._noteJumpMovementSpeed != null) NJS = settings.Wall._customData._noteJumpMovementSpeed.toFloat();
            if (settings.Wall._customData._noteJumpStartBeatOffset != null) Offset = settings.Wall._customData._noteJumpStartBeatOffset.toFloat();

            if (settings.ObjectOverride == ModelSettings.TypeOverride.Walls)
            {
                foreach (var s in Model.OffsetCorrectedCubes)
                {
                    s.isBomb = false;
                    s.isNote = false;
                }
                foreach (var s in Model.Cubes)
                {
                    s.isBomb = false;
                    s.isNote = false;
                }
            }
            else if (settings.ObjectOverride == ModelSettings.TypeOverride.Notes)
            {
                foreach (var s in Model.OffsetCorrectedCubes)
                {
                    s.isBomb = false;
                    s.isNote = true;
                }
                foreach (var s in Model.Cubes)
                {
                    s.isBomb = false;
                    s.isNote = true;
                }
            }
            else if (settings.ObjectOverride == ModelSettings.TypeOverride.Bombs)
            {
                foreach (var s in Model.OffsetCorrectedCubes)
                {
                    s.isBomb = true;
                    s.isNote = false;
                }
                foreach (var s in Model.Cubes)
                {
                    s.isBomb = true;
                    s.isNote = false;
                }

            }

            Model.OffsetCorrectedCubes = Cube.TransformCollection(Model.OffsetCorrectedCubes,settings.DeltaTransformation.Position, settings.DeltaTransformation.RotationEul, settings.DeltaTransformation.Scale.X).ToArray();
            Model.Cubes = Cube.TransformCollection(Model.Cubes, settings.DeltaTransformation.Position, settings.DeltaTransformation.RotationEul, settings.DeltaTransformation.Scale.X).ToArray();

            float realduration = BPMAdjuster.GetRealDuration(settings.Wall._duration.toFloat());
            float realstarttime = BPMAdjuster.GetRealTime(settings.Wall._time.toFloat());


            //camera
            if (settings.AssignCameraToTrack && Model.Cubes.Any(c => c.isCamera))
            {
                var camera = Model.Cubes.Where(c => c.isCamera).First();
                camera.Frames = camera.Frames.Select(f =>
                {
                    f.Matrix = f.Matrix.Value.TransformLoc(new System.Numerics.Vector3(0, -3, 0));
                    return f;
                }).ToArray();
                camera.Decompose();

                Output._customData._customEvents = Output._customData._customEvents.Append(new BeatMap.CustomData.CustomEvents()
                {
                    _time = BPMAdjuster.GetRealTime(settings.Wall.GetTime()),
                    _type = "AnimateTrack",
                    _data =
                    {
                        _track = camera.Name,
                        _duration = BPMAdjuster.GetRealDuration(settings.Wall._duration.toFloat()),
                        _position = camera.Frames.Select(f => new object[] { f.Transformation.Position.X * -1, f.Transformation.Position.Y, f.Transformation.Position.Z, f.Number.toFloat() / camera.Count.toFloat() }),
                        _localRotation = camera.Frames.Select(f => new object[] { f.Transformation.RotationEul.X, f.Transformation.RotationEul.Y * -1, f.Transformation.RotationEul.Z * -1, f.Number.toFloat() / camera.Count.toFloat() })
                    }
                }).ToArray();
                Output._customData._customEvents = Output._customData._customEvents.Append(new BeatMap.CustomData.CustomEvents()
                {
                    _time = 0,
                    _type = "AssignPlayerToTrack",
                    _data =
                    {
                        _track = camera.Name
                    }
                }).ToArray();
            }


            //walls
            List<BeatMap.Obstacle> walls = new List<BeatMap.Obstacle>();
            foreach (var cube in Model.OffsetCorrectedCubes.Where(c => !c.isBomb && !c.isCamera && !c.isNote))
            {
                var wall = new BeatMap.Obstacle()
                {
                    _time = settings.Wall.GetTime() + (Convert.ToSingle(rnd.Next(-100, 100)) / 100) * settings.spread,
                    _duration = settings.Wall._duration.toFloat(),
                    _lineIndex = 0,
                    _width = 0,
                    _type = 0,
                    _customData = new BeatMap.CustomData()
                    {
                        _animation = new BeatMap.CustomData.Animation()
                    }
                };


                switch (settings.technique)
                {
                    case ModelSettings.Technique.Definite:
                        {
                            wall._time = settings.Wall._time;
                            if (settings.Thicc.HasValue)
                            {
                                float thiccoffset = cube.Transformation.Scale.X - (1f / settings.Thicc.Value / 2f);
                                wall._customData._scale = new object[] { 1f / settings.Thicc.Value, 1f / settings.Thicc.Value, 1f / settings.Thicc.Value };
                                wall._customData._animation._definitePosition = new object[][] { new object[] { cube.Transformation.Position.X * -1f + thiccoffset, cube.Transformation.Position.Y, cube.Transformation.Position.Z, 0 } };
                                wall._customData._animation._scale = new object[][] { new object[] { cube.Transformation.Scale.X * 2f * settings.Thicc.Value, cube.Transformation.Scale.Y * 2f * settings.Thicc.Value, cube.Transformation.Scale.Z * 2f * settings.Thicc.Value, 0 } };
                            }
                            else
                            {
                                wall._customData._scale = new object[] { cube.Transformation.Scale.X * 2f, cube.Transformation.Scale.Y * 2f, cube.Transformation.Scale.Z * 2f };
                                wall._customData._animation._definitePosition = new object[][] { new object[] { cube.Transformation.Position.X * -1f, cube.Transformation.Position.Y, cube.Transformation.Position.Z, 0 } };
                                wall._customData._animation._scale = new object[][] { new object[] { 1, 1, 1, 0 } };
                            }

                            wall._customData._animation._localRotation = new object[][] { new object[] { cube.Transformation.RotationEul.X, cube.Transformation.RotationEul.Y * -1, cube.Transformation.RotationEul.Z * -1, 0 } };

                            if (settings.HasAnimation && cube.Frames != null && cube.Frames.Any())
                            {
                                wall._duration = BPMAdjuster.GetDefiniteDurationBeats(realduration * ((cube.FrameSpan.Val2.toFloat() - cube.FrameSpan.Val1.toFloat()) / cube.Count.Value.toFloat()));
                                wall._time = BPMAdjuster.GetPlaceTimeBeats(realstarttime + (realduration * (cube.FrameSpan.Val1.toFloat() / cube.Count.Value.toFloat())));
                                List<object[]> positionN = new List<object[]>();
                                List<object[]> rotationN = new List<object[]>();
                                List<object[]> scaleN = new List<object[]>();
                                List<object[]> colorN = new List<object[]>();
                                for (int i = 0; i < cube.Frames.Length; i++)
                                {
                                    float TimeStamp = i.toFloat() / cube.Frames.Length.toFloat();
                                    if (cube.Frames[i].Transformation != null)
                                    {
                                        var framerot = new object[] { cube.Frames[i].Transformation.RotationEul.X, cube.Frames[i].Transformation.RotationEul.Y * -1, cube.Frames[i].Transformation.RotationEul.Z * -1, TimeStamp };

                                        if (settings.Spline) framerot = framerot.Append("splineCatmullRom").ToArray();

                                        rotationN.Add(framerot);
                                        if (settings.Thicc.HasValue)
                                        {
                                            var framescale = new object[] { cube.Frames[i].Transformation.Scale.X * settings.Thicc.Value, cube.Frames[i].Transformation.Scale.Y * settings.Thicc.Value, cube.Frames[i].Transformation.Scale.Z * settings.Thicc.Value, TimeStamp };
                                            var framepos = new object[] { (cube.Frames[i].Transformation.Position.X * -1f) - (settings.Thicc.Value / 2f), cube.Frames[i].Transformation.Position.Y - (settings.Thicc.Value / 2f), cube.Frames[i].Transformation.Position.Z - (settings.Thicc.Value / 2f), TimeStamp };

                                            if (settings.Spline)
                                            {
                                                framescale = framescale.Append("splineCatmullRom").ToArray();
                                                framepos = framepos.Append("splineCatmullRom").ToArray();
                                            }

                                            scaleN.Add(framescale);
                                            positionN.Add(framepos);
                                        }
                                        else
                                        {
                                            var framescale = new object[] { cube.Frames[i].Transformation.Scale.X / cube.Transformation.Scale.X, cube.Frames[i].Transformation.Scale.Y / cube.Transformation.Scale.Y, cube.Frames[i].Transformation.Scale.Z / cube.Transformation.Scale.Z, TimeStamp };
                                            var framepos = new object[] { cube.Frames[i].Transformation.Position.X * -1f, cube.Frames[i].Transformation.Position.Y, cube.Frames[i].Transformation.Position.Z, TimeStamp };

                                            if (settings.Spline)
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
                                if (positionN.Any()) wall._customData._animation._definitePosition = positionN.ToArray();
                                if (rotationN.Any()) wall._customData._animation._localRotation = rotationN.ToArray();
                                if (scaleN.Any()) wall._customData._animation._scale = scaleN.ToArray();
                                if (colorN.Any()) wall._customData._animation._color = colorN.ToArray();
                            }
                        }
                        break;
                    case ModelSettings.Technique.Normal:
                        {

                            wall._customData._localRotation = new object[] { cube.Transformation.RotationEul.X, cube.Transformation.RotationEul.Y * -1f, cube.Transformation.RotationEul.Z * -1f };

                            float beatlength = (5f / 3f * (60f / settings.BPM) * NJS);

                            if (settings.Thicc.HasValue)
                            {
                                wall._customData._position = new object[] { ((cube.Transformation.Position.X * -1f) - 2f), cube.Transformation.Position.Y };
                                wall._customData._scale = new object[] { 1 / settings.Thicc.Value, 1 / settings.Thicc.Value, 1 / settings.Thicc.Value };
                                wall._customData._animation._scale = new object[][] { new object[] { cube.Transformation.Scale.X * settings.Thicc.Value, cube.Transformation.Scale.Y * settings.Thicc.Value, cube.Transformation.Scale.Z * settings.Thicc.Value, 0 } };
                            }
                            else
                            {
                                wall._customData._position = new object[] { (cube.Transformation.Position.X * -1f) - 2f, cube.Transformation.Position.Y };
                                wall._customData._scale = new object[] { cube.Transformation.Scale.X * 2f, cube.Transformation.Scale.Y * 2f };
                            }


                            if (!settings.PreserveTime)
                            {
                                wall._duration = (cube.Transformation.Scale.Z * 2f) / beatlength;
                                wall._time = (cube.Transformation.Position.Z / beatlength) + settings.Wall.GetTime();
                            }
                            else
                            {
                                wall._duration = cube.Transformation.Scale.Z * 2f;
                                wall._time = cube.Transformation.Position.Z;
                            }
                        }
                        break;
                }
                if (cube.Color != null) wall._customData._color = new object[] { cube.Color.R, cube.Color.G, cube.Color.B, cube.Color.A };
                if (settings.Wall._customData._color != null) wall._customData._color = settings.Wall._customData._color;
                if (settings.CreateTracks && !string.IsNullOrEmpty(cube.Track)) wall._customData._track = cube.Track;

                walls.Add(wall.Append(settings.Wall._customData, AppendTechnique.NoOverwrites));

            }
            Output._obstacles = walls.ToArray();

            //notes and bombs
            List<BeatMap.Note> notes = new List<BeatMap.Note>();
            foreach (var cube in Model.Cubes.Where(c => c.isBomb || c.isNote))
            {
                int type = 0;
                if (cube.isBomb) type = 3;

                var note = new BeatMap.Note()
                {
                    _time = settings.Wall.GetTime() + (Convert.ToSingle(rnd.Next(-100, 100)) / 100) * settings.spread,
                    _lineIndex = 0,
                    _cutDirection = 1,
                    _lineLayer = 0,
                    _type = type,
                    _customData = new BeatMap.CustomData()
                    {
                        _animation = new BeatMap.CustomData.Animation()
                    }
                };


                float notesizefactor = 2.33f;
                switch (settings.technique)
                {
                    case ModelSettings.Technique.Definite:
                        {
                            note._time = settings.Wall._time;
                            note._customData._animation._definitePosition = new object[][] { new object[] { cube.Transformation.Position.X * -1f, cube.Transformation.Position.Y, cube.Transformation.Position.Z, 0 } };
                            note._customData._animation._scale = new object[][] { new object[] { cube.Transformation.Scale.X * notesizefactor, cube.Transformation.Scale.Y * notesizefactor, cube.Transformation.Scale.Z * notesizefactor, 0 } };
                            note._customData._animation._localRotation = new object[][] { new object[] { cube.Transformation.RotationEul.X, cube.Transformation.RotationEul.Y * -1, cube.Transformation.RotationEul.Z * -1, 0 } };
                            if (settings.HasAnimation && cube.Frames != null && cube.Frames.Any())
                            {
                                float defnjsoffset = BPMAdjuster.GetDefiniteNjsOffsetBeats((realduration * ((cube.FrameSpan.Val2.toFloat() - cube.FrameSpan.Val1.toFloat()) / cube.Count.Value.toFloat())));

                                note._time = (realstarttime + (realduration * (cube.FrameSpan.Val1.toFloat() / cube.Count.Value.toFloat()))) + BPMAdjuster.GetJumps(defnjsoffset);
                                note._customData._noteJumpStartBeatOffset = defnjsoffset;

                                //note._time = bpmAdjuster.GetPlaceTimeBeats(realstarttime + (realduration * (cube.FrameSpan.Val1.toFloat() / cube.Count.Value.toFloat())));
                                List<object[]> positionN = new List<object[]>();
                                List<object[]> rotationN = new List<object[]>();
                                List<object[]> scaleN = new List<object[]>();
                                List<object[]> colorN = new List<object[]>();
                                for (int i = 0; i < cube.Frames.Length; i++)
                                {
                                    float TimeStamp = i.toFloat() / cube.Frames.Length.toFloat();
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
                                if (positionN.Any()) note._customData._animation._definitePosition = positionN.ToArray();
                                if (rotationN.Any()) note._customData._animation._localRotation = rotationN.ToArray();
                                if (scaleN.Any()) note._customData._animation._scale = scaleN.ToArray();
                                if (colorN.Any()) note._customData._animation._color = colorN.ToArray();
                            }
                        }
                        break;
                    case ModelSettings.Technique.Normal:
                        {
                            note._customData._position = new object[] { (cube.Transformation.Position.X * -1f) - 2f, cube.Transformation.Position.Y };
                            note._customData._localRotation = new object[] { cube.Transformation.RotationEul.X, cube.Transformation.RotationEul.Y * -1f, cube.Transformation.RotationEul.Z * -1f };
                            note._customData._animation._scale = new object[][] { new object[] { cube.Transformation.Scale.X * notesizefactor, cube.Transformation.Scale.Y * notesizefactor, cube.Transformation.Scale.Z * notesizefactor } };
                            float beatlength = (5f / 3f * (60f / settings.BPM) * NJS);

                            if (!settings.PreserveTime)
                            {
                                note._time = (cube.Transformation.Position.Z / beatlength) + settings.Wall.GetTime();
                            }
                            else
                            {
                                note._time = cube.Transformation.Position.Z;
                            }
                        }
                        break;
                }

                if (cube.Color != null) note._customData._color = new object[] { cube.Color.R, cube.Color.G, cube.Color.B, cube.Color.A };
                if (settings.Wall._customData._color != null) note._customData._color = settings.Wall._customData._color;
                if (settings.CreateTracks && string.IsNullOrEmpty(cube.Track)) note._customData._track = cube.Track;

                notes.Add(note.Append(settings.Wall._customData, AppendTechnique.NoOverwrites));
            }
            Output._notes = notes.ToArray();
        }
    }
    public class ModelSettings
    {

        public string Path { get; set; }
        public float spread { get; set; }
        public Technique technique { get; set; }
        /// <summary>
        /// controls normal and definite
        /// </summary>
        public bool HasAnimation { get; set; }
        public BeatMap.Obstacle Wall { get; set; }
        public float NJS { get; set; }
        public float BPM { get; set; }
        public float Offset { get; set; }
        public float? Thicc { get; set; }
        public TypeOverride ObjectOverride { get; set; }
        public bool PreserveTime { get; set; }
        public Transformation DeltaTransformation { get; set; }
        public bool Spline { get; set; }
        public bool AssignCameraToTrack { get; set; }
        public bool CreateTracks { get; set; }
        public bool CreateNotes { get; set; }
        public enum Technique
        {
            Definite,
            Normal
        }
        public enum TypeOverride
        {
            ModelDefined,
            Walls,
            Bombs,
            Notes
        }

    }

}


