using ModChart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Drawing;
using System.Text.Json;

namespace ScuffedWalls
{

    static class Internal
    {
        public static string[] toUsableCustomData(this Parameter[] parameter)
        {
            List<string> customDatas = new List<string>();
            foreach (var p in parameter)
            {
                customDatas.Add(p.parameter + ":" + p.argument);
            }
            return customDatas.ToArray();
        }
        public static T[] GetAllBetween<T>(this T[] mapObjects, float starttime, float endtime)
        {
            List<T> objectsBetween = new List<T>();
            foreach (var mapobj in mapObjects)
            {
                float time = Convert.ToSingle(typeof(T).GetProperty("_time").GetValue(mapobj, null).ToString());
                if (time >= starttime && time <= endtime)
                {
                    objectsBetween.Add(mapobj);
                }
            }
            return objectsBetween.ToArray();
        }
        static public bool MethodExists<t>(this string methodname)
        {
            foreach (var methods in typeof(t).GetMethods())
            {
                if (methods.Name == methodname) return true;
            }
            return false;
        }
        static public Parameter[] GetParameters(this string[] args)
        {
            List<Parameter> parameters = new List<Parameter>();
            foreach (var line in args)
            {
                if (line.Split(':', 2).Length < 2) ConsoleErrorLogger.ScuffedWorkspace.FunctionParser.Log("Missing Colon");
                string parameter = line.Split(':', 2)[0].ToLower();
                string argument = line.Split(':', 2)[1];
                while (argument.Contains("Random("))
                {
                    Random rnd = new Random();
                    string[] asplit = argument.Split("Random(", 2);
                    string[] randomparam = asplit[1].Split(',');
                    argument = asplit[0] + rnd.Next(Convert.ToInt32(randomparam[0]), Convert.ToInt32(randomparam[1].Split(')')[0]) + 1) + asplit[1].Split(')', 2)[1];
                }
                parameters.Add(new Parameter { parameter = parameter, argument = argument });
            }

            return parameters.ToArray();
        }
        public static string removeWhiteSpace(this string WhiteSpace)
        {
            return new string(WhiteSpace.Where(c => !Char.IsWhiteSpace(c)).ToArray());
        }

        //adjust for lower caseeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeee
        public static BeatMap.CustomData CustomDataParse(this string[] CustomNoodleData)
        {
            BeatMap.CustomData CustomData = new BeatMap.CustomData();
            BeatMap.CustomData.Animation Animation = new BeatMap.CustomData.Animation();

            foreach (var _customObject in CustomNoodleData)
            {
                string[] _customObjectSplit = _customObject.Split(':');

                if (_customObjectSplit[0] == "AnimateDefinitePosition".ToLower())
                {
                    Animation._definitePosition = JsonSerializer.Deserialize<object[][]>("[" + _customObjectSplit[1] + "]");
                }
                else if (_customObjectSplit[0] == "AnimatePosition".ToLower())
                {
                    Animation._position = JsonSerializer.Deserialize<object[][]>("[" + _customObjectSplit[1] + "]");
                }
                else if (_customObjectSplit[0] == "scale".ToLower())
                {
                    CustomData._scale = JsonSerializer.Deserialize<object[]>(_customObjectSplit[1]);
                }
                else if (_customObjectSplit[0] == "track".ToLower())
                {
                    CustomData._track = JsonSerializer.Deserialize<object>("\"" + _customObjectSplit[1] + "\"");
                }
                else if (_customObjectSplit[0] == "color".ToLower())
                {
                    CustomData._color = JsonSerializer.Deserialize<object[]>(_customObjectSplit[1]);
                }
                else if (_customObjectSplit[0] == "NJSOffset".ToLower())
                {
                    CustomData._noteJumpStartBeatOffset = JsonSerializer.Deserialize<object>(_customObjectSplit[1]);
                }
                else if (_customObjectSplit[0] == "NJS".ToLower())
                {
                    CustomData._noteJumpMovementSpeed = JsonSerializer.Deserialize<object>(_customObjectSplit[1]);
                }
                else if (_customObjectSplit[0] == "AnimateDissolve".ToLower())
                {
                    Animation._dissolve = JsonSerializer.Deserialize<object[][]>("[" + _customObjectSplit[1] + "]");
                }
                else if (_customObjectSplit[0] == "AnimateDissolveArrow".ToLower())
                {
                    Animation._dissolveArrow = JsonSerializer.Deserialize<object[][]>("[" + _customObjectSplit[1] + "]");
                }
                else if (_customObjectSplit[0] == "AnimateColor".ToLower())
                {
                    Animation._color = JsonSerializer.Deserialize<object[][]>("[" + _customObjectSplit[1] + "]");
                }
                else if (_customObjectSplit[0] == "AnimateRotation".ToLower())
                {
                    Animation._rotation = JsonSerializer.Deserialize<object[][]>("[" + _customObjectSplit[1] + "]");
                }
                else if (_customObjectSplit[0] == "AnimateLocalRotation".ToLower())
                {
                    Animation._localRotation = JsonSerializer.Deserialize<object[][]>("[" + _customObjectSplit[1] + "]");
                }
                else if (_customObjectSplit[0] == "AnimateScale".ToLower())
                {
                    Animation._scale = JsonSerializer.Deserialize<object[][]>("[" + _customObjectSplit[1] + "]");
                }
                else if (_customObjectSplit[0] == "isInteractable".ToLower())
                {
                    CustomData._interactable = JsonSerializer.Deserialize<object>(_customObjectSplit[1]);
                }
                else if (_customObjectSplit[0] == "rotation".ToLower())
                {
                    CustomData._rotation = JsonSerializer.Deserialize<object[]>(_customObjectSplit[1]);
                }
                else if (_customObjectSplit[0] == "fake".ToLower())
                {
                    CustomData._fake = JsonSerializer.Deserialize<object>(_customObjectSplit[1]);
                }
                else if (_customObjectSplit[0] == "position".ToLower())
                {
                    CustomData._position = JsonSerializer.Deserialize<object[]>(_customObjectSplit[1]);
                }
                else if (_customObjectSplit[0] == "cutDirection".ToLower())
                {
                    CustomData._cutDirection = JsonSerializer.Deserialize<object>(_customObjectSplit[1]);
                }
                else if (_customObjectSplit[0] == "NoSpawnEffect".ToLower())
                {
                    CustomData._disableSpawnEffect = JsonSerializer.Deserialize<object>(_customObjectSplit[1]);
                }

                CustomData._animation = Animation;

            }

            return CustomData;

        }

