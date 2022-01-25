using System.Linq;

namespace ScuffedWalls.Functions
{
    [SFunction("[NONCALLABLE] [CUSTOMFUNCTIONDHANDLER] Scuffedwalls_v2_infrastructure_CustomFunctionDeclarationParser")]
    class CustomFunction : ScuffedFunction
    {
        CustomFunctionHandler customFunction;
        protected override void Init()
        {
            customFunction = new CustomFunctionHandler(DefiningParameter.Clean.StringData);
        }
        readonly string[] excludes = { "repeat", "repeataddtime" };
        protected override void Update()
        {
            Workspace result = customFunction.GetResult(Time,
                UnderlyingParameters
                .Where(p => !excludes.Any(e => e == p.Clean.Name))
                .Select(p => new VariableRequest(p.Use().Name, p.StringData)),
                true);
            InstanceWorkspace.Add(result);
            Stats.AddStats(result.BeatMap.Stats);
        }
    }
}
