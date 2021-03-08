using ModChart;
using ModChart.Wall;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;

namespace ScuffedWalls
{

    static class Internal
    {
        public static T[] CombineWith<T>(this T[] first, params T[][] arrays)
        {
            List<T> list = new List<T>();
            list.AddRange(first);
            foreach (var array in arrays) if(array != null) list.AddRange(array);
            return list.ToArray();
        }
        public static string Remove(this string str, char ch)
        {
            return string.Join("", str.Split(ch));
        }
        public static string Remove(this string str, string ch)
        {
            return string.Join("", str.Split(ch));
        }
        public static T[] GetAllBetween<T>(this T[] mapObjects, float starttime, float endtime)
        {
            return mapObjects.Where(obj => (Convert.ToSingle(typeof(T).GetProperty("_time").GetValue(obj, null).ToString()) >= starttime) && (Convert.ToSingle(typeof(T).GetProperty("_time").GetValue(obj, null).ToString()) <= endtime)).ToArray();
        }
        static public bool MethodExists<t>(this string methodname, Type attribute)
        {
            foreach (var methods in typeof(t).GetMethods().Where(m => m.GetCustomAttributes(attribute).Count() > 0))
            {
                if (methods.Name == methodname) return true;
            }
            return false;
        }
        static public bool MethodExists<t>(this string methodname)
        {
            foreach (var methods in typeof(t).GetMethods())
            {
                if (methods.Name == methodname) return true;
            }
            return false;
        }
        public static bool needsNoodleExtensions(this BeatMap map)
        {
            //are there any custom events?
            if (map._customData != null && map._customData._customEvents != null && map._customData._customEvents.Length > 0) return true;

            //do any notes have any noodle data other than color?
            if (map._notes.Any(note => note._customData != null && (typeof(BeatMap.CustomData).GetProperties().Any(p => p.GetValue(note._customData) != null && p.Name != "_color")))) return true;

            //do any walls have any noodle data other than color?
            if (map._obstacles.Any(wall => wall._customData != null && (typeof(BeatMap.CustomData).GetProperties().Any(p => p.GetValue(wall._customData) != null && p.Name != "_color")))) return true;

            return false;
        }
        public static bool needsChroma(this BeatMap map)
        {
            //do light have color
            if (map._events.Any(light => light._customData != null && light._customData._color != null)) return true;

            //do wal have color or animate color
            if (map._obstacles.Any(wall => wall._customData != null && (wall._customData._color != null || (wall._customData._animation != null && wall._customData._animation._color != null)))) return true;

            //do note have color or animate color
            if (map._notes.Any(note => note._customData != null && (note._customData._color != null || (note._customData._animation != null && note._customData._animation._color != null)))) return true;

            return false;

        }
        public static int getCountByID(this int type)
        {
            if (type == 0 || type == 2 || type == 3) return 5;
            else if (type == 1) return 15;
            else if (type == 4) return 9;
            return 0;
        }
        public static int getValueFromOld(this int value)
        {
            if (value == 0) return 0;
            return 5;
        }
        public static T DeepClone<T>(this T a)
        {
            return JsonSerializer.Deserialize<T>(JsonSerializer.Serialize(a));
        }
        public static string ToFileString(this DateTime time)
        {
            return $"Backup - {time.ToFileTime()}";
        }

        public static string removeWhiteSpace(this string WhiteSpace)
        {
            return new string(WhiteSpace.Where(c => !Char.IsWhiteSpace(c)).ToArray());
        }
    }

    public class JsoValidator
    {
        public dynamic Deserialized;
        public bool WasSuccess;
        public string Raw;
        public static JsoValidator Check(string s)
        {
            var val = new JsoValidator() { Raw = s };

            try
            {
                val.Deserialized = JsonSerializer.Deserialize<object>(s);
                val.WasSuccess = true;
            }
            catch
            {
                val.WasSuccess = false;
            }

            return val;
        }
        public static JsoValidator Check<t>(string s)
        {
            var val = new JsoValidator() { Raw = s };

            try
            {
                val.Deserialized = JsonSerializer.Deserialize<t>(s);
                val.WasSuccess = true;
            }
            catch
            {
                val.WasSuccess = false;
            }

            return val;
        }
    }
    public class TimeKeeper
    {
        public DateTime StartTime;
        public void Start() => StartTime = DateTime.Now;
        public void Complete() =>  ScuffedLogger.Log($"Completed in {(DateTime.Now - StartTime).TotalSeconds} seconds");
    }




}
