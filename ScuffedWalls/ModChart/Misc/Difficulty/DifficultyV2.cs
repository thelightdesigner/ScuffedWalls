﻿/**using ScuffedWalls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;

namespace ModChart
{
    public interface ICustomDataMapObject : ITimeable, ICloneable
    {
        [JsonConverter(typeof(TreeDictionaryJsonConverter))]
        public TreeDictionary _customData { get; set; }
    }
    public interface ITimeable
    {
        public float? _time { get; set; }
        public float GetTime();
    }
    public class MapStatAttribute : Attribute
    {

    }

    public class DifficultyV2 : ICloneable
    {
        [JsonIgnore]
        public MapStats Stats => GetStats();
        public MapStats GetStats()
        {
            MapStats stats = new MapStats();
            foreach (var prop in typeof(DifficultyV2).GetProperties().Where(p => p.GetCustomAttributes<MapStatAttribute>().Any()))
            {
                object val = prop.GetValue(this);
                if (val is IEnumerable<object> array && array.Count() > 0)
                {
                    int count = array.Count();
                    stats.AddStat(new KeyValuePair<string, int>(Extensions.MakePlural(prop.Name, count), count));
                }
            }
            foreach (var item in _customData)
            {
                if (item.Value is IEnumerable<object> aray && aray.Count() > 0) stats.AddStat(new KeyValuePair<string, int>(Extensions.MakePlural(item.Key, aray.Count()), aray.Count()));
            }
            return stats;
        }
        public static DifficultyV2 Combine(DifficultyV2 b1, DifficultyV2 b2)
        {
            b1.AddMap(b2);
            return (DifficultyV2)b1.Clone();
        }
        public void AddMap(DifficultyV2 b2)
        {
            _notes.AddRange(b2._notes);
            _waypoints.AddRange(b2._waypoints);
            _obstacles.AddRange(b2._obstacles);
            _events.AddRange(b2._events);
            _customData = TreeDictionary.Merge(_customData, b2._customData);
        }

        public string _version { get; set; } = "2.2.0";
        [JsonConverter(typeof(TreeDictionaryJsonConverter))]
        public TreeDictionary _customData { get; set; } = ImportantMapCustomDataFields;
        [MapStat] public List<Event> _events { get; set; } = new List<Event>();
        [MapStat] public List<Note> _notes { get; set; } = new List<Note>();
        [MapStat] public List<Obstacle> _obstacles { get; set; } = new List<Obstacle>();
        [MapStat] public List<Waypoint> _waypoints { get; set; } = new List<Waypoint>();
        public class Waypoint : TreeDictionary
        {

        }

        public const string
               _height = "_height",
               _attenuation = "_attenuation",
               _offset = "_offset",
               _startY = "_startY",
               _position = "_position",
               _localPosition = "_localPosition",
               _scale = "_scale",
               _localRotation = "_localRotation",
               _rotation = "_rotation",
               _definitePosition = "_definitePosition",
                _noteJumpStartBeatOffset = "_noteJumpStartBeatOffset",
                _noteJumpMovementSpeed = "_noteJumpMovementSpeed",
               _dissolve = "_dissolve",
               _dissolveArrow = "_dissolveArrow",
               _time = "_time",
               _track = "_track",
               _color = "_color",
               _id = "_id",
               _data = "_data",
               _active = "_active",
               _lookupMethod = "_lookupMethod",
               _animation = "_animation",
               _duplicate = "_duplicate",
               _customEvents = "_customEvents",
               _pointDefinitions = "_pointDefinitions",
               _worldpositionstays = "_worldpositionstays",
               _bookmarks = "_bookmarks",
               _environment = "_environment",
               _BPMChanges = "_BPMChanges",
               AnimateTrack = "AnimateTrack",
               AssignPathAnimation = "AssignPathAnimation",
               AssignPlayerToTrack = "AssignPlayerToTrack",
               AssignTrackParent = "AssignTrackParent",
               AssignFogTrack = "AssignFogTrack";
        public void Prune()
        {
            _customData.DeleteNullValues();
            foreach (var mapobj in _events) if (mapobj._customData != null) mapobj._customData.DeleteNullValues();
            foreach (var mapobj in _notes) if (mapobj._customData != null) mapobj._customData.DeleteNullValues();
            foreach (var mapobj in _obstacles) if (mapobj._customData != null) mapobj._customData.DeleteNullValues();
        }

        public object Clone()
        {
            return new DifficultyV2()
            {
                _notes = _notes?.CloneArray().Cast<Note>().ToList(),
                _events = _events?.CloneArray().Cast<Event>().ToList(),
                _obstacles = _obstacles?.CloneArray().Cast<Obstacle>().ToList(),
                _version = _version,
                _waypoints = _waypoints,
                _customData = (TreeDictionary)_customData?.Clone()
            };
        }

        public static DifficultyV2 Empty => new DifficultyV2();
        public static TreeDictionary ImportantMapCustomDataFields => new TreeDictionary()
        {
            [_customEvents] = new List<object>(),
            [_pointDefinitions] = new List<object>(),
            [_bookmarks] = new List<object>(),
            [_environment] = new List<object>(),
            [_BPMChanges] = new List<object>()
        };
        public bool needsNoodleExtensions()
        {
            //are there any custom events?
            if (_customData != null && _customData["_customEvents"] != null && _customData.at<IEnumerable<object>>("_customEvents").Count() > 0) return true;

            //do any notes have any noodle data other than color?
            if (_notes.Any(note => note._customData != null && HasNoodleParams(note._customData))) return true;

            //do any walls have any noodle data other than color?
            if (_obstacles.Any(wall => wall._customData != null && HasNoodleParams(wall._customData))) return true;

            bool HasNoodleParams(TreeDictionary customData) =>
                customData.Any(p => DifficultyV2.NoodleExtensionsPropertyNames.Any(n => n == p.Key)) || //customData has one of the noodle properties listed
                (customData["_animation"] != null && customData.at("_animation").Any(p => NoodleExtensionsPropertyNames.Any(n => n == p.Key))); //animation exists in custom data and has noodle params

            return false;
        }
        public bool needsChroma()
        {
            //do light have color
            if (_events.Any(light => light._customData != null && light._customData["_color"] != null)) return true;

            //do wal have color or animate color
            if (_obstacles.Any(wall => wall._customData != null && (wall._customData["_color"] != null || (wall._customData["_animation"] != null && wall._customData["_animation._color"] != null)))) return true;

            //do note have color or animate color
            if (_notes.Any(note => note._customData != null && (note._customData["_color"] != null || (note._customData["_animation"] != null && note._customData["_animation._color"] != null)))) return true;

            return false;

        }
        public void Order()
        {
            string[] Keys = _customData.Keys.ToArray();

            foreach (string key in Keys)
            {
                if (_customData[key] is IList<object> array && array.All(arrayitem => arrayitem is IDictionary<string, object> dict && dict.ContainsKey("_time")))
                {
                    _customData[key] = array.OrderBy(obj => ((IDictionary<string, object>)obj)["_time"].ToFloat()).ToList();
                }
            }

            _events = _events.OrderBy(obj => obj.GetTime()).ToList();

            _notes = _notes.OrderBy(obj => obj.GetTime()).ToList();

            _obstacles = _obstacles.OrderBy(obj => obj.GetTime()).ToList();
        }

        public class Event : ICustomDataMapObject, ICloneable
        {
            public enum Type
            {
                BackLasers = 0,
                RingLights = 1,
                LeftRotatingLasers = 2,
                RightRotatingLasers = 3,
                CenterLights = 4,
                BoostLightColors = 5,
                RingSpin = 8,
                RingZoom = 9,
                OfficialBPMChange = 10,
                LeftRotatingLasersSpeed = 12,
                RightRotatingLasersSpeed = 13,
                EarlyRotation = 14,
                LateRotation = 15
            }
            public enum Value
            {
                Off = 0,
                OnBlue = 1,
                FlashBlue = 2,
                FadeBlue = 3,
                OnRed = 5,
                FlashRed = 6,
                FadeRed = 7
            }
            public float GetTime() => _time.Value;
            public float? _time { get; set; }
            public Type? _type { get; set; }
            public Value? _value { get; set; }
            [JsonConverter(typeof(TreeDictionaryJsonConverter))]
            public TreeDictionary _customData { get; set; }

            public object Clone()
            {
                return new Event()
                {
                    _time = _time,
                    _type = _type,
                    _value = _value,
                    _customData = (TreeDictionary)_customData?.Clone()
                };
            }
        }
        public class Note : ICustomDataMapObject, ICloneable
        {
            public enum NoteType
            {
                Left = 0,
                Right = 1,
                Bomb = 3
            }
            public enum CutDirection
            {
                Up = 0,
                Down = 1,
                Left = 2,
                Right = 3,
                UpLeft = 4,
                UpRight = 5,
                DownLeft = 6,
                DownRight = 7,
                Dot = 8
            }
            public float GetTime() => _time.Value;
            public float? _time { get; set; }
            public int? _lineIndex { get; set; }
            public int? _lineLayer { get; set; }
            public NoteType? _type { get; set; }
            public CutDirection? _cutDirection { get; set; }
            [JsonConverter(typeof(TreeDictionaryJsonConverter))]
            public TreeDictionary _customData { get; set; }

            public object Clone()
            {
                return new Note()
                {
                    _time = _time,
                    _lineIndex = _lineIndex,
                    _type = _type,
                    _cutDirection = _cutDirection,
                    _lineLayer = _lineLayer,
                    _customData = (TreeDictionary)_customData?.Clone()
                };
            }
        }
        public class Obstacle : ICustomDataMapObject, ICloneable
        {
            public enum Type
            {
                FullHeight = 0,
                Crouch = 1
            }
            public float GetTime() => _time.Value;
            public float? _time { get; set; }
            public int? _lineIndex { get; set; }
            public Type? _type { get; set; }
            public float? _duration { get; set; }
            public int? _width { get; set; }
            [JsonConverter(typeof(TreeDictionaryJsonConverter))]
            public TreeDictionary _customData { get; set; }

            public object Clone()
            {
                return new Obstacle()
                {
                    _time = _time,
                    _lineIndex = _lineIndex,
                    _type = _type,
                    _duration = _duration,
                    _width = _width,
                    _customData = (TreeDictionary)_customData?.Clone()
                };
            }
        }
        public static string[] NoodleExtensionsPropertyNames => new string[]
        {
            _height,
            _attenuation,
            _position,
            _rotation,
            _scale,
            _localPosition,
            _localRotation,
            _dissolve,
            _dissolveArrow,
            _time
        };

        public static void Append(ICustomDataMapObject MapObject, ICustomDataMapObject AppendObject, AppendPriority type)
        {
            switch (type)
            {
                case AppendPriority.Low:
                    foreach (var property in MapObject.GetType().GetProperties())
                        if (property.GetValue(MapObject) == null)
                            property.SetValue(MapObject, property.GetValue(AppendObject));

                    if (AppendObject._customData != null)
                    {
                        MapObject._customData = TreeDictionary.Merge(
                            MapObject._customData,
                            AppendObject._customData,
                            TreeDictionary.MergeType.Dictionaries | TreeDictionary.MergeType.Objects,
                            TreeDictionary.MergeBindingFlags.HasValue);
                    }
                    break;
                case AppendPriority.High:
                    foreach (var property in MapObject.GetType().GetProperties())
                        if (property.GetValue(AppendObject) != null)
                            property.SetValue(MapObject, property.GetValue(AppendObject));

                    if (MapObject._customData != null)
                    {
                        MapObject._customData = TreeDictionary.Merge(
                            AppendObject._customData,
                            MapObject._customData,
                            TreeDictionary.MergeType.Dictionaries | TreeDictionary.MergeType.Objects,
                            TreeDictionary.MergeBindingFlags.HasValue);
                    }
                    break;
            }
        }
        public enum AppendPriority
        {
            Low,
            High
        }
        public enum MapObjectType
        {
            Obstacle,
            Note,
            Event
        }
        public static ICustomDataMapObject GetInstance(MapObjectType type) =>
            type == MapObjectType.Note ? (ICustomDataMapObject)new Note() :
            type == MapObjectType.Obstacle ? (ICustomDataMapObject)new Obstacle() :
            type == MapObjectType.Event ? (ICustomDataMapObject)new Event() :
            throw new Exception();


    }


}**/