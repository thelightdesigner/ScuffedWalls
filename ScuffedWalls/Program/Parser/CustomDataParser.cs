using ModChart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace ScuffedWalls
{
    public static class CustomDataParser
    {
        public static BeatMap.CustomData CustomDataParse(this Parameter[] CustomNoodleData)
        {
            BeatMap.CustomData CustomData = new BeatMap.CustomData();
            BeatMap.CustomData.Animation Animation = new BeatMap.CustomData.Animation();

            foreach (var param in CustomNoodleData)
            {

                if (param.Name == "AnimateDefinitePosition".ToLower()) Animation._definitePosition = JsonSerializer.Deserialize<object[][]>($"[{param.Data}]");
                else if (param.Name == "DefineAnimateDefinitePosition".ToLower()) Animation._definitePosition = param.Data;

                else if (param.Name == "AnimatePosition".ToLower()) Animation._position = JsonSerializer.Deserialize<object[][]>($"[{param.Data}]");
                else if (param.Name == "DefineAnimatePosition".ToLower()) Animation._position = param.Data;

                else if (param.Name == "AnimateDissolve".ToLower()) Animation._dissolve = JsonSerializer.Deserialize<object[][]>($"[{param.Data}]");
                else if (param.Name == "DefineAnimateDissolve".ToLower()) Animation._dissolve = param.Data;

                else if (param.Name == "AnimateDissolveArrow".ToLower()) Animation._dissolveArrow = JsonSerializer.Deserialize<object[][]>($"[{param.Data}]");
                else if (param.Name == "DefineAnimateDissolveArrow".ToLower()) Animation._dissolveArrow = param.Data;

                else if (param.Name == "AnimateColor".ToLower()) Animation._color = JsonSerializer.Deserialize<object[][]>($"[{param.Data}]");
                else if (param.Name == "DefineAnimateColor".ToLower()) Animation._color = param.Data;

                else if (param.Name == "AnimateRotation".ToLower()) Animation._rotation = JsonSerializer.Deserialize<object[][]>("[" + param.Data + "]");
                else if (param.Name == "DefineAnimateRotation".ToLower()) Animation._rotation = param.Data;

                else if (param.Name == "AnimateLocalRotation".ToLower()) Animation._localRotation = JsonSerializer.Deserialize<object[][]>($"[{param.Data}]");
                else if (param.Name == "DefineAnimateLocalRotation".ToLower()) Animation._localRotation = param.Data;

                else if (param.Name == "AnimateScale".ToLower()) Animation._scale = JsonSerializer.Deserialize<object[][]>($"[{param.Data}]");
                else if (param.Name == "DefineAnimateScale".ToLower()) Animation._scale = param.Data;

                else if (param.Name == "AnimateInteractable".ToLower()) Animation._interactable = JsonSerializer.Deserialize<object>($"[{param.Data}]");
                else if (param.Name == "DefineAnimateInteractable".ToLower()) Animation._interactable = param.Data;

                else if (param.Name == "interactable".ToLower()) CustomData._interactable = JsonSerializer.Deserialize<object>(param.Data);

                else if (param.Name == "disableNoteGravity".ToLower()) CustomData._interactable = JsonSerializer.Deserialize<object>(param.Data);

                else if (param.Name == "rotation".ToLower()) CustomData._rotation = JsonSerializer.Deserialize<object[]>(param.Data);

                else if (param.Name == "LocalRotation".ToLower()) CustomData._localRotation = JsonSerializer.Deserialize<object[]>(param.Data);

                else if (param.Name == "fake".ToLower()) CustomData._fake = JsonSerializer.Deserialize<object>(param.Data);

                else if (param.Name == "position".ToLower()) CustomData._position = JsonSerializer.Deserialize<object[]>(param.Data);

                else if (param.Name == "cutDirection".ToLower()) CustomData._cutDirection = JsonSerializer.Deserialize<object>(param.Data);

                else if (param.Name == "scale".ToLower()) CustomData._scale = JsonSerializer.Deserialize<object[]>(param.Data);

                else if (param.Name == "track".ToLower()) CustomData._track = JsonSerializer.Deserialize<object>($"\"{param.Data.removeWhiteSpace()}\"");

                else if (param.Name == "color".ToLower()) CustomData._color = JsonSerializer.Deserialize<object[]>(param.Data);
                else if (param.Name == "rgbcolor".ToLower()) CustomData._color = JsonSerializer.Deserialize<object[]>(param.Data).Select(o => { return (object)(o.toFloat() / 255f); }).ToArray();

                else if (param.Name == "NJSOffset".ToLower()) CustomData._noteJumpStartBeatOffset = JsonSerializer.Deserialize<object>(param.Data);

                else if (param.Name == "NJS".ToLower()) CustomData._noteJumpMovementSpeed = JsonSerializer.Deserialize<object>(param.Data);

                else if (param.Name == "NoSpawnEffect".ToLower()) CustomData._disableSpawnEffect = JsonSerializer.Deserialize<object>(param.Data);

                else if (param.Name == "CPropID".ToLower()) CustomData._propID = JsonSerializer.Deserialize<object>(param.Data);

                else if (param.Name == "CLightID".ToLower()) CustomData._lightID = JsonSerializer.Deserialize<object>(param.Data);

                else if (param.Name == "CgradientDuration".ToLower()) { CustomData._lightGradient ??= new BeatMap.CustomData.LightGradient(); CustomData._lightGradient._duration = JsonSerializer.Deserialize<object[]>(param.Data); }

                else if (param.Name == "CgradientStartColor".ToLower()) { CustomData._lightGradient ??= new BeatMap.CustomData.LightGradient(); CustomData._lightGradient._startColor = JsonSerializer.Deserialize<object[]>(param.Data); }

                else if (param.Name == "CgradientEndColor".ToLower()) { CustomData._lightGradient ??= new BeatMap.CustomData.LightGradient(); CustomData._lightGradient._endColor = JsonSerializer.Deserialize<object[]>(param.Data); }

                else if (param.Name == "CgradientEasing".ToLower()) { CustomData._lightGradient ??= new BeatMap.CustomData.LightGradient(); CustomData._lightGradient._easing = JsonSerializer.Deserialize<object>($"\"{param.Data}\""); }

                else if (param.Name == "CLockPosition".ToLower()) CustomData._lockPosition = JsonSerializer.Deserialize<object>(param.Data);

                else if (param.Name == "CPreciseSpeed".ToLower()) CustomData._preciseSpeed = JsonSerializer.Deserialize<object>(param.Data);

                else if (param.Name == "Cdirection".ToLower()) CustomData._direction = JsonSerializer.Deserialize<object>(param.Data);

                else if (param.Name == "CNameFilter".ToLower()) CustomData._nameFilter = JsonSerializer.Deserialize<object>($"\"{param.Data}\"");

                else if (param.Name == "Creset".ToLower()) CustomData._reset = JsonSerializer.Deserialize<object>(param.Data);

                else if (param.Name == "Cstep".ToLower()) CustomData._step = JsonSerializer.Deserialize<object>(param.Data);

                else if (param.Name == "Cprop".ToLower()) CustomData._prop = JsonSerializer.Deserialize<object>(param.Data);

                else if (param.Name == "Cspeed".ToLower()) CustomData._speed = JsonSerializer.Deserialize<object>(param.Data);

                else if (param.Name == "CCounterSpin".ToLower()) CustomData._counterSpin = JsonSerializer.Deserialize<object>(param.Data);

                if (typeof(BeatMap.CustomData.Animation).GetProperties().Any(p => p.GetValue(Animation) != null)) CustomData._animation = Animation;

            }

            return CustomData;

        }




        //adjust for lowercaseeaaa
        public static BeatMap.CustomData.CustomEvents.Data CustomEventsDataParse(this Parameter[] CustomNoodleData)
        {

            BeatMap.CustomData.CustomEvents.Data Data = new BeatMap.CustomData.CustomEvents.Data();

            foreach (var param in CustomNoodleData)
            {


                if (param.Name == "track".ToLower()) Data._track = JsonSerializer.Deserialize<object>("\"" + param.Data + "\"");

                else if (param.Name == "parentTrack".ToLower()) Data._parentTrack = JsonSerializer.Deserialize<object>("\"" + param.Data + "\"");

                else if (param.Name == "duration".ToLower()) Data._duration = JsonSerializer.Deserialize<object>(param.Data);

                else if (param.Name == "easing".ToLower()) Data._easing = JsonSerializer.Deserialize<object>("\"" + param.Data + "\"");

                else if (param.Name == "childTracks".ToLower()) Data._childrenTracks = JsonSerializer.Deserialize<object[]>(param.Data);

                else if (param.Name == "AnimateDefinitePosition".ToLower()) Data._definitePosition = JsonSerializer.Deserialize<object[][]>($"[{param.Data}]");
                else if (param.Name == "DefineAnimateDefinitePosition".ToLower()) Data._definitePosition = param.Data;

                else if (param.Name == "AnimatePosition".ToLower()) Data._position = JsonSerializer.Deserialize<object[][]>($"[{param.Data}]");
                else if (param.Name == "DefineAnimatePosition".ToLower()) Data._position = param.Data;

                else if (param.Name == "AnimateDissolve".ToLower()) Data._dissolve = JsonSerializer.Deserialize<object[][]>($"[{param.Data}]");
                else if (param.Name == "DefineAnimateDissolve".ToLower()) Data._dissolve = param.Data;

                else if (param.Name == "AnimateDissolveArrow".ToLower()) Data._dissolveArrow = JsonSerializer.Deserialize<object[][]>($"[{param.Data}]");
                else if (param.Name == "DefineAnimateDissolveArrow".ToLower()) Data._dissolveArrow = param.Data;

                else if (param.Name == "AnimateColor".ToLower()) Data._color = JsonSerializer.Deserialize<object[][]>($"[{param.Data}]");
                else if (param.Name == "DefineAnimateColor".ToLower()) Data._color = param.Data;

                else if (param.Name == "AnimateRotation".ToLower()) Data._rotation = JsonSerializer.Deserialize<object[][]>("[" + param.Data + "]");
                else if (param.Name == "DefineAnimateRotation".ToLower()) Data._rotation = param.Data;

                else if (param.Name == "AnimateLocalRotation".ToLower()) Data._localRotation = JsonSerializer.Deserialize<object[][]>($"[{param.Data}]");
                else if (param.Name == "DefineAnimateLocalRotation".ToLower()) Data._localRotation = param.Data;

                else if (param.Name == "AnimateScale".ToLower()) Data._scale = JsonSerializer.Deserialize<object[][]>($"[{param.Data}]");
                else if (param.Name == "DefineAnimateScale".ToLower()) Data._scale = param.Data;

                else if (param.Name == "AnimateTime".ToLower()) Data._time = JsonSerializer.Deserialize<object[][]>("[" + param.Data + "]");
                else if (param.Name == "DefineAnimateTime".ToLower()) Data._time = param.Data;

                else if (param.Name == "AnimateInteractable".ToLower()) Data._interactable = JsonSerializer.Deserialize<object[][]>("[" + param.Data + "]");
                else if (param.Name == "DefineAnimateInteractable".ToLower()) Data._interactable = param.Data;

            }
            return Data;
        }
    }
}
