using ModChart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            public Parameter[] Parameters { get; set; } = new Parameter[] { };
            public FunctionRequest[] FunctionRequests { get; set; } = new FunctionRequest[] { };
            public VariableRequest[] VariableRequests { get; set; } = new VariableRequest[] { };
            public class FunctionRequest
            {
                public string Name { get; set; }
                public float Time { get; set; }
                public Parameter[] Parameters { get; set; } = new Parameter[] { };
            }
            public class VariableRequest
            {
                public string Name { get; set; }
                public Parameter[] Parameters { get; set; } = new Parameter[] { };
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
                    if (CurrentWorkspace != null) WorkspaceRequests = WorkspaceRequests.Append(CurrentWorkspace).ToArray();
                    CurrentWorkspace = new WorkspaceRequest() { Name = line.Data };
                }
                else if (line.Type == ParamType.Function && CurrentWorkspace != null)
                {
                    CurrentWorkspace.Parameters = CurrentWorkspace.Parameters.Append(line).ToArray();
                    if (CurrentFunction != null) CurrentWorkspace.FunctionRequests = CurrentWorkspace.FunctionRequests.Append(CurrentFunction).ToArray();
                    CurrentFunction = new WorkspaceRequest.FunctionRequest() { Name = line.Data, Time = line.Name.toFloat() };
                    CurrentInternal = ParamType.Function;
                }
                else if (line.Type == ParamType.Variable && CurrentWorkspace != null)
                {
                    CurrentWorkspace.Parameters = CurrentWorkspace.Parameters.Append(line).ToArray();
                    if (CurrentVariable != null) CurrentWorkspace.VariableRequests = CurrentWorkspace.VariableRequests.Append(CurrentVariable).ToArray();
                    CurrentVariable = new WorkspaceRequest.VariableRequest() { Name = line.Data };
                    CurrentInternal = ParamType.Variable;
                }
                else if (line.Type == ParamType.Parameter && CurrentWorkspace != null)
                {

                    CurrentWorkspace.Parameters = CurrentWorkspace.Parameters.Append(line).ToArray();
                    if (CurrentInternal == ParamType.Function) CurrentFunction.Parameters = CurrentFunction.Parameters.Append(line).ToArray();
                    else if (CurrentInternal == ParamType.Variable) CurrentVariable.Parameters = CurrentVariable.Parameters.Append(line).ToArray();
                }
            }
            if (CurrentVariable != null) CurrentWorkspace.VariableRequests = CurrentWorkspace.VariableRequests.Append(CurrentVariable).ToArray();
            if (CurrentFunction != null) CurrentWorkspace.FunctionRequests = CurrentWorkspace.FunctionRequests.Append(CurrentFunction).ToArray();
            if (CurrentWorkspace != null) WorkspaceRequests = WorkspaceRequests.Append(CurrentWorkspace).ToArray();
        }
    }



}
