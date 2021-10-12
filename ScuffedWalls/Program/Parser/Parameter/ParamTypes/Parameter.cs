using NCalc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ScuffedWalls
{
    public class Parameter : INameStringDataPair, ICloneable
    {
        public static Func<Parameter, string> Exposer => var => var.Name;
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
        public Lookup<AssignableInlineVariable> Variables { get; private set; }
        public static Lookup<AssignableInlineVariable> ConstantVariables { get; private set; } = new Lookup<AssignableInlineVariable>(AssignableInlineVariable.Exposer);
        public Parameter(string line, int index)
        {
            Line = line;
            GlobalIndex = index;
            Raw = Variable.Parse(line);
            Variables = new Lookup<AssignableInlineVariable>(AssignableInlineVariable.Exposer);
            Computer = new StringComputationExcecuter(Variables);
        }
        public Variable Raw { get; private set; } = new Variable();
        public Variable Clean => new Variable(Name?.ToLower().RemoveWhiteSpace(), StringData?.ToLower().RemoveWhiteSpace());
        public int GlobalIndex { get; private set; }
        public string Line { get; private set; } = "";
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
            get => Computer.Parse(Raw.StringData);
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
