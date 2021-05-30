using ModChart;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ScuffedWalls
{
    public class Workspace : ICloneable
    {
        public Workspace()
        {
            CustomData["_customEvents"] = CustomEvents;
            CustomData["_pointDefinitions"] = PointDefinitions;
            CustomData["_bookmarks"] = Bookmarks;
            CustomData["_environment"] = Environment;
            CustomData["_BPMChanges"] = BPMChanges;
        }
        public int Number { get; set; }
        public string Name { get; set; }
        public List<BeatMap.Note> Notes { get; set; } = new List<BeatMap.Note>();
        public List<BeatMap.Event> Lights { get; set; } = new List<BeatMap.Event>();
        public List<BeatMap.Obstacle> Walls { get; set; } = new List<BeatMap.Obstacle>();
        public List<TreeDictionary> CustomEvents { get; set; } = new List<TreeDictionary>();
        public List<TreeDictionary> PointDefinitions { get; set; } = new List<TreeDictionary>();
        public List<TreeDictionary> Bookmarks { get; set; } = new List<TreeDictionary>();
        public List<TreeDictionary> Environment { get; set; } = new List<TreeDictionary>();
        public List<TreeDictionary> BPMChanges { get; set; } = new List<TreeDictionary>();
        /// <summary>
        /// All lists involving CustomData are referenced here
        /// </summary>
        public TreeDictionary CustomData { get; set; } = new TreeDictionary();

        public object Clone()
        {
            return new Workspace()
            {
                Number = Number,
                Name = Name,
                Notes = Notes.CloneArray().Cast<BeatMap.Note>().ToList(),
                Lights = Lights.CloneArray().Cast<BeatMap.Event>().ToList(),
                Walls = Walls.CloneArray().Cast<BeatMap.Obstacle>().ToList(),
                CustomData = (TreeDictionary)CustomData.Clone()
            };
        }
    }
    static class WorkspaceHelper
    {
        public static BeatMap toBeatMap(this Workspace[] workspaces)
        {
            List<BeatMap.Obstacle> obstacles = new List<BeatMap.Obstacle>();
            List<BeatMap.Note> notes = new List<BeatMap.Note>();
            List<BeatMap.Event> events = new List<BeatMap.Event>();
            TreeDictionary customdata = new TreeDictionary();
            foreach (var workspace in workspaces)
            {
                notes.AddRange(workspace.Notes);
                obstacles.AddRange(workspace.Walls);
                events.AddRange(workspace.Lights);

                customdata = (TreeDictionary)TreeDictionary.Merge(
                    customdata, workspace.CustomData,
                    TreeDictionary.MergeType.Arrays | TreeDictionary.MergeType.Objects,
                    TreeDictionary.MergeBindingFlags.HasValue);
            }

            return new BeatMap()
            {
                _version = "2.2.0",
                _notes = notes.ToArray().OrderBy(o => o.GetTime()).ToArray(),
                _obstacles = obstacles.ToArray().OrderBy(o => o.GetTime()).ToArray(),
                _events = events.ToArray().OrderBy(o => o.GetTime()).ToArray(),
                _waypoints = new object[] { },
                _customData = customdata
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
                _customData = workspace.CustomData
            };
        }
    }



}
