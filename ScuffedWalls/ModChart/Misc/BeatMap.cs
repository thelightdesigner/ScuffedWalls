using ScuffedWalls;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
    public class BeatMap : ICloneable
    {
        public void Prune()
        {
            _customData.DeleteNullValues();
            foreach (var mapobj in _events) if (mapobj._customData != null) mapobj._customData.DeleteNullValues();
            foreach (var mapobj in _notes) if (mapobj._customData != null) mapobj._customData.DeleteNullValues();
            foreach (var mapobj in _obstacles) if (mapobj._customData != null) mapobj._customData.DeleteNullValues();
        }

        public object Clone()
        {
            return new BeatMap()
            {
                _notes = _notes.CloneArray().Cast<Note>().ToList(),
                _events = _events.CloneArray().Cast<Event>().ToList(),
                _obstacles = _obstacles.CloneArray().Cast<Obstacle>().ToList(),
                _version = _version,
                _waypoints = _waypoints,
                _customData = (TreeDictionary)_customData.Clone()
            };
        }

        public static BeatMap Empty => new BeatMap
        {
            _version = "2.2.0",
            _events = new List<Event>(),
            _notes = new List<Note>(),
            _obstacles = new List<Obstacle>(),
            _waypoints = new object[] { },
            _customData = new TreeDictionary()
        };


        public string _version { get; set; }
        [JsonConverter(typeof(TreeDictionaryJsonConverter))]
        public TreeDictionary _customData { get; set; }
        public List<Event> _events { get; set; }
        public List<Note> _notes { get; set; }
        public List<Obstacle> _obstacles { get; set; }
        public object[] _waypoints { get; set; }


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
            public float GetTime() => float.Parse(_time.ToString());
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
                    _customData = (TreeDictionary)_customData.Clone()
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
            public float GetTime() => float.Parse(_time.ToString());
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
                    _customData = (TreeDictionary)_customData.Clone()
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
            public float GetTime() => float.Parse(_time.ToString());
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
                    _customData = (TreeDictionary)_customData.Clone()
                };
            }
        }
        public static string[] NoodleExtensionsPropertyNames => new string[]
        {
            "_position",
            "_rotation",
            "_scale",
            "_localPosition",
            "_localRotation",
            "_dissolve",
            "_dissolveArrow",
            "_time"
        };
    }


}