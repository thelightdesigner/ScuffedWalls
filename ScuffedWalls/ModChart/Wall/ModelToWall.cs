using System;
using System.Collections.Generic;

namespace ModChart.Wall
{
    class WallModel
    {
        public BeatMap.Obstacle[] Walls { get; private set; }
        ModelSettings settings;
        public WallModel(ModelSettings settings)
        {
            this.settings = settings;
            Run();
        }
        void Run()
        {
            Model model = new Model(settings.Path, settings.HasAnimation);
            Random rnd = new Random();
            List<BeatMap.Obstacle> walls = new List<BeatMap.Obstacle>();
            float NJS = settings.NJS;
            if (settings.Wall._customData._noteJumpMovementSpeed != null) NJS = settings.Wall._customData._noteJumpMovementSpeed.toFloat();

            foreach (var cube in model.OffsetCorrectedCubes)
            {
                float time = settings.Wall.GetTime() + (Convert.ToSingle(rnd.Next(-100, 100)) / 100) * settings.spread;
                float duration = settings.Wall._duration.toFloat();
                object[][] animatedefiniteposition = null;
                object[][] animatelocalrotation = null;
                object[][] animatescale = null;
                object[] position = null;
                object[] scale = null;
                object[] localrotation = null;
                object[] color = null;

                switch (settings.technique)
                {
                    case ModelTechnique.Definite:
                        {
                            List<object[]> positionN = new List<object[]>();
                            List<object[]> rotationN = new List<object[]>();
                            List<object[]> scaleN = new List<object[]>();
                            for (int i = 0; i < cube.DTransformation.Length; i++)
                            {
                                float TimeStamp = i.toFloat() / cube.DTransformation.Length.toFloat();
                                positionN.Add(new object[] { cube.DTransformation[i].Position.X * -1f, cube.DTransformation[i].Position.Y, cube.DTransformation[i].Position.Z, TimeStamp });
                                rotationN.Add(new object[] { cube.DTransformation[i].Rotation.X, cube.DTransformation[i].Rotation.Y * -1, cube.DTransformation[i].Rotation.Z * -1, TimeStamp });
                                scaleN.Add(new object[] { cube.DTransformation[i].Scale.X / cube.DTransformation[0].Scale.X, cube.DTransformation[i].Scale.Y / cube.DTransformation[0].Scale.Y, cube.DTransformation[i].Scale.Z / cube.DTransformation[0].Scale.Z, TimeStamp });
                            }
                            animatedefiniteposition = positionN.ToArray();
                            animatelocalrotation = rotationN.ToArray();
                            animatescale = scaleN.ToArray();
                            scale = new object[] { cube.DTransformation[0].Scale.X * 2f, cube.DTransformation[0].Scale.Y * 2f, cube.DTransformation[0].Scale.Z * 2f };
                        }
                        break;
                    case ModelTechnique.Normal:
                        {
                            position = new object[] { (cube.DTransformation[0].Position.X * -1f) -2f, cube.DTransformation[0].Position.Y };
                            localrotation = new object[] { cube.DTransformation[0].Rotation.X, cube.DTransformation[0].Rotation.Y * -1f, cube.DTransformation[0].Rotation.Z * -1f };
                            scale = new object[] { cube.DTransformation[0].Scale.X * 2f, cube.DTransformation[0].Scale.Y * 2f};
                            float beatlength = (5f / 3f * (60f / settings.BPM) * NJS);
                            duration = (cube.DTransformation[0].Scale.Z * 2f) / beatlength;
                            time = (cube.DTransformation[0].Position.Z / beatlength) + settings.Wall.GetTime();
                        }
                        break;
                }


                if (cube.Color != null) color = new object[] { cube.Color.R, cube.Color.G, cube.Color.B, cube.Color.A };
                if (settings.Wall._customData._color != null) color = settings.Wall._customData._color;

                walls.Add(new BeatMap.Obstacle()
                {
                    _time = time,
                    _duration = duration,
                    _lineIndex = 0,
                    _width = 0,
                    _type = 0,
                    _customData = new BeatMap.CustomData()
                    {
                        _position = position,
                        _scale = scale,
                        _localRotation = localrotation,
                        _color = color,
                        _animation = new BeatMap.CustomData.Animation()
                        {
                            _definitePosition = animatedefiniteposition,
                            _localRotation = animatelocalrotation,
                            _scale = animatescale
                        }
                    }
                }.Append(settings.Wall._customData, AppendTechnique.NoOverwrites));
            }
            Walls = walls.ToArray();
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
        public bool HasAnimation { get; set; }
        public BeatMap.Obstacle Wall { get; set; }
        public float NJS { get; set; }
        public float BPM { get; set; }
        public float? Thicc { get; set; }
    }
    public enum ModelTechnique
    {
        Definite,
        Normal
    }
}
