using System;
using System.Collections.Generic;
using System.Linq;

namespace ScuffedWalls
{
    public class VariableRequest : Request, ICloneable
    {
        public const string VariableKeyword = "var";
        public static bool IsName(string name)
        {
            if (name == VariableKeyword) return true;
            return false;
        }
        public static VariableRequest GetSetup(List<Parameter> Lines)
        {
            var req = new VariableRequest();
            req.Setup(Lines);
            return req;
        }
        public VariableRequest()
        {

        }
        public VariableRequest(string name, string data, VariableRecomputeSettings recompute, bool _public)
        {
            Name = name;
            Data = data;
            VariableRecomputeSettings = recompute;
            Public = _public;
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
            Data = UnderlyingParameters.Get("data", "", p => p.Use().Raw.StringData);
            VariableRecomputeSettings = UnderlyingParameters.Get("recompute", VariableRecomputeSettings.OnCreationOnly, p => (VariableRecomputeSettings)int.Parse(p.Use().StringData));
            Public = UnderlyingParameters.Get("public", false, p => bool.Parse(p.Use().Clean.StringData));
            return this;
        }
        public object Clone() => new VariableRequest()
        {
            Name = Name,
            Parameters = Parameters,
            UnderlyingParameters = UnderlyingParameters,
            DefiningParameter = DefiningParameter,
            Data = Data,
            VariableRecomputeSettings = VariableRecomputeSettings,
            Public = Public
        };
    }
}
