//classes that provide additional features to modcharting
using ModChart;
using ModChart.Wall;
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
        public BeatMap BeatMap;
        public ScuffedRequest Request;
        static Type[] Functions;

        public static Workspace[] Workspaces;
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
            List<Variable> globalvariables = new List<Variable>();
            List<Workspace> workspaces = new List<Workspace>();
            foreach (var workreq in Request.WorkspaceRequests)
            {
                rnb.Next();
                if (workreq.Name != null && workreq.Name != string.Empty) ScuffedLogger.ScuffedWorkspace.Log($"Workspace {workspaces.Count()} : \"{workreq.Name}\"");
                else ScuffedLogger.ScuffedWorkspace.Log($"Workspace {workspaces.Count()}");
                Console.ResetColor();

                Workspace WorkspaceInstance = new Workspace() { Name = workreq.Name, Number = workreq.Number};
                foreach (var varreq in workreq.VariableRequests)
                {
                    Variable variable = new Variable()
                    {
                        Name = varreq.Name,
                        Data = varreq.Parameters.AddVariables(globalvariables.ToArray()).Where(p => p.Name == "data").First().Data
                    };
                    globalvariables.Add(variable);
                    ScuffedLogger.ScuffedWorkspace.FunctionParser.Log($"Added Variable \"{variable.Name}\" Val:{variable.Data}");
                }
                foreach (var funcreq in workreq.FunctionRequests)
                {
                    if(!Functions.Any(f => f.GetCustomAttributes<ScuffedFunctionAttribute>().Any(a => a.ParserName == funcreq.Name)))
                    {
                        ConsoleErrorLogger.Log($"Function {funcreq.Name} at Beat {funcreq.Time} in Workspace {workreq.Name} {workreq.Number} does NOT exist, skipping");
                        continue;
                    }
                    Type func = Functions.Where(f => f.GetCustomAttributes<ScuffedFunctionAttribute>().Any(a => a.ParserName == funcreq.Name)).First();

                    var funcInstance = Activator.CreateInstance(func);
                    func.GetMethod("InstantiateSFunction").Invoke(funcInstance, new object[] { funcreq.Parameters.AddVariables(globalvariables.ToArray()), WorkspaceInstance, funcreq.Time });

                    try
                    {
                        func.GetMethod("Run").Invoke(funcInstance, new object[] { });
                    }
                    catch(Exception e)
                    {
                        ConsoleErrorLogger.Log($"Error executing function {funcreq.Name} at Beat {funcreq.Time} in Workspace {workreq.Name} {workreq.Number}");
                    }
                }
                workspaces.Add(WorkspaceInstance);
                Workspaces = workspaces.ToArray();
            }
            Workspaces = workspaces.ToArray();
            BeatMap = Workspaces.toBeatMap();
            if (Startup.ScuffedConfig.IsAutoSimplifyPointDefinitionsEnabled)
            {
                ScuffedLogger.ScuffedWorkspace.Log("Simplifying Point Definitions...");
                BeatMap = BeatMap.SimplifyAllPointDefinitions();
            }
        }
    }




}
