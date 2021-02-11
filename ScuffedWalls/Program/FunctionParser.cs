using ModChart;
using ModChart.Event;
using ModChart.Note;
using ModChart.Wall;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace ScuffedWalls
{
    partial class FunctionParser
    {
        public FunctionParser(Workspace[] workspaces)
        {
            Workspaces = workspaces;
        }
        private readonly Workspace[] Workspaces;
        public List<BeatMap.Note> Notes = new List<BeatMap.Note>();
        public List<BeatMap.CustomData.PointDefinition> PointDefinitions = new List<BeatMap.CustomData.PointDefinition>();
        public List<BeatMap.Event> Lights = new List<BeatMap.Event>();
        public List<BeatMap.Obstacle> Walls = new List<BeatMap.Obstacle>();
        public List<BeatMap.CustomData.CustomEvents> CustomEvents = new List<BeatMap.CustomData.CustomEvents>();


        public static FunctionParser parseWorkspace(string[] args, Workspace[] workspaces)
        {
            FunctionParser Workspace = new FunctionParser(workspaces);
            for (int i = 0; i < args.Length; i++)
            {

                if (Char.IsNumber(args[i].removeWhiteSpace()[0]))
                {
                    string functionName = args[i].removeWhiteSpace().Split(':')[1].Trim(' ').ToLower();
                    float time = Convert.ToSingle(args[i].removeWhiteSpace().Split(':')[0].Trim(' '));
                    if (functionName.MethodExists<FunctionParser>())
                    {
                        try
                        {
                            typeof(FunctionParser).GetMethod(functionName)
                                .Invoke(Workspace, new object[] { ScuffedFile.getLinesUntilNextFunction(i, args), time });
                        }
                        catch (Exception e)
                        {
                            throw new ScuffedException($"Error parsing function \"{functionName}\" at beat {time}   ERR: {e.InnerException.Message}");

                        }
                    }
                    else throw new ScuffedException($"Function \"{functionName}\" does not exist");
                }

            }
            return Workspace;
        }
        [SFunction]
        public void appendtoalleventsbetween(string[] args, float time)
        {
            //clarify type of appendage
            int type = 0;
            bool b = false;
            int[] lightypes = { 1, 2, 3, 4, 5, 6, 7, 8 };
            float starttime = time;
            bool rainbow = false;
            bool props = false;
            float Rfactor = 1f;
            float Pfactor = 1f;
            float endtime = time + 30;

            foreach (var p in args.TryGetParameters())
            {
                if (p.Name == "appendtechnique") type = Convert.ToInt32(p.Data);
                else if (p.Name == "tobeat") { endtime = Convert.ToSingle(p.Data); b = true; }
                else if (p.Name == "converttoprops") props = bool.Parse(p.Data);
                else if (p.Name == "converttorainbow") rainbow = bool.Parse(p.Data);
                else if (p.Name == "rainbowfactor") Rfactor = Convert.ToSingle(p.Data);
                else if (p.Name == "propfactor") Pfactor = Convert.ToSingle(p.Data);
                else if (p.Name == "lighttype") lightypes = p.Data.Split(",").Select(a => { return Convert.ToInt32(a); }).ToArray();
            }
            if (!b) throw new ScuffedException($"missing \"ToBeat\" parameter -> AppendToAllWallsBetween at beat {time}");

            if (rainbow)
            {
                List<BeatMap.Event> ApendedEvents = new List<BeatMap.Event>();
                int i = 0;
                foreach (var light in Lights)
                {
                    if (light.GetTime() >= starttime && light.GetTime() <= endtime && lightypes.Any(t => t == light.GetEventType()))
                    {
                        ApendedEvents.Add(light.EventAppend(new BeatMap.CustomData()
                        {
                            _color = new object[]
                            {
                            0.5f * Math.Sin(Math.PI * Rfactor * light.GetTime()) + 0.5f,
                            0.5f * Math.Sin((Math.PI * Rfactor * light.GetTime()) - (Math.PI * (2f / 3f))) + 0.5f,
                            0.5f * Math.Sin((Math.PI * Rfactor * light.GetTime()) - (Math.PI * (4f / 3f))) + 0.5f,
                            1
                            }
                        }, AppendTechnique.Overwrites));
                        i++;
                    }
                    else
                    {
                        ApendedEvents.Add(light);
                    }
                }
                Lights = new List<BeatMap.Event>();
                Lights.AddRange(ApendedEvents.ToArray());
                ConsoleOut("Event", i, time, "Rainbow");
            }

            if (props)
            {
                List<BeatMap.Event> newEvents = new List<BeatMap.Event>();
                int c = 0;
                foreach (var light in Lights)
                {
                    if (light.GetTime() >= starttime && light.GetTime() <= endtime && lightypes.Any(t => t == light.GetEventType()))
                    {
                        int count = (Convert.ToInt32(light._type.ToString())).getCountByID();
                        for (int i = 0; i < count; i++)
                        {
                            newEvents.Add(new BeatMap.Event()
                            {
                                _time = light.GetTime() + Pfactor - (Convert.ToSingle(i) / (Pfactor * Convert.ToSingle(count))),
                                _type = light.GetEventType(),
                                _value = light.GetValue().getValueFromOld(),
                                _customData = new BeatMap.CustomData()
                                {
                                    _propID = i
                                }
                            }.EventAppend(light._customData, AppendTechnique.NoOverwrites));
                        }
                        c++;
                    }
                    else
                    {
                        newEvents.Add(light);
                    }
                }
                Lights = new List<BeatMap.Event>();
                Lights.AddRange(newEvents.ToArray());
                ConsoleOut("Event", c, time, "Ring Props");
            }

            {
                List<BeatMap.Event> ApendedEvents = new List<BeatMap.Event>();
                int i = 0;
                foreach (var light in Lights)
                {
                    if (Convert.ToSingle(light._time.ToString()) >= starttime && Convert.ToSingle(light._time.ToString()) <= endtime && lightypes.Any(t => t == light.GetEventType()))
                    {
                        ApendedEvents.Add(light.EventAppend(args.TryGetParameters().CustomDataParse(), (AppendTechnique)type));
                        i++;
                    }
                    else
                    {
                        ApendedEvents.Add(light);
                    }
                }
                Lights = new List<BeatMap.Event>();
                Lights.AddRange(ApendedEvents.ToArray());
                ScuffedLogger.ScuffedWorkspace.FunctionParser.Log($"Appended {i} events from beats {starttime} to {endtime}");
            }
        }
        [SFunction]
        public void texttowall(string[] args, float time)
        {
            List<string> lines = new List<string>();
            float letting = 1;
            float leading = 1;
            float size = 1;
            float thicc = 1;
            float duration = 1;
            float compression = 0.1f;
            float shift = 1;
            int linelength = 1000000;
            bool isblackempty = true;
            float alpha = 1;
            float smooth = 0;
            string path = "";
            foreach (var p in args.TryGetParameters())
            {
                switch (p.Name)
                {
                    case "line":
                        lines.Add(p.Data);
                        break;
                    case "duration":
                        duration = p.Data.toFloat();
                        break;
                    case "letting":
                        letting = p.Data.toFloat();
                        break;
                    case "leading":
                        leading = p.Data.toFloat();
                        break;
                    case "size":
                        size = p.Data.toFloat();
                        break;
                    case "path":
                        path = Startup.ScuffedConfig.MapFolderPath + @"\" + p.Data.removeWhiteSpace();
                        break;
                    case "fullpath":
                        path = p.Data;
                        break;
                    case "thicc":
                        thicc = p.Data.toFloat();
                        break;
                    case "compression":
                        compression = p.Data.toFloat();
                        break;
                    case "shift":
                        shift = p.Data.toFloat();
                        break;
                    case "maxlinelength":
                        linelength = Convert.ToInt32(p.Data);
                        break;
                    case "alpha":
                        alpha = p.Data.toFloat();
                        break;
                    case "spreadspawntime":
                        smooth = p.Data.toFloat();
                        break;
                    case "isblackempty":
                        isblackempty = bool.Parse(p.Data);
                        break;
                }
            }
            lines.Reverse();
            WallText text = new WallText(new TextSettings()
            {
                Leading = leading,
                Letting = letting,
                ImagePath = path,
                Text = lines.ToArray(),
                ImageSettings = new ImageSettings()
                {
                    scale = size,
                    shift = shift,
                    spread = smooth,
                    alfa = alpha,
                    centered = false,
                    isBlackEmpty = isblackempty,
                    maxPixelLength = linelength,
                    thicc = thicc,
                    tolerance = compression,
                    Wall = new BeatMap.Obstacle()
                    {
                        _time = time,
                        _duration = duration,
                        _customData = args.TryGetParameters().CustomDataParse()
                    }
                }
            });
            Walls.AddRange(text.Walls);
            ConsoleOut("Wall", text.Walls.Length, time, "TextToWall");
        }

        [SFunction]
        public void pointdefinition(string[] args, float time)
        {
            string name = null;
            object[][] points = null;
            foreach (var p in args.TryGetParameters())
            {
                switch (p.Name)
                {
                    case "name":
                        name = p.Data;
                        break;
                    case "points":
                        points = JsonSerializer.Deserialize<object[][]>($"[{p.Data}]");
                        break;
                }
            }

            PointDefinitions.Add(new BeatMap.CustomData.PointDefinition()
            {
                _name = name,
                _points = points
            });

            ConsoleOut("PointDefinition", 1, time, "PointDefinition");
        }
        [SFunction]
        public void clonefromworkspace(string[] args, float time)
        {
            int[] Type = { 0, 1, 2, 3 };
            string name = string.Empty;
            int Index = 0;
            float startbeat = time;
            float endbeat = 1000000;
            float addtime = 0;

            foreach (var p in args.TryGetParameters())
            {
                switch (p.Name)
                {
                    case "index":
                        Index = Convert.ToInt32(p.Data);
                        break;
                    case "name":
                        name = p.Data;
                        break;
                    case "type":
                        Type = p.Data.Split(",").Select(a => Convert.ToInt32(a)).ToArray();
                        break;
                    case "tobeat":
                        endbeat = Convert.ToSingle(p.Data);
                        break;
                    case "addtime":
                        addtime = Convert.ToSingle(p.Data);
                        break;
                }
            }
            BeatMap beatMap = null;
            beatMap = toBeatMap(Workspaces[Index].DeepClone());

            if (name != string.Empty)
            {
                beatMap = toBeatMap(Workspaces.Where(w => w.Name == name).First().DeepClone());
            }

            beatMap._customData._customEvents ??= new BeatMap.CustomData.CustomEvents[] { };
            if (Type.Any(t => t == 0))
            {
                Walls.AddRange(beatMap._obstacles.GetAllBetween(startbeat, endbeat).Select(o => { o._time = o._time.toFloat() + addtime; return o; }));
                ConsoleOut("Wall", beatMap._obstacles.GetAllBetween(startbeat, endbeat).Length, time, "CloneWorkspace");
            }
            if (Type.Any(t => t == 1))
            {
                Notes.AddRange(beatMap._notes.GetAllBetween(startbeat, endbeat).Select(o => { o._time = o._time.toFloat() + addtime; return o; }));
                ConsoleOut("Note", beatMap._notes.GetAllBetween(startbeat, endbeat).Length, time, "CloneWorkspace");
            }
            if (Type.Any(t => t == 2))
            {
                Lights.AddRange(beatMap._events.GetAllBetween(startbeat, endbeat).Select(o => { o._time = o._time.toFloat() + addtime; return o; }));
                ConsoleOut("Light", beatMap._events.GetAllBetween(startbeat, endbeat).Length, time, "CloneWorkspace");
            }
            if (Type.Any(t => t == 3))
            {
                CustomEvents.AddRange(beatMap._customData._customEvents.GetAllBetween(startbeat, endbeat).Select(o => { o._time = o._time.toFloat() + addtime; return o; }));
                ConsoleOut("CustomEvent", beatMap._customData._customEvents.GetAllBetween(startbeat, endbeat).Length, time, "CloneWorkspace");
            }
        }
        [SFunction]
        public void blackout(string[] args, float time)
        {
            ConsoleOut("Light",1,time,"Blackout");
            Lights.Add(new BeatMap.Event() { _time = time, _type = 0, _value = 0 });
        }
        [SFunction]
        public void animatetrack(string[] args, float time)
        {
            int repeatcount = 1;
            float repeatTime = 0;
            foreach (var p in args.TryGetParameters())
            {
                if (p.Name == "repeat") repeatcount = Convert.ToInt32(p.Data);
                else if (p.Name == "repeataddtime") repeatTime = Convert.ToSingle(p.Data);
            }
            for (float i = 0; i < repeatcount; i++)
            {
                CustomEvents.Add(new BeatMap.CustomData.CustomEvents()
                {
                    _time = time + (i * repeatTime),
                    _type = "AnimateTrack",
                    _data = args.TryGetParameters().CustomEventsDataParse()
                });
            }
            ConsoleOut("AnimateTrack", repeatcount, time, "CustomEvent");
        }
        [SFunction]
        public void assignpathanimation(string[] args, float time)
        {
            int repeatcount = 1;
            float repeatTime = 0;
            foreach (var p in args.TryGetParameters())
            {
                if (p.Name == "repeat") repeatcount = Convert.ToInt32(p.Data);
                else if (p.Name == "repeataddtime") repeatTime = Convert.ToSingle(p.Data);
            }
            for (float i = 0; i < repeatcount; i++)
            {
                CustomEvents.Add(new BeatMap.CustomData.CustomEvents()
                {
                    _time = time + (i * repeatTime),
                    _type = "AssignPathAnimation",
                    _data = args.TryGetParameters().CustomEventsDataParse()
                });
            }
            ConsoleOut("AssignPathAnimation", repeatcount, time, "CustomEvent");
        }
        [SFunction]
        public void assignplayertotrack(string[] args, float time)
        {

            CustomEvents.Add(new BeatMap.CustomData.CustomEvents()
            {
                _time = time,
                _type = "AssignPlayerToTrack",
                _data = args.TryGetParameters().CustomEventsDataParse()
            });
            ConsoleOut("AssignPlayerToTrack", 1, time, "CustomEvent");
        }
        [SFunction]
        public void parenttrack(string[] args, float time)
        {
            CustomEvents.Add(new BeatMap.CustomData.CustomEvents()
            {
                _time = time,
                _type = "AssignTrackParent",
                _data = args.TryGetParameters().CustomEventsDataParse()
            });
            ConsoleOut("AssignTrackParent", 1, time, "CustomEvent");
        }

        [SFunction]
        public void note(string[] args, float time)
        {
            int repeatcount = 1;
            float repeatTime = 0;
            int type = 1;
            int cutdirection = 1;
            //parse special parameters
            foreach (var p in args.TryGetParameters())
            {
                switch (p.Name)
                {
                    case "type":
                        type = Convert.ToInt32(p.Data);
                        break;
                    case "cutdirection":
                        cutdirection = Convert.ToInt32(p.Data);
                        break;
                    case "repeat":
                        repeatcount = Convert.ToInt32(p.Data);
                        break;
                    case "repeataddtime":
                        repeatTime = Convert.ToSingle(p.Data);
                        break;
                }
            }
            for (float i = 0; i < repeatcount; i++)
            {
                Notes.Add(new BeatMap.Note()
                {
                    _time = time + (i * repeatTime),
                    _lineIndex = 0,
                    _lineLayer = 0,
                    _cutDirection = cutdirection,
                    _type = type,
                    _customData = args.TryGetParameters().CustomDataParse()
                });
            }
            ConsoleOut("Note", repeatcount, time, "Note");
        }
        [SFunction]
        public void wall(string[] args, float time)
        {
            float duration = 1;
            int repeatcount = 1;
            float repeatTime = 0;

            foreach (var p in args.TryGetParameters())
            {
                switch (p.Name)
                {
                    case "repeat":
                        repeatcount = Convert.ToInt32(p.Data);
                        break;
                    case "repeataddtime":
                        repeatTime = Convert.ToSingle(p.Data);
                        break;
                    case "duration":
                        duration = Convert.ToSingle(p.Data);
                        break;
                }
            }
            for (float i = 0; i < repeatcount; i++)
            {
                Walls.Add(new BeatMap.Obstacle()
                {
                    _time = time + (i * repeatTime),
                    _duration = duration,
                    _lineIndex = 0,
                    _width = 0,
                    _type = 0,
                    _customData = args.TryGetParameters().CustomDataParse()
                });
            }

            ConsoleOut("Wall", repeatcount, time, "Wall");
        }
        [SFunction]
        public void import(string[] args, float time)
        {
            string Path = string.Empty;
            int[] Type = { 0, 1, 2, 3 };
            float startbeat = 0;
            float addtime = 0;
            float endbeat = 1000000;

            foreach (var p in args.TryGetParameters())
            {
                switch (p.Name)
                {
                    case "path":
                        Path = Startup.ScuffedConfig.MapFolderPath + @"\" + p.Data.removeWhiteSpace();
                        break;
                    case "fullpath":
                        Path = p.Data;
                        break;
                    case "type":
                        Type = p.Data.Split(",").Select(a => Convert.ToInt32(a)).ToArray();
                        break;
                    case "frombeat":
                        startbeat = Convert.ToSingle(p.Data);
                        break;
                    case "addtime":
                        addtime = Convert.ToSingle(p.Data);
                        break;
                    case "tobeat":
                        endbeat = Convert.ToSingle(p.Data);
                        break;
                }
            }

            BeatMap beatMap = JsonSerializer.Deserialize<BeatMap>(File.ReadAllText(Path));
            beatMap._customData ??= new BeatMap.CustomData();
            beatMap._customData._customEvents ??= new BeatMap.CustomData.CustomEvents[] { };
            if (Type.Any(t => t == 0))
            {
                Walls.AddRange(beatMap._obstacles.GetAllBetween(startbeat, endbeat).Select(o => { o._time = o._time.toFloat() + addtime; return o; }));
                ConsoleOut("Wall", beatMap._obstacles.GetAllBetween(startbeat, endbeat).Length, time, "Import");
            }
            if (Type.Any(t => t == 1))
            {
                Notes.AddRange(beatMap._notes.GetAllBetween(startbeat, endbeat).Select(o => { o._time = o._time.toFloat() + addtime; return o; }));
                ConsoleOut("Note", beatMap._notes.GetAllBetween(startbeat, endbeat).Length, time, "Import");
            }
            if (Type.Any(t => t == 2))
            {
                Lights.AddRange(beatMap._events.GetAllBetween(startbeat, endbeat).Select(o => { o._time = o._time.toFloat() + addtime; return o; }));
                ConsoleOut("Light", beatMap._events.GetAllBetween(startbeat, endbeat).Length, time, "Import");
            }
            if (Type.Any(t => t == 3) && beatMap._customData != null)
            {
                if (beatMap._customData._customEvents != null)
                {
                    CustomEvents.AddRange(beatMap._customData._customEvents.GetAllBetween(startbeat, endbeat).Select(o => { o._time = o._time.toFloat() + addtime; return o; }));
                    ConsoleOut("CustomEvent", beatMap._customData._customEvents.GetAllBetween(startbeat, endbeat).Length, time, "Import");
                }
                if (beatMap._customData._pointDefinitions != null)
                {
                    PointDefinitions.AddRange(beatMap._customData._pointDefinitions);
                    ConsoleOut("PointDefinition", beatMap._customData._pointDefinitions.Length, time, "Import");
                }
            }

        }
        [SFunction]
        public void modeltowall(string[] args, float time)
        {
            string Path = string.Empty;
            bool hasanimation = false;
            int normal = 0;
            float duration = 1;
            float smooth = 0;
            float MapBpm = Startup.Info._beatsPerMinute.toFloat();
            float MapNjs = Startup.InfoDifficulty._noteJumpMovementSpeed.toFloat();


            foreach (var p in args.TryGetParameters())
            {
                switch (p.Name)
                {
                    case "normal":
                        normal = Convert.ToInt32(bool.Parse(p.Data));
                        break;
                    case "path":
                        Path = Startup.ScuffedConfig.MapFolderPath + @"\" + p.Data.removeWhiteSpace();
                        break;
                    case "fullpath":
                        Path = p.Data;
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
                }
            }
            ModelSettings settings = new ModelSettings() { spread = smooth, Path = Path, technique = (ModelTechnique)normal, HasAnimation = hasanimation, BPM = MapBpm, NJS = MapNjs, Wall = new BeatMap.Obstacle() { _time = time, _duration = duration, _customData = args.TryGetParameters().CustomDataParse() } };
            BeatMap.Obstacle[] model = new WallModel(settings).Walls;
            Walls.AddRange(model);
            ConsoleOut("Wall", model.Length, time, "ModelToWall");

        }


        [SFunction]
        public void imagetowall(string[] args, float time)
        {
            string Path = string.Empty;
            float duration = 1;
            bool isBlackEmpty = false;
            bool centered = false;
            float size = 1;
            float shift = 2;
            float alpha = 1;
            float thicc = 1;
            int maxlength = 100000;
            float compression = 0;
            float spreadspawntime = 0;
            foreach (var p in args.TryGetParameters())
            {
                Console.WriteLine(p.Data + " " + p.Name);
                switch (p.Name)
                {
                    case "path":
                        Path = Startup.ScuffedConfig.MapFolderPath + @"\" + p.Data.removeWhiteSpace();
                        break;
                    case "fullpath":
                        Path = p.Data;
                        break;
                    case "duration":
                        duration = Convert.ToSingle(p.Data);
                        break;
                    case "maxlinelength":
                        maxlength = Convert.ToInt32(p.Data);
                        break;
                    case "isblackempty":
                        isBlackEmpty = Convert.ToBoolean(p.Data);
                        break;
                    case "size":
                        size = Convert.ToSingle(p.Data);
                        Console.WriteLine($"size has been changed to {size} argument {p.Data} to float {p.Data.toFloat()}");
                        break;
                    case "spreadspawntime":
                        spreadspawntime = Convert.ToSingle(p.Data);
                        break;
                    case "alpha":
                        alpha = Convert.ToSingle(p.Data);
                        break;
                    case "thicc":
                        thicc = Convert.ToSingle(p.Data);
                        break;
                    case "shift":
                        shift = Convert.ToSingle(p.Data);
                        break;
                    case "centered":
                        centered = Convert.ToBoolean(p.Data);
                        break;
                    case "compression":
                        compression = Convert.ToSingle(p.Data);
                        break;
                }
            }
            WallImage converter = new WallImage(Path, new ImageSettings() { maxPixelLength = maxlength, isBlackEmpty = isBlackEmpty, scale = size, thicc = thicc, shift = shift, centered = centered, spread = spreadspawntime, alfa = alpha, tolerance = compression, Wall = new BeatMap.Obstacle() { _time = time, _duration = duration, _customData = args.TryGetParameters().CustomDataParse() } });
            BeatMap.Obstacle[] image = converter.GetWalls();
            Walls.AddRange(image);
            ConsoleOut("Wall", image.Length, time, "ImageToWall");
        }
        [SFunction]
        public void appendtoallwallsbetween(string[] args, float time)
        {
            //clarify type of appendage
            AppendTechnique type = 0;

            bool b = false;
            float starttime = time;
            float endtime = float.PositiveInfinity;

            foreach (var p in args.TryGetParameters())
            {
                if (p.Name == "appendtechnique") type = (AppendTechnique)Convert.ToInt32(p.Data);
                else if (p.Name == "tobeat") { endtime = Convert.ToSingle(p.Data); b = true; }
            }
            if (!b) ConsoleErrorLogger.Log($"missing \"ToBeat\" parameter -> AppendToAllWallsBetween at beat {time}");

            int i = 0;
            List<BeatMap.Obstacle> ApendedWalls = new List<BeatMap.Obstacle>();
            foreach (var wall in Walls)
            {
                if (Convert.ToSingle(wall._time.ToString()) >= starttime && Convert.ToSingle(wall._time.ToString()) <= endtime)
                {
                    ApendedWalls.Add(wall.Append(args.TryGetParameters().CustomDataParse(), type));
                    //Console.WriteLine(((AppendTechnique)type).ToString());
                    i++;
                }
                else
                {
                    ApendedWalls.Add(wall);
                }
            }
            Walls = new List<BeatMap.Obstacle>();
            Walls.AddRange(ApendedWalls.ToArray());
            ScuffedLogger.ScuffedWorkspace.FunctionParser.Log($"Appended {i} walls from beats {starttime} to {endtime}");
        }
        [SFunction]
        public void appendtoallnotesbetween(string[] args, float time)
        {
            //clarify type of appendage
            int type = 0;

            bool b = false;
            float starttime = time;
            float endtime = float.PositiveInfinity;
            int[] notetype = { 0, 1, 2, 3 };

            foreach (var p in args.TryGetParameters())
            {
                switch (p.Name)
                {
                    case "appendtechnique":
                        type = Convert.ToInt32(p.Data);
                        break;
                    case "tobeat":
                        endtime = Convert.ToSingle(p.Data); b = true;
                        break;
                    case "notecolor":
                        if (p.Data == "red") notetype = new int[] { 0 };
                        else if (p.Data == "blue") notetype = new int[] { 1 };
                        else if (p.Data == "bomb") notetype = new int[] { 2 };
                        else notetype = p.Data.Split(",").Select(a => { return Convert.ToInt32(a); }).ToArray();
                        break;
                }
            }
            if (!b) throw new ScuffedException($"missing \"ToBeat\" parameter -> AppendToAllNotesBetween at beat {time}");

            List<BeatMap.Note> ApendedNotes = new List<BeatMap.Note>();
            int i = 0;
            foreach (var note in Notes)
            {
                if ((Convert.ToSingle(note._time.ToString()) >= starttime && Convert.ToSingle(note._time.ToString()) <= endtime) && notetype.Any(t => t == Convert.ToInt32(note._type.ToString())))
                {
                    ApendedNotes.Add(note.Append(args.TryGetParameters().CustomDataParse(), (AppendTechnique)type));
                    i++;
                }
                else
                {
                    ApendedNotes.Add(note);
                }
            }
            Notes = new List<BeatMap.Note>();
            Notes.AddRange(ApendedNotes.ToArray());
            ScuffedLogger.ScuffedWorkspace.FunctionParser.Log($"Appended {i} notes from beats {starttime} to {endtime}");
        }
    }

}
