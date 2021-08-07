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
            CustomData = new TreeDictionary();
        }
        public int Number { get; set; }
        public string Name { get; set; }
        public List<BeatMap.Note> Notes { get; set; } = new List<BeatMap.Note>();
        public List<BeatMap.Event> Lights { get; set; } = new List<BeatMap.Event>();
        public List<BeatMap.Obstacle> Walls { get; set; } = new List<BeatMap.Obstacle>();
        public List<object> CustomEvents { get => CustomData.at<List<object>>("_customEvents"); set { CustomData["_customEvents"] = value; } }
        public List<object> PointDefinitions { get => CustomData.at<List<object>>("_pointDefinitions"); set { CustomData["_pointDefinitions"] = value; } }
        public List<object> Bookmarks { get => CustomData.at<List<object>>("_bookmarks"); set { CustomData["_bookmarks"] = value; } }
        public List<object> Environment { get => CustomData.at<List<object>>("_environment"); set { CustomData["_environment"] = value; } }
        public List<object> BPMChanges { get => CustomData.at<List<object>>("_BPMChanges"); set { CustomData["_BPMChanges"] = value; } }
        private TreeDictionary _customData;
        public TreeDictionary CustomData { get => _customData; set { _customData = value; AddProps(); } }

        public void AddProps()
        {
            CustomData["_customEvents"] ??= new List<object>();
            CustomData["_pointDefinitions"] ??= new List<object>();
            CustomData["_bookmarks"] ??= new List<object>();
            CustomData["_environment"] ??= new List<object>();
            CustomData["_BPMChanges"] ??= new List<object>();
        }

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
        public static BeatMap ToBeatMap(this Workspace[] workspaces)
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

            //order point definitions

            customdata.OrderListsBy_time();

            return new BeatMap()
            {
                _version = "2.2.0",
                _notes = notes.ToArray().OrderBy(o => o.GetTime()).ToList(),
                _obstacles = obstacles.ToArray().OrderBy(o => o.GetTime()).ToList(),
                _events = events.ToArray().OrderBy(o => o.GetTime()).ToList(),
                _waypoints = new object[] { },
                _customData = customdata
            };
        }
        public static BeatMap toBeatMap(this Workspace workspace)
        {
            return new BeatMap()
            {
                _version = "2.2.0",
                _notes = workspace.Notes.OrderBy(o => o.GetTime()).ToList(),
                _obstacles = workspace.Walls.OrderBy(o => o.GetTime()).ToList(),
                _events = workspace.Lights.OrderBy(o => o.GetTime()).ToList(),
                _waypoints = new object[] { },
                _customData = workspace.CustomData
            };
        }
        public static void OrderListsBy_time(this IDictionary<string, object> _customData)
        {
            string[] Keys = _customData.Keys.ToArray();

            foreach (string key in Keys)
            {
                if (_customData[key] is IList<object> array && array.All(arrayitem => arrayitem is IDictionary<string, object> dict && dict.ContainsKey("_time")))
                {
                    _customData[key] = array.OrderBy(obj => ((IDictionary<string, object>)obj)["_time"].ToFloat()).ToList();
                }
            }
        }
    }
}
