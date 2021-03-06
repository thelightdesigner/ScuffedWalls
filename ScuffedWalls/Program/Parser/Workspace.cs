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
        public string Name { get; set; }
        public List<BeatMap.Note> Notes { get; set; } = new List<BeatMap.Note>();
        public List<BeatMap.Event> Lights { get; set; } = new List<BeatMap.Event>();
        public List<BeatMap.Obstacle> Walls { get; set; } = new List<BeatMap.Obstacle>();
        public List<BeatMap.CustomData.CustomEvents> CustomEvents { get; set; } = new List<BeatMap.CustomData.CustomEvents>();
        public List<BeatMap.CustomData.PointDefinition> PointDefinitions { get; set; } = new List<BeatMap.CustomData.PointDefinition>();
        public List<BeatMap.CustomData.Bookmark> Bookmarks { get; set; } = new List<BeatMap.CustomData.Bookmark>();
    }
    static class WorkspaceHelper
    {
        public static BeatMap toBeatMap(this Workspace[] workspaces)
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
                _notes = notes.ToArray().OrderBy(o => o.GetTime()).ToArray(),
                _obstacles = obstacles.ToArray().OrderBy(o => o.GetTime()).ToArray(),
                _events = events.ToArray().OrderBy(o => o.GetTime()).ToArray(),
                _customData = new BeatMap.CustomData() { _customEvents = customEvents.ToArray().OrderBy(o => float.Parse(o._time.ToString())).ToArray(), _pointDefinitions = pointDefinitions.ToArray() }
            };
        }
        public static BeatMap toBeatMap(this Workspace workspace)
        {
            return new BeatMap()
            {
                _version = "2.0.0",
                _notes = workspace.Notes.OrderBy(o => o.GetTime()).ToArray(),
                _obstacles = workspace.Walls.OrderBy(o => o.GetTime()).ToArray(),
                _events = workspace.Lights.OrderBy(o => o.GetTime()).ToArray(),
                _customData = new BeatMap.CustomData() { _customEvents = workspace.CustomEvents.OrderBy(o => float.Parse(o._time.ToString())).ToArray(), _pointDefinitions = workspace.PointDefinitions.ToArray() }
            };
        }
    }



}
