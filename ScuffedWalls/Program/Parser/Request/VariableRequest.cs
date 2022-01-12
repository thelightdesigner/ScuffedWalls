using System;
using System.Collections.Generic;
using System.Linq;

namespace ScuffedWalls
{
    public class VariableRequest : Request, ICloneable
    {
        public static readonly Func<VariableRequest, string> Exposer = var => var.Name;
        public const string VariableKeyword = "var";
        public static bool IsName(string name)
        {
            if (name == VariableKeyword) return true;
            return false;
        }
        public static VariableRequest GetSetup(List<Parameter> Lines)
        {
            var req = new VariableRequest();
            req.SetupFromLines(Lines);
            return req;
        }
        public void ResetDefaultValue()
        {
            Data = DefaultVal;
        }
        public VariableRequest()
        {

        }
        public VariableRequest(string name, string data, VariableRecomputeSettings recompute = VariableRecomputeSettings.OnCreationOnly, bool _public = true)
        {
            Name = name.Trim();
            Data = data;
            VariableRecomputeSettings = recompute;
            Public = _public; 
            DefaultVal = Data;
        }
        public override string ToString()
        {
            return $"{Name}:{Data}";
        }
        public VariableEnumType ContentsType { get; private set; } = VariableEnumType.Single;
        public bool Static { get; private set; } = false;
        public string DefaultVal { get; private set; }
        public string Name { get; set; }
        public string Data { get; set; }
        public VariableRecomputeSettings VariableRecomputeSettings { get; private set; }
        public bool Public { get; private set; }
        public override Request SetupFromLines(List<Parameter> Lines)
        {
            Parameters = new TreeList<Parameter>(Lines, Parameter.Exposer);
            DefiningParameter = Lines.First();
            UnderlyingParameters = new TreeList<Parameter>(Lines.Lasts(), Parameter.Exposer);
            Name = DefiningParameter.StringData?.Trim();
            ContentsType = UnderlyingParameters.Get("type", VariableEnumType.Single, p => Enum.Parse<VariableEnumType>(p.Clean.StringData,true));
            Static = UnderlyingParameters.Get("static", false, p => true);
            DefaultVal = Data;
            VariableRecomputeSettings = UnderlyingParameters.Get("recompute", VariableRecomputeSettings.OnCreationOnly, p => (VariableRecomputeSettings)int.Parse(p.Use().StringData));
            Public = UnderlyingParameters.Get("public", false, p => bool.Parse(p.Use().Clean.StringData));
            Data = string.Join(',', UnderlyingParameters.Where(p => p.Name.RemoveWhiteSpace().ToLower() == "data").Select(p => p.Raw.StringData));

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
    public enum VariableEnumType
    {
        Array,
        Vector,
        Single
    }
}
