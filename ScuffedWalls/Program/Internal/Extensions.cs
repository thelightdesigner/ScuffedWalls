using ModChart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace ScuffedWalls
{
    static class Extensions
    {
        public static string MakePlural(this string s, int amount)
        {
            if (amount == 1) return s.TrimEnd('s');
            else return s.SetEnd('s');
        }
        public static object GetClone(this object obj)
        {
            if (obj is ICloneable cloneable) return cloneable.Clone();
            else if (obj is IEnumerable<object> array) return array.CloneArray();
            else return obj;
        }
        public static string SetEnd(this string s, char character)
        {
            if (s.Last() == character) return s;
            else return s + character;
        }
        public static string ToFileString(this DateTime time)
        {
            return $"Backup - {time.ToFileTime()}";
        }
        public static string RemoveWhiteSpace(this string WhiteSpace)
        {
            return new string(WhiteSpace.Where(c => !Char.IsWhiteSpace(c)).ToArray());
        }
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
