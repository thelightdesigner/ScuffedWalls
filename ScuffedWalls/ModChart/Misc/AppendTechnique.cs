using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ModChart
{
    public static class AppendHelper
    {
        public static ICustomDataMapObject Append(this ICustomDataMapObject MapObject, ICustomDataMapObject AppendObject, AppendTechnique Type)
        {
            //MapObject._customData ??= new BeatMap.CustomData();
            //MapObject._customData._animation ??= new BeatMap.CustomData.Animation();
            //AppendObject._customData ??= new BeatMap.CustomData();
            //AppendObject._customData._animation ??= new BeatMap.CustomData.Animation();
            PropertyInfo[] propertiesBaseWall = typeof(ICustomDataMapObject).GetProperties();
            PropertyInfo[] propertiesCustomData = typeof(BeatMap.CustomData).GetProperties();
            PropertyInfo[] propertiesCustomDataAnimation = typeof(BeatMap.CustomData.Animation).GetProperties();

            if (Type == AppendTechnique.NoOverwrites)
            {
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
            }
            // append technique 1 adds on customdata, overwrites
            else if (Type == AppendTechnique.Overwrites)
            {
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
            }
            else if (Type == AppendTechnique.DeleteOldCustomData)
            {
                MapObject._customData = AppendObject._customData;
                return MapObject;
            }
            else if (Type == AppendTechnique.DeleteOldAnimation)
            {
                MapObject._customData ??= new BeatMap.CustomData();
                if(AppendObject._customData._animation != null) MapObject._customData._animation = AppendObject._customData._animation;
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
