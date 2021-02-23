using System;
using System.IO;
using System.Reflection;
using System.Text.Json;

namespace ModChart.Wall
{
    static class Helper
    {
        //creates a wall object
        public static BeatMap.Obstacle WallConstructor(float Time, float Duration, BeatMap.CustomData CustomData)
        {
            return new BeatMap.Obstacle()
            {
                _time = Time,
                _lineIndex = 0,
                _width = 0,
                _type = 0,
                _duration = Duration,
                _customData = CustomData
            };
        }
        public static BeatMap.Obstacle WallConstructor(float Time, float Duration)
        {
            return new BeatMap.Obstacle()
            {
                _time = Time,
                _lineIndex = 0,
                _width = 0,
                _type = 0,
                _duration = Duration
            };
        }
        // read all walls into array
        public static BeatMap.Obstacle[] ReadAllWalls(string MapPull)
        {
            return JsonSerializer.Deserialize<BeatMap>(File.ReadAllText(MapPull))._obstacles;
        }

        public static float GetTime(this BeatMap.Obstacle Wall)
        {
            return Convert.ToSingle(Wall._time.ToString());
        }

        public static BeatMap.Obstacle Append(this BeatMap.Obstacle CurrentWall, BeatMap.CustomData CustomData, AppendTechnique Type)
        {
            CurrentWall._customData ??= new BeatMap.CustomData();
            CurrentWall._customData._animation ??= new BeatMap.CustomData.Animation();
            CustomData ??= new BeatMap.CustomData();
            CustomData._animation ??= new BeatMap.CustomData.Animation();
            PropertyInfo[] propertiesCustomData = typeof(BeatMap.CustomData).GetProperties();
            PropertyInfo[] propertiesCustomDataAnimation = typeof(BeatMap.CustomData.Animation).GetProperties();
           // Console.WriteLine("Before Overwrite" + JsonSerializer.Serialize(CurrentWall, new JsonSerializerOptions { IgnoreNullValues = true }));

            // append technique 0 adds on customdata only if there is no existing customdata
            if (Type == AppendTechnique.NoOverwrites)
            {
                foreach (PropertyInfo property in propertiesCustomData)
                {
                    if (property.GetValue(CurrentWall._customData) == null)
                    {
                        property.SetValue(CurrentWall._customData, property.GetValue(CustomData));
                    }

                }
                foreach (PropertyInfo property in propertiesCustomDataAnimation)
                {
                    if (property.GetValue(CurrentWall._customData._animation) == null)
                    {
                        property.SetValue(CurrentWall._customData._animation, property.GetValue(CustomData._animation));
                    }

                }
                return CurrentWall;
            }
            // append technique 1 adds on customdata, overwrites
            else if (Type == AppendTechnique.Overwrites)
            {

                foreach (PropertyInfo property in propertiesCustomData)
                {
                    if (property.GetValue(CustomData) != null && property.Name != "_animation")
                    {
                        property.SetValue(CurrentWall._customData, property.GetValue(CustomData));
                    }

                }
                foreach (PropertyInfo property in propertiesCustomDataAnimation)
                {
                    if (property.GetValue(CustomData._animation) != null)
                    {
                        property.SetValue(CurrentWall._customData._animation, property.GetValue(CustomData._animation));

                    }

                }
                return CurrentWall;
            }
            else
            {
                CurrentWall._customData = CustomData;
                return CurrentWall;
            }
        }
    }
    
}
