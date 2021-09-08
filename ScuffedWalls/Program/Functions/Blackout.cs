using ModChart;

namespace ScuffedWalls.Functions
{
    [SFunction("Blackout")]
    class Blackout : ScuffedFunction
    {
        public override void Run()
        {
            FunLog();

            ConsoleOut("Light", 1, Time, "Blackout");
            InstanceWorkspace.Lights.Add(new BeatMap.Event() { _time = Time, _type = 0, _value = 0 });
            Parameter.ExternalVariables.RefreshAllParameters();
        }
    }


}
