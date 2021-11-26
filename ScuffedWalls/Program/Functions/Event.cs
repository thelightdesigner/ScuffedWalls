using System;
using System.Collections.Generic;
using System.Text;

namespace ScuffedWalls.Functions
{
    [SFunction("Event", "Light")]
    public class Event : ScuffedFunction
    {
        protected override void Update()
        {
            InstanceWorkspace.Lights.Add(new ModChart.BeatMap.Event()
            {
                _time = Time,
                _type = GetParam("type", ModChart.BeatMap.Event.Type.CenterLights, p => (ModChart.BeatMap.Event.Type)int.Parse(p)),
                _value = GetParam("value", ModChart.BeatMap.Event.Value.OnBlue, p => (ModChart.BeatMap.Event.Value)int.Parse(p)),
                _customData = UnderlyingParameters.CustomDataParse(new ModChart.BeatMap.Event())._customData
            });
            RegisterChanges("_event", 1);
        }
    }
}
