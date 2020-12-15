using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace ModChart
{
    class Map
    {
        //contains tools for generating and constructing a map
        public List<BeatMap.Note> Notes = new List<BeatMap.Note>();
        public List<BeatMap.Obstacle> Walls = new List<BeatMap.Obstacle>();
        public List<BeatMap.CustomData.CustomEvents> CustomEvents = new List<BeatMap.CustomData.CustomEvents>();
        public static BeatMap Empty = new BeatMap() { _version = "2.0.0", _obstacles = new BeatMap.Obstacle[] { }, _notes = new BeatMap.Note[] { }, _customData = new BeatMap.CustomData(),  _events = new BeatMap.Event[] { } };

        public static BeatMap.CustomData CustomDataParse(string[] CustomNoodleData)
        {
            BeatMap.CustomData CustomData = new BeatMap.CustomData();
            BeatMap.CustomData.Animation Animation = new BeatMap.CustomData.Animation();

            foreach (var _customObject in CustomNoodleData)
            {
                string[] _customObjectSplit = _customObject.Split(':');

                if (_customObjectSplit[0] == "AnimateDefinitePosition")
                {
                    Animation._definitePosition = JsonSerializer.Deserialize<object[][]>("[" + _customObjectSplit[1] + "]");
                }
                if (_customObjectSplit[0] == "AnimatePosition")
                {
                    Animation._position = JsonSerializer.Deserialize<object[][]>("[" + _customObjectSplit[1] + "]");
                }
                if (_customObjectSplit[0] == "scale")
                {
                    CustomData._scale = JsonSerializer.Deserialize<object[]>(_customObjectSplit[1]);
                }
                if (_customObjectSplit[0] == "track")
                {
                    CustomData._track = JsonSerializer.Deserialize<object>("\"" + _customObjectSplit[1] + "\"");
                }
                if (_customObjectSplit[0] == "color")
                {
                    CustomData._color = JsonSerializer.Deserialize<object[]>(_customObjectSplit[1]);
                }
                if (_customObjectSplit[0] == "NJSOffset")
                {
                    CustomData._noteJumpStartBeatOffset = JsonSerializer.Deserialize<object>(_customObjectSplit[1]);
                }
                if (_customObjectSplit[0] == "NJS")
                {
                    CustomData._noteJumpMovementSpeed = JsonSerializer.Deserialize<object>(_customObjectSplit[1]);
                }
                if (_customObjectSplit[0] == "AnimateDissolve")
                {
                    Animation._dissolve = JsonSerializer.Deserialize<object[][]>("[" + _customObjectSplit[1] + "]");
                }
                if (_customObjectSplit[0] == "AnimateDissolveArrow")
                {
                    Animation._dissolveArrow = JsonSerializer.Deserialize<object[][]>("[" + _customObjectSplit[1] + "]");
                }
                if (_customObjectSplit[0] == "AnimateColor")
                {
                    Animation._color = JsonSerializer.Deserialize<object[][]>("[" + _customObjectSplit[1] + "]");
                }
                if (_customObjectSplit[0] == "AnimateRotation")
                {
                    Animation._rotation = JsonSerializer.Deserialize<object[][]>("[" + _customObjectSplit[1] + "]");
                }
                if (_customObjectSplit[0] == "AnimateLocalRotation")
                {
                    Animation._localRotation = JsonSerializer.Deserialize<object[][]>("[" + _customObjectSplit[1] + "]");
                }
                if (_customObjectSplit[0] == "AnimateScale")
                {
                    Animation._scale = JsonSerializer.Deserialize<object[][]>("[" + _customObjectSplit[1] + "]");
                }
                if (_customObjectSplit[0] == "isInteractable")
                {
                    CustomData._interactable = JsonSerializer.Deserialize<object>(_customObjectSplit[1]);
                }
                if (_customObjectSplit[0] == "rotation")
                {
                    CustomData._rotation = JsonSerializer.Deserialize<object[]>(_customObjectSplit[1]);
                }
                if (_customObjectSplit[0] == "fake")
                {
                    CustomData._fake = JsonSerializer.Deserialize<object>(_customObjectSplit[1]);
                }
                if (_customObjectSplit[0] == "position")
                {
                    CustomData._position = JsonSerializer.Deserialize<object[]>(_customObjectSplit[1]);
                }
                if (_customObjectSplit[0] == "cutDirection")
                {
                    CustomData._cutDirection = JsonSerializer.Deserialize<object>(_customObjectSplit[1]);
                }
                if (_customObjectSplit[0] == "NoSpawnEffect")
                {
                    CustomData._disableSpawnEffect = JsonSerializer.Deserialize<object>(_customObjectSplit[1]);
                }

                CustomData._animation = Animation;

            }

            return CustomData;

        }

        //simple parameters to array converter
        public static string[] ParamsAndStringToArray(string[] CustomNoodleArray, params string[] CustomNoodleData)
        {
            List<string> Noodles = new List<string> { };
            Noodles.AddRange(CustomNoodleArray);
            Noodles.AddRange(CustomNoodleData);
            return Noodles.ToArray();
        }
        //same as above
        public static string[] ParamsToArray(params string[] CustomNoodleData)
        {
            return CustomNoodleData.ToArray();
        }

        public void GenerateToJson(string MapOutputFileLocation, string MapEmptyLocation)
        {
            JsonSerializerOptions jso = new JsonSerializerOptions();
            jso.IgnoreNullValues = true;
            jso.WriteIndented = true;

            BeatMap beatMap = JsonSerializer.Deserialize<BeatMap>(File.ReadAllText(MapEmptyLocation));
            List<BeatMap.Note> notes = new List<BeatMap.Note>();
            List<BeatMap.Obstacle> obstacles = new List<BeatMap.Obstacle>();
            List<BeatMap.CustomData.CustomEvents> customEvents = new List<BeatMap.CustomData.CustomEvents>();

            if (beatMap._notes != null) notes.AddRange(beatMap._notes);
            if (beatMap._obstacles != null) obstacles.AddRange(beatMap._obstacles);
            if (beatMap._customData._customEvents != null) customEvents.AddRange(beatMap._customData._customEvents);

            if (Notes != null) notes.AddRange(Notes);
            if (Walls != null) obstacles.AddRange(Walls);
            if (CustomEvents != null) customEvents.AddRange(CustomEvents);

            beatMap._notes = notes.ToArray();
            beatMap._obstacles = obstacles.ToArray();
            beatMap._customData._customEvents = customEvents.ToArray();

            File.WriteAllText(MapOutputFileLocation, JsonSerializer.Serialize(beatMap, jso));
        }

        public static void GenerateToJson(string MapOutputFileLocation, BeatMap beatMap)
        {
            JsonSerializerOptions jso = new JsonSerializerOptions();
            jso.IgnoreNullValues = true;
            jso.WriteIndented = false;
            File.WriteAllText(MapOutputFileLocation, JsonSerializer.Serialize(beatMap, jso));
        }
        public void GenerateToJson(string MapOutputFileLocation)
        {
            BeatMap beatMap = Empty;
            
            JsonSerializerOptions jso = new JsonSerializerOptions();
            jso.IgnoreNullValues = true;
            jso.WriteIndented = false;

            List<BeatMap.Note> notes = new List<BeatMap.Note>();
            List<BeatMap.Obstacle> obstacles = new List<BeatMap.Obstacle>();
            List<BeatMap.CustomData.CustomEvents> customEvents = new List<BeatMap.CustomData.CustomEvents>();

            if (beatMap._notes != null) notes.AddRange(beatMap._notes);
            if (beatMap._obstacles != null) obstacles.AddRange(beatMap._obstacles);
            if (beatMap._customData._customEvents != null) customEvents.AddRange(beatMap._customData._customEvents);

            if (Notes != null) notes.AddRange(Notes);
            if (Walls != null) obstacles.AddRange(Walls);
            if (CustomEvents != null) customEvents.AddRange(CustomEvents);

            beatMap._notes = notes.ToArray();
            beatMap._obstacles = obstacles.ToArray();
            beatMap._customData._customEvents = customEvents.ToArray();

            File.WriteAllText(MapOutputFileLocation, JsonSerializer.Serialize(beatMap, jso));
        }
    }
}
