using ScuffedWalls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ModChart
{
    public interface ITreeDictionary : ICloneable
    { 
        public TreeDictionary<T> Convert<T, C>(Func<T, C> converter);
        public void DeleteNullValues();
        public object at(string Key);
        public void set(string Key, object Value);
        public T at<T>(string Key);
        public object GetMultiple(string Key);
        public void SetMultiple(string Key, object value);
    }
    public class TreeDictionary<TreeType> : Dictionary<string, TreeType>, ITreeDictionary, ICloneable
    {
        public void DeleteNullValues()
        {
            var Nulls = Keys.Where(key => base[key] == null);
            foreach (string key in Nulls) Remove(key);

            foreach (var item in this)
            {
                if (item.Value is ITreeDictionary dict) dict.DeleteNullValues();
                else if (item.Value is IEnumerable<object> array) foreach (var element in array) if (element is ITreeDictionary dict2) dict2.DeleteNullValues();
            }
        }
        public object Clone()
        {
            TreeDictionary<TreeType> New = new TreeDictionary<TreeType>();

            foreach (var Item in this)
            {
                if (Item.Value is ICloneable cloneable) New[Item.Key] = (TreeType)cloneable.Clone();
                else if (Item.Value is IEnumerable<object> array) New[Item.Key] = (TreeType)array.CloneArray();
                else New[Item.Key] = Item.Value;
            }

            return New;
        }
        
        public TreeDictionary<H> Merge<H>(TreeDictionary<H> Dictionary2, MergeType mergeType = MergeType.Dictionaries | MergeType.Objects | MergeType.Arrays, MergeBindingFlags mergeBindingFlags = MergeBindingFlags.HasValue, bool PreserveInstance = false) => Merge(this, Dictionary2, mergeType, mergeBindingFlags, PreserveInstance);

        /// <summary>
        /// Merges two IDictionaries, prioritizes Dictionary1, Will not cast between two different typed generics
        /// </summary>
        /// <param name="Dictionary1"></param>
        /// <param name="Dictionary2"></param>
        /// <returns>A TreeDictionary as an IDictionary</returns>
        public static TreeDictionary<T> Merge<T>(TreeDictionary<T> Dictionary1, TreeDictionary<T> Dictionary2, MergeType mergeType = MergeType.Dictionaries | MergeType.Objects | MergeType.Arrays, MergeBindingFlags mergeBindingFlags = MergeBindingFlags.HasValue, bool PreserveInstance = false)
        {
            Dictionary1 ??= new TreeDictionary<T>();
            Dictionary2 ??= new TreeDictionary<T>();

            TreeDictionary<T> Merged = PreserveInstance ? Dictionary1 : new TreeDictionary<T>();

            if (!PreserveInstance) foreach (KeyValuePair<string, T> Item in Dictionary1) Merged[Item.Key] = Item.Value;

            foreach (KeyValuePair<string, T> Item in Dictionary2)
            {
                if (!dict1ItemExists(Item))
                    if (mergeType.HasFlag(MergeType.Objects))
                        Merged[Item.Key] = Item.Value;
                    else
                        continue;
                else
                {
                    if (Merged[Item.Key] is IDictionary<string, T> dictionary1 && Item.Value is IDictionary<string, object> dictionary2)
                        if (mergeType.HasFlag(MergeType.Dictionaries)) Merged[Item.Key] = Merge(dictionary1, dictionary2, mergeType, mergeBindingFlags);
                        else continue;

                    else if (Merged[Item.Key] is IList<object> List1 && Item.Value is IEnumerable<object> Array3)
                        if (mergeType.HasFlag(MergeType.Arrays)) foreach (var obj in Array3)
                                List1.Add(obj);
                        else continue;

                    else if (Merged[Item.Key] is IEnumerable<object> Array1 && Item.Value is IEnumerable<object> Array2)
                        if (mergeType.HasFlag(MergeType.Arrays)) Merged[Item.Key] = CombineWith(Array1,Array2);
                        else continue;

                }
            }
            return Merged;

            bool dict1ItemExists(KeyValuePair<string, T> Item)
            {
                switch (mergeBindingFlags)
                {
                    case MergeBindingFlags.Exists:
                        if (Dictionary1.ContainsKey(Item.Key)) return true;
                        else return false;
                    case MergeBindingFlags.HasValue:
                        if (Dictionary1.TryGetValue(Item.Key, out T Value) && Value != null) return true;
                        else return false;
                }
                return false;
            }

        }
        [Flags]
        public enum MergeType
        {
            Objects,
            Arrays,
            Dictionaries
        }
        [Flags]
        public enum MergeBindingFlags
        {
            Exists,
            HasValue
        }
        public object GetMultiple(string Key)
        {
            return getLastDictInChain(Key, out string last).at(last);
        }
        public void SetMultiple(string Key, object value)
        {
            getLastDictInChain(Key, out string last).set(last, value);
        }

        private ITreeDictionary getLastDictInChain(string Key, out string LastKey)
        {
            if (!Key.Contains('.'))
            {
                LastKey = Key;
                return this;
            }

            string[] Layers = Key.Split('.');

            object CurrentLayer = this;
            for (int i = 0; i < Layers.Length - 1; i++)
            {
                if (CurrentLayer is ITreeDictionary dictionary)
                {
                    CurrentLayer = dictionary.at(Layers[i]);
                }
                else throw new NullReferenceException($"TreeDictionary does not contain one or more of the SubTrees referenced {{{Key}}}");
            }
            LastKey = Layers.Last();
            return CurrentLayer as ITreeDictionary;
        }

        public TreeDictionary<T> Expose<T>() => (TreeDictionary<T>)(object)this;

        public TreeDictionary<T> Convert<T, C>(Func<T, C> converter)
        {
            throw new NotImplementedException();
        }

        public object at(string Key) => base[Key];

        public void set(string Key, object Value)
        {
            base[Key] = (TreeType)Value;
        }

        public T at<T>(string Key)
        {
            throw new NotImplementedException();
        }
    }
    public class TreeDictionaryJsonConverter : JsonConverter<ITreeDictionary>
    {
        public override ITreeDictionary Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject) throw new JsonException($"JsonTokenType was of type {reader.TokenType}, only objects are supported");

            var dictionary = new TreeDictionary<object>();
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject) return dictionary;
                if (reader.TokenType != JsonTokenType.PropertyName) throw new JsonException("JsonTokenType was not PropertyName");

                var propertyName = reader.GetString();

                if (string.IsNullOrWhiteSpace(propertyName)) throw new JsonException("Failed to get property name");

                reader.Read();

                dictionary.Add(propertyName, ExtractValue(ref reader, options));
            }

            return dictionary;
        }

        public override void Write(Utf8JsonWriter writer, ITreeDictionary value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value as IDictionary<string, object>, options);
        }


        private object ExtractValue(ref Utf8JsonReader reader, JsonSerializerOptions options)
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.String:
                    if (reader.TryGetDateTime(out var date)) return date;
                    return reader.GetString();
                case JsonTokenType.False:
                    return false;
                case JsonTokenType.True:
                    return true;
                case JsonTokenType.Null:
                    return null;
                case JsonTokenType.Number:
                    if (reader.TryGetInt64(out var result)) return result;
                    return reader.GetDecimal();
                case JsonTokenType.StartObject:
                    return Read(ref reader, null, options);
                case JsonTokenType.StartArray:
                    IList<object> list = new List<object>();
                    while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
                    {
                        list.Add(ExtractValue(ref reader, options));
                    }
                    return list;
                default:
                    throw new JsonException($"'{reader.TokenType}' is not supported");
            }
        }
    }
}