        //adjust for lowercaseeaaa
        public static BeatMap.CustomData.CustomEvents.Data CustomEventsDataParse(this string[] CustomNoodleData)
        {

            BeatMap.CustomData.CustomEvents.Data Data = new BeatMap.CustomData.CustomEvents.Data();

            foreach (var _customObject in CustomNoodleData)
            {
                string[] _customObjectSplit = _customObject.Split(':');


                if (_customObjectSplit[0] == "track".ToLower())
                {
                    Data._track = JsonSerializer.Deserialize<object>("\"" + _customObjectSplit[1] + "\"");
                }
                if (_customObjectSplit[0] == "parentTrack".ToLower())
                {
                    Data._parentTrack = JsonSerializer.Deserialize<object>("\"" + _customObjectSplit[1] + "\"");
                }
                if (_customObjectSplit[0] == "duration".ToLower())
                {
                    Data._duration = JsonSerializer.Deserialize<object>(_customObjectSplit[1]);
                }
                if (_customObjectSplit[0] == "easing".ToLower())
                {
                    Data._easing = JsonSerializer.Deserialize<object>("\"" + _customObjectSplit[1] + "\"");
                }
                if (_customObjectSplit[0] == "AnimateDefinitePosition".ToLower())
                {
                    Data._definitePosition = JsonSerializer.Deserialize<object[][]>("[" + _customObjectSplit[1] + "]");
                }
                if (_customObjectSplit[0] == "AnimateDissolve".ToLower())
                {
                    Data._dissolve = JsonSerializer.Deserialize<object[][]>("[" + _customObjectSplit[1] + "]");
                }
                if (_customObjectSplit[0] == "childTracks".ToLower())
                {
                    Data._childrenTracks = JsonSerializer.Deserialize<object[]>(_customObjectSplit[1]);
                }
                if (_customObjectSplit[0] == "AnimateDissolveArrow".ToLower())
                {
                    Data._dissolveArrow = JsonSerializer.Deserialize<object[][]>("[" + _customObjectSplit[1] + "]");
                }
                if (_customObjectSplit[0] == "AnimateColor".ToLower())
                {
                    Data._color = JsonSerializer.Deserialize<object[][]>("[" + _customObjectSplit[1] + "]");
                }
                if (_customObjectSplit[0] == "AnimateRotation".ToLower())
                {
                    Data._rotation = JsonSerializer.Deserialize<object[][]>("[" + _customObjectSplit[1] + "]");
                }
                if (_customObjectSplit[0] == "AnimatePosition".ToLower())
                {
                    Data._position = JsonSerializer.Deserialize<object[][]>("[" + _customObjectSplit[1] + "]");
                }
                if (_customObjectSplit[0] == "AnimateLocalRotation".ToLower())
                {
                    Data._localRotation = JsonSerializer.Deserialize<object[][]>("[" + _customObjectSplit[1] + "]");
                }
                if (_customObjectSplit[0] == "AnimateScale".ToLower())
                {
                    Data._scale = JsonSerializer.Deserialize<object[][]>("[" + _customObjectSplit[1] + "]");
                }
                if (_customObjectSplit[0] == "isInteractable".ToLower())
                {
                    Data._interactable = JsonSerializer.Deserialize<object[][]>("[" + _customObjectSplit[1] + "]");
                }

            }
            return Data;
        }
        public static BeatMap.Obstacle[] ImageToWall(string ImagePath, float Time, float Duration, bool isBlackEmpty, float scale, string[] IndividualCustomData)
        {
            Bitmap WallImageBitMap = new Bitmap(ImagePath, true);
            List<BeatMap.Obstacle> Walls = new List<BeatMap.Obstacle> { };


            for (int i = 0; i < WallImageBitMap.Height; i++)
            {

                for (int j = 0; j < WallImageBitMap.Width; j++)
                {
                    if (((WallImageBitMap.GetPixel(j, i).B) + (WallImageBitMap.GetPixel(j, i).R) + (WallImageBitMap.GetPixel(j, i).G) > 0 && isBlackEmpty) || isBlackEmpty == false)
                    {
                        float X;
                        string Easing = "";
                        string color = "";
                        List<string> PointDefList = new List<string> { };
                        string[] _customObjectSplit;
                        List<string> PointDefs = new List<string>();
                        List<float> ScaleMods = new List<float> { };
                        List<string> Noodles = new List<string> { };
                        bool centered = false;

                        foreach (var _customObject in IndividualCustomData)
                        {

                            _customObjectSplit = _customObject.Split(':');

                            if (_customObjectSplit[0] == "animatedefiniteposition")
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
                                Noodles.Add("animatescale:[" + ScaleMods[0] + "," + ScaleMods[1] + "," + ScaleMods[2] + ",0],[" + ScaleMods[0] + "," + ScaleMods[1] + "," + ScaleMods[2] + ",1]");
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

                        Walls.Add(NoodleWall.WallConstructor(Time,
                            Duration,
                            Map.ParamsAndStringToArray(Noodles.ToArray(),
                            "animatedefiniteposition:" + string.Join(',', PointDefList),
                            //"scale:[" + scale + "," + scale + "," + scale + "]"))));
                            "scale:[" + scale / ScaleMods[0] + "," + scale / ScaleMods[0] + "," + scale / ScaleMods[0] + "]").CustomDataParse()));

                    }
                }
            }
            return Walls.ToArray();
        }
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
                    positions.Add("[" + ((cube.Position[i].X * -1) + 2 - cube.Scale[i].X) + "," + cube.Position[i].Y + "," + cube.Position[i].Z + "," + (Convert.ToDouble(i) / Convert.ToDouble(cube.Position.Length)) + "]");
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
                    positions.Add("[" + ((cube.Position[0].X * -1) + 2 - cube.Scale[0].X) + "," + cube.Position[0].Y + "," + cube.Position[0].Z + ",1]");
                    rotations.Add("[" + cube.Rotation[0].X + "," + cube.Rotation[0].Y * -1 + "," + cube.Rotation[0].Z * -1 + ",1]");
                    scales.Add("[" + cube.Scale[0].X / cube.Scale[0].X + "," + cube.Scale[0].Y / cube.Scale[0].Y + "," + cube.Scale[0].Z / cube.Scale[0].Z + ",1]");
                }

