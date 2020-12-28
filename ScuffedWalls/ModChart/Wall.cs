using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text.Json;

namespace ModChart
{
    static class NoodleWall
    {
        //creates a wall object
        public static BeatMap.Obstacle WallConstructor(float Time, float Duration, BeatMap.CustomData CustomData)
        {
            return new BeatMap.Obstacle()
            {
                _time = Time,
                _lineIndex = 0,
                _width = 0,
                _type = 0,
                _duration = Duration,
                _customData = CustomData
            };
        }
        public static BeatMap.Obstacle WallConstructor(float Time, float Duration)
        {
            return new BeatMap.Obstacle()
            {
                _time = Time,
                _lineIndex = 0,
                _width = 0,
                _type = 0,
                _duration = Duration
            };
        }
        // read all walls into array
        public static BeatMap.Obstacle[] ReadAllWalls(string MapPull)
        {
            return JsonSerializer.Deserialize<BeatMap>(File.ReadAllText(MapPull))._obstacles;
        }

        public static float GetTime(this BeatMap.Obstacle Wall)
        {
            return Convert.ToSingle(Wall._time.ToString());
        }

        public static BeatMap.Obstacle[] Image2Wall(string ImagePath, bool isBlackEmpty, float scale, float thicc, bool track, bool centered, float spread, float alfa, BeatMap.CustomData customData, BeatMap.Obstacle baseWall)
        {
            Bitmap WallImageBitMap = new Bitmap(ImagePath, true);
            List<BeatMap.Obstacle> Walls = new List<BeatMap.Obstacle>();
            Random rnd = new Random();
            customData._animation ??= new BeatMap.CustomData.Animation();

            for (int y = 0; y < WallImageBitMap.Height; y++)
            {
                for (int x = 0; x < WallImageBitMap.Width; x++)
                {
                    if (!isBlackEmpty || (((WallImageBitMap.GetPixel(x, y).R + WallImageBitMap.GetPixel(x, y).G + WallImageBitMap.GetPixel(x, y).B > 10) && isBlackEmpty) && WallImageBitMap.GetPixel(x, y).A > 10))
                    {
                        //position
                        object[][] defPos = null;
                        object[] pos = null;

                        if (!track)
                        {
                            customData._animation._definitePosition ??= new object[][] { new object[] { 0, 0, 0, 0 }, new object[] { 0, 0, 0, 1 } };
                            List<object[]> DefPos = new List<object[]>();
                            foreach (var defposset in customData._animation._definitePosition)
                            {
                                object X = (x * scale) + Convert.ToSingle(defposset[0].ToString());
                                if (centered) X = (((scale * x) - ((scale * WallImageBitMap.Width) / 2)) + 2);

                                List<object> points = new List<object>();
                                points.AddRange(new object[] {
                                X,
                                (scale * (WallImageBitMap.Height - y)) + Convert.ToSingle(defposset[1].ToString()),
                                Convert.ToSingle(defposset[2].ToString()),
                                Convert.ToSingle(defposset[3].ToString())
                                });
                                if (defposset.Length > 4) points.Add(defposset[4]);

                                DefPos.Add(points.ToArray());
                            }
                            defPos = DefPos.ToArray();
                        }
                        else
                        {
                            customData._position ??= new object[] { 0, 0 };
                            object X;
                            if (!centered) X = (x * scale) + Convert.ToSingle(customData._position[0].ToString());
                            else X = (((scale * x) - ((scale * WallImageBitMap.Width) / 2)));

                            pos = new object[] {
                                X,
                                (scale * (WallImageBitMap.Height - y)) + Convert.ToSingle(customData._position[1].ToString())
                            };
                        }

                        object[] color = { Convert.ToSingle(WallImageBitMap.GetPixel(x, y).R) / 255f, Convert.ToSingle(WallImageBitMap.GetPixel(x, y).G) / 255f, Convert.ToSingle(WallImageBitMap.GetPixel(x, y).B) / 255f, alfa };
                        if (customData._color != null) color = customData._color;

                        Walls.Add(WallAppend(new BeatMap.Obstacle()
                        {
                            _time = baseWall.GetTime() + (Convert.ToSingle(rnd.Next(-100, 100)) / 100) * spread,
                            _duration = baseWall._duration,
                            _lineIndex = baseWall._lineIndex,
                            _type = baseWall._type,
                            _width = baseWall._width,
                            _customData = new BeatMap.CustomData()
                            {
                                _color = color,
                                _position = pos,
                                _scale = new object[] { scale / thicc, scale / thicc, scale / thicc },
                                _animation = new BeatMap.CustomData.Animation()
                                {
                                    _definitePosition = defPos,
                                    _scale = new object[][] { new object[] { thicc, thicc, thicc, 0 }, new object[] { thicc, thicc, thicc, 1 } }
                                }
                            }
                        }, customData, 0)); //no overwrites
                    }
                }
            }
            return Walls.ToArray();
        }

