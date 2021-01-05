using ModChart;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace ScuffedWalls
{
    partial class FunctionParser
    {
        public FunctionParser(string mapfolderpath, Workspace[] workspaces)
        {
            MapFolderPath = mapfolderpath;
            Workspaces = workspaces;
        }
        private Workspace[] Workspaces;
        public List<BeatMap.Note> Notes = new List<BeatMap.Note>();
        public List<BeatMap.CustomData.PointDefinition> PointDefinitions = new List<BeatMap.CustomData.PointDefinition>();
        public List<BeatMap.Event> Lights = new List<BeatMap.Event>();
        public List<BeatMap.Obstacle> Walls = new List<BeatMap.Obstacle>();
        public List<BeatMap.CustomData.CustomEvents> CustomEvents = new List<BeatMap.CustomData.CustomEvents>();
        private string MapFolderPath;


        public static FunctionParser parseWorkspace(string[] args, string ThisMapFolder, Workspace[] workspaces)
        {
            FunctionParser Workspace = new FunctionParser(ThisMapFolder, workspaces);
            for (int i = 0; i < args.Length; i++)
            {

                if (Char.IsNumber(args[i][0]))
                {
                    string functionName = args[i].Split(':')[1].Trim(' ').ToLower();
                    float time = Convert.ToSingle(args[i].Split(':')[0].Trim(' '));
                    if (functionName.MethodExists<FunctionParser>())
                    {
                        try
                        {
                            typeof(FunctionParser).GetMethod(functionName)
                                .Invoke(Workspace, new object[] { ScuffedFile.getLinesUntilNextFunction(i, args), time });
                        }
                        catch
                        {
                            throw new ScuffedException($"Error parsing function \"{functionName}\" at beat {time}");
                        }
                    }
                    else throw new ScuffedException($"Function \"{functionName}\" does not exist, Skipping");
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
                if (p.parameter == "appendtechnique") type = Convert.ToInt32(p.argument);
                else if (p.parameter == "tobeat") { endtime = Convert.ToSingle(p.argument); b = true; }
                else if (p.parameter == "converttoprops") props = bool.Parse(p.argument);
                else if (p.parameter == "converttorainbow") rainbow = bool.Parse(p.argument);
                else if (p.parameter == "rainbowfactor") Rfactor = Convert.ToSingle(p.argument);
                else if (p.parameter == "propfactor") Pfactor = Convert.ToSingle(p.argument);
                else if (p.parameter == "lighttype") lightypes = p.argument.Split(",").Select(a => { return Convert.ToInt32(a); }).ToArray();
            }
            if (!b) throw new ScuffedException($"missing \"ToBeat\" parameter -> AppendToAllWallsBetween at beat {time}");

            if (rainbow)
            {
                List<BeatMap.Event> ApendedEvents = new List<BeatMap.Event>();
                int i = 0;
                foreach (var light in Lights)
                {
                    if (light.GetTime() >= starttime && light.GetTime() <= endtime && lightypes.Any(t => t == Event.GetType(light)))
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
                ConsoleOut("Event", i, time);
            }

            if (props)
            {
                List<BeatMap.Event> newEvents = new List<BeatMap.Event>();
                int c = 0;
                foreach (var light in Lights)
                {
                    if (light.GetTime() >= starttime && light.GetTime() <= endtime && lightypes.Any(t => t == Event.GetType(light)))
                    {
                        int count = (Convert.ToInt32(light._type.ToString())).getCountByID();
                        for (int i = 0; i < count; i++)
                        {
                            newEvents.Add(new BeatMap.Event()
                            {
                                _time = light.GetTime() + Pfactor - (Convert.ToSingle(i) / (Pfactor * Convert.ToSingle(count))),
                                _type = Event.GetType(light),
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
                ConsoleOut("Event", c, time);
            }

            {
                List<BeatMap.Event> ApendedEvents = new List<BeatMap.Event>();
                int i = 0;
                foreach (var light in Lights)
                {
                    if (Convert.ToSingle(light._time.ToString()) >= starttime && Convert.ToSingle(light._time.ToString()) <= endtime && lightypes.Any(t => t == Event.GetType(light)))
                    {
                        ApendedEvents.Add(light.EventAppend(args.TryGetParameters().toUsableCustomData().CustomDataParse(), (AppendTechnique)type));
                        i++;
                    }
                    else
                    {
                        ApendedEvents.Add(light);
                    }
                }
                Lights = new List<BeatMap.Event>();
                Lights.AddRange(ApendedEvents.ToArray());
                ConsoleOut("Event", i, time);
            }
        }
        [SFunction]
        public void texttowall()
        {
            //nothing yet...
        }

        [SFunction]
        public void pointdefinition(string[] args, float time)
        {
            string name = null;
            object[][] points = null;
            foreach (var p in args.TryGetParameters())
            {
                switch (p.parameter)
                {
                    case "name":
                        name = p.argument;
                        break;
                    case "points":
                        points = JsonSerializer.Deserialize<object[][]>($"[{p.argument}]");
                        break;
                }
            }

            PointDefinitions.Add(new BeatMap.CustomData.PointDefinition()
            {
                _name = name,
                _points = points
            });

            ConsoleOut("PointDefinition", 1, time);
        }
        [SFunction]
        public void clonefromworkspacebyindex(string[] args, float time)
        {
            int Type = 0;
            int Index = 0;
            float startbeat = 0;
            float endbeat = 1000000;

            foreach (var p in args.TryGetParameters())
            {
                switch (p.parameter)
                {
                    case "index":
                        Index = Convert.ToInt32(p.argument);
                        break;
                    case "type":
                        Type = Convert.ToInt32(p.argument);
                        break;
                    case "frombeat":
                        startbeat = Convert.ToSingle(p.argument);
                        break;
                    case "tobeat":
                        endbeat = Convert.ToSingle(p.argument);
                        break;
                }
            }
            BeatMap beatMap = toBeatMap(Workspaces[Index].DeepClone());
            beatMap._customData._customEvents ??= new BeatMap.CustomData.CustomEvents[] { };
            if (Type == 0)
            {
                Walls.AddRange(beatMap._obstacles.GetAllBetween(startbeat, endbeat));
                ConsoleOut("Wall", beatMap._obstacles.GetAllBetween(startbeat, endbeat).Length, time);
                Notes.AddRange(beatMap._notes.GetAllBetween(startbeat, endbeat));
                ConsoleOut("Note", beatMap._notes.GetAllBetween(startbeat, endbeat).Length, time);
                Lights.AddRange(beatMap._events.GetAllBetween(startbeat, endbeat));
                ConsoleOut("Light", beatMap._events.GetAllBetween(startbeat, endbeat).Length, time);
            }
            else if (Type == 1)
            {
                Walls.AddRange(beatMap._obstacles.GetAllBetween(startbeat, endbeat));
                ConsoleOut("Wall", beatMap._obstacles.GetAllBetween(startbeat, endbeat).Length, time);
            }
            else if (Type == 2)
            {
                Notes.AddRange(beatMap._notes.GetAllBetween(startbeat, endbeat));
                ConsoleOut("Note", beatMap._notes.GetAllBetween(startbeat, endbeat).Length, time);
            }
            else if (Type == 3)
            {
                Lights.AddRange(beatMap._events.GetAllBetween(startbeat, endbeat));
                ConsoleOut("Light", beatMap._events.GetAllBetween(startbeat, endbeat).Length, time);
            }
        }
        [SFunction]
        public void blackout(string[] args, float time)
        {
            Lights.Add(new BeatMap.Event() { _time = 1, _type = 0, _value = 0 });
        }
        [SFunction]
        public void animatetrack(string[] args, float time)
        {
            int repeatcount = 1;
            float repeatTime = 0;
            foreach (var p in args.TryGetParameters())
            {
                if (p.parameter == "repeat") repeatcount = Convert.ToInt32(p.argument);
                else if (p.parameter == "repeataddtime") repeatTime = Convert.ToSingle(p.argument);
            }
            for (float i = 0; i < repeatcount; i++)
            {
                CustomEvents.Add(NoodleCustomEvents.CustomEventConstructor(time + (i * repeatTime), "AnimateTrack", args.TryGetParameters().toUsableCustomData().CustomEventsDataParse()));
            }
            ConsoleOut("AnimateTrack", repeatcount, time);
        }
        [SFunction]
        public void assignpathanimation(string[] args, float time)
        {
            int repeatcount = 1;
            float repeatTime = 0;
            foreach (var p in args.TryGetParameters())
            {
                if (p.parameter == "repeat") repeatcount = Convert.ToInt32(p.argument);
                else if (p.parameter == "repeataddtime") repeatTime = Convert.ToSingle(p.argument);
            }
            for (float i = 0; i < repeatcount; i++)
            {
                CustomEvents.Add(NoodleCustomEvents.CustomEventConstructor(time + (i * repeatTime), "AssignPathAnimation", args.TryGetParameters().toUsableCustomData().CustomEventsDataParse()));
            }
            ConsoleOut("AssignPathAnimation", repeatcount, time);
        }
        [SFunction]
        public void assignplayertotrack(string[] args, float time)
        {
            CustomEvents.Add(NoodleCustomEvents.CustomEventConstructor(time, "AssignPlayerToTrack", args.TryGetParameters().toUsableCustomData().CustomEventsDataParse()));
            ConsoleOut("AssignPlayerToTrack", 1, time);
        }
        [SFunction]
        public void parenttrack(string[] args, float time)
        {
            CustomEvents.Add(NoodleCustomEvents.CustomEventConstructor(time, "AssignTrackParent", args.TryGetParameters().toUsableCustomData().CustomEventsDataParse()));
            ConsoleOut("AssignTrackParent", 1, time);
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
                switch (p.parameter)
                {
                    case "type":
                        type = Convert.ToInt32(p.argument);
                        break;
                    case "cutdirection":
                        cutdirection = Convert.ToInt32(p.argument);
                        break;
                    case "repeat":
                        repeatcount = Convert.ToInt32(p.argument);
                        break;
                    case "repeataddtime":
                        repeatTime = Convert.ToSingle(p.argument);
                        break;
                }
            }
            for (float i = 0; i < repeatcount; i++)
            {
                Notes.Add(NoodleNote.NoteConstructor(time + (i * repeatTime), type, cutdirection, args.TryGetParameters().toUsableCustomData().CustomDataParse()));
            }
            ConsoleOut("Note", repeatcount, time);
        }
        [SFunction]
        public void wall(string[] args, float time)
        {
            float duration = 1;
            int repeatcount = 1;
            float repeatTime = 0;

            foreach (var p in args.TryGetParameters())
            {
                switch (p.parameter)
                {
                    case "repeat":
                        repeatcount = Convert.ToInt32(p.argument);
                        break;
                    case "repeataddtime":
                        repeatTime = Convert.ToSingle(p.argument);
                        break;
                    case "duration":
                        duration = Convert.ToSingle(p.argument);
                        break;
                }
            }
            for (float i = 0; i < repeatcount; i++)
            {
                Walls.Add(NoodleWall.WallConstructor(time + (i * repeatTime), duration, args.TryGetParameters().toUsableCustomData().CustomDataParse()));
            }

            ConsoleOut("Wall", repeatcount, time);
        }
        [SFunction]
        public void import(string[] args, float time)
        {
            string Path = string.Empty;
            int Type = 0;
            float startbeat = 0;
            float endbeat = 1000000;

            foreach (var p in args.TryGetParameters())
            {
                switch (p.parameter)
                {
                    case "path":
                        Path = MapFolderPath + @"\" + p.argument;
                        break;
                    case "fullpath":
                        Path = p.argument;
                        break;
                    case "type":
                        Type = Convert.ToInt32(p.argument);
                        break;
                    case "frombeat":
                        startbeat = Convert.ToSingle(p.argument);
                        break;
                    case "tobeat":
                        endbeat = Convert.ToSingle(p.argument);
                        break;
                }
            }

            BeatMap beatMap = JsonSerializer.Deserialize<BeatMap>(File.ReadAllText(Path));
            beatMap._customData._customEvents ??= new BeatMap.CustomData.CustomEvents[] { };
            switch (Type)
            {
                case 0:
                    Walls.AddRange(beatMap._obstacles.GetAllBetween(startbeat, endbeat));
                    ConsoleOut("Wall", beatMap._obstacles.GetAllBetween(startbeat, endbeat).Length, time);
                    Notes.AddRange(beatMap._notes.GetAllBetween(startbeat, endbeat));
                    ConsoleOut("Note", beatMap._notes.GetAllBetween(startbeat, endbeat).Length, time);
                    Lights.AddRange(beatMap._events.GetAllBetween(startbeat, endbeat));
                    ConsoleOut("Light", beatMap._events.GetAllBetween(startbeat, endbeat).Length, time);
                    break;
                case 1:
                    Walls.AddRange(beatMap._obstacles.GetAllBetween(startbeat, endbeat));
                    ConsoleOut("Wall", beatMap._obstacles.GetAllBetween(startbeat, endbeat).Length, time);
                    break;
                case 2:
                    Notes.AddRange(beatMap._notes.GetAllBetween(startbeat, endbeat));
                    ConsoleOut("Note", beatMap._notes.GetAllBetween(startbeat, endbeat).Length, time);
                    break;
                case 3:
                    Lights.AddRange(beatMap._events.GetAllBetween(startbeat, endbeat));
                    ConsoleOut("Light", beatMap._events.GetAllBetween(startbeat, endbeat).Length, time);
                    break;
            }
            CustomEvents.AddRange(beatMap._customData._customEvents);
            ConsoleOut("CustomEvent", beatMap._customData._customEvents.Length, time);

        }
        [SFunction]
        public void modeltowall(string[] args, float time)
        {
            string Path = string.Empty;
            bool hasanimation = false;
            int normal = 0;
            float duration = 1;
            float smooth = 0;

            foreach (var p in args.TryGetParameters())
            {
                switch (p.parameter)
                {
                    case "normal":
                        if (bool.Parse(p.argument) == true) normal = 1;
                        break;
                    case "path":
                        Path = MapFolderPath + @"\" + p.argument;
                        break;
                    case "fullpath":
                        Path = p.argument;
                        break;
                    case "hasanimation":
                        hasanimation = Convert.ToBoolean(p.argument);
                        break;
                    case "duration":
                        duration = Convert.ToSingle(p.argument);
                        break;
                    case "spreadspawntime":
                        smooth = Convert.ToSingle(p.argument);
                        break;
                }
            }

            BeatMap.Obstacle[] model = NoodleWall.Model2Wall(Path, smooth, normal, hasanimation, args.TryGetParameters().toUsableCustomData().CustomDataParse(), new BeatMap.Obstacle() { _time = time, _duration = duration });
            Walls.AddRange(model);
            ConsoleOut("Wall", model.Length, time);
        }
        [SFunction]
        public void imagetowall(string[] args, float time)
        {
            string Path = string.Empty;
            float duration = 1;
            bool isBlackEmpty = false;
            bool centered = false;
            float size = 1;
            float alpha = 1;
            float thicc = 1;
            bool track = false;
            float spreadspawntime = 0;
            foreach (var p in args.TryGetParameters())
            {
                switch (p.parameter)
                {
                    case "path":
                        Path = MapFolderPath + @"\" + p.argument;
                        break;
                    case "fullpath":
                        Path = p.argument;
                        break;
                    case "duration":
                        duration = Convert.ToSingle(p.argument);
                        break;
                    case "isblackempty":
                        isBlackEmpty = Convert.ToBoolean(p.argument);
                        break;
                    case "size":
                        size = Convert.ToSingle(p.argument);
                        break;
                    case "alpha":
                        alpha = Convert.ToSingle(p.argument);
                        break;
                    case "thicc":
                        thicc = Convert.ToSingle(p.argument);
                        break;
                    case "normal":
                        track = Convert.ToBoolean(p.argument);
                        break;
                    case "centered":
                        centered = Convert.ToBoolean(p.argument);
                        break;
                }
            }
            BeatMap.Obstacle[] image = NoodleWall.Image2Wall(Path, isBlackEmpty, size, thicc, track, centered, spreadspawntime, alpha, args.TryGetParameters().toUsableCustomData().CustomDataParse(), NoodleWall.WallConstructor(time, duration));
            Walls.AddRange(image);
            ConsoleOut("Wall", image.Length, time);
        }
        [SFunction]
        public void appendtoallwallsbetween(string[] args, float time)
        {
            //clarify type of appendage
            int type = 0;

            bool b = false;
            float starttime = time;
            float endtime = time + 30;

            foreach (var p in args.TryGetParameters())
            {
                if (p.parameter == "appendtechnique") type = Convert.ToInt32(p.argument);
                else if (p.parameter == "tobeat") endtime = Convert.ToSingle(p.argument); b = true;
            }
            if (!b) throw new ScuffedException($"missing \"ToBeat\" parameter -> AppendToAllWallsBetween at beat {time}");

            int i = 0;
            List<BeatMap.Obstacle> ApendedWalls = new List<BeatMap.Obstacle>();
            foreach (var wall in Walls)
            {
                if (Convert.ToSingle(wall._time.ToString()) >= starttime && Convert.ToSingle(wall._time.ToString()) <= endtime)
                {
                    ApendedWalls.Add(NoodleWall.WallAppend(wall, args.TryGetParameters().toUsableCustomData().CustomDataParse(), type));
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
            float endtime = time + 30;
            int[] notetype = { 0, 1, 2, 3 };

            foreach (var p in args.TryGetParameters())
            {
                switch (p.parameter)
                {
                    case "appendtechnique":
                        type = Convert.ToInt32(p.argument);
                        break;
                    case "tobeat":
                        endtime = Convert.ToSingle(p.argument); b = true;
                        break;
                    case "notecolor":
                        if (p.argument == "red") notetype = new int[] { 0 };
                        else if (p.argument == "blue") notetype = new int[] { 1 };
                        else if (p.argument == "bomb") notetype = new int[] { 2 };
                        else notetype = p.argument.Split(",").Select(a => { return Convert.ToInt32(a); }).ToArray();
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
                    ApendedNotes.Add(NoodleNote.NoteAppend(note, args.TryGetParameters().toUsableCustomData().CustomDataParse(), type));
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
