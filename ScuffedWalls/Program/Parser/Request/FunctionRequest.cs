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
        public float Time { get; private set; }
        public override Request Setup(List<Parameter> Lines)
        {
            Parameters = new Lookup<Parameter>(Lines, Parameter.Exposer);
            DefiningParameter = Lines.First();
            UnderlyingParameters = new Lookup<Parameter>(Lines.Lasts(), Parameter.Exposer);
            Name = DefiningParameter.Clean.StringData;
            Time = float.Parse(DefiningParameter.Name);
            return this;
        }
    }
}
