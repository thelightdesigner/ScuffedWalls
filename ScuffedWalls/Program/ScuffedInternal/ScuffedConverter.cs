using System;
using System.Collections.Generic;
using ModChart;
using ModChart.Note;
using ModChart.Event;
using ModChart.Wall;
using System.Linq;

namespace ScuffedWalls
{
    partial class FunctionParser
    {
        static public void ConsoleOut(string Type, int Amount, float Beat, string Purpose)
        {
            Console.ForegroundColor = ConsoleColor.White;
            ScuffedLogger.ScuffedWorkspace.FunctionParser.Log($"Added {Purpose} at beat {Beat} ({Amount} {Type}s)");
            Console.ResetColor();
        }
        public Workspace toWorkspace()
        {
            return new Workspace()
            {
                Notes = Notes.ToArray(),
                Walls = Walls.ToArray(),
                CustomEvents = CustomEvents.ToArray(),
                Lights = Lights.ToArray(),
                PointDefinitions = PointDefinitions.ToArray()
            };
        }
        public static BeatMap toBeatMap(Workspace[] workspaces)
        {
            List<BeatMap.Obstacle> obstacles = new List<BeatMap.Obstacle>();
            List<BeatMap.Note> notes = new List<BeatMap.Note>();
            List<BeatMap.Event> events = new List<BeatMap.Event>();
            List<BeatMap.CustomData.CustomEvents> customEvents = new List<BeatMap.CustomData.CustomEvents>();
            List<BeatMap.CustomData.PointDefinition> pointDefinitions = new List<BeatMap.CustomData.PointDefinition>();
            foreach (var workspace in workspaces)
            {
                customEvents.AddRange(workspace.CustomEvents);
                notes.AddRange(workspace.Notes);
                obstacles.AddRange(workspace.Walls);
                events.AddRange(workspace.Lights);
                pointDefinitions.AddRange(workspace.PointDefinitions);
            }
            return new BeatMap()
            {
                _version = "2.0.0",
                _notes = notes.ToArray().OrderBy(o =>  o.GetTime()).ToArray(),
                _obstacles = obstacles.ToArray().OrderBy(o => o.GetTime()).ToArray(),
                _events = events.ToArray().OrderBy(o => o.GetTime()).ToArray(),
                _customData = new BeatMap.CustomData() { _customEvents = customEvents.ToArray().OrderBy(o => float.Parse(o._time.ToString())).ToArray(), _pointDefinitions = pointDefinitions.ToArray() }
            };
        }
        public static BeatMap toBeatMap(Workspace workspace)
        {
            return new BeatMap()
            {
                _version = "2.0.0",
                _notes = workspace.Notes.OrderBy(o => o.GetTime()).ToArray(),
                _obstacles = workspace.Walls.OrderBy(o => o.GetTime()).ToArray(),
                _events = workspace.Lights.OrderBy(o => o.GetTime()).ToArray(),
                _customData = new BeatMap.CustomData() { _customEvents = workspace.CustomEvents.OrderBy(o => float.Parse(o._time.ToString())).ToArray(), _pointDefinitions = workspace.PointDefinitions}
            };
        }
    }
}
