using System;
using System.Reflection;

namespace ModChart.Event
{
    static class Helper
    {
        public static BeatMap.Event EventAppend(this BeatMap.Event CurrentEvent, BeatMap.CustomData CustomData, AppendTechnique Type)
        {
            CurrentEvent._customData ??= new BeatMap.CustomData();
            CustomData ??= new BeatMap.CustomData();
            PropertyInfo[] propertiesCustomData = typeof(BeatMap.CustomData).GetProperties();

            // append technique 0 adds on customdata only if there is no existing customdata
            if (Type == AppendTechnique.NoOverwrites)
            {
                foreach (PropertyInfo property in propertiesCustomData)
                {
                    if (property.GetValue(CurrentEvent._customData) == null) property.SetValue(CurrentEvent._customData, property.GetValue(CustomData));
                }
                return CurrentEvent;
            }
            // append technique 1 adds on customdata, overwrites
            else if (Type == AppendTechnique.Overwrites)
            {
                foreach (PropertyInfo property in propertiesCustomData)
                {
                    if (property.GetValue(CustomData) != null) property.SetValue(CurrentEvent._customData, property.GetValue(CustomData));
                }
                return CurrentEvent;
            }
            // append technique 2 completely replaces the customdata
            else
            {
                CurrentEvent._customData = CustomData;
                return CurrentEvent;
            }

        }
        public static float GetTime(this BeatMap.Event Event)
        {
            return Convert.ToSingle(Event._time.ToString());
        }
        public static int GetEventType(this BeatMap.Event Event)
        {
            return Convert.ToInt32(Event._type.ToString());
        }
        public static int GetValue(this BeatMap.Event Event)
        {
            return Convert.ToInt32(Event._value.ToString());
        }

    }
}
