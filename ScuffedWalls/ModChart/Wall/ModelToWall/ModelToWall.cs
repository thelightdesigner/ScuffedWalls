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

        ModelSettings settings;
        public WallModel(ModelSettings settings)
        {
            this.settings = settings;
            SetWalls();
        }
        void SetWalls()
        {
            Model model = new Model(settings.Path);
            Random rnd = new Random();
            float NJS = settings.NJS;
            float Offset = settings.Offset;
            if (settings.Wall._customData._noteJumpMovementSpeed != null) NJS = settings.Wall._customData._noteJumpMovementSpeed.toFloat();
            if (settings.Wall._customData._noteJumpStartBeatOffset != null) Offset = settings.Wall._customData._noteJumpStartBeatOffset.toFloat();
            BpmAdjuster bpmAdjuster = new BpmAdjuster(settings.BPM, NJS, Offset);
            float realduration = bpmAdjuster.GetRealDuration(settings.Wall._duration.toFloat());
            float realstarttime = bpmAdjuster.GetRealTime(settings.Wall._time.toFloat());

            //camera
            if (settings.AssignCameraToTrack && model.Cubes.Any(c => c.isCamera))
            {
                var camera = model.Cubes.Where(c => c.isCamera).First();
                camera.Frames = camera.Frames.Select(f =>
                {
                    f.Matrix = f.Matrix.Value.TransformLoc(new System.Numerics.Vector3(0, -3, 0));
                    return f;
                }).ToArray();
                camera.Decompose();

                Output._customData._customEvents = Output._customData._customEvents.Append(new BeatMap.CustomData.CustomEvents()
                {
                    _time = bpmAdjuster.GetRealTime(settings.Wall.GetTime()),
                    _type = "AnimateTrack",
                    _data = new BeatMap.CustomData.CustomEvents.Data()
                    {
                        _track = camera.Name,
                        _duration = bpmAdjuster.GetRealDuration(settings.Wall._duration.toFloat()),
                        _position = camera.Frames.Select(f => new object[] { f.Transformation.Position.X * -1, f.Transformation.Position.Y, f.Transformation.Position.Z, f.Number.toFloat() / camera.Count.toFloat() }),
                        _localRotation = camera.Frames.Select(f => new object[] { f.Transformation.Rotation.X, f.Transformation.Rotation.Y * -1, f.Transformation.Rotation.Z * -1, f.Number.toFloat() / camera.Count.toFloat() })
                    }
                }).ToArray();
                Output._customData._customEvents = Output._customData._customEvents.Append(new BeatMap.CustomData.CustomEvents()
                {
                    _time = 0,
                    _type = "AssignPlayerToTrack",
                    _data = new BeatMap.CustomData.CustomEvents.Data()
                    {
                        _track = camera.Name
                    }
                }).ToArray();
            }

            //walls
            List<BeatMap.Obstacle> walls = new List<BeatMap.Obstacle>();
            foreach (var cube in model.OffsetCorrectedCubes.Where(c => !c.isBomb && !c.isCamera && !c.isNote))
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

                // var sdsdgnjsg = cube.Matrix.Value.GetBoundingBox();
                // Console.WriteLine($"bounding box: X{sdsdgnjsg.X} Y{sdsdgnjsg.Y} Z{sdsdgnjsg.Z}");

                switch (settings.technique)
                {
                    case ModelTechnique.Definite:
                        {
                            wall._time = settings.Wall._time;
                            if (settings.Thicc.HasValue)
                            {
                                wall._customData._scale = new object[] { 1f / settings.Thicc.Value, 1f / settings.Thicc.Value, 1f / settings.Thicc.Value };
                                wall._customData._animation._definitePosition = new object[][] { new object[] { cube.Transformation.Position.X - (settings.Thicc.Value / 2f), cube.Transformation.Position.Y, cube.Transformation.Position.Z, 0 } };
                                wall._customData._animation._scale = new object[][] { new object[] { cube.Transformation.Scale.X * settings.Thicc.Value, cube.Transformation.Scale.Y * settings.Thicc.Value, cube.Transformation.Scale.Z * settings.Thicc.Value, 0 } };
                            }
                            else
                            {
                                wall._customData._scale = new object[] { cube.Transformation.Scale.X * 2f, cube.Transformation.Scale.Y * 2f, cube.Transformation.Scale.Z * 2f };
                                wall._customData._animation._definitePosition = new object[][] { new object[] { cube.Transformation.Position.X, cube.Transformation.Position.Y, cube.Transformation.Position.Z, 0 } };
                                wall._customData._animation._scale = new object[][] { new object[] { cube.Transformation.Scale.X, cube.Transformation.Scale.Y, cube.Transformation.Scale.Z, 0 } };
                            }

                            wall._customData._animation._localRotation = new object[][] { new object[] { cube.Transformation.Rotation.X, cube.Transformation.Rotation.Y, cube.Transformation.Rotation.Z, 0 } };

                            if (settings.HasAnimation && cube.Frames != null && cube.Frames.Any())
                            {
                                wall._duration = bpmAdjuster.GetDefiniteDurationBeats(realduration * ((cube.FrameSpan.Val2.toFloat() - cube.FrameSpan.Val1.toFloat()) / cube.Count.Value.toFloat()));
                                wall._time = bpmAdjuster.GetPlaceTimeBeats(realstarttime + (realduration * (cube.FrameSpan.Val1.toFloat() / cube.Count.Value.toFloat())));
                                List<object[]> positionN = new List<object[]>();
                                List<object[]> rotationN = new List<object[]>();
                                List<object[]> scaleN = new List<object[]>();
                                List<object[]> colorN = new List<object[]>();
                                for (int i = 0; i < cube.Frames.Length; i++)
                                {
                                    float TimeStamp = i.toFloat() / cube.Frames.Length.toFloat();
                                    if (cube.Frames[i].Transformation != null)
                                    {

                                        rotationN.Add(new object[] { cube.Frames[i].Transformation.Rotation.X, cube.Frames[i].Transformation.Rotation.Y * -1, cube.Frames[i].Transformation.Rotation.Z * -1, TimeStamp });
                                        if (settings.Thicc.HasValue)
                                        {
                                            scaleN.Add(new object[] { cube.Frames[i].Transformation.Scale.X * settings.Thicc.Value, cube.Frames[i].Transformation.Scale.Y * settings.Thicc.Value, cube.Frames[i].Transformation.Scale.Z * settings.Thicc.Value, TimeStamp });
                                            positionN.Add(new object[] { (cube.Frames[i].Transformation.Position.X * -1f) - (settings.Thicc.Value / 2f), cube.Frames[i].Transformation.Position.Y - (settings.Thicc.Value / 2f), cube.Frames[i].Transformation.Position.Z - (settings.Thicc.Value / 2f), TimeStamp });
                                        }
                                        else
                                        {
                                            scaleN.Add(new object[] { cube.Frames[i].Transformation.Scale.X / cube.Transformation.Scale.X, cube.Frames[i].Transformation.Scale.Y / cube.Transformation.Scale.Y, cube.Frames[i].Transformation.Scale.Z / cube.Transformation.Scale.Z, TimeStamp });
                                            positionN.Add(new object[] { cube.Frames[i].Transformation.Position.X * -1f, cube.Frames[i].Transformation.Position.Y, cube.Frames[i].Transformation.Position.Z, TimeStamp });
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
                    case ModelTechnique.Normal:
                        {
                            wall._customData._position = new object[] { (cube.Transformation.Position.X * -1f) - 2f, cube.Transformation.Position.Y };
                            wall._customData._localRotation = new object[] { cube.Transformation.Rotation.X, cube.Transformation.Rotation.Y * -1f, cube.Transformation.Rotation.Z * -1f };
                            wall._customData._scale = new object[] { cube.Transformation.Scale.X * 2f, cube.Transformation.Scale.Y * 2f };
                            float beatlength = (5f / 3f * (60f / settings.BPM) * NJS);

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
                if (settings.CreateTracks && string.IsNullOrEmpty(cube.Track)) wall._customData._track = cube.Track;

                walls.Add(wall.Append(settings.Wall._customData, AppendTechnique.NoOverwrites));

            }
            Output._obstacles = walls.ToArray();

            //notes and bombs
            List<BeatMap.Note> notes = new List<BeatMap.Note>();
            foreach (var cube in model.Cubes.Where(c => c.isBomb || c.isNote))
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
                    case ModelTechnique.Definite:
                        {
                            note._time = settings.Wall._time;
                            note._customData._animation._definitePosition = new object[][] { new object[] { cube.Transformation.Position.X, cube.Transformation.Position.Y, cube.Transformation.Position.Z, 0 } };
                            note._customData._animation._localRotation = new object[][] { new object[] { cube.Transformation.Rotation.X, cube.Transformation.Rotation.Y, cube.Transformation.Rotation.Z, 0 } };
                            note._customData._animation._scale = new object[][] { new object[] { cube.Transformation.Scale.X * notesizefactor, cube.Transformation.Scale.Y * notesizefactor, cube.Transformation.Scale.Z * notesizefactor, 0 } };
                            if (settings.HasAnimation && cube.Frames != null && cube.Frames.Any())
                            {
                                note._customData._noteJumpStartBeatOffset = bpmAdjuster.GetDefiniteNjsOffsetBeats(realduration * ((cube.FrameSpan.Val2.toFloat() - cube.FrameSpan.Val1.toFloat()) / cube.Count.Value.toFloat()));

                                note._time = bpmAdjuster.GetPlaceTimeBeats(realstarttime + (realduration * (cube.FrameSpan.Val1.toFloat() / cube.Count.Value.toFloat())));
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
                                        rotationN.Add(new object[] { cube.Frames[i].Transformation.Rotation.X, cube.Frames[i].Transformation.Rotation.Y * -1, cube.Frames[i].Transformation.Rotation.Z * -1, TimeStamp });
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
                    case ModelTechnique.Normal:
                        {
                            note._customData._position = new object[] { (cube.Transformation.Position.X * -1f) - 2f, cube.Transformation.Position.Y };
                            note._customData._localRotation = new object[] { cube.Transformation.Rotation.X, cube.Transformation.Rotation.Y * -1f, cube.Transformation.Rotation.Z * -1f };
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
        public ModelSettings(string ModelPath, float spread, ModelTechnique technique, bool HasAnimation, BeatMap.Obstacle Wall)
        {
            Path = ModelPath;
            this.spread = spread;
            this.technique = technique;
            this.HasAnimation = HasAnimation;
            this.Wall = Wall;
        }
        public ModelSettings() { }

        public string Path { get; set; }
        public float spread { get; set; }
        public ModelTechnique technique { get; set; }
        /// <summary>
        /// controls normal and definite
        /// </summary>
        public bool HasAnimation { get; set; }
        public BeatMap.Obstacle Wall { get; set; }
        public float NJS { get; set; }
        public float BPM { get; set; }
        public float Offset { get; set; }
        public float? Thicc { get; set; }
        public bool PreserveTime { get; set; }
        public DeltaTransform DeltaTransformation { get; set; }
        public bool Spline { get; set; }
        public bool AssignCameraToTrack { get; set; }
        public bool CreateTracks { get; set; }
        public bool CreateNotes { get; set; }
        public class DeltaTransform
        {
            /// <summary>
            /// based around median point
            /// First transformation
            /// </summary>
            public Vector3 Position { get; set; }
            /// <summary>
            /// based around median point
            /// Third transformation
            /// </summary>
            public Vector3 Rotation { get; set; }
            /// <summary>
            /// based around median point
            /// Second transformation
            /// </summary>
            public Vector3 Scale { get; set; }
        }

    }
    public enum ModelTechnique
    {
        Definite,
        Normal
    }



}


