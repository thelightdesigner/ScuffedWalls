using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace ModChart
{
    class NoodleCustomEvents
    {
        public static BeatMap.CustomData.CustomEvents CustomEventConstructor(float Time, string Type, BeatMap.CustomData.CustomEvents.Data Data)
        {
            return new BeatMap.CustomData.CustomEvents()
            {
                _time = Time,
                _type = Type,
                _data = Data
            };
        }

        public static BeatMap.CustomData.CustomEvents.Data CustomDataParse(string[] CustomNoodleData)
        {

            BeatMap.CustomData.CustomEvents.Data Data = new BeatMap.CustomData.CustomEvents.Data();

            foreach (var _customObject in CustomNoodleData)
            {
                string[] _customObjectSplit = _customObject.Split(':');


                if (_customObjectSplit[0] == "track")
                {
                    Data._track = JsonSerializer.Deserialize<object>("\"" + _customObjectSplit[1] + "\"");
                }
                if (_customObjectSplit[0] == "parentTrack")
                {
                    Data._parentTrack = JsonSerializer.Deserialize<object>("\"" + _customObjectSplit[1] + "\"");
                }
                if (_customObjectSplit[0] == "duration")
                {
                    Data._duration = JsonSerializer.Deserialize<object>(_customObjectSplit[1]);
                }
                if (_customObjectSplit[0] == "easing")
                {
                    Data._easing = JsonSerializer.Deserialize<object>("\"" + _customObjectSplit[1] + "\"");
                }
                if (_customObjectSplit[0] == "AnimateDefinitePosition")
                {
                    Data._definitePosition = JsonSerializer.Deserialize<object[][]>("[" + _customObjectSplit[1] + "]");
                }
                if (_customObjectSplit[0] == "AnimateDissolve")
                {
                    Data._dissolve = JsonSerializer.Deserialize<object[][]>("[" + _customObjectSplit[1] + "]");
                }
                if (_customObjectSplit[0] == "childTracks")
                {
                    Data._childrenTracks = JsonSerializer.Deserialize<object[]>(_customObjectSplit[1]);
                }
                if (_customObjectSplit[0] == "AnimateDissolveArrow")
                {
                    Data._dissolveArrow = JsonSerializer.Deserialize<object[][]>("[" + _customObjectSplit[1] + "]");
                }
                if (_customObjectSplit[0] == "AnimateColor")
                {
                    Data._color = JsonSerializer.Deserialize<object[][]>("[" + _customObjectSplit[1] + "]");
                }
                if (_customObjectSplit[0] == "AnimateRotation")
                {
                    Data._rotation = JsonSerializer.Deserialize<object[][]>("[" + _customObjectSplit[1] + "]");
                }
                if (_customObjectSplit[0] == "AnimatePosition")
                {
                    Data._position = JsonSerializer.Deserialize<object[][]>("[" + _customObjectSplit[1] + "]");
                }
                if (_customObjectSplit[0] == "AnimateLocalRotation")
                {
                    Data._localRotation = JsonSerializer.Deserialize<object[][]>("[" + _customObjectSplit[1] + "]");
                }
                if (_customObjectSplit[0] == "AnimateScale")
                {
                    Data._scale = JsonSerializer.Deserialize<object[][]>("[" + _customObjectSplit[1] + "]");
                }
                if (_customObjectSplit[0] == "isInteractable")
                {
                    Data._interactable = JsonSerializer.Deserialize<object[][]>("[" + _customObjectSplit[1] + "]");
                }

            }
            return Data;
        }
    }
}
