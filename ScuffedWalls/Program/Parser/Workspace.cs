using ModChart;
using ModChart.Event;
using ModChart.Note;
using ModChart.Wall;
using System.Collections.Generic;
using System.Linq;

namespace ScuffedWalls
{
    public class Workspace
    {
        public int Number { get; set; }
        public string Name { get; set; }
        public List<BeatMap.Note> Notes { get; set; } = new List<BeatMap.Note>();
        public List<BeatMap.Event> Lights { get; set; } = new List<BeatMap.Event>();
        public List<BeatMap.Obstacle> Walls { get; set; } = new List<BeatMap.Obstacle>();
        public List<BeatMap.CustomData.CustomEvent> CustomEvents { get; set; } = new List<BeatMap.CustomData.CustomEvent>();
        public List<BeatMap.CustomData.PointDefinition> PointDefinitions { get; set; } = new List<BeatMap.CustomData.PointDefinition>();
        public List<BeatMap.CustomData.Bookmark> Bookmarks { get; set; } = new List<BeatMap.CustomData.Bookmark>();
        public List<BeatMap.CustomData.Environment> Environment { get; set; } = new List<BeatMap.CustomData.Environment>();
    }
    static class WorkspaceHelper
    {
        public static BeatMap toBeatMap(this Workspace[] workspaces)
        {
            List<BeatMap.Obstacle> obstacles = new List<BeatMap.Obstacle>();
            List<BeatMap.Note> notes = new List<BeatMap.Note>();
            List<BeatMap.Event> events = new List<BeatMap.Event>();
            List<BeatMap.CustomData.CustomEvent> customEvents = new List<BeatMap.CustomData.CustomEvent>();
            List<BeatMap.CustomData.PointDefinition> pointDefinitions = new List<BeatMap.CustomData.PointDefinition>();
            List<BeatMap.CustomData.Environment> ev = new List<BeatMap.CustomData.Environment>();
            foreach (var workspace in workspaces)
            {
                customEvents.AddRange(workspace.CustomEvents);
                notes.AddRange(workspace.Notes);
                obstacles.AddRange(workspace.Walls);
                events.AddRange(workspace.Lights);
                pointDefinitions.AddRange(workspace.PointDefinitions);
                ev.AddRange(workspace.Environment);
            }
            return new BeatMap()
            {
                _version = "2.2.0",
                _notes = notes.ToArray().OrderBy(o => o.GetTime()).ToArray(),
                _obstacles = obstacles.ToArray().OrderBy(o => o.GetTime()).ToArray(),
                _events = events.ToArray().OrderBy(o => o.GetTime()).ToArray(),
                _waypoints = new object[] { },
                _customData = new BeatMap.CustomData()
                {
                    _environment = ev.ToArray(),
                    _customEvents = customEvents.ToArray().OrderBy(o => float.Parse(o._time.ToString())).ToArray(),
                    _pointDefinitions = pointDefinitions.ToArray()
                }
            };
        }
        public static BeatMap toBeatMap(this Workspace workspace)
        {
            return new BeatMap()
            {
                _version = "2.2.0",
                _notes = workspace.Notes.OrderBy(o => o.GetTime()).ToArray(),
                _obstacles = workspace.Walls.OrderBy(o => o.GetTime()).ToArray(),
                _events = workspace.Lights.OrderBy(o => o.GetTime()).ToArray(),
                _waypoints = new object[] { },
                _customData = new BeatMap.CustomData()
                {
                    _environment = workspace.Environment.ToArray(),
                    _customEvents = workspace.CustomEvents.OrderBy(o => float.Parse(o._time.ToString())).ToArray(),
                    _pointDefinitions = workspace.PointDefinitions.ToArray()
                }
            };
        }
    }



}
