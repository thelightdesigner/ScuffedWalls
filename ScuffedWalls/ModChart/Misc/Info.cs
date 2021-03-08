using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace ModChart
{
    public class Info
    {

        public object _songName { get; set; }
        public object _beatsPerMinute { get; set; }
        public DifficultySet[] _difficultyBeatmapSets { get; set; }
        public class DifficultySet
        {
            public Difficulty[] _difficultyBeatmaps { get; set; }
            public class Difficulty
            {
                public object _beatmapFilename { get; set; }
                public object _noteJumpMovementSpeed { get; set; }
                public object _noteJumpStartBeatOffset { get; set; }
                public customData _customData { get; set; }
                public class customData
                {
                    public object[] _requirements { get; set; } = { };
                    public object[] _suggestions { get; set; } = { };
                }
            }
        }



    }
}
