using ModChart;
using ScuffedWalls.Functions;
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
        public bool HideLogs { get; set; }
        public FunctionRequest CurrentRequest => _request;
        public BeatMap Result => _latestResultObjs;
        public FunctionRequestParser(FunctionRequest request, Workspace instance = null, bool hideLogs = false)
        {
            HideLogs = hideLogs;
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

            bool isCustom = ScuffedRequestParser.Instance.CurrentRequest.CustomFunctionExists(_request.Name);

            if (!isCustom && !Functions.Any(f => f.GetCustomAttributes<SFunctionAttribute>().Any(a => a.ParserName.Any(n => n == _request.Name))))
            {
                throw new InvalidFilterCriteriaException($"Function {_request.Name} at Beat {_request.Time} does NOT exist, skipping");
            }

            Type func = 
                isCustom ? typeof(CustomFunction) :
                Functions.First(f => f.BaseType == typeof(ScuffedFunction) && f.GetCustomAttributes<SFunctionAttribute>().Any(a => !a.ParserName.Contains("[NONCALLABLE]") && a.ParserName.Any(n => n == _request.Name)));
            

            ScuffedFunction funcInstance = (ScuffedFunction)Activator.CreateInstance(func);

            funcInstance.InstantiateSFunction(_request.UnderlyingParameters, _request.DefiningParameter, _instanceWorkspace, _request.Time);

            Debug.TryAction(() =>
            {
                float initialTime = _request.Time;
                TreeList<AssignableInlineVariable> repeatVars = new TreeList<AssignableInlineVariable>(AssignableInlineVariable.Exposer);
                AssignableInlineVariable repeat = new AssignableInlineVariable("repeat", "0");
                AssignableInlineVariable beat = new AssignableInlineVariable("time", _request.Time.ToString());
                repeatVars.Add(repeat);
                repeatVars.Add(beat);
                foreach(var re in _request.Parameters) re.Variables.Register(repeatVars);

                int repeatCount = _request.RepeatCount != null ? int.Parse(_request.RepeatCount.StringData) : 1;
                float repeatTime = _request.RepeatAddTime != null ? float.Parse(_request.RepeatAddTime.StringData) : 0.0f;

                for (int i = 0; i < repeatCount; i++)
                {
                    repeat.StringData = i.ToString();
                    beat.StringData = _request.Time.ToString();

                    funcInstance.Time = _request.Time + (i * repeatTime);
                    funcInstance.Run();

                    WorkspaceRequestParser.Instance.RefreshCurrentParameters();
                }
                if (!HideLogs)
                {
                    string stats = string.Join(", ", funcInstance.Stats.Select(st => $"{st.Value} {st.Key.MakePlural(st.Value)}"));
                    ScuffedWalls.Print($"Added \"{_request.Name}\" at beat {initialTime} {(string.IsNullOrEmpty(stats) ? "" : $"({stats})")}", Color: ConsoleColor.White, OverrideStackFrame: func.Name);
                }

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
