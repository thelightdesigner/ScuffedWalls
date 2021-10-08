using ModChart;
using System.Collections.Generic;
using System.Linq;
using static ScuffedWalls.ScuffedRequest;

namespace ScuffedWalls
{

    public class WorkspaceRequestParser : IRequestParser<ContainerRequest, Workspace>
    {
        public ContainerRequest CurrentRequest => _request;
        public Workspace Result => _latestWorkspaceResult;

        private ContainerRequest _request;
        private Workspace _latestWorkspaceResult;
        public WorkspaceRequestParser(ContainerRequest containerrequest)
        {
            _request = containerrequest;
        }
        private IEnumerator<VariableRequest> _variableRequestEnumerator;
        private IEnumerator<FunctionRequest> _functionRequestEnumerator;
        public Workspace GetResult()
        {
            Lookup<AssignableInlineVariable> globalvariables = new Lookup<AssignableInlineVariable>(AssignableInlineVariable.Exposer);
            Workspace workspace = new Workspace(BeatMap.Empty, _request.Name);
            _variableRequestEnumerator = _request.VariableRequests.GetEnumerator();
            _functionRequestEnumerator = _request.FunctionRequests.GetEnumerator();

            while (_variableRequestEnumerator.MoveNext())
            {
                var varreq = _variableRequestEnumerator.Current;

                Debug.TryAction(() =>
                {
                   // Parameter.ExternalVariables = globalvariables.ToArray();

                    AssignableInlineVariable variable = new AssignableInlineVariable(
                        varreq.Name,
                        varreq.UnderlyingParameters.Get("data").Raw.StringData,
                        ScuffedRequestParser.GetParam("recompute", VariableRecomputeSettings.OnCreationOnly, p => (VariableRecomputeSettings)int.Parse(p), varreq.UnderlyingParameters));

                    globalvariables.Add(variable);

                    ScuffedWalls.Print($"Added Variable \"{variable.Name}\" Val:{variable.StringData}");
                }, e =>
                {
                    ScuffedWalls.Print($"Error adding global variable {varreq.Name} ERROR:{e.Message} ", ScuffedWalls.LogSeverity.Error);
                });

            }

            while (_functionRequestEnumerator.MoveNext())
            {
                new FunctionRequestParser(_functionRequestEnumerator.Current, workspace).GetResult();
            }
            return workspace;
        }
    }
}
