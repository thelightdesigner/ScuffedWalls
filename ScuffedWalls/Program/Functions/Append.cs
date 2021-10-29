using ModChart;
using System;
using System.Linq;
using static ModChart.BeatMap;
namespace ScuffedWalls.Functions
{

    [SFunction("AppendToAllWallsBetween", "AppendWalls", "AppendWall")]
    class AppendWalls : ScuffedFunction
    {
        public AssignableInlineVariable WallIndex;
        public void SetParameters()
        {
            WallIndex = new AssignableInlineVariable("index", "0");
            Variables.Add(WallIndex);
        }
        public override void Run()
        {
            SetParameters();

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

                FunLog();

                Append(wall, UnderlyingParameters.CustomDataParse(new Obstacle()), type);
                i++;
            }

            ScuffedWalls.Print($"Appended {i} walls from beats {starttime} to {endtime}");
            //  Parameter.ExternalVariables.RefreshAllParameters();
        }
    }
    [SFunction("AppendToAllNotesBetween", "AppendNotes", "AppendNote")]
    class AppendNotes : ScuffedFunction
    {
        public AssignableInlineVariable NoteIndex;
        public void SetParameters()
        {
            NoteIndex = new AssignableInlineVariable("index", "0");
            Variables.Add(NoteIndex);
        }
        public override void Run()
        {
            SetParameters();
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

                FunLog();

                Append(note, UnderlyingParameters.CustomDataParse(new BeatMap.Note()), type);
                i++;
            }

            ScuffedWalls.Print($"Appended {i} notes from beats {starttime} to {endtime}");
        }
    }
    [SFunction("AppendToAllEventsBetween", "AppendLights", "AppendEvent", "AppendEvents")]
    class AppendEvents : ScuffedFunction
    {
        public AssignableInlineVariable EventIndex;
        public void SetParameters()
        {
            EventIndex = new AssignableInlineVariable("index", "0");
            Variables.Add(EventIndex);
        }
        public override void Run()
        {
            SetParameters();
            AppendPriority type = GetParam("appendtechnique", AppendPriority.Low, p => (AppendPriority)int.Parse(p));
            int[] lightypes = GetParam("selecttype", new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 }, p => p.Split(",").Select(a => Convert.ToInt32(a)).ToArray());
            float starttime = Time;
            VariablePopulator internalvars = new VariablePopulator();
            Variables.Register(internalvars.Properties);
            float Rfactor = GetParam("rainbowfactor", 1, p => float.Parse(p));
            float endtime = GetParam("tobeat", float.PositiveInfinity, p => float.Parse(p));
            Parameter select = UnderlyingParameters.FirstOrDefault(p => p.Clean.Name == "select");
            if (select != null) select.WasUsed = true;
            bool selectable() => select == null || bool.Parse(select.StringData);

            int i = 0;
            foreach (var even in InstanceWorkspace.Lights.Where(w => w._time.ToFloat() >= starttime && w._time.ToFloat() <= endtime && selectable()))
            {
                EventIndex.StringData = i.ToString();
                internalvars.UpdateProperties(even);

                FunLog();

                Append(even, UnderlyingParameters.CustomDataParse(new BeatMap.Note()), type);
                i++;
            }

            ScuffedWalls.Print($"Appended {i} events from beats {starttime} to {endtime}");
        }
    }
}
