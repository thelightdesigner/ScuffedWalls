using ModChart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace ScuffedWalls
{
    public static class CustomDataParser
    {
        public static Func<string, object> TrackConverter => track => DeserializeDefaultToString<object[]>(track);
        public static Func<string, float> FloatConverter => val => float.Parse(val);
        public static Func<string, bool> BoolConverter => val => bool.Parse(val);
        public static Func<string, string> StringConverter => val => val;
        public static Func<string, object> ArrayConverter => val => JsonSerializer.Deserialize<object[]>(val);
        public static Func<string, object> NestedArrayDefaultStringConverter => val => DeserializeDefaultToString<object[][]>($"[{val}]");
        public static ICustomDataMapObject CustomDataParse(this TreeList<Parameter> parameters, ICustomDataMapObject objInstance)
        {
            objInstance._time = GetParam("time", null, p => (float?)float.Parse(p));
            var customdata = new TreeDictionary
            {
                ["_interactable"] = GetParam("interactable", null, p => (object)bool.Parse(p)),
                ["_disableNoteGravity"] = GetParam("disablenotegravity", null, p => (object)bool.Parse(p)),
                ["_cutDirection"] = GetParam("cutdirection", null, p => (object)float.Parse(p)),
                ["_noteJumpMovementSpeed"] = GetParam("njs", null, p => (object)float.Parse(p)),
                ["_noteJumpStartBeatOffset"] = GetParam("njsoffset", null, p => (object)float.Parse(p)),
                ["_track"] = GetParam("track", null, TrackConverter),
                ["_fake"] = GetParam("fake", null, p => (object)bool.Parse(p)),
                ["_rotation"] = GetParam("rotation", null, p => JsonSerializer.Deserialize<object[]>(p)) ?? GetParam("Crotation", null, p => (object)float.Parse(p)),
                ["_localRotation"] = GetParam("localrotation", null, p => JsonSerializer.Deserialize<object[]>(p)),
                ["_position"] = GetParam("position", null, p => JsonSerializer.Deserialize<object[]>(p)),
                ["_scale"] = GetParam("scale", null, p => JsonSerializer.Deserialize<object[]>(p)),
                ["_propID"] = GetParam("cpropid", null, p => (object)int.Parse(p)),
                ["_lightID"] = GetParam("clightid", null, p => (object)int.Parse(p)),
                ["_disableSpawnEffect"] = GetParam("disablespawneffect", null, p => (object)bool.Parse(p)),
                ["_color"] = GetParam("color", null, p => JsonSerializer.Deserialize<object[]>(p)) ?? GetParam("rgbcolor", null, p => JsonSerializer.Deserialize<object[]>(p).Select(o => (object)(o.ToFloat() / 255f)).ToArray()),
                ["_lockPosition"] = GetParam("clockposition", null, p => (object)bool.Parse(p)),
                ["_preciseSpeed"] = GetParam("CPreciseSpeed", null, p => (object)float.Parse(p)),
                ["_direction"] = GetParam("Cdirection", null, p => (object)int.Parse(p)),
                ["_nameFilter"] = GetParam("CNameFilter", null, p => (object)p),
                ["_reset"] = GetParam("Creset", null, p => (object)bool.Parse(p)),
                ["_step"] = GetParam("cstep", null, p => (object)float.Parse(p)),
                ["_prop"] = GetParam("cprop", null, p => (object)float.Parse(p)),
                ["_speed"] = GetParam("cspeed", null, p => (object)float.Parse(p)),
                ["_counterSpin"] = GetParam("ccounterspin", null, p => (object)bool.Parse(p)),
                ["_disableNoteLook"] = GetParam("disablenotelook", null, p => (object)bool.Parse(p))
            };
            var animation = new TreeDictionary()
            {
                ["_definitePosition"] = GetParam("animatedefiniteposition", null, p => DeserializeDefaultToString<object[][]>($"[{p}]")),
                ["_position"] = GetParam("animateposition", null, p => DeserializeDefaultToString<object[][]>($"[{p}]")),
                ["_dissolve"] = GetParam("animatedissolve", null, p => DeserializeDefaultToString<object[][]>($"[{p}]")),
                ["_dissolveArrow"] = GetParam("animatedissolvearrow", null, p => DeserializeDefaultToString<object[][]>($"[{p}]")),
                ["_color"] = GetParam("animatecolor", null, p => DeserializeDefaultToString<object[][]>($"[{p}]")),
                ["_rotation"] = GetParam("animaterotation", null, p => DeserializeDefaultToString<object[][]>($"[{p}]")),
                ["_localRotation"] = GetParam("animatelocalrotation", null, p => DeserializeDefaultToString<object[][]>($"[{p}]")),
                ["_scale"] = GetParam("animatescale", null, p => DeserializeDefaultToString<object[][]>($"[{p}]")),
                ["_interactable"] = GetParam("animateinteractable", null, p => DeserializeDefaultToString<object[][]>($"[{p}]")),
                ["_time"] = GetParam("animatetime", null, p => DeserializeDefaultToString<object[][]>($"[{p}]"))
            };
            var gradient = new TreeDictionary()
            {
                ["_duration"] = GetParam("cgradientduration", null, p => (object)float.Parse(p)),
                ["_easing"] = GetParam("cgradienteasing", null, p => (object)p),
                ["_endColor"] = GetParam("cgradientendcolor", null, p => JsonSerializer.Deserialize<object[]>(p)),
                ["_startColor"] = GetParam("cgradientstartcolor", null, p => JsonSerializer.Deserialize<object[]>(p))
            };

            customdata.DeleteNullValues();
            animation.DeleteNullValues();
            gradient.DeleteNullValues();


            if (animation.Any()) customdata["_animation"] = animation;
            if (gradient.Any()) customdata["_lightGradient"] = gradient;
            if (customdata.Any()) objInstance._customData = customdata;


            //Console.WriteLine(Instance._customData._animation._definitePosition);

            return objInstance;


            T GetParam<T>(string Name, T DefaultValue, Func<string, T> Converter)
            {
                var param = parameters.Get(Name);
                if (param == null) return DefaultValue;
                try
                {
                    var converted = Converter(param.StringData);
                    param.WasUsed = true;
                    return converted;
                }
                catch (Exception e)
                {
                    ScuffedWalls.Print($"{Name} Couldnt be parsed ERROR: {e.Message}", ScuffedWalls.LogSeverity.Error);
                    return DefaultValue;
                }
            }
        }
        static object DeserializeDefaultToString<T>(string JSON)
        {
            char[] JSONChars = { ',' };
            try
            {
                return JsonSerializer.Deserialize<T>(JSON);
            }
            catch (Exception e)
            {
                if (JSONChars.Any(j => JSON.Contains(j))) throw e;
                return JSON.TrimStart('[').TrimEnd(']');
            }
        }

        public static TreeDictionary CustomEventsDataParse(this TreeList<Parameter> parameters)
        {
            var customdata = new TreeDictionary()
            {
                ["_height"] = GetParam("animateheight", null, p => JsonSerializer.Deserialize<object[][]>($"[{p}]")) ?? GetParam("defineanimateheight", null, p => (object)p),
                ["_attenuation"] = GetParam("animateattenuation", null, p => JsonSerializer.Deserialize<object[][]>($"[{p}]")) ?? GetParam("defineanimateattenuation", null, p => (object)p),
                ["_startY"] = GetParam("animatestartY", null, p => JsonSerializer.Deserialize<object[][]>($"[{p}]")) ?? GetParam("defineanimatestartY", null, p => (object)p),
                ["_offset"] = GetParam("animatoffset", null, p => JsonSerializer.Deserialize<object[][]>($"[{p}]")) ?? GetParam("defineanimatoffset", null, p => (object)p),
                ["_definitePosition"] = GetParam("animatedefiniteposition", null, p => JsonSerializer.Deserialize<object[][]>($"[{p}]")) ?? GetParam("defineanimatedefiniteposition", null, p => (object)p),
                ["_position"] = GetParam("animateposition", null, p => JsonSerializer.Deserialize<object[][]>($"[{p}]")) ?? GetParam("defineanimateposition", null, p => (object)p),
                ["_dissolve"] = GetParam("animatedissolve", null, p => JsonSerializer.Deserialize<object[][]>($"[{p}]")) ?? GetParam("defineanimatedissolve", null, p => (object)p),
                ["_dissolveArrow"] = GetParam("animatedissolvearrow", null, p => JsonSerializer.Deserialize<object[][]>($"[{p}]")) ?? GetParam("defineanimatedissolvearrow", null, p => (object)p),
                ["_color"] = GetParam("animatecolor", null, p => JsonSerializer.Deserialize<object[][]>($"[{p}]")) ?? GetParam("defineanimatecolor", null, p => (object)p),
                ["_rotation"] = GetParam("animaterotation", null, p => JsonSerializer.Deserialize<object[][]>($"[{p}]")) ?? GetParam("defineanimaterotation", null, p => (object)p),
                ["_localRotation"] = GetParam("animatelocalrotation", null, p => JsonSerializer.Deserialize<object[][]>($"[{p}]")) ?? GetParam("defineanimatelocalrotation", null, p => (object)p),
                ["_scale"] = GetParam("animatescale", null, p => JsonSerializer.Deserialize<object[][]>($"[{p}]")) ?? GetParam("defineanimatescale", null, p => (object)p),
                ["_interactable"] = GetParam("animateinteractable", null, p => JsonSerializer.Deserialize<object[][]>($"[{p}]")) ?? GetParam("defineanimateinteractable", null, p => (object)p),
                ["_time"] = GetParam("animatetime", null, p => JsonSerializer.Deserialize<object[][]>($"[{p}]")) ?? GetParam("defineanimatetime", null, p => (object)p),
                ["_parentTrack"] = GetParam("parenttrack", null, p => (object)p),
                ["_childrenTracks"] = GetParam("childtracks", null, p => JsonSerializer.Deserialize<object[]>(p)),
                ["_duration"] = GetParam("duration", null, p => (object)float.Parse(p)),
                ["_easing"] = GetParam("easing", null, p => (object)p),
                ["_track"] = GetParam("track", null, p => p.TrimStart()),
                ["_localPosition"] = GetParam("animatelocalposition", null, p => JsonSerializer.Deserialize<object[][]>($"[{p}]")) ?? GetParam("defineanimatelocalposition", null, p => (object)p),
                ["_localScale"] = GetParam("animatelocalscale", null, p => JsonSerializer.Deserialize<object[][]>($"[{p}]")) ?? GetParam("defineanimatelocalscale", null, p => (object)p)
            };

            return customdata;

            T GetParam<T>(string Name, T DefaultValue, Func<string, T> Converter)
            {
                var param = parameters.Get(Name);
                if (param == null) return DefaultValue;
                try
                {
                    var converted = Converter(param.StringData);
                    param.WasUsed = true;
                    return converted;
                }
                catch (Exception e)
                {
                    ScuffedWalls.Print($"{Name} Couldnt be parsed ERROR: {e.Message}", ScuffedWalls.LogSeverity.Error);
                    return DefaultValue;
                }
            }
        }
    }
}
