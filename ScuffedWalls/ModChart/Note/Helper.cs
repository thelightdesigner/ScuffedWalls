using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace ModChart.Note
{
    static class Helper
    {
        //returns a note
        public static BeatMap.Note NoteConstructor(float Time, int Type, int cutDirection, BeatMap.CustomData CustomData)
        {
            return new BeatMap.Note()
            {
                _time = Time,
                _cutDirection = cutDirection,
                _type = Type,
                _lineLayer = 0,
                _lineIndex = 0,
                _customData = CustomData
            };
        }




        //returns an array of all notes
        public static BeatMap.Note[] ReadAllNotes(string MapPull)
        {
            BeatMap jsonFile = JsonSerializer.Deserialize<BeatMap>(File.ReadAllText(MapPull));
            return jsonFile._notes;
        }

        public static float GetTime(this BeatMap.Note Note)
        {
            return Convert.ToSingle(Note._time.ToString());
        }
        public static float GetType(BeatMap.Note Note)
        {
            return Convert.ToSingle(Note._type.ToString());
        }

        public static BeatMap.Note Append(this BeatMap.Note CurrentNote, BeatMap.CustomData CustomData,  AppendTechnique Type)
        {
            CurrentNote._customData ??= new BeatMap.CustomData();
            CurrentNote._customData._animation ??= new BeatMap.CustomData.Animation();
            CustomData ??= new BeatMap.CustomData();
            CustomData._animation ??= new BeatMap.CustomData.Animation();
            PropertyInfo[] propertiesCustomData = typeof(BeatMap.CustomData).GetProperties();
            PropertyInfo[] propertiesCustomDataAnimation = typeof(BeatMap.CustomData.Animation).GetProperties();

            // append technique 0 adds on customdata only if there is no existing customdata
            if (Type == AppendTechnique.NoOverwrites)
            {
                

                foreach (PropertyInfo property in propertiesCustomData)
                {
                    if (property.GetValue(CurrentNote._customData) == null)
                    {
                        property.SetValue(CurrentNote._customData, property.GetValue(CustomData));
                    }

                }
                foreach (PropertyInfo property in propertiesCustomDataAnimation)
                {
                    if (property.GetValue(CurrentNote._customData._animation) == null)
                    {
                        property.SetValue(CurrentNote._customData._animation, property.GetValue(CustomData._animation));
                    }

                }
                return CurrentNote;
            }
            // append technique 1 adds on customdata, overwrites
            else if (Type == AppendTechnique.Overwrites)
            {

                foreach (PropertyInfo property in propertiesCustomData)
                {
                    if (property.GetValue(CustomData) != null)
                    {
                        property.SetValue(CurrentNote._customData, property.GetValue(CustomData));
                    }

                }
                foreach (PropertyInfo property in propertiesCustomDataAnimation)
                {
                    if (property.GetValue(CustomData._animation) != null)
                    {
                        property.SetValue(CurrentNote._customData._animation, property.GetValue(CustomData._animation));
                    }

                }
                return CurrentNote;
            }
            // append technique 2 completely replaces the customdata
            else
            {
                CurrentNote._customData = CustomData;
                return CurrentNote;
            }

        }
    }
}
