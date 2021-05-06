using ModChart;
using System;
using System.Text.Json;

namespace ScuffedWalls.Functions
{
    [ScuffedFunction("PointDefinition")] 
    class PointDefinition : SFunction
    {
        public override void Run()
        {
            string name = GetParam("name", "unimplemented_pointdefinition", p => p);
            object[][] points = GetParam("points", null, p => JsonSerializer.Deserialize<object[][]>($"[{p}]"));
            
            InstanceWorkspace.PointDefinitions.Add(new BeatMap.CustomData.PointDefinition()
            {
                _name = name,
                _points = points
            });

            ConsoleOut("PointDefinition", 1, Time, "PointDefinition");

            Parameter.ExternalVariables.RefreshAllParameters();
        }
    }
    [ScuffedFunction("AnimateTrack")]
    class CustomEventAnimateTrack : SFunction
    {
        public Parameter Repeat;
        public Parameter Beat;
        public void SetParameters()
        {
            Repeat = new Parameter("repeat", "1");
            Beat = new Parameter("time", Time.ToString());
            Parameters.SetInteralVariables(new Parameter[] { Repeat, Beat });
        }
        public override void Run()
        {
            SetParameters();
            int repeatcount = GetParam("repeat", 1, p => int.Parse(p));
            float repeatTime = GetParam("repeataddtime", 0, p => float.Parse(p));
            for (float i = 0; i < repeatcount; i++)
            {
                InstanceWorkspace.CustomEvents.Add(new BeatMap.CustomData.CustomEvent()
                {
                    _time = Time + (i * repeatTime),
                    _type = "AnimateTrack",
                    _data = Parameters.CustomEventsDataParse()
                });
                Repeat.StringData = i.ToString();
                Beat.StringData = (Time + (i * repeatTime)).ToString();
                Parameter.ExternalVariables.RefreshAllParameters();
            }
            ConsoleOut("AnimateTrack", repeatcount, Time, "CustomEvent");

        }
    }
    [ScuffedFunction("AssignPathAnimation")]
    class CustomEventAssignpath : SFunction
    {
        public Parameter Repeat;
        public Parameter Beat;
        public void SetParameters()
        {
            Repeat = new Parameter("repeat", "1");
            Beat = new Parameter("time", Time.ToString());
            Parameters.SetInteralVariables(new Parameter[] { Repeat, Beat });
        }
        public override void Run()
        {
            SetParameters();
            int repeatcount = GetParam("repeat", 1, p => int.Parse(p));
            float repeatTime = GetParam("repeataddtime", 0, p => float.Parse(p));
            for (float i = 0; i < repeatcount; i++)
            {
                InstanceWorkspace.CustomEvents.Add(new BeatMap.CustomData.CustomEvent()
                {
                    _time = Time + (i * repeatTime),
                    _type = "AssignPathAnimation",
                    _data = Parameters.CustomEventsDataParse()
                });
                Repeat.StringData = i.ToString();
                Beat.StringData = (Time + (i * repeatTime)).ToString();
                Parameter.ExternalVariables.RefreshAllParameters();
            }
            ConsoleOut("AssignPathAnimation", repeatcount, Time, "CustomEvent");
        }
    }
    [ScuffedFunction("AssignPlayerToTrack")]
    public class CustomEventPlayerTrack : SFunction
    {
        public override void Run()
        {
            InstanceWorkspace.CustomEvents.Add(new BeatMap.CustomData.CustomEvent()
            {
                _time = Time,
                _type = "AssignPlayerToTrack",
                _data = Parameters.CustomEventsDataParse()
            });
            ConsoleOut("AssignPlayerToTrack", 1, Time, "CustomEvent");
            Parameter.ExternalVariables.RefreshAllParameters();
        }
    }
    
    [ScuffedFunction("ParentTrack")]
    public class CustomEventParent : SFunction
    {
        public override void Run()
        {
            InstanceWorkspace.CustomEvents.Add(new BeatMap.CustomData.CustomEvent()
            {
                _time = Time,
                _type = "AssignTrackParent",
                _data = Parameters.CustomEventsDataParse()
            });
            ConsoleOut("AssignTrackParent", 1, Time, "CustomEvent");
            Parameter.ExternalVariables.RefreshAllParameters();
        }
    }


    
}
