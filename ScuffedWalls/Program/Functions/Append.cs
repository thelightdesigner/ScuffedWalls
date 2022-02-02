using ModChart;
using System;
using System.Collections.Generic;
using System.Linq;
using static ModChart.BeatMap;
namespace ScuffedWalls.Functions
{
    [SFunction(
        "AppendToAllEventsBetween", "AppendEvents", "AppendEvent", "ForeachEvent",
        "AppendToAllWallsBetween", "AppendWalls", "AppendWall", "ForeachWall",
        "AppendToAllNotesBetween", "AppendNotes", "AppendNote", "ForeachNote",
        "Foreach", "Append")]
    class AppendEvents : ScuffedFunction
    {
        public AssignableInlineVariable EventIndex;
        protected override void Init()
        {
            EventIndex = new AssignableInlineVariable("index", "0");
            Variables.Add(EventIndex);

            AppendPriority type = GetParam("appendtechnique", AppendPriority.Low, p => (AppendPriority)int.Parse(p));
            float starttime = Time;
            VariablePopulator internalvars = new VariablePopulator();
            Variables.Register(internalvars.Properties);

            string name = GetParam("to", null, p => p.ToLower().RemoveWhiteSpace()) ?? DefiningParameter.Clean.StringData;
            MapObjectType AppendObjectType =
                name.Contains("wall") ? MapObjectType.Obstacle :
                name.Contains("note") ? MapObjectType.Note :
                name.Contains("event") ? MapObjectType.Event :
                throw new ArgumentException("Invalid append map object type! (not set?)");

            bool delete = GetParam("delete",false, CustomDataParser.BoolConverter);

            float endtime = GetParam("tobeat", float.PositiveInfinity, p => float.Parse(p));
            string callfun = GetParam("call", null, p => p);

            Parameter select = UnderlyingParameters.Get("select");
            if (select != null) select.WasUsed = true;
            bool selectable() => select == null || bool.Parse(select.StringData);

            CustomFunctionHandler customFunction = GetParam("call", null, p => new CustomFunctionHandler(p));
            Workspace containResult = new Workspace();


            IEnumerable<ICustomDataMapObject> appendObjects =
                AppendObjectType == MapObjectType.Note ? InstanceWorkspace.Notes.Cast<ICustomDataMapObject>() :
                AppendObjectType == MapObjectType.Obstacle ? InstanceWorkspace.Walls.Cast<ICustomDataMapObject>() :
                AppendObjectType == MapObjectType.Event ? InstanceWorkspace.Lights.Cast<ICustomDataMapObject>() :
                throw new Exception();


            var FilteredObjects = appendObjects.Where(x => starttime <= x._time.Value && x._time.Value <= endtime).ToArray();

            int index = 0;
            for (int i = 0; i < FilteredObjects.Length; i++)
            {
                ICustomDataMapObject current = FilteredObjects[i];
                internalvars.UpdateProperties(current);
                if (!selectable()) continue;
                
                EventIndex.StringData = index.ToString();

                if (customFunction != null)
                {
                    containResult.Add(customFunction.GetResult(current.GetTime(), Variables.Select(v => new VariableRequest(v.Name, v.StringData)), false));
                }

                if (!delete) Append(current, UnderlyingParameters.CustomDataParse(GetInstance(AppendObjectType)), type);
                else deleteInstanceWorkspaceItem(current);

                index++;
            }

            void deleteInstanceWorkspaceItem(object mapObject)
            {
                switch (AppendObjectType)
                {
                    case MapObjectType.Obstacle: InstanceWorkspace.Walls.Remove((Obstacle)mapObject);
                            break;
                    case MapObjectType.Event:
                        InstanceWorkspace.Lights.Remove((BeatMap.Event)mapObject);
                        break;
                    case MapObjectType.Note:
                        InstanceWorkspace.Notes.Remove((BeatMap.Note)mapObject);
                        break;

                }
            }

            Stats.AddStats(containResult.BeatMap.Stats);
            InstanceWorkspace.Add(containResult);

            ScuffedWalls.Print($"Modified {index} {AppendObjectType.ToString().MakePlural(FilteredObjects.Count())} from beats {starttime} to {endtime}");
        }

    }
}
