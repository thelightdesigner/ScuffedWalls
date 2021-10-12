using ModChart;
using System.Text.Json;

namespace ScuffedWalls.Functions
{
    [SFunction("PointDefinition")] 
    class PointDefinition : ScuffedFunction
    {
        public override void Run()
        {
            FunLog();


            string name = GetParam("name", "unimplemented_pointdefinition", p => p);
            object[][] points = GetParam("points", null, p => JsonSerializer.Deserialize<object[][]>($"[{p}]"));
            
            InstanceWorkspace.PointDefinitions.Add(new TreeDictionary()
            {
                ["_name"] = name,
                ["_points"] = points
            });

            ConsoleOut("PointDefinition", 1, Time, "PointDefinition");

            Parameter.ExternalVariables.RefreshAllParameters();
        }
    }
    [SFunction("AnimateTrack")]
    class CustomEventAnimateTrack : ScuffedFunction
    {
        public Parameter Repeat;
        public Parameter Beat;
        public void SetParameters()
        {
            Repeat = new Parameter("repeat", "0");
            Beat = new Parameter("time", Time.ToString());
            UnderlyingParameters.SetInteralVariables(new Parameter[] { Repeat, Beat });
        }
        public override void Run()
        {
            SetParameters();
            int repeatcount = GetParam("repeat", 1, p => int.Parse(p));
            float repeatTime = GetParam("repeataddtime", 0, p => float.Parse(p));
            for (float i = 0; i < repeatcount; i++)
            {
                Repeat.StringData = i.ToString();
                Beat.StringData = (Time + (i * repeatTime)).ToString();

                FunLog();


                InstanceWorkspace.CustomEvents.Add(new TreeDictionary()
                {
                    ["_time"] = Time + (i * repeatTime),
                    ["_type"] = "AnimateTrack",
                    ["_data"] = UnderlyingParameters.CustomEventsDataParse()
                });
                Parameter.ExternalVariables.RefreshAllParameters();
            }
            ConsoleOut("AnimateTrack", repeatcount, Time, "CustomEvent");

        }
    }
    [SFunction("AssignPathAnimation")]
    class CustomEventAssignpath : ScuffedFunction
    {
        public Parameter Repeat;
        public Parameter Beat;
        public void SetParameters()
        {
            Repeat = new Parameter("repeat", "0");
            Beat = new Parameter("time", Time.ToString());
            UnderlyingParameters.SetInteralVariables(new Parameter[] { Repeat, Beat });
        }
        public override void Run()
        {
            SetParameters();
            int repeatcount = GetParam("repeat", 1, p => int.Parse(p));
            float repeatTime = GetParam("repeataddtime", 0, p => float.Parse(p));
            for (float i = 0; i < repeatcount; i++)
            {
                Repeat.StringData = i.ToString();
                Beat.StringData = (Time + (i * repeatTime)).ToString();

                FunLog();


                InstanceWorkspace.CustomEvents.Add(new TreeDictionary()
                {
                    ["_time"] = Time + (i * repeatTime),
                    ["_type"] = "AssignPathAnimation",
                    ["_data"] = UnderlyingParameters.CustomEventsDataParse()
                });
                Parameter.ExternalVariables.RefreshAllParameters();
            }
            ConsoleOut("AssignPathAnimation", repeatcount, Time, "CustomEvent");
        }
    }
    [SFunction("AssignPlayerToTrack")]
    public class CustomEventPlayerTrack : ScuffedFunction
    {
        public override void Run()
        {
            FunLog();


            InstanceWorkspace.CustomEvents.Add(new TreeDictionary()
            {
                ["_time"] = Time,
                ["_type"] = "AssignPlayerToTrack",
                ["_data"] = UnderlyingParameters.CustomEventsDataParse()
            });
            ConsoleOut("AssignPlayerToTrack", 1, Time, "CustomEvent");
            Parameter.ExternalVariables.RefreshAllParameters();
        }
    }
    
    [SFunction("ParentTrack")]
    public class CustomEventParent : ScuffedFunction
    {
        public override void Run()
        {
            FunLog();


            InstanceWorkspace.CustomEvents.Add(new TreeDictionary()
            {
                ["_time"] = Time,
                ["_type"] = "AssignTrackParent",
                ["_data"] = UnderlyingParameters.CustomEventsDataParse()
            });
            ConsoleOut("AssignTrackParent", 1, Time, "CustomEvent");
            Parameter.ExternalVariables.RefreshAllParameters();
        }
    }


    
}
