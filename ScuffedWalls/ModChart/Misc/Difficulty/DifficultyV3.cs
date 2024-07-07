using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ModChart
{
    public class DifficultyV3
    {
        public static DifficultyV3 Parse(string rawDiff) => JsonSerializer.Deserialize<DifficultyV3>(rawDiff, jsonOptions);

        [JsonIgnore]
        public string Json => JsonSerializer.Serialize(this, jsonOptions);

        


        [JsonIgnore]
        private static JsonSerializerOptions jsonOptions
        {
            get
            {
                var jsonOptions = new JsonSerializerOptions();
                jsonOptions.Converters.Add(new SDictionaryJsonConverter());
                jsonOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                return jsonOptions;
            }
        }

        [JsonPropertyName("version")]
        public string Version => "3.2.0";


        [JsonPropertyName("colorNotes"), MapStat]
        public List<ColorNote> ColorNotes { get; private set; } = new();

        public class ColorNote : CustomDataMapObject
        {
            [JsonPropertyName("b")] public float Beat { get; set; }
            [JsonPropertyName("x")] public int X { get; set; }
            [JsonPropertyName("y")] public int Y { get; set; }
            [JsonPropertyName("c")] public ColorType Color { get; set; }
            [JsonPropertyName("d")] public DirectionType Direction { get; set; }
            [JsonPropertyName("a")] public int DirectionOffset { get; set; }

            public ColorNote(float beat, int x, int y, ColorType color, DirectionType direction, int directionOffset)
            {
                Beat = beat;
                X = x;
                Y = y;
                Color = color;
                Direction = direction;
                DirectionOffset = directionOffset;
            }

            public enum ColorType
            {
                RED,
                BLUE
            }

            public enum DirectionType
            {
                UP,
                DOWN,
                LEFT,
                RIGHT,
                UP_LEFT,
                UP_RIGHT,
                DOWN_LEFT,
                DOWN_RIGHT,
                DOT
            }
        }


        [JsonPropertyName("bombNotes"), MapStat]
        public List<BombNote> BombNotes { get; private set; } = new();

        public class BombNote : CustomDataMapObject
        {
            [JsonPropertyName("b")] public float Beat { get; set; }
            [JsonPropertyName("x")] public int X { get; set; }
            [JsonPropertyName("y")] public int Y { get; set; }

            public BombNote(float beat, int x, int y)
            {
                Beat = beat;
                X = x;
                Y = y;
            }
        }


        [JsonPropertyName("obstacles"), MapStat]
        public List<Obstacle> Obstacles { get; private set; } = new();

        public class Obstacle : CustomDataMapObject
        {
            [JsonPropertyName("b")] public float Beat { get; set; }
            [JsonPropertyName("x")] public int X { get; set; }
            [JsonPropertyName("y")] public ObstacleY Y { get; set; }
            [JsonPropertyName("d")] public float Duration { get; set; }
            [JsonPropertyName("w")] public int Width { get; set; }
            [JsonPropertyName("h")] public int Height { get; set; }

            public Obstacle(float beat, int x, ObstacleY y, float duration, int width, int height)
            {
                Beat = beat;
                X = x;
                Y = y;
                Height = height;
                Duration = duration;
                Width = width;
                Height = height;
            }

            public enum ObstacleY
            {
                GROUNDED,
                PRONE,
                CROUCH
            }
        }


        [JsonPropertyName("sliders"), MapStat]
        public List<Slider> Sliders { get; private set; } = new();

        public class Slider : CustomDataMapObject
        {
            [JsonPropertyName("b")] public float HeadBeat { get; set; }
            [JsonPropertyName("c")] public ColorNote.ColorType ColorType { get; set; }
            [JsonPropertyName("x")] public int X { get; set; }
            [JsonPropertyName("y")] public int Y { get; set; }
            [JsonPropertyName("d")] public ColorNote.DirectionType HeadDirection { get; set; }
            [JsonPropertyName("mu")] public float HeadMultiplier { get; set; }
            [JsonPropertyName("tb")] public float TailBeat { get; set; }
            [JsonPropertyName("tx")] public int TailX { get; set; }
            [JsonPropertyName("ty")] public int TailY { get; set; }
            [JsonPropertyName("tc")] public ColorNote.DirectionType TailDirection { get; set; }
            [JsonPropertyName("tmu")] public float TailMultiplier { get; set; }
            [JsonPropertyName("m")] public MidAnchorType MidAnchorMode { get; set; }

            public Slider(float headBeat, ColorNote.ColorType color, int x, int y, ColorNote.DirectionType headDirection,
                float headMultiplier, float tailBeat, int tailX, int tailY, ColorNote.DirectionType tailDirection, float tailMultiplier,
                MidAnchorType midAnchorMode)
            {
                HeadBeat = headBeat;
                ColorType = color;
                X = x;
                Y = y;
                HeadDirection = headDirection;
                HeadMultiplier = headMultiplier;
                TailBeat = tailBeat;
                TailX = tailX;
                TailY = tailY;
                TailDirection = tailDirection;
                TailMultiplier = tailMultiplier;
                MidAnchorMode = midAnchorMode;
            }

            public enum MidAnchorType
            {
                STRAIGHT,
                CLOCKWISE,
                COUNTERCLOCKWISE
            }

        }


        [JsonPropertyName("burstSliders"), MapStat]
        public List<BurstSlider> BurstSliders { get; private set; } = new();

        public class BurstSlider : CustomDataMapObject
        {
            [JsonPropertyName("b")] public float HeadBeat { get; set; }
            [JsonPropertyName("c")] public ColorNote.ColorType Color { get; set; }
            [JsonPropertyName("x")] public int X { get; set; }
            [JsonPropertyName("y")] public int Y { get; set; }
            [JsonPropertyName("d")] public ColorNote.DirectionType HeadDirection { get; set; }
            [JsonPropertyName("tb")] public float TailBeat { get; set; }
            [JsonPropertyName("tx")] public int TailX { get; set; }
            [JsonPropertyName("ty")] public int TailY { get; set; }
            [JsonPropertyName("sc")] public int SegmentCount { get; set; }
            [JsonPropertyName("s")] public float SquishFactor { get; set; }

            public BurstSlider(float headBeat, ColorNote.ColorType color, int x, int y, ColorNote.DirectionType headDirection, float tailBeat,
                int tailX, int tailY, int segmentCount, float squishFactor)
            {
                HeadBeat = headBeat;
                Color = color;
                X = x;
                Y = y;
                HeadDirection = headDirection;
                TailBeat = tailBeat;
                TailX = tailX;
                TailY = tailY;
                SegmentCount = segmentCount;
                SquishFactor = squishFactor;
            }
        }


        [JsonPropertyName("waypoints")]
        public List<SDictionary> Waypoints { get; private set; } = new();


        [JsonPropertyName("basicBeatmapEvents"), MapStat]
        public List<BasicBeatmapEvent> BasicBeatmapEvents { get; private set; } = new();

        public class BasicBeatmapEvent
        {
            [JsonPropertyName("b")] public float Beat { get; set; }
            [JsonPropertyName("et")] public EventType Type { get; set; }
            [JsonPropertyName("i")] public EventValue Value { get; set; }
            [JsonPropertyName("f")] public float FloatValue { get; set; }

            public enum EventValue
            {
                OFF,
                ON_BLUE,
                FLASH_BLUE,
                FADE_BLUE,
                TRANSITION_TO_BLUE,
                ON_RED,
                FLASH_RED,
                FADE_RED,
                TRANSITION_TO_RED,
                ON_WHITE,
                FLASH_WHITE,
                FADE_WHITE,
                TRANSITION_TO_WHITE,
            }

            public enum EventType
            {
                BACK_LASERS,
                RING_LIGHTS,
                LEFT_LASERS,
                RIGHT_LASERS,
                CENTER_LIGHTS,
                RING_SPIN = 8,
                RING_ZOOM,
                EXTRA_LIGHTS,
                LEFT_LASER_SPEED,
                RIGHT_LASER_SPEED,
                EARLY_ROTATION,
                LATE_ROTATION,
                V2_BPM_CHANGE = 100
            }
        }


        [JsonPropertyName("colorBoostBeatmapEvents")]
        public List<ColorBoostBeatmapEvent> ColorBoostBeatmapEvents { get; private set; } = new();

        public class ColorBoostBeatmapEvent
        {
            [JsonPropertyName("b")] public float Beat { get; set; }
            [JsonPropertyName("o")] public bool BoostLightingEnabled { get; set; }

            public ColorBoostBeatmapEvent(float beat, bool boostLightingEnabled)
            {
                Beat = beat;
                BoostLightingEnabled = boostLightingEnabled;
            }
        }


        [JsonPropertyName("lightColorEventBoxGroups")]
        public List<LightColorEventBoxGroup> LightColorEventBoxGroups { get; private set; } = new();

        public class LightColorEventBoxGroup
        {
            [JsonPropertyName("b")] public float Beat { get; set; }
            [JsonPropertyName("g")] public int Group { get; set; }
            [JsonPropertyName("e")] public List<LightColorEventLane> LightColorEventLanes { get; set; }

            public LightColorEventBoxGroup(float beat, int group, List<LightColorEventLane> lightColorEventLanes)
            {
                Beat = beat;
                Group = group;
                LightColorEventLanes = lightColorEventLanes;
            }

            public class LightColorEventLane
            {
                [JsonPropertyName("f")] public Filter FilterObject { get; set; }
                [JsonPropertyName("w")] public float BeatDistribution { get; set; }
                [JsonPropertyName("d")] public DistributionType BeatDistributionType { get; set; }
                [JsonPropertyName("r")] public float BrightnessDistribution { get; set; }
                [JsonPropertyName("t")] public DistributionType BrightnessDistributionType { get; set; }
                [JsonPropertyName("b")] public int BrightnessDistributionAffectsFirst { get; set; }
                [JsonPropertyName("i")] public Easing BrightnessDistributionEasing { get; set; }
                [JsonPropertyName("e")] public List<SDictionary> Events { get; set; }

                public class EventData
                {
                    [JsonPropertyName("b")] public float Beat { get; set; }
                    [JsonPropertyName("i")] public TransitionType Transition { get; set; }
                    [JsonPropertyName("c")] public EventColor Color { get; set; }
                    [JsonPropertyName("s")] public float Brightness { get; set; }
                    [JsonPropertyName("f")] public int FlickerFrequency { get; set; }

                    public enum EventColor
                    {
                        RED,
                        BLUE,
                        WHITE
                    }
                    public enum TransitionType
                    {
                        INSTANT,
                        TRANSITION,
                        EXTEND
                    }
                }

                public enum Easing
                {
                    LINEAR,
                    EASEINQUAD,
                    EASEOUTQUAD,
                    EASEINOUTQUAD
                }

                public enum DistributionType
                {
                    WAVE = 1,
                    STEP
                }
            }
        }
        [JsonPropertyName("bpmEvents")]
        public List<BpmEvent> BpmEvents { get; private set; } = new();

        public class BpmEvent
        {
            [JsonPropertyName("b")] public float Beat { get; set; }
            [JsonPropertyName("m")] public float NewBpm { get; set; }

            public BpmEvent(float beat, float newBpm)
            {
                Beat = beat;
                NewBpm = newBpm;
            }
        }


        [JsonPropertyName("rotationEvents")]
        public List<RotationEvent> RotationEvents { get; private set; } = new();

        public class RotationEvent
        {
            [JsonPropertyName("b")] public float Beat { get; set; }
            [JsonPropertyName("e")] public EventType Event { get; set; }
            [JsonPropertyName("r")] public float Rotation { get; set; }

            public RotationEvent(float beat, EventType _event, float rotation)
            {
                Beat = beat;
                Event = _event;
                Rotation = rotation;
            }

            public enum EventType
            {
                EARLY_ROTATION,
                LATE_ROTATION
            }
        }

        [JsonPropertyName("lightRotationEventBoxGroups")]
        public List<LightRotationEventBoxGroup> LightRotationEventBoxGroups { get; private set; } = new();

        public class LightRotationEventBoxGroup
        {
            [JsonPropertyName("b")] public float Beat { get; set; }
            [JsonPropertyName("g")] public int Group { get; set; }
            [JsonPropertyName("e")] public List<LightRotationEventLane> LightRotationEventLanes { get; set; }

            public LightRotationEventBoxGroup(float beat, int group, List<LightRotationEventLane> lightRotationEventLanes)
            {
                Beat = beat;
                Group = group;
                LightRotationEventLanes = lightRotationEventLanes;
            }

            public class LightRotationEventLane
            {
                [JsonPropertyName("f")] public Filter FilterObject { get; set; }
                [JsonPropertyName("w")] public float BeatDistribution { get; set; }
                [JsonPropertyName("d")] public DistributionType BeatDistributionType { get; set; }
                [JsonPropertyName("s")] public float RotationDistribution { get; set; }
                [JsonPropertyName("t")] public DistributionType RotationDistributionType { get; set; }
                [JsonPropertyName("b")] public int RotationDistributionAffectsFirst { get; set; }
                [JsonPropertyName("i")] public Easing RotationDistributionEasing { get; set; }
                [JsonPropertyName("a")] public int Axis { get; set; }
                [JsonPropertyName("r")] public int Reverse { get; set; }
                [JsonPropertyName("l")] public List<EventData> Events { get; set; }

                public class EventData
                {
                    [JsonPropertyName("b")] public float Beat { get; set; }
                    [JsonPropertyName("p")] public TransitionType Transition { get; set; }
                    [JsonPropertyName("e")] public Easing Easing { get; set; }
                    [JsonPropertyName("l")] public int AdditionalLoops { get; set; }
                    [JsonPropertyName("r")] public float Rotation { get; set; }
                    [JsonPropertyName("o")] public RotationDirection Direction { get; set; }

                    public enum TransitionType
                    {
                        TRANSITION,
                        EXTEND
                    }
                    public enum RotationDirection
                    {
                        AUTOMATIC,
                        CLOCKWISE,
                        COUNTERCLOCKWISE
                    }
                }

                public enum Easing
                {
                    LINEAR,
                    EASEINQUAD,
                    EASEOUTQUAD,
                    EASEINOUTQUAD,
                    NONE = -1
                }

                public enum DistributionType
                {
                    WAVE = 1,
                    STEP
                }
            }
        }


        [JsonPropertyName("lightTranslationEventBoxGroups")]
        public List<LightTranslationEventBoxGroup> LightTranslationEventBoxGroups { get; private set; } = new();

        public class LightTranslationEventBoxGroup
        {
            [JsonPropertyName("b")] public float Beat { get; set; }
            [JsonPropertyName("g")] public int Group { get; set; }
            [JsonPropertyName("e")] public List<LightTranslationEventLane> LightTranslationEventLanes { get; set; }

            public LightTranslationEventBoxGroup(float beat, int group, List<LightTranslationEventLane> lightTranslationEventLanes)
            {
                Beat = beat;
                Group = group;
                LightTranslationEventLanes = lightTranslationEventLanes;
            }

            public class LightTranslationEventLane
            {
                [JsonPropertyName("f")] public Filter FilterObject { get; set; }
                [JsonPropertyName("w")] public float BeatDistribution { get; set; }
                [JsonPropertyName("d")] public DistributionType BeatDistributionType { get; set; }
                [JsonPropertyName("s")] public float RotationDistribution { get; set; }
                [JsonPropertyName("t")] public DistributionType RotationDistributionType { get; set; }
                [JsonPropertyName("b")] public int RotationDistributionAffectsFirst { get; set; }
                [JsonPropertyName("i")] public Easing RotationDistributionEasing { get; set; }
                [JsonPropertyName("a")] public int Axis { get; set; }
                [JsonPropertyName("r")] public int Reverse { get; set; }
                [JsonPropertyName("l")] public List<EventData> Events { get; set; }

                public class EventData
                {
                    [JsonPropertyName("b")] public float Beat { get; set; }
                    [JsonPropertyName("p")] public TransitionType Transition { get; set; }
                    [JsonPropertyName("e")] public Easing Easing { get; set; }
                    [JsonPropertyName("t")] public float Value { get; set; }

                    public enum TransitionType
                    {
                        TRANSITION,
                        EXTEND
                    }
                    public enum RotationDirection
                    {
                        AUTOMATIC,
                        CLOCKWISE,
                        COUNTERCLOCKWISE
                    }
                }

                public enum Easing
                {
                    LINEAR,
                    EASEINQUAD,
                    EASEOUTQUAD,
                    EASEINOUTQUAD,
                    NONE = -1
                }

                public enum DistributionType
                {
                    WAVE = 1,
                    STEP
                }
            }
        }

        public class Filter
        {
            [JsonPropertyName("c")] public int Chunks { get; set; }
            [JsonPropertyName("f")] public FilterType Type { get; set; }
            [JsonPropertyName("p")] public int Parameter0 { get; set; }
            [JsonPropertyName("t")] public int Parameter1 { get; set; }
            [JsonPropertyName("r")] public int Reverse { get; set; }
            [JsonPropertyName("n")] public RandomBehaviorType RandomBehavior { get; set; }
            [JsonPropertyName("s")] public int RandomSeed { get; set; }
            [JsonPropertyName("l")] public float LimitPercentage { get; set; }
            [JsonPropertyName("d")] public LimitBehaviorType LimitBehavior { get; set; }

            public enum FilterType
            {
                SECTIONS,
                STEP_OFFSET
            }
            public enum RandomBehaviorType
            {
                STANDARD_ORDER1,
                STANDARD_ORDER2,
                RANDOM_ORDER,
                RANDOM_START_INDEX
            }
            public enum LimitBehaviorType
            {
                SECTIONS,
                SECTIONS_AND_DURATIONS,
                SECTIONS_AND_BRIGHTNESSDISTRIBUTION,
                ALL
            }
        }


        [JsonPropertyName("basicEventTypesWithKeywords")]
        public SDictionary BasicEventTypesWithKeywords { get; private set; } = new();


        [JsonPropertyName("useNormalEventsAsCompatibleEvents")]
        public bool UseNormalEventsAsCompatibleEvents { get; private set; }


        [JsonPropertyName("customData"), MapStat]
        public SDictionary CustomData { get; private set; } = new();



        /*  public string Json
          {
              get
              {
                  JsonSerializerOptions options = new JsonSerializerOptions();
                  options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                  options.Converters.Add(new SDictionaryJsonConverter());
                  return JsonSerializer.Serialize(this, options);
              }
          }
          public string PrettyJson
          {
              get
              {
                  JsonSerializerOptions options = new JsonSerializerOptions();
                  options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                  options.WriteIndented = true;
                  options.Converters.Add(new SDictionaryJsonConverter());
                  return JsonSerializer.Serialize(this, options);
              }
          }*/

        public class CustomDataMapObject
        {
            [JsonPropertyName("customData"), JsonConverter(typeof(SDictionaryJsonConverter))] public SDictionary CustomData { get; set; }
        }
        public class MapStatAttribute : Attribute
        {

        }
    }

    
}
