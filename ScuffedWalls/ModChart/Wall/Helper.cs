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

        
        
    }
    


}
