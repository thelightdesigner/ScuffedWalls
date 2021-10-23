using ModChart;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ScuffedWalls
{
    /// <summary>
    /// Adds this FunctionRequest's results to the given workspace and returns it.
    /// </summary>
    public class FunctionRequestParser : IRequestParser<FunctionRequest, BeatMap>
    {
        public FunctionRequest CurrentRequest => _request;
        public BeatMap Result => _latestResultObjs;
        public FunctionRequestParser(FunctionRequest request, Workspace instance = null)
        {
            _request = request;
            _instanceWorkspace = instance ?? BeatMap.Empty;
        }
        private Workspace _instanceWorkspace;
        private FunctionRequest _request;
        private BeatMap _latestResultObjs = BeatMap.Empty;

        public static readonly Type[] Functions = Assembly
                 .GetExecutingAssembly()
                 .GetTypes()
                 .Where(t => t.Namespace == "ScuffedWalls.Functions" && t.GetCustomAttributes<SFunctionAttribute>().Any())
                 .ToArray();
        public BeatMap GetResult()
        {
           // Parameter.UnUseAll(_request.UnderlyingParameters);


            if (!Functions.Any(f => f.GetCustomAttributes<SFunctionAttribute>().Any(a => a.ParserName.Any(n => n == _request.Name))))
            {
                throw new InvalidFilterCriteriaException($"Function {_request.Name} at Beat {_request.Time} does NOT exist, skipping");
            }

            Type func = Functions.Where(f => f.BaseType == typeof(ScuffedFunction) && f.GetCustomAttributes<SFunctionAttribute>().Any(a => a.ParserName.Any(n => n == _request.Name))).First();

            ScuffedFunction funcInstance = (ScuffedFunction)Activator.CreateInstance(func);

            funcInstance.InstantiateSFunction(_request.UnderlyingParameters, _request.DefiningParameter, _instanceWorkspace, _request.Time);

            Debug.TryAction(() =>
            {
                float initialTime = _request.Time;
                for (int i = 0; i < _request.RepeatCount; i++)
                {
                    _request.Time += _request.RepeatAddTime;
                    funcInstance.Run();
                }
                ScuffedWalls.Print($"Added \"{_request.Name}\" at beat {initialTime} ({string.Join(", ", funcInstance.Stats.Select(st => $"{st.Value} {st.Key.MakePlural(st.Value)}"))})", Color: ConsoleColor.White, OverrideStackFrame: func.Name);

            }, e =>
            {
                throw new Exception($"Error executing function {_request.Name} at Beat {_request.Time}", e.InnerException ?? e);
            });

            Parameter.Check(_request.UnderlyingParameters);
            _latestResultObjs = _instanceWorkspace;
            return _instanceWorkspace;
        }
    }
}
