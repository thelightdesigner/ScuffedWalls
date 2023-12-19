//classes that provide additional features to modcharting
using ModChart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
//functions

namespace ScuffedWalls
{
    class FunctionParser
    {
        public static Rainbow rnb;
        public BeatMap BeatMap { get; private set; }
        public ScuffedRequest Request;
        static Type[] Functions;

        public static Workspace[] Workspaces { get; private set; }
        public FunctionParser(ScuffedRequest request)
        {
            if (rnb == null) rnb = new Rainbow();
            Request = request;
            if (Functions == null) Functions = Assembly
                 .GetExecutingAssembly()
                 .GetTypes()
                 .Where(t => t.Namespace == "ScuffedWalls.Functions" && t.GetCustomAttributes<ScuffedFunctionAttribute>().Any())
                 .ToArray();
            RunRequest();
        }
        void RunRequest()
        {

            Workspaces = new Workspace[] { };
            List<Workspace> workspaces = new List<Workspace>();
            foreach (var workreq in Request.WorkspaceRequests)
            {
                ScuffedWalls.Print($"Workspace {workspaces.Count()} {(string.IsNullOrEmpty(workreq.Name) ? "" : $"\"{workreq.Name}\"")}", Color: rnb.Next());

                Workspace WorkspaceInstance = new Workspace() { Name = workreq.Name };
                foreach (var varreq in workreq.VariableRequests)
                {
                    try
                    {

                        Parameter variable = new Parameter(
                varreq.Name,
                varreq.Parameters.at"data").Raw.StringData,
                GetParam("recompute", VariableRecomputeSettings.OnCreationOnly, p => (VariableRecomputeSettings)int.Parse(p), varreq.Parameters));
                        variable.


                        ScuffedWalls.Print($"Added Variable \"{variable.Name}\" Val:{variable.StringData}");

                    }
                    catch (Exception e)
                    {
                        ScuffedWalls.Print($"Error adding global variable {varreq.Name} ERROR:{e.Message}",ScuffedWalls.LogSeverity.Error);
                    }

                }

                Parameter.ExternalVariables = globalvariables.ToArray();

                foreach (var funcreq in workreq.FunctionRequests)
                {
                    Parameter.UnUseAll(funcreq.Parameters);


                    if (!Functions.Any(f => f.GetCustomAttributes<ScuffedFunctionAttribute>().Any(a => a.ParserName.Any(n => n == funcreq.Name))))
                    {
                        ScuffedWalls.Print($"Function {funcreq.Name} at Beat {funcreq.Time} in Workspace {workreq.Name} {workreq.Number} does NOT exist, skipping",ScuffedWalls.LogSeverity.Warning);
                        continue;
                    }

                    Type func = Functions.Where(f => f.BaseType == typeof(SFunction) && f.GetCustomAttributes<ScuffedFunctionAttribute>().Any(a => a.ParserName.Any(n => n == funcreq.Name))).First();

                    SFunction funcInstance = (SFunction)Activator.CreateInstance(func);

                    funcInstance.InstantiateSFunction(funcreq.Parameters, WorkspaceInstance, funcreq.Time, funcreq.Name);

                    try
                    {
                        funcInstance.Run();
                    }
                    catch (Exception e)
                    {
                        ScuffedWalls.Print($"Error executing function {funcreq.Name} at Beat {funcreq.Time} in Workspace {workreq.Name} {workreq.Number} ERROR:{(e.InnerException ?? e).Message}",ScuffedWalls.LogSeverity.Error);
                        throw e;
                    }

                    Parameter.Check(funcreq.Parameters);

                }
                workspaces.Add(WorkspaceInstance);
                Workspaces = workspaces.ToArray();
            }
            Workspaces = workspaces.ToArray();
            BeatMap = Workspace.GetBeatMap(workspaces);
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
                    ScuffedWalls.Print($"Failed to simplify Point Definitions, A point definition may be missing a value? ERROR:{e.Message}", ScuffedWalls.LogSeverity.Error);
                }
                catch (Exception e)
                {
                    ScuffedWalls.Print($"Failed to simplify Point Definitions ERROR:{e.Message}", ScuffedWalls.LogSeverity.Error);
                }
            }
        }

        public static T GetParam<T>(string name, T defaultval, Func<string, T> converter, Parameter[] parameters)
        {
            if (!parameters.Any(p => p.Name.ToLower().RemoveWhiteSpace().Equals(name))) return defaultval;
            return converter(parameters.Where(p => p.Name.ToLower().RemoveWhiteSpace().Equals(name)).First().StringData);
        }
    }




}