                bool customColorOverride = false;
                foreach (var arg in IndividualCustomData)
                {
                    if (arg.Split(':')[1] == "animatedefiniteposition" || arg == "animaterotation" || arg == "animatescale" || arg == "scale") { }
                    else if (arg.Split(':')[1] == "color")
                    {
                        noodles.Add(arg);
                        customColorOverride = true;
                    }
                    else noodles.Add(arg);
                }

                noodles.Add("animatedefiniteposition:" + string.Join(',', positions));
                noodles.Add("animatelocalrotation:" + string.Join(',', rotations));
                noodles.Add("animatescale:" + string.Join(',', scales));
                noodles.Add("scale:[" + cube.Scale[0].X * 2 + "," + cube.Scale[0].Y * 2 + "," + cube.Scale[0].Z * 2 + "]");
                if (cube.Color != null && !customColorOverride) noodles.Add("color:[" + cube.Color.R + "," + cube.Color.G + "," + cube.Color.B + "," + cube.Color.A + "]");
                walls.Add(NoodleWall.WallConstructor(Time + (Convert.ToSingle(rnd.Next(-100, 100)) / 100) * spread, Duration, noodles.ToArray().CustomDataParse()));
            }
            return walls.ToArray();
        }
    }
    public struct Parameter
    {
        public string parameter;
        public string argument;
    }

}
