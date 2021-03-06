﻿//classes that provide additional features to modcharting
using ModChart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using static ScuffedWalls.ScuffedLogger.Default.ScuffedWorkspace;
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
            StringFunction.Populate();

            Workspaces = new Workspace[] { };
            List<Workspace> workspaces = new List<Workspace>();
            foreach (var workreq in Request.WorkspaceRequests)
            {
                List<Parameter> globalvariables = new List<Parameter>();
                rnb.Next();
                if (workreq.Name != null && workreq.Name != string.Empty) Log($"Workspace {workspaces.Count()} : \"{workreq.Name}\"");
                else Log($"Workspace {workspaces.Count()}");
                Console.ResetColor();

                Workspace WorkspaceInstance = new Workspace() { Name = workreq.Name, Number = workreq.Number };
                foreach (var varreq in workreq.VariableRequests)
                {
                    try
                    {
                        Parameter.ExternalVariables = globalvariables.ToArray();

                        Parameter variable = new Parameter(
                            varreq.Name,
                            varreq.Parameters.Where(p => p.Name == "data").First().Raw.StringData,
                            GetParam("recompute", VariableRecomputeSettings.OnCreationOnly, p => (VariableRecomputeSettings)int.Parse(p), varreq.Parameters));

                        globalvariables.Add(variable);

                        ScuffedLogger.Default.ScuffedWorkspace.FunctionParser.Log($"Added Variable \"{variable.Name}\" Val:{variable.StringData}");

                    }
                    catch (Exception e)
                    {
                        ScuffedLogger.Error.Log($"Error adding global variable {varreq.Name} ERROR:{e.Message} ");
                    }

                }

                Parameter.ExternalVariables = globalvariables.ToArray();

                foreach (var funcreq in workreq.FunctionRequests)
                {
                    Parameter.UnUseAll(funcreq.Parameters);


                    if (!Functions.Any(f => f.GetCustomAttributes<ScuffedFunctionAttribute>().Any(a => a.ParserName.Any(n => n == funcreq.Name))))
                    {
                        ScuffedLogger.Warning.Log($"Function {funcreq.Name} at Beat {funcreq.Time} in Workspace {workreq.Name} {workreq.Number} does NOT exist, skipping");
                        continue;
                    }

                    Type func = Functions.Where(f => f.BaseType == typeof(SFunction) && f.GetCustomAttributes<ScuffedFunctionAttribute>().Any(a => a.ParserName.Any(n => n == funcreq.Name))).First();

                    SFunction funcInstance = (SFunction)Activator.CreateInstance(func);

                    funcInstance.InstantiateSFunction(funcreq.Parameters, WorkspaceInstance, funcreq.Time);

                    try
                    {
                        funcInstance.Run();
                    }
                    catch (Exception e)
                    {
                        ScuffedLogger.Error.Log($"Error executing function {funcreq.Name} at Beat {funcreq.Time} in Workspace {workreq.Name} {workreq.Number} ERROR:{(e.InnerException ?? e).Message}");
                    }

                    Parameter.Check(funcreq.Parameters);

                }
                workspaces.Add(WorkspaceInstance);
                Workspaces = workspaces.ToArray();
            }
            Workspaces = workspaces.ToArray();
            BeatMap = Workspaces.ToBeatMap();
            BeatMap.Prune();
            if (Utils.ScuffedConfig.IsAutoSimplifyPointDefinitionsEnabled)
            {
                try
                {
                    Log("Simplifying Point Definitions...");
                    BeatMap = BeatMap.SimplifyAllPointDefinitions();
                }
                catch (IndexOutOfRangeException e)
                {
                    ScuffedLogger.Error.Log($"Failed to simplify Point Definitions, A point definition may be missing a value? ERROR:{e.Message}");
                }
                catch (Exception e)
                {
                    ScuffedLogger.Error.Log($"Failed to simplify Point Definitions ERROR:{e.Message}");
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
