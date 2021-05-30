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
                    foreach (var property in MapObject.GetType().GetProperties()) if (property.GetValue(MapObject) == null) property.SetValue(MapObject, property.GetValue(AppendObject));
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

            #region old
            /*
            if (type == AppendTechnique.NoOverwrites)
            {

                #region old
                /*
                if (MapObject != null && AppendObject != null)
                {
                    foreach (PropertyInfo property in propertiesBaseWall)
                    {
                        if (property.GetValue(MapObject) == null)
                        {
                            property.SetValue(MapObject, property.GetValue(AppendObject));
                        }
                    }
                    if (AppendObject._customData != null)
                    {
                        MapObject._customData ??= new BeatMap.CustomData();
                        foreach (PropertyInfo property in propertiesCustomData)
                        {
                            if (property.GetValue(MapObject._customData) == null)
                            {
                                property.SetValue(MapObject._customData, property.GetValue(AppendObject._customData));
                            }

                        }
                        if (AppendObject._customData._animation != null)
                        {
                            MapObject._customData._animation ??= new BeatMap.CustomData.Animation();
                            foreach (PropertyInfo property in propertiesCustomDataAnimation)
                            {
                                if (property.GetValue(MapObject._customData._animation) == null)
                                {
                                    property.SetValue(MapObject._customData._animation, property.GetValue(AppendObject._customData._animation));
                                }
                            }
                        }
                    }
                }
                return MapObject;
                
                #endregion
            }
            // append technique 1 adds on customdata, overwrites
            else if (type == AppendTechnique.Overwrites)
            {

                #region old
                /*
                if (MapObject != null && AppendObject != null)
                {
                    foreach (PropertyInfo property in propertiesBaseWall)
                    {
                        if (property.GetValue(AppendObject) != null && property.Name != "_customData")
                        {
                            property.SetValue(MapObject._customData, property.GetValue(AppendObject));
                        }
                    }
                    if (AppendObject._customData != null)
                    {
                        MapObject._customData ??= new BeatMap.CustomData();
                        foreach (PropertyInfo property in propertiesCustomData)
                        {
                            if (property.GetValue(AppendObject._customData) != null && property.Name != "_animation")
                            {
                                property.SetValue(MapObject._customData, property.GetValue(AppendObject._customData));
                            }

                        }
                        if (AppendObject._customData._animation != null)
                        {
                            MapObject._customData._animation ??= new BeatMap.CustomData.Animation();
                            foreach (PropertyInfo property in propertiesCustomDataAnimation)
                            {
                                if (property.GetValue(AppendObject._customData._animation) != null)
                                {
                                    property.SetValue(MapObject._customData._animation, property.GetValue(AppendObject._customData._animation));

                                }

                            }
                        }
                    }
                }
                
                
                return MapObject;
                
                #endregion
            }
            else if (type == AppendTechnique.DeleteOldCustomData)
            {
                MapObject._customData = AppendObject._customData;
                return MapObject;
            }
            else if (type == AppendTechnique.DeleteOldAnimation)
            {
                MapObject._customData ??= new BeatMap.CustomData();
                if (AppendObject._customData._animation != null) MapObject._customData._animation = AppendObject._customData._animation;
                return MapObject;
            }
            else
            {
                return AppendObject;
            }
            */
            #endregion
        }
    }
    public enum AppendPriority
    {
        Low,
        High,
        Complete
    }


}
