namespace ScuffedWalls.Functions
{
    [SFunction("[NONCALLABLE] [CUSTOMFUNCTIONDHANDLER] Scuffedwalls_v2_infrastructure_CustomFunctionDeclarationParser")]
    class CustomFunction : ScuffedFunction
    {
        public override void Run()
        {
            ContainerRequest customFunction = ScuffedRequestParser.Instance.CustomFunctions.Find(func => func.DefiningParameter.Clean.StringData == DefiningParameter.Clean.StringData);
            customFunction.VariableRequests.Add(new VariableRequest("calltime", Time.ToString(), VariableRecomputeSettings.AllReferences, false));

            foreach (var param in UnderlyingParameters)
            {
                VariableRequest _var = customFunction.VariableRequests.Find(v => v.Public && v.DefiningParameter.Clean.StringData == param.Clean.Name);
                if (_var == null) continue;
                _var.Data = param.Use().Raw.StringData;
            }

            Workspace result = new WorkspaceRequestParser(customFunction, hideLogs: true).GetResult();

            InstanceWorkspace.Add(result);
            Stats.AddStats(result.BeatMap.Stats);
        }
    }
}
