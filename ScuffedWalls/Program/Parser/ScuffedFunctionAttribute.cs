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
        public FuncParameterAttribute(params string[] name)
        {
            Name = name;
            ParserName = name.Select(n => n.ToLower()).ToArray();
        }
        public FuncParameterAttribute(InputFormat format, params string[] name)
        {
            ConverterFormat = format;
            Name = name;
            ParserName = name.Select(n => n.ToLower()).ToArray();
        }
        public InputFormat ConverterFormat;
        public string[] Name;
        public string[] ParserName;
    }
    public enum InputFormat
    {
        ConvertToType,
        Json,
        CommaSeperatedArray
    }
    
}