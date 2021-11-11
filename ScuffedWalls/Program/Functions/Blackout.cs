using ModChart;

namespace ScuffedWalls.Functions
{
    [SFunction("Blackout")]
    class Blackout : ScuffedFunction
    {
        protected override void Update()
        {
            RegisterChanges("Light", 1);
            InstanceWorkspace.Lights.Add(new BeatMap.Event() { _time = Time, _type = 0, _value = 0 });
        }
    }
}
