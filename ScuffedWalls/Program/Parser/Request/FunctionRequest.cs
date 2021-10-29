using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ScuffedWalls
{
    public class FunctionRequest : Request
    {
        public const string FunctionKeyword = @"^([+-]?([0-9]+\.?[0-9]*|\.[0-9]+)|fun)$";
        public static bool IsName(string name)
        {
            if (Regex.IsMatch(name, FunctionKeyword)) return true;
            return false;
        }
        public string Name { get; private set; }
        private float _time;
        public float Time => TimeParam != null ? float.Parse(TimeParam.StringData) : _time;
        public Parameter RepeatCount { get; private set; }
        public Parameter RepeatAddTime { get; private set; }
        public Parameter TimeParam { get; private set; }
        public override Request Setup(List<Parameter> Lines)
        {
            Parameters = new TreeList<Parameter>(Lines, Parameter.Exposer);
            DefiningParameter = Lines.First();
            UnderlyingParameters = new TreeList<Parameter>(Lines.Lasts(), Parameter.Exposer);

            _time = float.TryParse(DefiningParameter.Name, out float result) ? result : 0;
            Name = DefiningParameter.Clean.StringData;
            RepeatCount = UnderlyingParameters.Get("repeat", null, p => p.Use());
            RepeatAddTime = UnderlyingParameters.Get("repeataddtime", null, p => p.Use());
            TimeParam = UnderlyingParameters.Get("funtime", null, p => p.Use());
            return this;
        }
    }
}
