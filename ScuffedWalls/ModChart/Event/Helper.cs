using System;
using System.Reflection;

namespace ModChart.Event
{
    static class Helper
    {
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