        public static BeatMap.Obstacle[] Model2Wall(string ModelPath, float spread, bool HasAnimation, BeatMap.CustomData customData, BeatMap.Obstacle baseWall)
        {
            Model model = new Model(ModelPath, HasAnimation);
            Random rnd = new Random();
            List<BeatMap.Obstacle> walls = new List<BeatMap.Obstacle>();
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

                walls.Add(WallAppend(new BeatMap.Obstacle()
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
                }, customData, 0)); //no overwrite
            }
            return walls.ToArray();
        }


        public static BeatMap.Obstacle WallAppend(BeatMap.Obstacle CurrentWall, BeatMap.CustomData CustomData, int Type)
        {
            // append technique 0 adds on customdata only if there is no existing customdata
            if (Type == 0)
            {
                CurrentWall._customData ??= new BeatMap.CustomData();
                CurrentWall._customData._animation ??= new BeatMap.CustomData.Animation();
                CustomData ??= new BeatMap.CustomData();
                CustomData._animation ??= new BeatMap.CustomData.Animation();
                PropertyInfo[] propertiesCustomData = typeof(BeatMap.CustomData).GetProperties();
                PropertyInfo[] propertiesCustomDataAnimation = typeof(BeatMap.CustomData.Animation).GetProperties();

                foreach (PropertyInfo property in propertiesCustomData)
                {
                    if (property.GetValue(CurrentWall._customData) == null)
                    {
                        property.SetValue(CurrentWall._customData, property.GetValue(CustomData));
                    }

                }
                foreach (PropertyInfo property in propertiesCustomDataAnimation)
                {
                    if (property.GetValue(CurrentWall._customData._animation) == null)
                    {
                        property.SetValue(CurrentWall._customData._animation, property.GetValue(CustomData._animation));
                    }

                }
                return CurrentWall;
            }
            // append technique 1 adds on customdata, overwrites
            else if (Type == 1)
            {
                CurrentWall._customData ??= new BeatMap.CustomData();
                CurrentWall._customData._animation ??= new BeatMap.CustomData.Animation();
                CustomData ??= new BeatMap.CustomData();
                CustomData._animation ??= new BeatMap.CustomData.Animation();
                PropertyInfo[] propertiesCustomData = typeof(BeatMap.CustomData).GetProperties();
                PropertyInfo[] propertiesCustomDataAnimation = typeof(BeatMap.CustomData.Animation).GetProperties();

                foreach (PropertyInfo property in propertiesCustomData)
                {
                    if (property.GetValue(CustomData) != null)
                    {
                        property.SetValue(CurrentWall._customData, property.GetValue(CustomData));
                    }

                }
                foreach (PropertyInfo property in propertiesCustomDataAnimation)
                {
                    if (property.GetValue(CustomData._animation) != null)
                    {
                        property.SetValue(CurrentWall._customData._animation, property.GetValue(CustomData._animation));
                    }

                }
                return CurrentWall;
            }
            // append technique 2 completely replaces the customdata
            else
            {
                CurrentWall._customData ??= new BeatMap.CustomData();
                CurrentWall._customData = CustomData;
                return CurrentWall;
            }

        }
    }
}
