using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ScuffedWalls
{
    public class FunctionRequest : Request, ICloneable
    {
        public enum Keyword
        {
            Number,
            Fun,
            CallTime
        }
        public const string FunctionKeyword = @"^([+-]?([0-9]+\.?[0-9]*|\.[0-9]+)|fun|x*)$";
        public static bool IsName(string name)
        {
            if (Regex.IsMatch(name, FunctionKeyword)) return true;
            return false;
        }
        public static Keyword MatchCallSign(string name)
        {
            return float.TryParse(name, out _) ? Keyword.Number : name == "fun" ? Keyword.Fun : Keyword.CallTime;
        }
        public string Name { get; private set; }
        public Keyword CallSign { get; private set; }
        public void SetCallTime(float calltime)
        {
            if (CallSign == Keyword.CallTime) _time = calltime;
        }
        public override string ToString()
        {
            return $"{Time}:{Name}";
        }
        private float _time;
        public float Time => TimeParam != null && float.TryParse(TimeParam.StringData, out float help) ? help : _time;

        public int RepeatCount => RepeatCountParam != null && int.TryParse(RepeatCountParam.StringData, out int help) ? help : 1;
        public Parameter RepeatCountParam { get; private set; }
        public Parameter RepeatAddTime { get; private set; }
        public Parameter TimeParam { get; private set; }
        public FunctionRequest()
        {

        }
        public FunctionRequest(float time, string name, IEnumerable<Parameter> parameterss)
        {
            Name = name;
            _time = time;
            UnderlyingParameters = new TreeList<Parameter>(parameterss, Parameter.Exposer);
        }
        public override Request SetupFromLines(List<Parameter> Lines)
        {
            Parameters = new TreeList<Parameter>(Lines, Parameter.Exposer);
            DefiningParameter = Lines.First();
            UnderlyingParameters = new TreeList<Parameter>(Lines.Lasts(), Parameter.Exposer);

            string name = DefiningParameter.Clean.Name;
            CallSign = MatchCallSign(name);

            _time = CallSign == Keyword.Number ? float.Parse(name) : 0;

            Name = DefiningParameter.Clean.StringData;
            RepeatCountParam = UnderlyingParameters.Get("repeat", null, p => p.Use());
            RepeatAddTime = UnderlyingParameters.Get("repeataddtime", null, p => p.Use());
            TimeParam = UnderlyingParameters.Get("funtime", null, p => p.Use());
            if (TimeParam != null) TimeParam.Computer.HandleExceptions = true;
            return this;
        }
        public object Clone() => new FunctionRequest()
        {
            Name = Name,
            _time = _time,
            Parameters = Parameters,
            DefiningParameter = DefiningParameter,
            UnderlyingParameters = UnderlyingParameters,
            RepeatAddTime = (Parameter)RepeatAddTime?.Clone(),
            RepeatCountParam = (Parameter)RepeatCountParam?.Clone(),
            TimeParam = (Parameter)TimeParam?.Clone()
        };
    }
}
