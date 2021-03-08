using ModChart;
using NCalc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ScuffedWalls
{
    public class Parameter
    {
        public Variable[] InternalVariables { get; set; } = new Variable[] { };
        public Variable[] ExternalVariables { get; set; } = new Variable[] { };
        public Parameter()
        {
            SetRaw();
            SetType();
            SetNameAndData();
        }
        public Parameter(string line)
        {
            Line = line;
            SetRaw();
            SetType();
            SetNameAndData();
        }
        public Parameter(string line, int index)
        {
            Line = line;
            GlobalIndex = index;
            SetRaw();
            SetType();
            SetNameAndData();
        }
        public void SetRaw()
        {
            string[] split = Line.Split(':', 2);
            Raw.Name = split[0];
            if (split.Length > 1) Raw.Data = split[1];
        }

        public void SetNameAndData()
        {
            Internal.Name = Line.removeWhiteSpace().ToLower().Split(':', 2)[0];

            if (Raw.Data != null)
            {
                if (Type == ParamType.Function) Internal.Data = Line.Split(':', 2)[1].removeWhiteSpace().ToLower(); //function names are lower and without whitespace
                else if (Type == ParamType.Variable) Internal.Data = Line.Split(':', 2)[1].removeWhiteSpace(); //variable names can have casing but no space
                else if (Type == ParamType.Workspace || Type == ParamType.Parameter) Internal.Data = Line.Split(':', 2)[1]; 
            }

        }
        public void SetType()
        {
            if (char.IsDigit(Raw.Name.ToLower().removeWhiteSpace()[0])) Type = ParamType.Function;
            else if (Raw.Name.ToLower().removeWhiteSpace() == "workspace") Type = ParamType.Workspace;
            else if (Raw.Name.ToLower().removeWhiteSpace() == "var") Type = ParamType.Variable;
            else Type = ParamType.Parameter;
        }

        //replaces Random(v1,v2) with a random single precision floating point number
        public string ParseRandom(string s)
        {
            Random rnd = new Random();
            try
            {
                while (s.Contains("Random("))
                {
                    //Console.WriteLine("wow");
                    string[] asplit = s.Split("Random(", 2);
                    string[] randomparams = asplit[1].Split(',',2);
                    float first = randomparams[0].toFloat();
                    float last = randomparams[1].Split(")")[0].toFloat();
                    double random = rnd.NextDouble() * (last - first) + first;
                    s = asplit[0] + random + asplit[1].Split(')', 2)[1];

                }

            }
            catch { throw new ScuffedException($"Unable to parse Random() Line:{s}"); }

            return s;
        }
        //replaces a variable name with a number
        public string ParseVar(string s)
        {
            foreach (var v in ExternalVariables)
            {
                while (s.Contains(v.Name))
                {
                    string[] split = s.Split(v.Name, 2);
                    s = split[0] + v.Data + split[1];
                }
            }
            foreach (var v in InternalVariables)
            {
                while (s.Contains(v.Name))
                {
                    string[] split = s.Split(v.Name, 2);
                    s = split[0] + v.Data + split[1];
                }
            }
            return s;
        }
        //computes things in {} i guess
        public string ParseMath(string s)
        {
           // try
           // {
                while (s.ToLower().Contains("{"))
                {
                    string[] asplit = s.Split("{", 2);
                    string[] endsplit = asplit[1].Split('}', 2);
                    Expression e = new Expression(endsplit[0]);

                    s = asplit[0] + e.Evaluate().ToString() + endsplit[1];
                }
           // }
           // catch { throw new ScuffedException($"Unable to parse Math {{}} Line:{s}"); }

            return s;
        }

        Variable Internal { get; set; } = new Variable();
        public Variable Raw { get; set; } = new Variable();
        public int GlobalIndex { get; set; }
        public string Line { get; set; }
        public ParamType Type { get; private set; } = ParamType.Parameter;
        public string Name
        {
            get
            {
                return Internal.Name
                            .Clone()
                            .ToString();
            }
        }

        public string Data
        {
            get
            {
                if (Internal.Data == null) return Internal.Data;
                return ParseMath(
                    ParseRandom(
                        ParseVar(
                            Internal.Data
                            .Clone()
                            .ToString())));
            }
        }
    }
    public static class ParameterHelper
    {
        public static Parameter[] AddVariables(this Parameter[] parameters, Variable[] var)
        {
            return parameters.Select(p =>
            {
                p.ExternalVariables = p.ExternalVariables.CombineWith(var);
                return p;
            }).ToArray();
        }
    }


    public class Variable
    {
        public Variable() { }
        public Variable(string name, string data)
        {
            Name = name;
            Data = data;
        }
        public string Name { get; set; }
        public string Data { get; set; }
        public override string ToString()
        {
            return $"Name:{Name} Data:{Data}";
        }
    }
    public enum ParamType
    {
        Workspace,
        Function,
        Parameter,
        Variable
    }
}
