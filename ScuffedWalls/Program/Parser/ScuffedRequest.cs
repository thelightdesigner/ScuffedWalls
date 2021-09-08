using ModChart;
using System.Collections.Generic;
using System.Linq;

namespace ScuffedWalls
{
    //the base class for scuffedwalls to do actions off of
    public class ScuffedRequest
    {
        public Parameter[] Parameters { get; set; } = new Parameter[] { };
        public WorkspaceRequest[] WorkspaceRequests { get; set; } = new WorkspaceRequest[] { };
        public class WorkspaceRequest
        {
            public string Name { get; set; }
            public int Number { get; set; }
            public FunctionRequest[] FunctionRequests { get; set; } = new FunctionRequest[] { };
            public VariableRequest[] VariableRequests { get; set; } = new VariableRequest[] { };
            public class FunctionRequest
            {
                public string Name { get; set; }
                public float Time { get; set; }
                public List<Parameter> Parameters { get; } = new List<Parameter>();
            }
            public class VariableRequest
            {
                public string Name { get; set; }
                public List<Parameter> Parameters { get; } = new List<Parameter>();
            }
        }
        public ScuffedRequest(Parameter[] Lines)
        {
            Parameters = Lines;
            Parse();
        }
        public void Parse()
        {
            WorkspaceRequest CurrentWorkspace = null;
            WorkspaceRequest.FunctionRequest CurrentFunction = null;
            WorkspaceRequest.VariableRequest CurrentVariable = null;
            ParamType CurrentInternal = ParamType.Workspace;

            foreach (var line in Parameters.OrderBy(l => l.GlobalIndex))
            {
                if (line.Type == ParamType.Workspace)
                {
                    //add last funcs/variables to last workspace
                    if (CurrentFunction != null) CurrentWorkspace.FunctionRequests = CurrentWorkspace.FunctionRequests.Append(CurrentFunction).ToArray();
                    if (CurrentVariable != null) CurrentWorkspace.VariableRequests = CurrentWorkspace.VariableRequests.Append(CurrentVariable).ToArray();
                    CurrentFunction = null;
                    CurrentVariable = null;

                    //add last workspace
                    if (CurrentWorkspace != null) WorkspaceRequests = WorkspaceRequests.Append(CurrentWorkspace).ToArray();

                    //create new workspace
                    CurrentWorkspace = new WorkspaceRequest() { Name = line.StringData, Number = WorkspaceRequests.Length };

                    CurrentInternal = ParamType.Workspace;

                }
                else if (line.Type == ParamType.Function && CurrentWorkspace != null)
                {


                    //add last function
                    if (CurrentFunction != null) CurrentWorkspace.FunctionRequests = CurrentWorkspace.FunctionRequests.Append(CurrentFunction).ToArray();

                    //create new function
                    CurrentFunction = new WorkspaceRequest.FunctionRequest() { Name = line.StringData, Time = line.Name.ToFloat() };

                    CurrentInternal = ParamType.Function;
                }
                else if (line.Type == ParamType.Variable && CurrentWorkspace != null)
                {


                    //add last variable
                    if (CurrentVariable != null) CurrentWorkspace.VariableRequests = CurrentWorkspace.VariableRequests.Append(CurrentVariable).ToArray();

                    //create new variable
                    CurrentVariable = new WorkspaceRequest.VariableRequest() { Name = line.Raw.StringData };


                    CurrentInternal = ParamType.Variable;
                }
                else if (line.Type == ParamType.Parameter && CurrentWorkspace != null)
                {
                    //add line to current func/var
                    if (CurrentInternal == ParamType.Function) CurrentFunction.Parameters.Add(line);
                    else if (CurrentInternal == ParamType.Variable) CurrentVariable.Parameters.Add(line);
                }
            }
            if (CurrentVariable != null) CurrentWorkspace.VariableRequests = CurrentWorkspace.VariableRequests.Append(CurrentVariable).ToArray();
            if (CurrentFunction != null) CurrentWorkspace.FunctionRequests = CurrentWorkspace.FunctionRequests.Append(CurrentFunction).ToArray();
            if (CurrentWorkspace != null) WorkspaceRequests = WorkspaceRequests.Append(CurrentWorkspace).ToArray();
        }
    }



}
