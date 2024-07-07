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
            InstanceWorkspace.Lights.Add(new ModChart.DifficultyV2.Event()
            {
                _time = Time,
                _type = GetParam("type", ModChart.DifficultyV2.Event.Type.CenterLights, p => (ModChart.DifficultyV2.Event.Type)int.Parse(p)),
                _value = GetParam("value", ModChart.DifficultyV2.Event.Value.OnBlue, p => (ModChart.DifficultyV2.Event.Value)int.Parse(p)),
                _customData = UnderlyingParameters.CustomDataParse(new ModChart.DifficultyV2.Event())._customData
            });
            RegisterChanges("_event", 1);
        }
    }
}
