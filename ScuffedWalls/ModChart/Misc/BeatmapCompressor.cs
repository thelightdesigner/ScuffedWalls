using ModChart;
using ModChart.Wall;
using ScuffedWalls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;

namespace ModChart
{
    static class BeatmapCompressor
    {
        public static BeatMap SimplifyAllPointDefinitions(this BeatMap Map)
        {

            //simplify custom event point definitions
            if (Map._customData != null && Map._customData._customEvents != null && Map._customData._customEvents.Length > 0)
            {
                
                Map._customData._customEvents = Map._customData._customEvents.Select(mapobj =>
                {
                    try
                    {
                        if (mapobj._data != null) mapobj._data = mapobj._data.SimplifyAllPointDefinitions();
                        return mapobj;
                    }
                    catch (Exception e)
                    {
                        ScuffedLogger.Error.Log($"Error on _customEvent at beat {mapobj._time}");
                        throw e;
                    }
                }).ToArray();
            }



            //simplify wall point definitions
            if (Map._obstacles != null && Map._obstacles.Length > 0)
            {
                Map._obstacles = Map._obstacles.Select(mapobj =>
                {
                    try
                    {
                        if (mapobj._customData != null && mapobj._customData._animation != null) mapobj._customData._animation = mapobj._customData._animation.SimplifyAllPointDefinitions();
                        return mapobj;
                    }
                    catch(Exception e)
                    {
                        ScuffedLogger.Error.Log($"Error on _obstacle at beat {mapobj._time}");
                        throw e;
                    }
                }).ToArray();
            }

            //simplify note point definitions
            if (Map._notes != null && Map._notes.Length > 0)
            {
                Map._notes = Map._notes.Select(mapobj =>
                {
                    try
                    {
                        if (mapobj._customData != null && mapobj._customData._animation != null) mapobj._customData._animation = mapobj._customData._animation.SimplifyAllPointDefinitions();
                        return mapobj;
                    }
                    catch (Exception e)
                    {
                        ScuffedLogger.Error.Log($"Error on _note at beat {mapobj._time}");
                        throw e;
                    }
                }).ToArray();
            }

            return Map;
        }
        public static int GetImportantValuesFromAnimationProperty(this PropertyInfo property)
        {
            string name = property.Name;
            if (name == "_color") return 4;
            else if (name == "_dissolve" || name == "_dissolveArrow") return 1;
            else return 3;
        }
        public static BeatMap.CustomData.Animation SimplifyAllPointDefinitions(this BeatMap.CustomData.Animation animation)
        {
            foreach (var property in typeof(BeatMap.CustomData.Animation).GetProperties().Where(p => new string[] { "_color", "_rotation", "_localRotation", "_position", "_definitePosition", "_dissolve", "_dissolveArrow", "_scale" }.Any(name => name == p.Name)))
            {
                try
                {
                    dynamic customprop = property.GetValue(animation);
                    if ((customprop is JsonElement || customprop != null) && isArray(customprop))
                    {
                        property.SetValue(animation, ((object[][])ToObject<object[][]>(customprop)).SimplifyPointDefinition(property.GetImportantValuesFromAnimationProperty()));
                    }
                }
                catch(Exception e)
                {
                    ScuffedLogger.Error.Log($"Error simplifying point definiton on {property.Name}");
                    throw e;
                }
                
            }
            return animation;
        }
        public static BeatMap.CustomData.CustomEvent.Data SimplifyAllPointDefinitions(this BeatMap.CustomData.CustomEvent.Data animation)
        {
            foreach (var property in typeof(BeatMap.CustomData.CustomEvent.Data).GetProperties().Where(p => new string[] { "_color", "_rotation", "_localRotation", "_position", "_definitePosition", "_dissolve", "_dissolveArrow", "_scale" }.Any(name => name == p.Name)))
            {
                dynamic customprop = property.GetValue(animation);
                if ((customprop is JsonElement || customprop != null) && isArray(customprop))
                {
                    property.SetValue(animation, ((object[][])ToObject<object[][]>(customprop)).SimplifyPointDefinition(property.GetImportantValuesFromAnimationProperty()));
                }
            }
            return animation;
        }

        public static bool EqualsArray(this object[] array1, object[] array2)
        {
            if (array2.Length != array1.Length) return false;


            for (int length = 0; length < array1.Length; length++)
            {
                if (array1[length].toFloat() != array2[length].toFloat()) return false;
            }
            return true;
        }
        public static object[][] SimplifyPointDefinition(this object[][] pointDefinition, int importantvalues)
        {
            List<object[]> CleanedPoints = new List<object[]>();
            for (int i = 0; i < pointDefinition.Length; i++)
            {
                if (i != 0 &&
                i < pointDefinition.Length - 1 &&
                (pointDefinition[i].Slice(0, importantvalues).EqualsArray(pointDefinition[i - 1].Slice(0, importantvalues))) &&
                (pointDefinition[i].Slice(0, importantvalues).EqualsArray(pointDefinition[i + 1].Slice(0, importantvalues)))) continue;
                CleanedPoints.Add(pointDefinition[i]);


            }
            if (CleanedPoints.Last()[importantvalues].toFloat() < 1f)
            {
                var lastPoint = CleanedPoints.Last().DeepClone();
                lastPoint[importantvalues] = 1;
                CleanedPoints.Add(lastPoint);
            }
            else if (CleanedPoints.Last()[importantvalues].toFloat() > 1f)
            {
                ScuffedLogger.Warning.Log($"[Warning] Noodle Extensions point definitions don't end with values higher than 1");

            }
            return CleanedPoints.ToArray();
        }
        public static T ToObject<T>(dynamic element)
        {
            //speed
            if (element is T) return (T)element;
            return JsonSerializer.Deserialize<T>(JsonSerializer.Serialize((object)element));
        }
        public static bool isArray(dynamic array)
        {
            if ((array is JsonElement && ((JsonElement)array).ValueKind.ToString() == "Array") || (array is object[][])) return true;

            return false;
        }
    }
}
