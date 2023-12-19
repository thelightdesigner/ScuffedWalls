using System;
using System.Text.Json;

namespace ModChart
{
    public static class AppendHelper
    {
        /// <summary>
        /// Merges _customData and _time
        /// </summary>
        /// <param name="MapObject"></param>
        /// <param name="AppendObject"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static ICustomDataMapObject Append(this ICustomDataMapObject MapObject, ICustomDataMapObject AppendObject, AppendPriority type)
        {
            switch (type)
            {
                case AppendPriority.Low:
                    foreach (var property in MapObject.GetType().GetProperties()) 
                        if (property.GetValue(MapObject) == null) 
                            property.SetValue(MapObject, property.GetValue(AppendObject));

                    if (AppendObject._customData != null)
                    {
                        MapObject._customData = (TreeDictionary)TreeDictionary.Merge(
                            MapObject._customData, 
                            AppendObject._customData,
                            TreeDictionary.MergeType.Dictionaries | TreeDictionary.MergeType.Objects,
                            TreeDictionary.MergeBindingFlags.HasValue);
                    }
                    return MapObject;
                case AppendPriority.High:
                    return AppendObject.Append(MapObject, AppendPriority.Low);
                case AppendPriority.Complete:
                    MapObject._time = AppendObject._time;
                    MapObject._customData = AppendObject._customData;
                    return MapObject;
                default:
                    return null;
            }

        }
    }
    public enum AppendPriority
    {
        Low,
        High,
        Complete
    }


}
