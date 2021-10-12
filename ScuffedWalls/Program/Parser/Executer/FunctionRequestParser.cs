using ModChart;
using System;
using System.Collections.Generic;
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

        private static readonly Type[] _functions = Assembly
                 .GetExecutingAssembly()
                 .GetTypes()
                 .Where(t => t.Namespace == "ScuffedWalls.Functions" && t.GetCustomAttributes<SFunctionAttribute>().Any())
                 .ToArray();
        public BeatMap GetResult()
        {
            Parameter.UnUseAll(_request.UnderlyingParameters);


            if (!_functions.Any(f => f.GetCustomAttributes<SFunctionAttribute>().Any(a => a.ParserName.Any(n => n == _request.Name))))
            {
                throw new InvalidFilterCriteriaException($"Function {_request.Name} at Beat {_request.Time} does NOT exist, skipping");
            }

            Type func = _functions.Where(f => f.BaseType == typeof(ScuffedFunction) && f.GetCustomAttributes<SFunctionAttribute>().Any(a => a.ParserName.Any(n => n == _request.Name))).First();

            ScuffedFunction funcInstance = (ScuffedFunction)Activator.CreateInstance(func);

            funcInstance.InstantiateSFunction(_request.UnderlyingParameters, _request.DefiningParameter, _instanceWorkspace, _request.Time);

            Debug.TryAction(() =>
            {
                funcInstance.Run();
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
