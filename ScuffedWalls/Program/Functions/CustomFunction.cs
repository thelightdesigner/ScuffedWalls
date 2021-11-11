using System.Linq;

namespace ScuffedWalls.Functions
{
    [SFunction("[NONCALLABLE] [CUSTOMFUNCTIONDHANDLER] Scuffedwalls_v2_infrastructure_CustomFunctionDeclarationParser")]
    class CustomFunction : ScuffedFunction
    {
        ContainerRequest customFunction;
        VariableRequest callTime;
        protected override void Init()
        {
            customFunction = ScuffedRequestParser.Instance.CustomFunctions.FirstOrDefault(func => func.DefiningParameter.Clean.StringData == DefiningParameter.Clean.StringData);
            
        }
        protected override void Update()
        {
            customFunction.ResetDefaultValues(Time);
            callTime = customFunction.VariableRequests.FirstOrDefault(v => v.Name == "calltime");
            if (callTime == null)
            {
                callTime = new VariableRequest("calltime", Time.ToString(), VariableRecomputeSettings.OnCreationOnly, false);
                customFunction.VariableRequests.Add(callTime);
            }
            else
            {
                callTime.Data = Time.ToString();
            }
            foreach (var param in UnderlyingParameters)
            {
                VariableRequest _var = customFunction.VariableRequests.FirstOrDefault(v => v.Public && v.DefiningParameter.Clean.StringData == param.Clean.Name);
                if (_var == null) continue;
                _var.Data = param.Use().StringData;
            }

            Workspace result = new WorkspaceRequestParser(customFunction, hideLogs: true).GetResult();

            InstanceWorkspace.Add(result);
            Stats.AddStats(result.BeatMap.Stats);
        }
    }
}
