using ModChart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace ScuffedWalls
{
    static class Extensions
    {
        /// <summary>
        /// Attempts a deep clone of an array and all of the nested arrays, clones ICloneable
        /// </summary>
        /// <param name="Array"></param>
        /// <returns></returns>
        public static IEnumerable<object> CloneArray(this IEnumerable<object> Array)
        {
            return Array.Select(item =>
            {
                if (item is IEnumerable<object> nestedArray) return nestedArray.CloneArray();
                else if (item is ICloneable cloneable) return cloneable.Clone();
                else return item;
            });
        }
        public static IEnumerable<T> CombineWith<T>(this IEnumerable<T> first, params IEnumerable<T>[] arrays)
        {
            List<T> list = new List<T>();
            list.AddRange(first);
            foreach (var array in arrays) if (array != null) list.AddRange(array);

            return list.ToArray();
        }
        /*
        public static string Remove(this string str, char ch)
        {
            return string.Join("", str.Split(ch));
        }
        public static string Remove(this string str, string ch)
        {
            return string.Join("", str.Split(ch));
        }
        */
        public static IEnumerable<ITimeable> GetAllBetween(this IEnumerable<ITimeable> mapObjects, float starttime, float endtime)
        {
            return mapObjects.Where(obj => obj._time.ToFloat() >= starttime && obj._time.ToFloat() <= endtime).ToArray();
        }
        /*
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
        */
        public static string MakePlural(this string s, int amount)
        {
            if (amount == 0) return s.TrimEnd('s');
            else return s.SetEnd('s');
        }

        public static string SetEnd(this string s, char character)
        {
            if (s.Last() == character) return s;
            else return s + character;
        }
        public static List<T> Lasts<T>(this IEnumerable<T> list)
        {
            var newlist = new List<T>();
            for(int i = 1; i < list.Count(); i++) newlist.Add(list.ElementAt(i));
            return newlist;
        }

        /*
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
        */
        public static string ToFileString(this DateTime time)
        {
            return $"Backup - {time.ToFileTime()}";
        }

        public static string RemoveWhiteSpace(this string WhiteSpace)
        {
            return new string(WhiteSpace.Where(c => !Char.IsWhiteSpace(c)).ToArray());
        }
        /*
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> Array)
        {
            Random rnd = new Random();
            return Array.OrderBy(x => rnd.Next());
        }
        */
    }

    public class JsonValidator
    {
        public dynamic Deserialized;
        public bool WasSuccess;
        public string Raw;
        public static JsonValidator Check(string s)
        {
            var val = new JsonValidator() { Raw = s };

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
        public static JsonValidator Check<t>(string s)
        {
            var val = new JsonValidator() { Raw = s };

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
}
