using ModChart;
using System.Collections.Generic;
using System.Linq;

namespace ScuffedWalls
{

    public class WorkspaceRequestParser : IRequestParser<ContainerRequest, Workspace>
    { 
        public void RefreshCurrentParameters()
        {
            foreach (var param in CurrentRequest.Parameters) param.RefreshVariables();
        }
        public static WorkspaceRequestParser Instance { get; private set; }
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
            TreeList<AssignableInlineVariable> globalvariables = new TreeList<AssignableInlineVariable>(AssignableInlineVariable.Exposer);
            foreach (var param in _request.Parameters) param.Variables.Register(globalvariables);

            Workspace workspace = new Workspace(BeatMap.Empty, _request.Name);

            _variableRequestEnumerator = _request.VariableRequests.GetEnumerator();
            _functionRequestEnumerator = _request.FunctionRequests.GetEnumerator();

            while (_variableRequestEnumerator.MoveNext())
            {
                var result = new VariableRequestParser(_variableRequestEnumerator.Current).GetResult();
                if (result != null) globalvariables.Add(result);
            }

           // Parameter.AssignVariables(_request.Parameters, globalvariables);

            while (_functionRequestEnumerator.MoveNext())
            {
                new FunctionRequestParser(_functionRequestEnumerator.Current, workspace).GetResult();
            }
            return workspace;
        }
    }
}
