using ModChart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace ScuffedWalls
{
    static class Extensions
    {
        public static string[] ParseSWArray(this string array) =>  array.Replace("[", "").Replace("]", "").SplitExcludeParanthesis();
        public static string[] SplitExcludeBrackets(BracketAnalyzer analyzer)
        {
            List<int> splits = new List<int>();

            for (int i = 0; i < analyzer.FullLine.Length; i++)
            {
                if (analyzer.IsOpeningBracket(i)) i = analyzer.GetPosOfClosingSymbol(i);
                else if (analyzer.FullLine[i] == ',') splits.Add(i);
            }
            
            return analyzer.FullLine.SplitAt(splits.ToArray());
        }
        public static string[] SplitAt(this string source, int[] indexes)
        {
            indexes = indexes.OrderBy(x => x).ToArray();
            string[] output = new string[indexes.Length + 1];
            int lastpos = 0;

            for (int i = 0; i < indexes.Length; i++)
            {
                output[i] = source.Substring(lastpos, indexes[i] - lastpos);
                lastpos = indexes[i] + 1;
            }

            output[indexes.Length] = source.Substring(lastpos);
            return output;
        }
        public static string[] SplitExcludeParanthesis(this string line) => SplitExcludeBrackets(new BracketAnalyzer(line, '(',')'));
        public static TreeList<T> ToTreeList<T>(this IEnumerable<T> enumerable, Func<T, string> exposer) => new TreeList<T>(enumerable, exposer);
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
        public static IEnumerable<ITimeable> GetAllBetween(this IEnumerable<ITimeable> mapObjects, float starttime, float endtime)
        {
            return mapObjects.Where(obj => obj._time.ToFloat() >= starttime && obj._time.ToFloat() <= endtime).ToArray();
        }
        public static string MakePlural(this string s, int amount)
        {
            if (amount == 1) return s.TrimEnd('s');
            else return s.SetEnd('s');
        }
        public static void AddRange<K,T>(this Dictionary<K, T> dict, IEnumerable<KeyValuePair<K, T>> items)
        {
            foreach (var item in items)
            {
                dict[item.Key] = item.Value;
            }
        }
        public static List<Parameter> ToParameters(this IEnumerable<KeyValuePair<int, string>> lines)
        {
            return lines.Select(line => new Parameter(line.Value, line.Key)).ToList();
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
        public static string ToFileString(this DateTime time)
        {
            return $"Backup - {time.ToFileTime()}";
        }

        public static string RemoveWhiteSpace(this string WhiteSpace)
        {
            return new string(WhiteSpace.Where(c => !Char.IsWhiteSpace(c)).ToArray());
        }
    }

}
