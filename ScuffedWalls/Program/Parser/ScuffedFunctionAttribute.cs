using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScuffedWalls
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ScuffedFunctionAttribute : Attribute 
    {
        public ScuffedFunctionAttribute(params string[] name)
        {
            ParserName = name.Select(n => n.ToLower().RemoveWhiteSpace()).ToArray();
            Name = name.First();
        }
        public string[] ParserName;
        public string Name;
    }
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class FuncParameterAttribute : Attribute
    {
        public FuncParameterAttribute(Func<string,object> Converter, params string[] Name)
        {
            this.Converter = Converter;
            this.Name = Name;
            ParserName = Name.Select(n => n.ToLower()).ToArray();
        }
        public Func<string, object> Converter;
        public string[] Name;
        public string[] ParserName;
    }
}