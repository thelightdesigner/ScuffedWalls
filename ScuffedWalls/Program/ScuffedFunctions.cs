using ModChart;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace ScuffedWalls
{

    class NoodleFunctions
    {
        public NoodleFunctions(string mapfolderpath, NoodleFunctions[] workspaces)
        {
            MapFolderPath = mapfolderpath;
            Workspaces = workspaces;
        }
        public NoodleFunctions[] Workspaces;
        public List<BeatMap.Note> Notes = new List<BeatMap.Note>();
        public List<BeatMap.Event> Lights = new List<BeatMap.Event>();
        public List<BeatMap.Obstacle> Walls = new List<BeatMap.Obstacle>();
        public List<BeatMap.CustomData.CustomEvents> CustomEvents = new List<BeatMap.CustomData.CustomEvents>();
        private string MapFolderPath;
        static public void ConsoleOut(string Type, int Amount, float Beat)
        {
            Console.ForegroundColor = ConsoleColor.White;
            ConsoleLogger.ScuffedWorkspace.FunctionParser.Log($"Added {Amount} {Type}'s at beat {Beat}");
            Console.ResetColor();
        }

        public static NoodleFunctions parseWorkspace(string[] args, string ThisMapFolder, List<NoodleFunctions> workspaces)
        {
            NoodleFunctions Workspace = new NoodleFunctions(ThisMapFolder, workspaces.ToArray());
            for (int i = 0; i < args.Length; i++)
            {

                if (Char.IsNumber(args[i][0]))
                {
                    string functionName = args[i].Split(':')[1].Trim(' ').ToLower();
                    float time = Convert.ToSingle(args[i].Split(':')[0].Trim(' '));
                    if (functionName.MethodExists<NoodleFunctions>())
                    {
                        try
                        {
                            typeof(NoodleFunctions).GetMethod(functionName).Invoke(Workspace, new object[] { ScuffedFile.getLinesUntilNextFunction(i, args), time });
                        }
                        catch
                        {
                            ConsoleErrorLogger.ScuffedWorkspace.FunctionParser.Log($"Error parsing function \"{functionName}\" at beat {time}");
                        }
                    }
                    else ConsoleErrorLogger.ScuffedWorkspace.FunctionParser.Log($"Function \"{functionName}\" does not exist, Skipping");
                }

            }
            return Workspace;
        }
        public static BeatMap toBeatMap(NoodleFunctions[] workspaces)
        {
            List<BeatMap.Obstacle> obstacles = new List<BeatMap.Obstacle>();
            List<BeatMap.Note> notes = new List<BeatMap.Note>();
            List<BeatMap.Event> events = new List<BeatMap.Event>();
            List<BeatMap.CustomData.CustomEvents> customEvents = new List<BeatMap.CustomData.CustomEvents>();
            foreach (var workspace in workspaces)
            {
                customEvents.AddRange(workspace.CustomEvents);
                notes.AddRange(workspace.Notes);
                obstacles.AddRange(workspace.Walls);
                events.AddRange(workspace.Lights);
            }
            return new BeatMap()
            {
                _version = "2.0.0",
                _notes = notes.ToArray(),
                _obstacles = obstacles.ToArray(),
                _events = events.ToArray(),
                _customData = new BeatMap.CustomData() { _customEvents = customEvents.ToArray() }
            };
        }
        public static BeatMap toBeatMap(NoodleFunctions workspace)
        {
            List<BeatMap.Obstacle> obstacles = new List<BeatMap.Obstacle>();
            List<BeatMap.Note> notes = new List<BeatMap.Note>();
            List<BeatMap.Event> events = new List<BeatMap.Event>();
            List<BeatMap.CustomData.CustomEvents> customEvents = new List<BeatMap.CustomData.CustomEvents>();
            customEvents.AddRange(workspace.CustomEvents);
            notes.AddRange(workspace.Notes);
            obstacles.AddRange(workspace.Walls);
            events.AddRange(workspace.Lights);

            return new BeatMap()
            {
                _version = "2.0.0",
                _notes = notes.ToArray(),
                _obstacles = obstacles.ToArray(),
                _events = events.ToArray(),
                _customData = new BeatMap.CustomData() { _customEvents = customEvents.ToArray() }
            };
        }
        public void clonefromworkspacebyindex(string[] args, float time)
        {
            int Type = 0;
            int Index = 0;
            float startbeat = 0;
            float endbeat = 1000000;

            foreach (var p in args.GetParameters())
            {
                if (p.parameter == "indexofworkspace") Index = Convert.ToInt32(p.argument);
                else if (p.parameter == "type") Type = Convert.ToInt32(p.argument);
                else if (p.parameter == "frombeat") startbeat = Convert.ToSingle(p.argument);
                else if (p.parameter == "tobeat") endbeat = Convert.ToSingle(p.argument);
            }
            BeatMap beatMap = NoodleFunctions.toBeatMap(Workspaces[Index]);
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
            CustomEvents.AddRange(beatMap._customData._customEvents);
        }
        public void blackout(string[] args, float time)
        {
            Lights.Add(new BeatMap.Event() { _time = 1, _type = 0, _value = 0 });
        }
        public void animatetrack(string[] args, float time)
        {
            int repeatcount = 1;
            float repeatTime = 0;
            foreach (var p in args.GetParameters())
            {
                if (p.parameter == "repeat") repeatcount = Convert.ToInt32(p.argument);
                else if (p.parameter == "repeataddtime") repeatTime = Convert.ToSingle(p.argument);
            }
            for (float i = 0; i < repeatcount; i++)
            {
                CustomEvents.Add(NoodleCustomEvents.CustomEventConstructor(time + (i * repeatTime), "AnimateTrack", args.GetParameters().toUsableCustomData().CustomEventsDataParse()));
            }
            ConsoleOut("AnimateTrack", repeatcount, time);
        }

        public void assignpathanimation(string[] args, float time)
        {
            int repeatcount = 1;
            float repeatTime = 0;
            foreach (var p in args.GetParameters())
            {
                if (p.parameter == "repeat") repeatcount = Convert.ToInt32(p.argument);
                else if (p.parameter == "repeataddtime") repeatTime = Convert.ToSingle(p.argument);
            }
            for (float i = 0; i < repeatcount; i++)
            {
                CustomEvents.Add(NoodleCustomEvents.CustomEventConstructor(time + (i * repeatTime), "AssignPathAnimation", args.GetParameters().toUsableCustomData().CustomEventsDataParse()));
            }
            ConsoleOut("AssignPathAnimation", repeatcount, time);
        }

        public void assignplayertotrack(string[] args, float time)
        {
            CustomEvents.Add(NoodleCustomEvents.CustomEventConstructor(time, "AssignPlayerToTrack", args.GetParameters().toUsableCustomData().CustomEventsDataParse()));
            ConsoleOut("AssignPlayerToTrack", 1,time);
        }

        public void parenttrack(string[] args, float time)
        {
            CustomEvents.Add(NoodleCustomEvents.CustomEventConstructor(time, "ParentTrack", args.GetParameters().toUsableCustomData().CustomEventsDataParse()));
            ConsoleOut("AssignPlayerToTrack", 1, time);
        }


        public void note(string[] args, float time)
        {
            BeatMap.Note note = new BeatMap.Note() { _time = time, _lineIndex = 0, _type = 0, _lineLayer = 0, _cutDirection = 1 };
            BeatMap.CustomData data = new BeatMap.CustomData();

            //parse special parameters
            foreach (var p in args.GetParameters())
            {
                if (p.parameter == "lineindex") note._lineIndex = Convert.ToInt32(p.argument);
                else if (p.parameter == "linelayer") note._lineLayer = Convert.ToInt32(p.argument);
                else if (p.parameter == "type") note._type = Convert.ToInt32(p.argument);
                else if (p.parameter == "cutdirection") note._cutDirection = Convert.ToInt32(p.argument);
            }
            //parse generic customdata
            data = args.GetParameters().toUsableCustomData().CustomDataParse();

            note._customData = data;
            Notes.Add(note);
            ConsoleOut("Note", 1, time);
        }
        public void wall(string[] args, float time)
        {
            float duration = 1;
            int repeatcount = 1;
            float repeatTime = 0;

            foreach (var p in args.GetParameters())
            {
                if (p.parameter == "repeat") repeatcount = Convert.ToInt32(p.argument);
                else if (p.parameter == "repeataddtime") repeatTime = Convert.ToSingle(p.argument);
                else if (p.parameter == "duration") duration = Convert.ToInt32(p.argument);
            }
            for (float i = 0; i < repeatcount; i++)
            {
                Walls.Add(NoodleWall.WallConstructor(time + (i * repeatTime), duration, args.GetParameters().toUsableCustomData().CustomDataParse()));
            }

            ConsoleOut("Wall", repeatcount, time);
        }
        public void import(string[] args, float time)
        {
            string Path = string.Empty;
            int Type = 0;
            float startbeat = 0;
            float endbeat = 1000000;

            foreach (var p in args.GetParameters())
            {
                if (p.parameter == "path") Path = MapFolderPath + @"\" + p.argument;
                else if (p.parameter == "fullpath") Path = p.argument;
                else if (p.parameter == "type") Type = Convert.ToInt32(p.argument);
                else if (p.parameter == "frombeat") startbeat = Convert.ToSingle(p.argument);
                else if (p.parameter == "tobeat") endbeat = Convert.ToSingle(p.argument);
            }

            BeatMap beatMap = JsonSerializer.Deserialize<BeatMap>(File.ReadAllText(Path));
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
            CustomEvents.AddRange(beatMap._customData._customEvents);
            ConsoleOut("CustomEvent", beatMap._customData._customEvents.Length, time);

        }
        public void modeltowall(string[] args, float time)
        {
            string Path = string.Empty;
            bool hasanimation = false;
            float duration = 1;
            float smooth = 0;

            foreach (var p in args.GetParameters())
            {
                if (p.parameter == "path") Path = MapFolderPath + @"\" + p.argument;
                else if (p.parameter == "fullpath") Path = p.argument;
                else if (p.parameter == "hasanimation") hasanimation = bool.Parse(p.argument);
                else if (p.parameter == "duration") duration = Convert.ToInt32(p.argument);
                else if (p.parameter == "spreadspawntime") smooth = Convert.ToSingle(p.argument);
            }

            BeatMap.Obstacle[] model = Internal.Model2Wall(Path, time, smooth, duration, hasanimation, args.GetParameters().toUsableCustomData());
            Walls.AddRange(model);
            ConsoleOut("Wall", model.Length, time);
        }

        public void imagetowall(string[] args, float time)
        {
            string Path = string.Empty;
            float duration = 1;
            bool isBlackEmpty = false;
            float size = 1;
            foreach (var p in args.GetParameters())
            {
                if (p.parameter == "path") Path = MapFolderPath + @"\" + p.argument;
                else if (p.parameter == "fullpath") Path = p.argument;
                else if (p.parameter == "duration") duration = Convert.ToSingle(p.argument);
                else if (p.parameter == "isblackempty") isBlackEmpty = Convert.ToBoolean(p.argument);
                else if (p.parameter == "size") size = Convert.ToSingle(p.argument);
            }
            BeatMap.Obstacle[] image = Internal.ImageToWall(Path, time, duration, isBlackEmpty, size, args.GetParameters().toUsableCustomData());
            Walls.AddRange(image);
            ConsoleOut("Wall", image.Length, time);
        }
        public void appendtoallwallsbetween(string[] args, float time)
        {
            //clarify type of appendage
            int type = 0;

            bool b = false;
            float starttime = time;
            float endtime = time + 30;

            foreach (var p in args.GetParameters())
            {
                if (p.parameter == "appendtechnique") type = Convert.ToInt32(p.argument);
                else if (p.parameter == "tobeat") endtime = Convert.ToSingle(p.argument); b = true;
            }
            if (!b) ConsoleErrorLogger.ScuffedWorkspace.FunctionParser.Log($"missing \"ToBeat\" parameter -> AppendToAllWallsBetween at beat {time}");

            int i = 0;
            List<BeatMap.Obstacle> ApendedWalls = new List<BeatMap.Obstacle>();
            foreach (var wall in Walls)
            {
                if (Convert.ToSingle(wall._time.ToString()) >= starttime && Convert.ToSingle(wall._time.ToString()) <= endtime)
                {
                    ApendedWalls.Add(NoodleWall.WallAppend(wall, args.GetParameters().toUsableCustomData().CustomDataParse(), type));
                    i++;
                }
                else
                {
                    ApendedWalls.Add(wall);
                }
            }
            Walls = new List<BeatMap.Obstacle>();
            Walls.AddRange(ApendedWalls.ToArray());
            ConsoleLogger.ScuffedWorkspace.FunctionParser.Log($"Appended {i} walls from beats {starttime} to {endtime}");
        }
        public void appendtoallnotesbetween(string[] args, float time)
        {
            //clarify type of appendage
            int type = 0;

            bool b = false;
            float starttime = time;
            float endtime = time + 30;
            int notecolor = 3;

            foreach (var p in args.GetParameters())
            {
                if (p.parameter == "appendtechnique") type = Convert.ToInt32(p.argument);
                else if (p.parameter == "tobeat") { endtime = Convert.ToSingle(p.argument); b = true; }
                else if (p.parameter == "notecolor") { if (p.argument == "red") notecolor = 0; }
                else if (p.parameter == "notecolor") { if (p.argument == "blue") notecolor = 1; }
                else if (p.parameter == "notecolor") { if (p.argument == "bomb") notecolor = 2; }
            }
            if (!b) ConsoleErrorLogger.ScuffedWorkspace.FunctionParser.Log($"missing \"ToBeat\" parameter -> AppendToAllNotesBetween at beat {time}");

            List<BeatMap.Note> ApendedNotes = new List<BeatMap.Note>();
            int i = 0;
            foreach (var note in Notes)
            {
                if ((Convert.ToSingle(note._time.ToString()) >= starttime && Convert.ToSingle(note._time.ToString()) <= endtime) && (notecolor == 3 || Convert.ToInt32(note._type.ToString()) == notecolor))
                {
                    ApendedNotes.Add(NoodleNote.NoteAppend(note, args.GetParameters().toUsableCustomData().CustomDataParse(), type));
                    i++;
                }
                else
                {
                    ApendedNotes.Add(note);
                }
            }
            Notes = new List<BeatMap.Note>();
            Notes.AddRange(ApendedNotes.ToArray());
            ConsoleLogger.ScuffedWorkspace.FunctionParser.Log($"Appended {i} notes from beats {starttime} to {endtime}");
        }

    }
}
