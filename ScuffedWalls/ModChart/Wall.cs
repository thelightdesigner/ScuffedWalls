using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
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
        // read all walls into array
        public static BeatMap.Obstacle[] ReadAllWalls(string MapPull)
        {
            return JsonSerializer.Deserialize<BeatMap>(File.ReadAllText(MapPull))._obstacles;
        }

        public static float GetTime(BeatMap.Obstacle Wall)
        {
            return Convert.ToSingle(Wall._time.ToString());
        }


        //Converts an image to walls. supports: scale, position animation, time, duration, isblackempty, and custom noodle data, returns complete wall objects
        public static BeatMap.Obstacle[] ImageToWall(string ImagePath, float Time, float Duration, bool isBlackEmpty, float scale, string[] IndividualCustomData)
        {
            Bitmap WallImageBitMap = new Bitmap(ImagePath, true);
            List<BeatMap.Obstacle> Walls = new List<BeatMap.Obstacle> { };

            //scale, distance calculation
            List<string> PointDefList = new List<string> { };
            string[] _customObjectSplit;
            List<string> PointDefs = new List<string>();
            string Easing = "";
            List<float> ScaleMods = new List<float> { };
            int f = 0;
            bool centered = false;
            float X = 0;
            string color = "";



            for (int i = 0; i < WallImageBitMap.Height; i++)
            {

                for (int j = 0; j < WallImageBitMap.Width; j++)
                {
                    if (((WallImageBitMap.GetPixel(j, i).B) + (WallImageBitMap.GetPixel(j, i).R) + (WallImageBitMap.GetPixel(j, i).G) > 0 && isBlackEmpty) || isBlackEmpty == false)
                    {
                        List<string> Noodles = new List<string> { };

                        foreach (var _customObject in IndividualCustomData)
                        {

                            _customObjectSplit = _customObject.Split(':');

                            if (_customObjectSplit[0] == "AnimateDefinitePosition")
                            {
                                PointDefs = new List<string>(_customObjectSplit[1].TrimStart('[').Split('['));
                                //(PointDefs[2].TrimEnd(new char[] { ']', ',' }));

                            }
                            else if (_customObjectSplit[0] == "thicc")
                            {
                                string[] ScaleMod = _customObjectSplit[1].Split(',');
                                foreach (var scalemod in ScaleMod)
                                {
                                    ScaleMods.Add((float)Convert.ToDouble(scalemod));
                                }
                                Noodles.Add("AnimateScale:[" + ScaleMods[0] + "," + ScaleMods[1] + "," + ScaleMods[2] + ",0],[" + ScaleMods[0] + "," + ScaleMods[1] + "," + ScaleMods[2] + ",1]");
                            }
                            else if (_customObjectSplit[0] == "centered")
                            {
                                centered = true;
                            }
                            else if (_customObjectSplit[0] == "color")
                            {
                                color = _customObject;
                            }
                            else if (_customObjectSplit[0] == "addcolor")
                            {
                                color = "color:[" + (Convert.ToDouble(WallImageBitMap.GetPixel(j, i).R) / 255) + "," + (Convert.ToDouble(WallImageBitMap.GetPixel(j, i).G) / 255) + "," + (Convert.ToDouble(WallImageBitMap.GetPixel(j, i).B / 255)) + "]";
                            }
                            else
                            {
                                Noodles.Add(_customObject);
                            }

                        }
                        foreach (var Set in PointDefs)
                        {

                            List<string> Points = new List<string>(Set.TrimEnd(new char[] { ']', ',' }).Split(','));

                            if (Points.Count == 5)
                            {
                                Easing = ",\"" + Points[4] + "\""; //easing support
                            }
                            else if (Points.Count == 4)
                            {
                                Easing = "";
                            }

                            if (centered)
                            {
                                X = (float)(((scale * j) - ((scale * WallImageBitMap.Width) / 2)) + 2);
                            }
                            else
                            {
                                X = (float)((scale * j) + (Convert.ToDouble(Points[0])));
                            }


                            PointDefList.Add("[" + X + "," + ((scale * (WallImageBitMap.Height - i)) + (Convert.ToDouble(Points[1]))) + "," + (Convert.ToDouble(Points[2])) + "," + (Convert.ToDouble(Points[3])) + Easing + "]");

                        }
                        Noodles.Add(color);

                        Walls.Add(WallConstructor(Time,
                            Duration,
                            Map.CustomDataParse(Map.ParamsAndStringToArray(Noodles.ToArray(),
                            "AnimateDefinitePosition:" + string.Join(',', PointDefList),
                            //"scale:[" + scale + "," + scale + "," + scale + "]"))));
                            "scale:[" + scale / ScaleMods[0] + "," + scale / ScaleMods[0] + "," + scale / ScaleMods[0] + "]"
                            ))));


                        //return WallConstructor(Time,Duration, CustomDataParse(ParamsAndStringToArray(Noodles.ToArray(), "AnimateDefinitePosition:" + string.Join(',', PointDefList), "scale:[" + scale + "," + scale + "," + scale + "]")));
                        PointDefList.Clear();
                        PointDefs.Clear();

                    }
                }
            }
            return Walls.ToArray();
        }
        /*
        public static BeatMap.Obstacle[] Image2Wall(string ImagePath, float Time, float Duration, bool isBlackEmpty, float scale, bool track, string[] IndividualCustomData)
        {
            Bitmap WallImageBitMap = new Bitmap(ImagePath, true);
            List<BeatMap.Obstacle> Walls = new List<BeatMap.Obstacle>();
            List<string> Noodles = new List<string>();
            for(int h = 0; h < WallImageBitMap.Height; h++)
            {
                for(int w = 0; w < WallImageBitMap.Width; h++)
                {
                    if (isBlackEmpty || (WallImageBitMap.GetPixel(w, h).R == 0 && WallImageBitMap.GetPixel(w, h).G == 0 && WallImageBitMap.GetPixel(w, h).B == 0 &&)) 
                    {
                        Walls.Add(WallConstructor(Time, Duration, Map.CustomDataParse(Noodles.ToArray()));
                    }
                }
            }
        }
        */
        public static BeatMap.Obstacle[] Model2Wall(string ModelPath, float Time, float spread, float Duration, bool HasAnimation, string[] IndividualCustomData)
        {
            Model model = new Model(ModelPath, HasAnimation);
            Random rnd = new Random();
            List<BeatMap.Obstacle> walls = new List<BeatMap.Obstacle>();
            foreach (var cube in model.cubes)
            {
                List<string> positions = new List<string>();
                List<string> rotations = new List<string>();
                List<string> scales = new List<string>();
                List<string> noodles = new List<string>();
                for (int i = 0; i < cube.Position.Length; i++)
                {
                    positions.Add("[" + ((cube.Position[i].X*-1) + 2 - cube.Scale[i].X) + "," + cube.Position[i].Y + "," + cube.Position[i].Z+ "," + (Convert.ToDouble(i) / Convert.ToDouble(cube.Position.Length)) + "]");
                }
                for (int i = 0; i < cube.Rotation.Length; i++)
                {
                    rotations.Add("[" + cube.Rotation[i].X + "," + cube.Rotation[i].Y * -1 + "," + cube.Rotation[i].Z * -1 + "," + (Convert.ToDouble(i) / Convert.ToDouble(cube.Rotation.Length)) + "]");
                }
                for (int i = 0; i < cube.Scale.Length; i++)
                {
                    scales.Add("[" + cube.Scale[i].X / cube.Scale[0].X + "," + cube.Scale[i].Y / cube.Scale[0].Y + "," + cube.Scale[i].Z / cube.Scale[0].Z + "," + (Convert.ToDouble(i) / Convert.ToDouble(cube.Scale.Length)) + "]");
                }
                if (cube.Position.Length == 1)
                {
                    positions.Add("[" + ((cube.Position[0].X*-1) + 2 - cube.Scale[0].X) + "," + cube.Position[0].Y + "," + cube.Position[0].Z + ",1]");
                    rotations.Add("[" + cube.Rotation[0].X + "," + cube.Rotation[0].Y * -1 + "," + cube.Rotation[0].Z * -1 + ",1]");
                    scales.Add("[" + cube.Scale[0].X / cube.Scale[0].X + "," + cube.Scale[0].Y / cube.Scale[0].Y + "," + cube.Scale[0].Z / cube.Scale[0].Z + ",1]");
                }

                bool customColorOverride = false;
                foreach (var arg in IndividualCustomData)
                {
                    if (arg.Split(':')[1] == "AnimateDefinitePosition" || arg == "AnimateRotation" || arg == "AnimateScale" || arg == "scale") { }
                    else if (arg.Split(':')[1] == "color")
                    {
                        noodles.Add(arg); 
                        customColorOverride = true;
                    }
                    else noodles.Add(arg);
                }

                noodles.Add("AnimateDefinitePosition:" + string.Join(',', positions));
                noodles.Add("AnimateLocalRotation:" + string.Join(',', rotations));
                noodles.Add("AnimateScale:" + string.Join(',', scales));
                noodles.Add("scale:[" + cube.Scale[0].X * 2 + "," + cube.Scale[0].Y * 2 + "," + cube.Scale[0].Z * 2 + "]");
                if (cube.Color != null && !customColorOverride) noodles.Add("color:[" + cube.Color.R + "," + cube.Color.G + "," + cube.Color.B + "," + cube.Color.A + "]");
                walls.Add(WallConstructor(Time + (Convert.ToSingle(rnd.Next(-100, 100)) / 100) * spread, Duration, Map.CustomDataParse(noodles.ToArray())));
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
