using System;
using System.Collections.Generic;
using System.Text;

namespace ScuffedWalls
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ScuffedFunctionAttribute : Attribute 
    {
        public ScuffedFunctionAttribute(string name)
        {
            ParserName = name.ToLower().removeWhiteSpace();
            Name = name;
        }
        public string ParserName;
        public string Name;
    }



}