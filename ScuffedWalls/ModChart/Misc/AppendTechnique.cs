using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ModChart
{
    public static class AppendHelper
    {
        public static IMapObject Append(this IMapObject MapObject, IMapObject AppendObject, AppendTechnique Type)
        {
            MapObject._customData ??= new BeatMap.CustomData();
            MapObject._customData._animation ??= new BeatMap.CustomData.Animation();
            AppendObject._customData ??= new BeatMap.CustomData();
            AppendObject._customData._animation ??= new BeatMap.CustomData.Animation();
            PropertyInfo[] propertiesBaseWall = typeof(IMapObject).GetProperties();
            PropertyInfo[] propertiesCustomData = typeof(BeatMap.CustomData).GetProperties();
            PropertyInfo[] propertiesCustomDataAnimation = typeof(BeatMap.CustomData.Animation).GetProperties();

            if (Type == AppendTechnique.NoOverwrites)
            {
                foreach (PropertyInfo property in propertiesBaseWall)
                {
                    if (property.GetValue(MapObject) == null)
                    {
                        property.SetValue(MapObject, property.GetValue(AppendObject));
                    }
                }
                foreach (PropertyInfo property in propertiesCustomData)
                {
                    if (property.GetValue(MapObject._customData) == null)
                    {
                        property.SetValue(MapObject._customData, property.GetValue(AppendObject._customData));
                    }

                }
                foreach (PropertyInfo property in propertiesCustomDataAnimation)
                {
                    if (property.GetValue(MapObject._customData._animation) == null)
                    {
                        property.SetValue(MapObject._customData._animation, property.GetValue(AppendObject._customData._animation));
                    }

                }
                return MapObject;
            }
            // append technique 1 adds on customdata, overwrites
            else if (Type == AppendTechnique.Overwrites)
            {
                foreach (PropertyInfo property in propertiesBaseWall)
                {
                    if (property.GetValue(AppendObject) != null && property.Name != "_customData")
                    {
                        property.SetValue(MapObject._customData, property.GetValue(AppendObject));
                    }
                }
                foreach (PropertyInfo property in propertiesCustomData)
                {
                    if (property.GetValue(AppendObject._customData) != null && property.Name != "_animation")
                    {
                        property.SetValue(MapObject._customData, property.GetValue(AppendObject._customData));
                    }

                }
                foreach (PropertyInfo property in propertiesCustomDataAnimation)
                {
                    if (property.GetValue(AppendObject._customData._animation) != null)
                    {
                        property.SetValue(MapObject._customData._animation, property.GetValue(AppendObject._customData._animation));

                    }

                }
                return MapObject;
            }
            else if (Type == AppendTechnique.DeleteOldCustomData)
            {
                MapObject._customData = AppendObject._customData;
                return MapObject;
            }
            else if (Type == AppendTechnique.DeleteOldAnimation)
            {
                MapObject._customData._animation = AppendObject._customData._animation;
                return MapObject;
            }
            else
            {
                return AppendObject;
            }
        }
    }
    public enum AppendTechnique
    {
        NoOverwrites,
        Overwrites,
        DeleteOldAnimation,
        DeleteOldCustomData,
        DeleteOldObject
    }

    
}
