using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static ScuffedWalls.ScuffedRequestParser;

namespace ScuffedWalls
{
    public class VariableRequest : Request
    {
        public static bool IsName(string name)
        {
            if (name == "var") return true;
            return false;
        }
        public static VariableRequest GetSetup(List<Parameter> Lines)
        {
            var req = new VariableRequest();
            req.Setup(Lines);
            return req;
        }
        public string Name { get; set; }
        public string Data { get; set; }
        public VariableRecomputeSettings VariableRecomputeSettings { get; private set; }
        public bool Public { get; private set; }
        public override Request Setup(List<Parameter> Lines)
        {
            Parameters = new TreeList<Parameter>(Lines, Parameter.Exposer);
            DefiningParameter = Lines.First();
            UnderlyingParameters = new TreeList<Parameter>(Lines.Lasts(), Parameter.Exposer);

            Name = DefiningParameter.StringData;
            Data = UnderlyingParameters.Get("data", "", p => p.Use().StringData);
            VariableRecomputeSettings = UnderlyingParameters.Get("recompute", VariableRecomputeSettings.OnCreationOnly, p => (VariableRecomputeSettings)int.Parse(p.Use().StringData));
            Public = UnderlyingParameters.Get("public", false, p => bool.Parse(p.Use().Clean.StringData));
            return this;
        }
    }
}
