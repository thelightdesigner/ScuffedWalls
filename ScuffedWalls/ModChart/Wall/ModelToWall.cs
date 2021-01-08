using System;
using System.Collections.Generic;

namespace ModChart.Wall
{
    static class ModelConvert
    {
        public static BeatMap.Obstacle[] Model2Wall(string ModelPath, float spread, int technique, bool HasAnimation, BeatMap.CustomData customData, BeatMap.Obstacle baseWall)
        {
            Model model = new Model(ModelPath, HasAnimation);
            Random rnd = new Random();
            List<BeatMap.Obstacle> walls = new List<BeatMap.Obstacle>();

            switch (technique)
            {
                case 0:
                    foreach (var cube in model.cubes)
                    {

                        List<object[]> positionN = new List<object[]>();

                        for (int i = 0; i < cube.Position.Length; i++)
                        {
                            positionN.Add(new object[] { ((cube.Position[i].X * -1) + 2 - cube.Scale[i].X), cube.Position[i].Y, cube.Position[i].Z, (Convert.ToSingle(i) / Convert.ToSingle(cube.Position.Length)) });
                        }
                        List<object[]> rotationN = new List<object[]>();
                        for (int i = 0; i < cube.Rotation.Length; i++)
                        {
                            rotationN.Add(new object[] { cube.Rotation[i].X, cube.Rotation[i].Y * -1, cube.Rotation[i].Z * -1, (Convert.ToSingle(i) / Convert.ToSingle(cube.Rotation.Length)) });
                        }
                        List<object[]> scaleN = new List<object[]>();
                        for (int i = 0; i < cube.Scale.Length; i++)
                        {
                            scaleN.Add(new object[] { cube.Scale[i].X / cube.Scale[0].X, cube.Scale[i].Y / cube.Scale[0].Y, cube.Scale[i].Z / cube.Scale[0].Z, (Convert.ToSingle(i) / Convert.ToSingle(cube.Scale.Length)) });
                        }
                        if (cube.Position.Length == 1)
                        {
                            positionN.Add(new object[] { ((cube.Position[0].X * -1) + 2 - cube.Scale[0].X), cube.Position[0].Y, cube.Position[0].Z, 1 });
                            rotationN.Add(new object[] { cube.Rotation[0].X, cube.Rotation[0].Y * -1, cube.Rotation[0].Z * -1, 1 });
                            scaleN.Add(new object[] { cube.Scale[0].X / cube.Scale[0].X, cube.Scale[0].Y / cube.Scale[0].Y, cube.Scale[0].Z / cube.Scale[0].Z, 1 });
                        }

                        object[] color = null;
                        if (cube.Color != null) color = new object[] { cube.Color.R, cube.Color.G, cube.Color.B, cube.Color.A };
                        if (customData._color != null) color = customData._color;

                        walls.Add(new BeatMap.Obstacle()
                        {
                            _time = baseWall.GetTime() + (Convert.ToSingle(rnd.Next(-100, 100)) / 100) * spread,
                            _duration = baseWall._duration,
                            _lineIndex = 0,
                            _type = 0,
                            _width = 0,
                            _customData = new BeatMap.CustomData()
                            {
                                _color = color,
                                _scale = new object[] { cube.Scale[0].X * 2, cube.Scale[0].Y * 2, cube.Scale[0].Z * 2 },
                                _animation = new BeatMap.CustomData.Animation()
                                {
                                    _definitePosition = positionN.ToArray(),
                                    _localRotation = rotationN.ToArray(),
                                    _scale = scaleN.ToArray()
                                }
                            }
                        }.Append(customData, 0)); //no overwrite
                    }
                    break;
                case 1:
                    foreach (var cube in model.cubes)
                    {
                        object[] pos = { ((cube.Position[0].X * -1) + 2 - cube.Scale[0].X), cube.Position[0].Y };
                        object[] rot = { cube.Rotation[0].X, cube.Rotation[0].Y * -1, cube.Rotation[0].Z * -1 };
                        object[] sca = { cube.Scale[0].X * 2, cube.Scale[0].Y * 2 };
                        float duration = cube.Scale[0].Z;
                        float time = cube.Position[0].Z + baseWall.GetTime();


                        object[] color = null;
                        if (cube.Color != null) color = new object[] { cube.Color.R, cube.Color.G, cube.Color.B, cube.Color.A };
                        if (customData._color != null) color = customData._color;

                        walls.Add(new BeatMap.Obstacle()
                        {
                            _time = time,
                            _duration = duration,
                            _lineIndex = 0,
                            _width = 0,
                            _type = 0,
                            _customData = new BeatMap.CustomData()
                            {
                                _position = pos,
                                _localRotation = rot,
                                _scale = sca,
                                _color = color
                            }
                        }.Append(customData, 0)); //no overwrite

                    }
                    break;
            }

            return walls.ToArray();
        }
    }
}
