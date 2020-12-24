using System;
using System.Collections.Generic;
using ModChart;
using System.Text;

namespace ScuffedWalls
{
    partial class FunctionParser
    {
        static public void ConsoleOut(string Type, int Amount, float Beat)
        {
            Console.ForegroundColor = ConsoleColor.White;
            ScuffedLogger.ScuffedWorkspace.FunctionParser.Log($"Added {Amount} {Type}'s at beat {Beat}");
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
                _notes = notes.ToArray(),
                _obstacles = obstacles.ToArray(),
                _events = events.ToArray(),
                _customData = new BeatMap.CustomData() { _customEvents = customEvents.ToArray(), _pointDefinitions = pointDefinitions.ToArray() }
            };
        }
        public static BeatMap toBeatMap(Workspace workspace)
        {
            return new BeatMap()
            {
                _version = "2.0.0",
                _notes = workspace.Notes,
                _obstacles = workspace.Walls,
                _events = workspace.Lights,
                _customData = new BeatMap.CustomData() { _customEvents = workspace.CustomEvents, _pointDefinitions = workspace.PointDefinitions}
            };
        }
    }
}
