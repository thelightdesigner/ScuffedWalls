using NCalc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ScuffedWalls
{
    public class Parameter : INameStringDataPair, ICloneable
    {
        public Parameter Use()
        {
            WasUsed = true;
            return this;
        }
        public static Func<Parameter, string> Exposer => var => var.Clean.Name;
        public static void UnUseAll(IEnumerable<Parameter> parameters)
        {
            foreach (var p in parameters) p.WasUsed = false;
        }
        public static void Check(IEnumerable<Parameter> parameters)
        {
            foreach (var p in parameters) if (!p.WasUsed) ScuffedWalls.Print($"Parameter {p.Name} at line {p.GlobalIndex} may be unused (Mispelled?)", ScuffedWalls.LogSeverity.Warning);
        }
        public bool WasUsed { get; set; }
        public StringComputationExcecuter Computer { get; private set; }
        public TreeList<AssignableInlineVariable> Variables => Computer.Variables;
        public Parameter(string line, int index)
        {
            Line = line;
            GlobalIndex = index;
            Raw = Variable.Parse(line);
            Computer = new StringComputationExcecuter();
        }
        public Parameter(string name, string data)
        {
            Raw = new Variable(name, data);
        }
        public Variable Raw { get; private set; } = new Variable();
        public Variable Clean => new Variable(Raw?.Name?.ToLower().RemoveWhiteSpace(), Raw?.StringData?.ToLower().RemoveWhiteSpace());
        public int GlobalIndex { get; private set; }
        public string Line { get; private set; }
        public string Name
        {
            get => Raw.Name; //support for name computing soon
            set
            {
                Raw.Name = value;
            }
        }

        public string StringData
        {
            get => Raw.StringData != null ? Computer.Parse(Raw.StringData) : null;
            set
            {
                Raw.StringData = value;
            }
        }
        public override string ToString()
        {
            return $"{Name}:{StringData}";
        }

        public object Clone()
        {
            return new Parameter(Line, GlobalIndex);
        }

        public static void AssignVariables(IEnumerable<Parameter> parameters, TreeList<AssignableInlineVariable> variables)
        {
            foreach (var param in parameters) param.Variables.Register(variables);
        }
    }
    /*
    public static class ParameterHelper
    {

        public static void SetInteralVariables(this IEnumerable<Parameter> parameters, Parameter[] variables)
        {
            foreach (var p in parameters)
            {
                p.InternalVariables = variables;
            }
        }
        public static void RefreshAllParameters(this IEnumerable<Parameter> ps)
        {
            foreach (var p in ps) p.Refresh();
        }
    }
    /// <summary>
    /// Holds instructions for parsing functions called from strings and replacing with values
    /// </summary>
    */

    /// <summary>
    /// The base container for simple name and string data pairing
    /// </summary>
    public class Variable : INameStringDataPair, ICloneable
    {
        public static Variable Parse(string line)
        {
            Variable raw = new Variable();
            string[] split = line.Split(':', 2);
            raw.Name = split[0];
            if (split.Length > 1) raw.StringData = split[1];
            return raw;
        }
        public Variable() { }
        public Variable(string name, string data)
        {
            Name = name;
            StringData = data;
        }
        public string Name { get; set; }
        public string StringData { get; set; }

        public object Clone()
        {
            return new Variable(Name, StringData);
        }

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
}
