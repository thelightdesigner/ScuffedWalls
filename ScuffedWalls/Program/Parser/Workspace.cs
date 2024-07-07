using ModChart;
using System;
using System.Collections.Generic;
using System.Linq;


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
        public Workspace(DifficultyV3 Map, string name = "")
        {
            difficulty = Map;
            Name = name;
        }
        private readonly DifficultyV3 difficulty = new();
        public bool Enabled { get; set; } = true;
        public string Name { get; set; }
        public DifficultyV3 Difficulty
        {
            get
            {
                if (Enabled) return difficulty;
                else return new();
            }
        }
        public SDictionary CustomData => Difficulty.CustomData;
        public List<DifficultyV3.ColorNote> Notes => diff;
        public List<Event> Lights => BeatMap._events;
        public List<Obstacle> Walls => BeatMap._obstacles;
        public List<object> CustomEvents => CustomData.at<List<object>>(_customEvents);
        public List<object> PointDefinitions => CustomData.at<List<object>>(_pointDefinitions);
        public List<object> Bookmarks => CustomData.at<List<object>>(_bookmarks);
        public List<object> Environment => CustomData.at<List<object>>(_environment);
        public List<object> BPMChanges => CustomData.at<List<object>>(_BPMChanges);

        public object Clone()
        {
            return new Workspace((DifficultyV2)BeatMap.Clone(), Name);
        }
        public void Add(Workspace workspace)
        {
            BeatMap.AddMap(workspace);
        }
        public static implicit operator Workspace(DifficultyV2 b) => new Workspace(b, null);
        public static implicit operator DifficultyV2(Workspace w) => w.BeatMap;
        public static DifficultyV2 Combine(IEnumerable<Workspace> workspaces)
        {
            var beatMap = new DifficultyV2();
            foreach (var work in workspaces) beatMap.AddMap(work);
            beatMap.Order();
            

            return beatMap;
        }
    }
}
