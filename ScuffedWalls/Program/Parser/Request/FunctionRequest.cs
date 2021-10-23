using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScuffedWalls
{
    public class FunctionRequest : Request
    {
        public static bool IsName(string name)
        {
            if (name.All(chars => char.IsDigit(chars))) return true;
            return false;
        }
        public string Name { get; private set; }
        public float Time { get; set; }
        public int RepeatCount { get; private set; }
        public float RepeatAddTime { get; private set; }
        public override Request Setup(List<Parameter> Lines)
        {
            Parameters = new TreeList<Parameter>(Lines, Parameter.Exposer);
            DefiningParameter = Lines.First();
            UnderlyingParameters = new TreeList<Parameter>(Lines.Lasts(), Parameter.Exposer);
            Name = DefiningParameter.Clean.StringData;
            Time = float.Parse(DefiningParameter.Use().Name);
            RepeatCount = UnderlyingParameters.Get("repeat", 1, p => int.Parse(p.Use().StringData));
            RepeatAddTime = UnderlyingParameters.Get("repeataddtime", 0.0f, p => float.Parse(p.Use().StringData));
            return this;
        }
    }
}
