using System;
using System.Collections.Generic;
using System.Text.Json;

namespace ScuffedWalls
{
    public class CustomDataParser
    {
        public static Func<string, object> TrackConverter => track => track.ParseDynamicStringArray();
        public static Func<string, float> FloatConverter => val => float.Parse(val);
        public static Func<string, bool> BoolConverter => val => bool.Parse(val);
        public static Func<string, string> StringConverter => val => val;
        public static Func<string, object[]> JsonArrayConverter => val => JsonSerializer.Deserialize<object[]>(val);
        public static Func<string, object[]> JsonJaggedArrayConverter => val => JsonSerializer.Deserialize<object[]>(val);
        public static Func<string, object[]> JsonLazyArrayConverter => val => JsonSerializer.Deserialize<object[]>(val);
        public static Func<string, object> JsonConverter => val => JsonSerializer.Deserialize<object>(val);
        //  public static Func<string, object> NestedArrayDefaultStringConverter => val => DeserializeDefaultToString<object[][]>($"[{val}]");

        public static readonly CustomDataParser Instance = new CustomDataParser();

        //see this, convert to that 
        private static readonly Dictionary<string, Func<string, object>> heckStringToTypeParsers = new()
        {
            ["coordinates"] = JsonArrayConverter,
            ["worldrotation"] = JsonConverter
        };

        /// <summary>
        /// A lazy array is one that is defined by a string which can or can not have
        /// surrounding [square brackets]. They can have any data
        /// types and can be jagged.
        /// </summary>
        /// <returns>An object array containing the parsed results.</returns>
        public static object[] ParseLazyArray(string array)
        {
            BracketAnalyzer br = new BracketAnalyzer(array, '[', ']');
            if (br.NestingLevel() > 1) br.FocusFirst();
            if ()

            //[2,7,5],[1,76,35,"hello"],["hi"],"hello"

        }

        /*
         * public CustomDataParser()
        {

        }
        public CustomDataParser(TreeList<Parameter> paramss)
        {
            this.parameters = paramss;
        }

        private TreeList<Parameter> parameters;
        public DifficultyV3.CustomDataMapObject ReadToCustomData(TreeList<Parameter> parameters, DifficultyV3.CustomDataMapObject instance)
        {
            this.parameters = parameters;
            instance.CustomData = Read();
            //if (time.HasValue) instance._time = time.Value;

            return instance;
        }

        public SDictionary ReadAnimation(TreeList<Parameter> parameters)
        {
            this.parameters = parameters;
            var customdata = getAnimation(true);

            return customdata;
        }
        public SDictionary Read()
        {
            var customdata = new SDictionary
            {
                ["uninteractable"] = GetParam("uninteractable", null, p => (object)bool.Parse(p)),
                ["_disableNoteGravity"] = GetParam("disablenotegravity", null, p => (object)bool.Parse(p)),
                ["_cutDirection"] = GetParam("cutdirection", null, p => (object)float.Parse(p)),
                ["_noteJumpMovementSpeed"] = GetParam("njs", null, p => (object)float.Parse(p)),
                ["_noteJumpStartBeatOffset"] = GetParam("njsoffset", null, p => (object)float.Parse(p)),
                ["_track"] = GetParam("track", null, TrackConverter),
                //["_fake"] = GetParam("fake", null, p => (object)bool.Parse(p)),
                ["worldRotation"] = GetParam("rotation", null, p => JsonSerializer.Deserialize<object[]>(p)) ?? GetParam("Crotation", null, p => (object)float.Parse(p)),
                ["_localRotation"] = GetParam("localrotation", null, p => JsonSerializer.Deserialize<object[]>(p)),
                ["coordinates"] = GetParam("coordinates", null, p => JsonSerializer.Deserialize<object[]>(p)),
                ["_scale"] = GetParam("scale", null, p => JsonSerializer.Deserialize<object[]>(p)),
                ["_propID"] = GetParam("cpropid", null, p => (object)int.Parse(p)),
                ["_lightID"] = GetParam("clightid", null, p => p.ParseSWArray().Select(p => int.Parse(p))),
                ["_disableSpawnEffect"] = GetParam("disablespawneffect", null, p => (object)bool.Parse(p)),
                ["_color"] = GetParam("color", null, p => JsonSerializer.Deserialize<object[]>(p)),
                ["_lockPosition"] = GetParam("clockposition", null, p => (object)bool.Parse(p)),
                ["_preciseSpeed"] = GetParam("CPreciseSpeed", null, p => (object)float.Parse(p)),
                ["_direction"] = GetParam("Cdirection", null, p => (object)int.Parse(p)),
                ["_nameFilter"] = GetParam("CNameFilter", null, p => (object)p),
                ["_reset"] = GetParam("Creset", null, p => (object)bool.Parse(p)),
                ["_step"] = GetParam("cstep", null, p => (object)float.Parse(p)),
                ["_prop"] = GetParam("cprop", null, p => (object)float.Parse(p)),
                ["_speed"] = GetParam("cspeed", null, p => (object)float.Parse(p)),
                ["_counterSpin"] = GetParam("ccounterspin", null, p => (object)bool.Parse(p)),
                ["_disableNoteLook"] = GetParam("disablenotelook", null, p => (object)bool.Parse(p)),

            };
            var animation = getAnimation(false);
            var gradient = new SDictionary()
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

            return customdata;

        }
        private T GetParam<T>(string Name, T DefaultValue, Func<string, T> Converter)
        {
            var param = parameters.Get(Name);
            if (param == null || string.IsNullOrEmpty(param.StringData)) return DefaultValue;
            
            try
            {
                
                var converted = Converter(param.StringData);
                param.Use();
                return converted;
            }
            catch (Exception e)
            {
                ScuffedWalls.Print($"{Name} Couldnt be parsed ERROR: {e.Message}", ScuffedWalls.LogSeverity.Error);
                return DefaultValue;
            }
        }
        private SDictionary getAnimation(bool isEventData)
        {
            var data = new SDictionary()
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
                ["_time"] = GetParam("animatetime", null, p => DeserializeDefaultToString<object[][]>($"[{p}]")),
                ["_height"] = GetParam("animateheight", null, p => DeserializeDefaultToString<object[][]>($"[{p}]")),
                ["_attenuation"] = GetParam("animateattenuation", null, p => DeserializeDefaultToString<object[][]>($"[{p}]")),
                ["_startY"] = GetParam("animatestartY", null, p => DeserializeDefaultToString<object[][]>($"[{p}]")),
                ["_offset"] = GetParam("animateoffset", null, p => DeserializeDefaultToString<object[][]>($"[{p}]")),

            };
            if (isEventData)
            {
                data["_track"] = GetParam("track", null, TrackConverter);
                data["_parentTrack"] = GetParam("parenttrack", null, p => (object)p);
                data["_childrenTracks"] = GetParam("childtracks", null, p => p.ParseSWArray());
                data["_duration"] = GetParam("duration", null, p => (object)float.Parse(p));
                data["_easing"] = GetParam("easing", null, p => (object)p);
                data["_localPosition"] = GetParam("animatelocalposition", null, p => DeserializeDefaultToString<object[][]>($"[{p}]"));
                data["_localScale"] = GetParam("animatelocalscale", null, p => DeserializeDefaultToString<object[][]>($"[{p}]"));
                data["_worldPositionStays"] = GetParam("worldpositionstays", null, p => (object)bool.Parse(p));
            }
            return data;
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
    }*/
    }
