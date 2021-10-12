using System.Linq;

namespace ScuffedWalls.Functions
{
    /*
    [SFunction("WorkspaceDefault", "Default")]
    class WorkspaceDefault : ScuffedFunction
    {
        static string[] funcParams = { "Enabled" };
        public override void Run()
        {
            if (!GetParam("Enabled", true, CustomDataParser.BoolConverter)) InstanceWorkspace.Enabled = false;

            var DefaultParams = UnderlyingParameters.Where(para => funcParams.All(funcParam => para.Name.ToLower() != funcParam.ToLower())).ToArray();

            foreach (var funrequest in InstanceWorkspaceRequest.CurrentWorkspaceRequest.FunctionRequests)
            {
                funrequest.UnderlyingParameters
                .AddRange(DefaultParams.CloneArray()
                .Cast<Parameter>());
            }
            foreach (var p in UnderlyingParameters) p.WasUsed = true;
        }
    }
    */
}
