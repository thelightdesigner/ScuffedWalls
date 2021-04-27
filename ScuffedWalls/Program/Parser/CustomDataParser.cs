using ModChart;
using System;
using System.Linq;
using System.Text.Json;

namespace ScuffedWalls
{
    public static class CustomDataParser
    {
        public static ICustomDataMapObject CustomDataParse(this Parameter[] CustomNoodleData, ICustomDataMapObject Instance)
        {
            Instance._time = GetParam("time", null, p => (object)float.Parse(p));
            var customdata = new BeatMap.CustomData()
            {
                _interactable = GetParam("interactable", null, p => (object)bool.Parse(p)),
                _disableNoteGravity = GetParam("disablenotegravity", null, p => (object)bool.Parse(p)),
                _cutDirection = GetParam("cutdirection", null, p => (object)float.Parse(p)),
                _noteJumpMovementSpeed = GetParam("njs", null, p => (object)float.Parse(p)),
                _noteJumpStartBeatOffset = GetParam("njsoffset", null, p => (object)float.Parse(p)),
                _track = GetParam("track", null, p => (object)p.TrimStart()),
                _fake = GetParam("fake", null, p => (object)bool.Parse(p)),
                _rotation = GetParam("rotation", null, p => JsonSerializer.Deserialize<object[]>(p)),
                _localRotation = GetParam("localrotation", null, p => JsonSerializer.Deserialize<object[]>(p)),
                _position = GetParam("position", null, p => JsonSerializer.Deserialize<object[]>(p)),
                _scale = GetParam("scale", null, p => JsonSerializer.Deserialize<object[]>(p)),
                _propID = GetParam("cpropid", null, p => (object)int.Parse(p)),
                _lightID = GetParam("clightid", null, p => (object)int.Parse(p)),
                _disableSpawnEffect = GetParam("disablespawneffect", null, p => (object)bool.Parse(p)),
                _color = GetParam("color", null, p => JsonSerializer.Deserialize<object[]>(p)) ?? GetParam("rgbcolor", null, p => JsonSerializer.Deserialize<object[]>(p).Select(o => (object)(o.toFloat() / 255f)).ToArray()),
                _lockPosition = GetParam("clockposition", null, p => (object)bool.Parse(p)),
                _preciseSpeed = GetParam("CPreciseSpeed", null, p => (object)float.Parse(p)),
                _direction = GetParam("Cdirection", null, p => (object)int.Parse(p)),
                _nameFilter = GetParam("CNameFilter", null, p => (object)p),
                _reset = GetParam("Creset", null, p => (object)bool.Parse(p)),
                _step = GetParam("cstep", null, p => (object)float.Parse(p)),
                _prop = GetParam("cprop", null, p => (object)float.Parse(p)),
                _speed = GetParam("cspeed", null, p => (object)float.Parse(p)),
                _counterSpin = GetParam("ccounterspin", null, p => (object)bool.Parse(p)),
                _disableNoteLook = GetParam("disablenotelook", null,p => (object)bool.Parse(p))
            };
            var animation = new BeatMap.CustomData.Animation()
            {
                _definitePosition = GetParam("animatedefiniteposition", null, p => JsonSerializer.Deserialize<object[][]>($"[{p}]")) ?? GetParam("defineanimatedefiniteposition", null, p => (object)p),
                _position = GetParam("animateposition", null, p => JsonSerializer.Deserialize<object[][]>($"[{p}]")) ?? GetParam("defineanimateposition", null, p => (object)p),
                _dissolve = GetParam("animatedissolve", null, p => JsonSerializer.Deserialize<object[][]>($"[{p}]")) ?? GetParam("defineanimatedissolve", null, p => (object)p),
                _dissolveArrow = GetParam("animatedissolvearrow", null, p => JsonSerializer.Deserialize<object[][]>($"[{p}]")) ?? GetParam("defineanimatedissolvearrow", null, p => (object)p),
                _color = GetParam("animatecolor", null, p => JsonSerializer.Deserialize<object[][]>($"[{p}]")) ?? GetParam("defineanimatecolor", null, p => (object)p),
                _rotation = GetParam("animaterotation", null, p => JsonSerializer.Deserialize<object[][]>($"[{p}]")) ?? GetParam("defineanimaterotation", null, p => (object)p),
                _localRotation = GetParam("animatelocalrotation", null, p => JsonSerializer.Deserialize<object[][]>($"[{p}]")) ?? GetParam("defineanimatelocalrotation", null, p => (object)p),
                _scale = GetParam("animatescale", null, p => JsonSerializer.Deserialize<object[][]>($"[{p}]")) ?? GetParam("defineanimatescale", null, p => (object)p),
                _interactable = GetParam("animateinteractable", null, p => JsonSerializer.Deserialize<object[][]>($"[{p}]")) ?? GetParam("defineanimateinteractable", null, p => (object)p),
                _time = GetParam("animatetime", null, p => JsonSerializer.Deserialize<object[][]>($"[{p}]")) ?? GetParam("defineanimatetime", null, p => (object)p)
            };
            var gradient = new BeatMap.CustomData.LightGradient()
            {
                _duration = GetParam("cgradientduration", null, p => (object)float.Parse(p)),
                _easing = GetParam("cgradienteasing", null, p => (object)p),
                _endColor = GetParam("cgradientendcolor", null, p => JsonSerializer.Deserialize<object[]>(p)),
                _startColor = GetParam("cgradientstartcolor", null, p => JsonSerializer.Deserialize<object[]>(p))
            };


            if (typeof(BeatMap.CustomData.Animation).GetProperties().Any(p => p.GetValue(animation) != null)) customdata._animation = animation;
            if (typeof(BeatMap.CustomData.LightGradient).GetProperties().Any(p => p.GetValue(gradient) != null)) customdata._lightGradient = gradient;
            if (typeof(BeatMap.CustomData).GetProperties().Any(p => p.GetValue(customdata) != null)) Instance._customData = customdata;


            //Console.WriteLine(Instance._customData._animation._definitePosition);

            return Instance;

            T GetParam<T>(string Name, T DefaultValue, Func<string, T> Converter)
            {
                if (!CustomNoodleData.Any(p => p.Name.ToLower() == Name.ToLower())) return DefaultValue;
                try
                {
                    var filtered = CustomNoodleData.Where(p => p.Name.ToLower() == Name.ToLower()).First();
                    var converted = Converter(filtered.StringData);
                    filtered.WasUsed = true;
                    return converted;
                }
                catch (Exception e)
                {
                    ScuffedLogger.Error.Log($"{Name} Couldnt be parsed ERROR: {e.Message}");
                    return DefaultValue;
                }
            }
        }




