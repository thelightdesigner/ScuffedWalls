using System.Text.Json;
using ModChart;

namespace ScuffedWalls.Functions
{
    [ScuffedFunction("Environment", "EnvironmentEnhancement")]
    class EnvironmentEnhancement : SFunction
    {
        private Parameter Repeat;
        public override void Run()
        {
            FunLog();

            Repeat = new Parameter("repeat","0");
            Parameter.SetInteralVariables(Parameters, new Parameter[] { Repeat });

            int repeat = GetParam("repeat", 1, p => int.Parse(p));

            for (int i = 0; i < repeat; i++)
            {
                Repeat.StringData = i.ToString();
                InstanceWorkspace.Environment.Add(new TreeDictionary()
                {
                    ["_id"] = GetParam("id", null, p => (object)p),
                    ["_track"] = GetParam("track", null, p => (object)p),
                    ["_lookupMethod"] = GetParam("lookupmethod", null, p => (object)p),
                    ["_duplicate"] = GetParam("duplicate", null, p => (object)int.Parse(p)),
                    ["_active"] = GetParam("active", null, p => (object)bool.Parse(p)),
                    ["_scale"] = GetParam("scale", null, p => JsonSerializer.Deserialize<object[]>(p)),
                    ["_localPosition"] = GetParam("localposition", null, p => JsonSerializer.Deserialize<object[]>(p)),
                    ["_localRotation"] = GetParam("localrotation", null, p => JsonSerializer.Deserialize<object[]>(p)),
                    ["_position"] = GetParam("position", null, p => JsonSerializer.Deserialize<object[]>(p)),
                    ["_rotation"] = GetParam("rotation", null, p => JsonSerializer.Deserialize<object[]>(p)),
                });
                Parameter.RefreshAllParameters();
            }

            ConsoleOut(Name,repeat,0,"Environment Enhancement");
        }
    }
}
