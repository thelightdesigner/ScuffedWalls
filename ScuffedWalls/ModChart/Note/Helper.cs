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


        
    }
}
