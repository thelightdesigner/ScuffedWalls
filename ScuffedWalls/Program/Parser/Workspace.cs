using ModChart;
using System;
using System.Collections.Generic;
using System.Linq;

using static ModChart.BeatMap;

namespace ScuffedWalls
{
    public class Workspace : ICloneable
    {
        public Workspace()
        {
        }
        public Workspace(BeatMap Map, string name = "")
        {
            _beatMap = Map;
            Name = name;
        }
        private readonly BeatMap _beatMap = Empty;
        public string Name { get; set; }
        public BeatMap BeatMap => _beatMap;
        public TreeDictionary CustomData { get => _beatMap._customData; set { BeatMap._customData = value; } }
        public List<Note> Notes { get => _beatMap._notes; set { BeatMap._notes = value; } }
        public List<Event> Lights { get => _beatMap._events; set { BeatMap._events = value; } }
        public List<Obstacle> Walls { get => _beatMap._obstacles; set { BeatMap._obstacles = value; } }
        public List<object> CustomEvents => CustomData.at<List<object>>(_customEvents);
        public List<object> PointDefinitions => CustomData.at<List<object>>(_pointDefinitions);
        public List<object> Bookmarks => CustomData.at<List<object>>(_bookmarks);
        public List<object> Environment => CustomData.at<List<object>>(_environment);
        public List<object> BPMChanges => CustomData.at<List<object>>(_BPMChanges);


        public object Clone()
        {
            return new Workspace((BeatMap)BeatMap.Clone(), Name);
        }

        public static BeatMap GetBeatMap(IEnumerable<Workspace> workspaces)
        {
            List<Obstacle> obstacles = new List<Obstacle>();
            List<Note> notes = new List<Note>();
            List<Event> events = new List<Event>();
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

            var beatMap = new BeatMap()
            {
                _notes = notes.OrderBy(o => o.GetTime()).ToList(),
                _obstacles = obstacles.OrderBy(o => o.GetTime()).ToList(),
                _events = events.OrderBy(o => o.GetTime()).ToList(),
                _customData = customdata
            };
            beatMap.OrderCustomEventLists();

            return beatMap;
        }
    }
}
