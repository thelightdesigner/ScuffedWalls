using ModChart;
using NCalc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ScuffedWalls
{
    public class Parameter : INameStringDataPair
    {
        public static void UnUseAll(Parameter[] parameters)
        {
            foreach (var p in parameters) p.WasUsed = false;    
        }
        public static void Check(Parameter[] parameters)
        {
            foreach (var p in parameters) if (!p.WasUsed) ScuffedLogger.Warning.Log($"Unused Parameter {p.Name} at line {p.GlobalIndex}! (Mispelled?)");
        }
        public bool WasUsed { get; set; }
        public Parameter[] InternalVariables { get; set; } = new Parameter[] { };
        public static Parameter[] ExternalVariables { get; set; } = new Parameter[] { };
        public Parameter(VariableRecomputeSettings RecomputeSettings = VariableRecomputeSettings.AllReferences)
        {
            SetRaw();
            SetType();
            SetNameAndData();
            SetInstance();
            VariableComputeSettings = RecomputeSettings;
        }
        public Parameter(string line, VariableRecomputeSettings RecomputeSettings = VariableRecomputeSettings.AllReferences)
        {
            Line = line;
            SetRaw();
            SetType();
            SetNameAndData();
            SetInstance();
            VariableComputeSettings = RecomputeSettings;
        }
        public Parameter(string line, int index, VariableRecomputeSettings RecomputeSettings = VariableRecomputeSettings.AllReferences)
        {
            Line = line;
            GlobalIndex = index;
            SetRaw();
            SetType();
            SetNameAndData();
            SetInstance();
            VariableComputeSettings = RecomputeSettings;
        }
        /// <summary>
        /// This constructor should be used if this parameter is used like a Variable
        /// </summary>
        /// <param name="name"></param>
        /// <param name="data"></param>
        /// <param name="RecomputeSettings"></param>
        public Parameter(string name, string data, VariableRecomputeSettings RecomputeSettings = VariableRecomputeSettings.OnCreationOnly)
        {
            Raw = new Variable(name,data);
            VariableComputeSettings = RecomputeSettings;
            Type = ParamType.VariableContainer;
            Internal = Raw;
            SetInstance();
        }
        public void SetRaw()
        {
            string[] split = Line.Split(':', 2);
            Raw.Name = split[0];
            if (split.Length > 1) Raw.StringData = split[1];
        }
        /// <summary>
        /// Recomputes the internal variables if Recompute Settings is set to do so.
        /// </summary>
        public void Refresh()
        {
            if (VariableComputeSettings == VariableRecomputeSettings.AllRefreshes)
            {
                SetInstance();
            }
        }
        void SetNameAndData()
        {
            Internal.Name = Line.RemoveWhiteSpace().ToLower().Split(':', 2)[0];

            if (Raw.StringData != null)
            {
                if (Type == ParamType.Function) Internal.StringData = Line.Split(':', 2)[1].RemoveWhiteSpace().ToLower(); //function names are lower and without whitespace
                else if (Type == ParamType.Variable) Internal.StringData = Line.Split(':', 2)[1].RemoveWhiteSpace(); //variable names can have casing but no space
                else if (Type == ParamType.Workspace || Type == ParamType.Parameter) Internal.StringData = Line.Split(':', 2)[1]; //parameters are totaly raw
                else if (Type == ParamType.VariableContainer)
                {
                    Internal = Raw;
                }
            }

        }
        private void SetInstance()
        {
            if (Type == ParamType.VariableContainer) Instance = new Variable(Internal.Name, ParseAllNonsense(Internal.StringData ?? ""));
            else Instance = Internal;
        }
        public void SetType()
        {

            if (char.IsDigit(Raw.Name.ToLower().RemoveWhiteSpace()[0])) Type = ParamType.Function;
            else if (Raw.Name.ToLower().RemoveWhiteSpace() == "workspace") Type = ParamType.Workspace;
            else if (Raw.Name.ToLower().RemoveWhiteSpace() == "var") Type = ParamType.Variable;
            else Type = ParamType.Parameter;
        }

        string ParseAllNonsense(string s)
        {
            return ParseMath(
                        ParseRandom(
                            ParseVar(
                                s.Clone().ToString(), InternalVariables.CombineWith(ExternalVariables))));
        }

        //replaces Random(v1,v2) with a random single precision floating point number
        public static string ParseRandom(string s)
        {
            Random rnd = new Random();
            try
            {
                while (s.Contains("Random("))
                {
                    //Console.WriteLine("wow");
                    string[] asplit = s.Split("Random(", 2);
                    string[] randomparams = asplit[1].Split(',', 2);
                    float first = randomparams[0].toFloat();
                    float last = randomparams[1].Split(")")[0].toFloat();

                    if (last < first)
                    {
                        float f = first;
                        float l = last;
                        first = l;
                        last = f;
                    }

                    double random = rnd.NextDouble() * (last - first) + first;
                    s = asplit[0] + random + asplit[1].Split(')', 2)[1];

                }
                while (s.Contains("RandomInt("))
                {
                    //Console.WriteLine("wow");
                    string[] asplit = s.Split("RandomInt(", 2);
                    string[] randomparams = asplit[1].Split(',', 2);
                    int first = int.Parse(randomparams[0]);
                    int last = int.Parse(randomparams[1].Split(")")[0]);

                    if (last < first)
                    {
                        int f = first;
                        int l = last;
                        first = l;
                        last = f;
                    }

                    int random = rnd.Next(first, last);
                    s = asplit[0] + random + asplit[1].Split(')', 2)[1];

                }
            }
            catch { throw new FormatException($"Unable to parse Random() Line:{s}"); }

            return s;
        }
        //replaces a variable name with a number
        public static string ParseVar(string s, IEnumerable<INameStringDataPair> Variables)
        {
            foreach (var v in Variables)
            {
                while (s.Contains(v.Name))
                {
                    string[] split = s.Split(v.Name, 2);
                    s = split[0] + v.StringData + split[1];
                }
            }
            return s;
        }
        //computes things in {} i guess
        public static string ParseMath(string s)
        {
            try
            {
                while (s.ToLower().Contains("{"))
                {
                    string[] asplit = s.Split("{", 2);
                    string[] endsplit = asplit[1].Split('}', 2);
                    Expression e = new Expression(endsplit[0]);

                    s = asplit[0] + e.Evaluate().ToString() + endsplit[1];
                }
            }
            catch { throw new FormatException($"Unable to parse Math {{}} Line:{s}"); }

            return s;
        }

        Variable Internal { get; set; } = new Variable();
        Variable Instance { get; set; } = new Variable();
        public Variable Raw { get; set; } = new Variable();
        public VariableRecomputeSettings VariableComputeSettings { get; set; } = VariableRecomputeSettings.AllReferences;
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
            set 
            {
                Internal.Name = value;
            }
        }

        public string StringData
        {
            get
            {
                if (VariableComputeSettings == VariableRecomputeSettings.AllReferences)
                {
                    if (Internal.StringData == null) return null;
                    return ParseAllNonsense(Internal.StringData);
                }
                else
                {
                    if (Instance == null || Instance.StringData == null) return null;
                    return Instance.StringData;
                }
            }
            set 
            {
                Internal.StringData = value;
                SetInstance();
            }
        }
        public override string ToString()
        {
            return $@"Raw:{{ Name:{Raw.Name} Data:{Raw.StringData} }}
Internal:{{ Name:{Internal.Name} Data:{Internal.StringData} }} (private)
Instance:{{ Name:{Instance.Name} Data:{Instance.StringData} }} (private)
Output {{ Name:{Name} Data:{StringData} }}";
        }
    }
    public static class ParameterHelper
    {
        
        public static void SetInteralVariables(this Parameter[] parameters, Parameter[] variables)
        {
            foreach(var p in parameters)
            {
                p.InternalVariables = variables;
            }
        }
        
        public static void RefreshAllParameters(this Parameter[] ps)
        {
            foreach (var p in ps) p.Refresh();
        }
    }

    public class ParameterChecker
    {
        public bool WasParameterUsed { get; private set; } = false;
        public void UseParameter() => WasParameterUsed = true;
    }



    public enum VariableRecomputeSettings
    {
        AllReferences,
        AllRefreshes,
        OnCreationOnly
    }
    /// <summary>
    /// The base container for simple name and string data pairing
    /// </summary>
    public class Variable : INameStringDataPair
    {
        public Variable() { }
        public Variable(string name, string data)
        {
            Name = name;
            StringData = data;
        }
        public string Name { get; set; }
        public string StringData { get; set; }
        public override string ToString()
        {
            return $"Name:{Name} Data:{StringData}";
        }
    }
    public interface INameStringDataPair
    {
        public string Name { get; set; }
        public string StringData { get; set; }
    }
    public enum ParamType
    {
        Workspace,
        Function,
        Parameter,
        Variable,
        VariableContainer
    }
}
