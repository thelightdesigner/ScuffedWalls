using ScuffedWalls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ModChart
{
    public class TreeDictionary : Dictionary<string, object>, IDictionary<string, object>, ICloneable
    {
        public void DeleteNullValues()
        {
            var Nulls = Keys.Where(key => base[key] == null);
            foreach (string key in Nulls) Remove(key);

            foreach (var item in this)
            {
                if (item.Value is TreeDictionary dict) dict.DeleteNullValues();
                else if (item.Value is IEnumerable<object> array) foreach (var element in array) if (element is TreeDictionary dict2) dict2.DeleteNullValues();
            }
        }
        public object Clone()
        {
            IDictionary<string, object> New = new TreeDictionary();

            foreach (var Item in this)
            {
                if (Item.Value is ICloneable cloneable) New[Item.Key] = cloneable.Clone();
                else if (Item.Value is IEnumerable<object> array) New[Item.Key] = array.CloneArray();
                else New[Item.Key] = Item.Value;
            }

            return New;
        }
        public static TreeDictionary Tree() => new TreeDictionary();
        public static TreeDictionary Tree(IDictionary<string, object> IDict)
        {
            TreeDictionary tree = new TreeDictionary();
            foreach (var item in IDict) tree.Add(item.Key, item.Value);
            return tree;
        }

        /// <summary>
        /// Merges two IDictionaries, prioritizes Dictionary1
        /// </summary>
        /// <param name="Dictionary1"></param>
        /// <param name="Dictionary2"></param>
        /// <returns>A TreeDictionary as an IDictionary</returns>
        public static TreeDictionary Merge(IDictionary<string, object> Dictionary1, IDictionary<string, object> Dictionary2, MergeType mergeType = MergeType.Dictionaries | MergeType.Objects | MergeType.Arrays, MergeBindingFlags mergeBindingFlags = MergeBindingFlags.HasValue)
        {
            Dictionary1 ??= new TreeDictionary();
            Dictionary2 ??= new TreeDictionary();

            TreeDictionary Merged = new TreeDictionary();
            foreach (KeyValuePair<string, object> Item in Dictionary1)
            {
                Merged[Item.Key] = Item.Value;
            }
            foreach (KeyValuePair<string, object> Item in Dictionary2)
            {
                if (!TreeItemExists(Item))
                    if (mergeType.HasFlag(MergeType.Objects))
                        Merged[Item.Key] = Item.Value;
                    else
                        continue;
                else
                {
                    if (Merged[Item.Key] is IDictionary<string, object> dictionary1 && Item.Value is IDictionary<string, object> dictionary2)
                        if (mergeType.HasFlag(MergeType.Dictionaries)) Merged[Item.Key] = Merge(dictionary1, dictionary2, mergeType, mergeBindingFlags);
                        else continue;
                    else if (Merged[Item.Key] is IList<object> List1 && Item.Value is IEnumerable<object> Array3)
                        if (mergeType.HasFlag(MergeType.Arrays)) foreach (var obj in Array3)
                                List1.Add(obj);
                        else continue;
                    else if (Merged[Item.Key] is IEnumerable<object> Array1 && Item.Value is IEnumerable<object> Array2)
                        if (mergeType.HasFlag(MergeType.Arrays)) Merged[Item.Key] = Array1.CombineWith(Array2);
                        else continue;
                }
            }
            return Merged;

            bool TreeItemExists(KeyValuePair<string, object> Item)
            {
                switch (mergeBindingFlags)
                {
                    case MergeBindingFlags.Exists:
                        if (Dictionary1.ContainsKey(Item.Key)) return true;
                        else return false;
                    case MergeBindingFlags.HasValue:
                        if (Dictionary1.TryGetValue(Item.Key, out object Value) && Value != null) return true;
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
        public TreeDictionary at(string Key) => (TreeDictionary)base[Key];
        public T at<T>(string Key) => (T)base[Key];

        public new object this[string Key]
        {
            get
            {
                if (!Key.Contains('.'))
                {
                    TryGetValue(Key, out object Value);
                    return Value;
                }
                else
                {
                    var Layers = Key.Split('.');

                    object CurrentLayer = this;
                    for (int i = 0; i < Layers.Length; i++)
                    {
                        if (CurrentLayer is IDictionary<string, object> dictionary) dictionary.TryGetValue(Layers[i], out CurrentLayer);
                        else throw new NullReferenceException($"TreeDictionary does not contain one or more of the SubTrees referenced {{{Key}}}");
                    }

                    return CurrentLayer;
                }
            }
            set
            {
                if (!Key.Contains('.'))
                {
                    base[Key] = value;
                    return;
                }
                else
                {
                    string[] Layers = Key.Split('.');

                    object CurrentLayer = this;
                    for (int i = 0; i < Layers.Length - 1; i++)
                    {
                        if (CurrentLayer is IDictionary<string, object> dictionary) dictionary.TryGetValue(Layers[i], out CurrentLayer);
                        else throw new NullReferenceException($"TreeDictionary does not contain one or more of the SubTrees referenced {{{Key}}}");
                    }
                    ((IDictionary<string, object>)CurrentLayer)[Layers.Last()] = value;
                }
            }
        }
    }
    public class TreeDictionaryJsonConverter : JsonConverter<TreeDictionary>
    {
        public override TreeDictionary Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject) throw new JsonException($"JsonTokenType was of type {reader.TokenType}, only objects are supported");

            var dictionary = new TreeDictionary();
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

        public override void Write(Utf8JsonWriter writer, TreeDictionary value, JsonSerializerOptions options)
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