        //adjust for lowercaseeaaa
        public static BeatMap.CustomData.CustomEvent.Data CustomEventsDataParse(this Parameter[] CustomNoodleData)
        {
            var customdata = new BeatMap.CustomData.CustomEvent.Data()
            {
                _definitePosition = GetParam("animatedefiniteposition", null, p => JsonSerializer.Deserialize<object[][]>($"[{p}]")) ?? GetParam("defineanimatedefiniteposition", null, p => (object)p),
                _position = GetParam("animateposition", null, p => JsonSerializer.Deserialize<object[][]>($"[{p}]")) ?? GetParam("defineanimateposition", null, p => (object)p),
                _dissolve = GetParam("animatedissolve", null, p => JsonSerializer.Deserialize<object[][]>($"[{p}]")) ?? GetParam("defineanimatedissolve", null, p => (object)p),
                _dissolveArrow = GetParam("animatedissolvearrow", null, p => JsonSerializer.Deserialize<object[][]>($"[{p}]")) ?? GetParam("defineanimatedissolvearrow", null, p => (object)p),
                _color = GetParam("animatecolor", null, p => JsonSerializer.Deserialize<object[][]>($"[{p}]")) ?? GetParam("defineanimatecolor", null, p => (object)p),
                _rotation = GetParam("animaterotation", null, p => JsonSerializer.Deserialize<object[][]>($"[{p}]")) ?? GetParam("defineanimaterotation", null, p => (object)p),
                _localRotation = GetParam("animatelocalrotation", null, p => JsonSerializer.Deserialize<object[][]>($"[{p}]")) ?? GetParam("defineanimatelocalrotation", null, p => (object)p),
                _scale = GetParam("animatescale", null, p => JsonSerializer.Deserialize<object[][]>($"[{p}]")) ?? GetParam("defineanimatescale", null, p => (object)p),
                _interactable = GetParam("animateinteractable", null, p => JsonSerializer.Deserialize<object[][]>($"[{p}]")) ?? GetParam("defineanimateinteractable", null, p => (object)p),
                _time = GetParam("animatetime", null, p => JsonSerializer.Deserialize<object[][]>($"[{p}]")) ?? GetParam("defineanimatetime", null, p => (object)p),
                _parentTrack = GetParam("parenttrack", null, p => (object)p),
                _childrenTracks = GetParam("childtracks", null, p => JsonSerializer.Deserialize<object[]>(p)),
                _duration = GetParam("duration", null, p => (object)float.Parse(p)),
                _easing = GetParam("easing", null, p => (object)p),
                _track = GetParam("track", null, p => p.TrimStart()),
                _localPosition = GetParam("animatelocalposition", null, p => JsonSerializer.Deserialize<object[][]>($"[{p}]")) ?? GetParam("defineanimatelocalposition", null, p => (object)p),
                _localScale = GetParam("animatelocalscale", null, p => JsonSerializer.Deserialize<object[][]>($"[{p}]")) ?? GetParam("defineanimatelocalscale", null, p => (object)p)
            };

            return customdata;

            T GetParam<T>(string Name, T DefaultValue, Func<string, T> Converter)
            {
                if (!CustomNoodleData.Any(p => p.Name.ToLower() == Name.ToLower())) return DefaultValue;
                try
                {
                    var filtered = CustomNoodleData.Where(p => p.Name.ToLower() == Name.ToLower()).First();
                    var converted = Converter(filtered.StringData);
                    filtered.WasUsed = true;
                    return converted;
                }
                catch (Exception e)
                {
                    ScuffedLogger.Error.Log($"{Name} Couldnt be parsed ERROR: {e.Message}");
                    return DefaultValue;
                }
            }
        }
    }
}
