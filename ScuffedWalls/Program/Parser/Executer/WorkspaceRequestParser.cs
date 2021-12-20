using ModChart;
using System.Collections.Generic;
using System.Linq;

namespace ScuffedWalls
{

    public class WorkspaceRequestParser : IRequestParser<ContainerRequest, Workspace>
    { 
        public ContainerRequest CurrentRequest => _request;
        public bool HideLogs { get; set; }
        public Workspace Result => _latestWorkspaceResult;
        public TreeList<AssignableInlineVariable> GlobalVariables { get; private set; }

        private ContainerRequest _request;
        private Workspace _latestWorkspaceResult;
        public WorkspaceRequestParser(ContainerRequest containerrequest, bool hideLogs = false)
        {
            //Instance = this;
            HideLogs = hideLogs;
            _request = containerrequest;
        }
        private IEnumerator<VariableRequest> _variableRequestEnumerator;
        private IEnumerator<FunctionRequest> _functionRequestEnumerator;
        public Workspace GetResult()
        {
            GlobalVariables = new TreeList<AssignableInlineVariable>(AssignableInlineVariable.Exposer);
            foreach (var param in _request.Parameters) param.Variables.Register(GlobalVariables);

            Workspace workspace = new Workspace(BeatMap.Empty, _request.Name);

            _variableRequestEnumerator = _request.VariableRequests.GetEnumerator();
            _functionRequestEnumerator = _request.FunctionRequests.GetEnumerator();

            while (_variableRequestEnumerator.MoveNext())
            {
                var result = new VariableRequestParser(_variableRequestEnumerator.Current, HideLogs).GetResult();
                if (result != null) GlobalVariables.AddRange(result);
            }

           // Parameter.AssignVariables(_request.Parameters, globalvariables);

            while (_functionRequestEnumerator.MoveNext())
            {
                new FunctionRequestParser(_functionRequestEnumerator.Current, workspace, HideLogs).GetResult();
            }
            _latestWorkspaceResult = workspace;
            return workspace;
        }
    }
}
