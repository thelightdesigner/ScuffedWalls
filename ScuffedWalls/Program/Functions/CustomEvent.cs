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

            RegisterChanges("PointDefinition", 1);
        }
    }
    [SFunction("AnimateTrack")]
    class CustomEventAnimateTrack : ScuffedFunction
    {
        public override void Run()
        {
            FunLog();

            InstanceWorkspace.CustomEvents.Add(new TreeDictionary()
            {
                ["_time"] = Time,
                ["_type"] = "AnimateTrack",
                ["_data"] = UnderlyingParameters.CustomEventsDataParse()
            });

            RegisterChanges("AnimateTrack", 1);

        }
    }
    [SFunction("AssignPathAnimation")]
    class CustomEventAssignpath : ScuffedFunction
    {
        public override void Run()
        {
            FunLog();

            InstanceWorkspace.CustomEvents.Add(new TreeDictionary()
            {
                ["_time"] = Time,
                ["_type"] = "AssignPathAnimation",
                ["_data"] = UnderlyingParameters.CustomEventsDataParse()
            });

            RegisterChanges("AssignPathAnimation", 1);
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
            RegisterChanges("AssignPlayerToTrack", 1);
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
            RegisterChanges("AssignTrackParent", 1);
        }
    }



}
