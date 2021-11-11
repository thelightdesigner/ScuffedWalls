using ModChart;
using System;
using System.Linq;
using static ModChart.BeatMap;
namespace ScuffedWalls.Functions
{

    [SFunction("AppendToAllWallsBetween", "AppendWalls", "AppendWall", "ForeachWall")]
    class AppendWalls : ScuffedFunction
    {
        AssignableInlineVariable WallIndex;
        protected override void Init()
        {
            WallIndex = new AssignableInlineVariable("index", "0");
            Variables.Add(WallIndex);

            AppendPriority type = GetParam("appendtechnique", AppendPriority.Low, p => (AppendPriority)int.Parse(p));
            VariablePopulator internalvars = new VariablePopulator();
            Variables.Register(internalvars.Properties);
            float starttime = Time;
            float endtime = GetParam("tobeat", float.PositiveInfinity, p => float.Parse(p));
            string tracc = GetParam("ontrack", null, p => p);
            var lineindex = GetParam("selectlineindex", new int[] { 0, 1, 2, 3, 4 }, p => p.Split(',').Select(val => int.Parse(val)));

            Parameter select = UnderlyingParameters.FirstOrDefault(p => p.Clean.Name == "select");
            if (select != null) select.WasUsed = true;
            bool selectable() => select == null || bool.Parse(select.StringData);

            int i = 0;
            foreach (var wall in InstanceWorkspace.Walls.Where(w => w._time.ToFloat() >= starttime && w._time.ToFloat() <= endtime && selectable()))
            {
                WallIndex.StringData = i.ToString();
                internalvars.UpdateProperties(wall);


                Append(wall, UnderlyingParameters.CustomDataParse(new Obstacle()), type);
                i++;
            }

            ScuffedWalls.Print($"Appended {i} walls from beats {starttime} to {endtime}");
            //  Parameter.ExternalVariables.RefreshAllParameters();
        }
    }
    [SFunction("AppendToAllNotesBetween", "AppendNotes", "AppendNote", "ForeachNote")]
    class AppendNotes : ScuffedFunction
    {
        public AssignableInlineVariable NoteIndex;
        protected override void Init()
        {
            NoteIndex = new AssignableInlineVariable("index", "0");
            Variables.Add(NoteIndex);
            AppendPriority type = GetParam("appendtechnique", AppendPriority.Low, p => (AppendPriority)int.Parse(p));
            VariablePopulator internalvars = new VariablePopulator();
            Variables.Register(internalvars.Properties);

            Parameter select = UnderlyingParameters.FirstOrDefault(p => p.Clean.Name == "select")?.Use();
            bool selectable() => select == null || bool.Parse(select.StringData);

            float starttime = Time;
            float endtime = GetParam("tobeat", float.PositiveInfinity, p => float.Parse(p));
            string tracc = GetParam("ontrack", null, p => p);
            int i = 0;
            foreach (var note in InstanceWorkspace.Notes.Where(w => w._time.ToFloat() >= starttime && w._time.ToFloat() <= endtime && selectable()))
            {
                NoteIndex.StringData = i.ToString();
                internalvars.UpdateProperties(note);


                Append(note, UnderlyingParameters.CustomDataParse(new BeatMap.Note()), type);
                i++;
            }

            ScuffedWalls.Print($"Appended {i} notes from beats {starttime} to {endtime}");
        }
    }
    [SFunction("AppendToAllEventsBetween", "AppendLights", "AppendEvent", "ForeachEvent")]
    class AppendEvents : ScuffedFunction
    {
        public AssignableInlineVariable EventIndex;
        protected override void Init()
        {
            EventIndex = new AssignableInlineVariable("index", "0");
            Variables.Add(EventIndex);
            AppendPriority type = GetParam("appendtechnique", AppendPriority.Low, p => (AppendPriority)int.Parse(p));
            int[] lightypes = GetParam("selecttype", new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 }, p => p.Split(",").Select(a => Convert.ToInt32(a)).ToArray());
            float starttime = Time;
            VariablePopulator internalvars = new VariablePopulator();
            Variables.Register(internalvars.Properties);
            float Rfactor = GetParam("rainbowfactor", 1, p => float.Parse(p));
            float endtime = GetParam("tobeat", float.PositiveInfinity, p => float.Parse(p));
            string callfun = GetParam("call", null, p => p);
            Parameter select = UnderlyingParameters.FirstOrDefault(p => p.Clean.Name == "select");
            if (select != null) select.WasUsed = true;
            bool selectable() => select == null || bool.Parse(select.StringData);

            int i = 0;
            foreach (var even in InstanceWorkspace.Lights.Where(w => w._time.ToFloat() >= starttime && w._time.ToFloat() <= endtime && selectable()))
            {
                EventIndex.StringData = i.ToString();
                internalvars.UpdateProperties(even);

                Append(even, UnderlyingParameters.CustomDataParse(new BeatMap.Note()), type);
                i++;
            }

            ScuffedWalls.Print($"Appended {i} events from beats {starttime} to {endtime}");
        }
    }
}
