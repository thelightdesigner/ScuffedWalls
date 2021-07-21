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
            CustomData = new TreeDictionary
            {
                ["_customEvents"] = CustomEvents,
                ["_pointDefinitions"] = PointDefinitions,
                ["_bookmarks"] = Bookmarks,
                ["_environment"] = Environment,
                ["_BPMChanges"] = BPMChanges
            };
        }
        public int Number { get; set; }
        public string Name { get; set; }
        public List<BeatMap.Note> Notes { get; set; } = new List<BeatMap.Note>();
        public List<BeatMap.Event> Lights { get; set; } = new List<BeatMap.Event>();
        public List<BeatMap.Obstacle> Walls { get; set; } = new List<BeatMap.Obstacle>();
        public List<object> CustomEvents { get; set; } = new List<object>();
        public List<object> PointDefinitions { get; set; } = new List<object>();
        public List<object> Bookmarks { get; set; } = new List<object>();
        public List<object> Environment { get; set; } = new List<object>();
        public List<object> BPMChanges { get; set; } = new List<object>();
        

        private TreeDictionary _customData;
        /// <summary>
        /// All lists involving CustomData are referenced here, Setting this will overrite all values present in the extracted lists
        /// </summary>
        public TreeDictionary CustomData
        {
            get => _customData; 
            set 
            { 
                _customData = value; 
                ReferenceListsInCustomData(); 
            }
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
        public void ReferenceListsInCustomData()
        {
            CustomEvents = GetList("_customEvents");
            PointDefinitions = GetList("_pointDefinitions");
            Bookmarks = GetList("_Bookmarks");
            Environment = GetList("_environment");
            BPMChanges = GetList("_BPMChanges");

            List<object> GetList(string Name)
            {
                if (CustomData[Name] is List<object> list) return list;
                else return new List<object>();
            }
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
