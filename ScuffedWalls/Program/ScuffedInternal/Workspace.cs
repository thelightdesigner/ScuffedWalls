using ModChart;

namespace ScuffedWalls
{
    class Workspace
    {
        public string Name { get; set; }
        public BeatMap.Note[] Notes { get; set; }
        public BeatMap.Event[] Lights { get; set; }
        public BeatMap.Obstacle[] Walls { get; set; }
        public BeatMap.CustomData.CustomEvents[] CustomEvents { get; set; }
        public BeatMap.CustomData.PointDefinition[] PointDefinitions { get; set; }
    }
}
