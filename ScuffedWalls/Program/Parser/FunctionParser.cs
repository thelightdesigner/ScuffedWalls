using ModChart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ScuffedWalls
{
    public class FunctionParser
    {
        public static Rainbow WorkspaceRainbow = new Rainbow();
        public BeatMap BeatMap { get; private set; }
        public ScuffedRequest Request { get; private set; }
        public ScuffedRequest.WorkspaceRequest CurrentWorkspaceRequest { get; private set; }
        public ScuffedRequest.WorkspaceRequest.FunctionRequest CurrentFunctionRequest { get; private set; }
        public ScuffedRequest.WorkspaceRequest.VariableRequest CurrentVariableRequest { get; private set; }
        public List<Workspace> Workspaces { get; private set; } = new List<Workspace>();

        private static readonly Type[] _functions = Assembly
                 .GetExecutingAssembly()
                 .GetTypes()
                 .Where(t => t.Namespace == "ScuffedWalls.Functions" && t.GetCustomAttributes<SFunctionAttribute>().Any())
                 .ToArray();
        public FunctionParser(ScuffedRequest request)
        {
            Request = request;
            RunRequest();
        }

        private void RunRequest()
        {
            StringFunction.Populate();

            foreach (var workreq in Request.WorkspaceRequests)
            {
                CurrentWorkspaceRequest = workreq;
                List<Parameter> globalvariables = new List<Parameter>();
                if (workreq.Name != null && workreq.Name != string.Empty) ScuffedWalls.Print($"Workspace {Workspaces.Count()} : \"{workreq.Name}\"", Color: WorkspaceRainbow.Next());
                else ScuffedWalls.Print($"Workspace {Workspaces.Count()}", Color: WorkspaceRainbow.Next());
                Console.ResetColor();

                Workspace WorkspaceInstance = new Workspace(BeatMap.Empty, workreq.Name);
                foreach (var varreq in workreq.VariableRequests)
                {
                    CurrentVariableRequest = varreq;
                    try
                    {
                        Parameter.ExternalVariables = globalvariables.ToArray();

                        Parameter variable = new Parameter(
                            varreq.Name,
                            varreq.Parameters.Where(p => p.Name == "data").First().Raw.StringData,
                            GetParam("recompute", VariableRecomputeSettings.OnCreationOnly, p => (VariableRecomputeSettings)int.Parse(p), varreq.Parameters));

                        globalvariables.Add(variable);

                        ScuffedWalls.Print($"Added Variable \"{variable.Name}\" Val:{variable.StringData}");

                    }
                    catch (Exception e)
                    {
                        ScuffedWalls.Print($"Error adding global variable {varreq.Name} ERROR:{e.Message} ", ScuffedWalls.LogSeverity.Error);
                    }

                }

                Parameter.ExternalVariables = globalvariables.ToArray();

                foreach (var funcreq in workreq.FunctionRequests)
                {
                    CurrentFunctionRequest = funcreq;
                    Parameter.UnUseAll(funcreq.Parameters);


                    if (!_functions.Any(f => f.GetCustomAttributes<SFunctionAttribute>().Any(a => a.ParserName.Any(n => n == funcreq.Name))))
                    {
                        ScuffedWalls.Print($"Function {funcreq.Name} at Beat {funcreq.Time} in Workspace {workreq.Name} {workreq.Number} does NOT exist, skipping", ScuffedWalls.LogSeverity.Warning);
                        continue;
                    }

                    Type func = _functions.Where(f => f.BaseType == typeof(ScuffedFunction) && f.GetCustomAttributes<SFunctionAttribute>().Any(a => a.ParserName.Any(n => n == funcreq.Name))).First();

                    ScuffedFunction funcInstance = (ScuffedFunction)Activator.CreateInstance(func);

                    funcInstance.InstantiateSFunction(funcreq.Parameters, WorkspaceInstance, funcreq.Time, this);

                    try
                    {
                        funcInstance.Run();
                    }
                    catch (Exception e)
                    {
                        ScuffedWalls.Print($"Error executing function {funcreq.Name} at Beat {funcreq.Time} in Workspace {workreq.Name} {workreq.Number} ERROR:{(e.InnerException ?? e).Message}", ScuffedWalls.LogSeverity.Error);
                    }

                    Parameter.Check(funcreq.Parameters);

                }
                Workspaces.Add(WorkspaceInstance);
            }
            BeatMap = Workspace.GetBeatMap(Workspaces);
            BeatMap.Prune();
            if (Utils.ScuffedConfig.IsAutoSimplifyPointDefinitionsEnabled)
            {
                try
                {
                    ScuffedWalls.Print("Simplifying Point Definitions...");
                    BeatMap = BeatMap.SimplifyAllPointDefinitions();
                }
                catch (IndexOutOfRangeException e)
                {
                    ScuffedWalls.Print($"Failed to simplify Point Definitions, A point definition may be missing a value? ERROR:{e.Message}", ScuffedWalls.LogSeverity.Warning);
                }
                catch (Exception e)
                {
                    ScuffedWalls.Print($"Failed to simplify Point Definitions ERROR:{e.Message}", ScuffedWalls.LogSeverity.Warning);
                }
            }
        }

        public static T GetParam<T>(string name, T defaultval, Func<string, T> converter, IEnumerable<Parameter> parameters)
        {
            if (!parameters.Any(p => p.Name.ToLower().RemoveWhiteSpace().Equals(name))) return defaultval;
            return converter(parameters.Where(p => p.Name.ToLower().RemoveWhiteSpace().Equals(name)).First().StringData);
        }
    }




}
