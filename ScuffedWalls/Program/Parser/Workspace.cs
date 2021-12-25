using ModChart;
using System;
using System.Collections.Generic;
using System.Linq;

using static ModChart.BeatMap;

namespace ScuffedWalls
{
    /// <summary>
    /// Exposes useful parts of a BeatMap
    /// </summary>
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
        public bool Enabled { get; set; } = true;
        public string Name { get; set; }
        public BeatMap BeatMap
        {
            get
            {
                if (Enabled) return _beatMap;
                else return Empty;
            }
        }
        public TreeDictionary CustomData => BeatMap._customData;
        public List<Note> Notes => BeatMap._notes;
        public List<Event> Lights => BeatMap._events;
        public List<Obstacle> Walls => BeatMap._obstacles;
        public List<object> CustomEvents => CustomData.at<List<object>>(_customEvents);
        public List<object> PointDefinitions => CustomData.at<List<object>>(_pointDefinitions);
        public List<object> Bookmarks => CustomData.at<List<object>>(_bookmarks);
        public List<object> Environment => CustomData.at<List<object>>(_environment);
        public List<object> BPMChanges => CustomData.at<List<object>>(_BPMChanges);

        public object Clone()
        {
            return new Workspace((BeatMap)BeatMap.Clone(), Name);
        }
        public void Add(Workspace workspace)
        {
            BeatMap.AddMap(workspace);
        }
        public static implicit operator Workspace(BeatMap b) => new Workspace(b, null);
        public static implicit operator BeatMap(Workspace w) => w.BeatMap;
        public static BeatMap Combine(IEnumerable<Workspace> workspaces)
        {
            var beatMap = new BeatMap();
            foreach (var work in workspaces) beatMap.AddMap(work);
            beatMap.Order();
            

            return beatMap;
        }
    }
}
