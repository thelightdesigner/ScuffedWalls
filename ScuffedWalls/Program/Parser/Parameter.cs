using NCalc;
using System;
using System.Collections.Generic;

namespace ScuffedWalls
{
    /// <summary>
    /// A more featured "Variable"
    /// </summary>
    public class Parameter : INameStringDataPair
    {
        public static void UnUseAll(Parameter[] parameters)
        {
            foreach (var p in parameters) p.WasUsed = false;
        }
        public static void Check(Parameter[] parameters)
        {
            foreach (var p in parameters) if (!p.WasUsed) ScuffedLogger.Warning.Log($"Parameter {p.Name} at line {p.GlobalIndex} may be unused (Mispelled?)");
        }

        public bool WasUsed { get; set; }
        public Parameter[] InternalVariables { get; set; } = new Parameter[] { };
        public static Parameter[] ExternalVariables { get; set; } = new Parameter[] { };
        public static StringFunction[] StringFunctions { get; set; } = new StringFunction[] { };
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
            Raw = new Variable(name, data);
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
            string LastAttempt = string.Empty;
            string ThisAttempt = s.Clone().ToString();
            Exception MostRecentError = null;

            //efficiency maybe?
            bool SkipVar = false;
            bool SkipMath = false;
            bool SkipFunc = false;

            while (!LastAttempt.Equals(ThisAttempt))
            {
                LastAttempt = ThisAttempt.Clone().ToString();

                if (!SkipVar) 
                {
                    try //Variables
                    {
                        KeyValuePair<bool, string> Modified = ParseVar(ThisAttempt.Clone().ToString(), InternalVariables.CombineWith(ExternalVariables));
                        if (Modified.Key)
                        {
                            ThisAttempt = Modified.Value;
                            MostRecentError = null;
                        }
                        else SkipVar = true;
                    }
                    catch (Exception e) { MostRecentError = e; }
                }

                if (!SkipMath)
                {
                    try //Math
                    {
                        KeyValuePair<bool, string> Modified = ParseMath(ThisAttempt.Clone().ToString());
                        if (Modified.Key)
                        {
                            ThisAttempt = Modified.Value;
                            MostRecentError = null;
                        }
                        else SkipMath = true;
                    }
                    catch (Exception e) { MostRecentError = e; }
                }

                if (!SkipFunc)
                {
                    try //Functions
                    {
                        KeyValuePair<bool, string> Modified = ParseFuncs(ThisAttempt.Clone().ToString());
                        if (Modified.Key)
                        {
                            ThisAttempt = Modified.Value;
                            MostRecentError = null;
                        }
                        else SkipFunc = true;
                    }
                    catch (Exception e) { MostRecentError = e; }
                }
            }
            if (MostRecentError != null) throw MostRecentError;

            return ThisAttempt;
        }


        //replaces a variable name with a different value

        public static KeyValuePair<bool, string> ParseVar(string s, IEnumerable<INameStringDataPair> Variables)
        {
            string currentvar = "";
            string BeforeModifications = s.Clone().ToString();
            try
            {
                foreach (var v in Variables)
                {
                    currentvar = v.Name;
                    while (s.Contains(v.Name))
                    {
                        string[] split = s.Split(v.Name, 2);
                        s = split[0] + v.StringData + split[1];
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception($"Error implimenting variable {currentvar} ERROR:{e.Message}");
            }
            return new KeyValuePair<bool, string>(!BeforeModifications.Equals(s), s);
        }

        //computes things in {} i guess, uses recursion when appropriate to attempt to compute all possible usages
        public static KeyValuePair<bool, string> ParseMath(string s)
        {
            string BeforeModifications = s.Clone().ToString();
            string MathStringContents = null;
            BracketAnalyzer br = new BracketAnalyzer(s, '{', '}');
            try
            {
                while (s.ToLower().Contains("{"))
                {
                    br.FullLine = s;
                    br.FocusFirst();
                    MathStringContents = br.TextInsideOfBrackets;
                    Expression e = new Expression(br.TextInsideOfBrackets);

                    s = br.TextBeforeFocused + e.Evaluate().ToString() + br.TextAfterFocused;
                }
            }
            catch (Exception e)
            {
                //if it already made a modification, return anyways
                if (!BeforeModifications.Equals(s)) return new KeyValuePair<bool, string>(!BeforeModifications.Equals(s), s);
                else 
                {
                    //recursion to attempt to parse deeper math expressions, only if the first outer attempt failed
                    try
                    {
                        KeyValuePair<bool, string> Attempt = ParseMath(MathStringContents);
                        if (Attempt.Key) s = br.TextBeforeFocused + br.OpeningBracket + Attempt.Value + br.ClosingBracket + br.TextAfterFocused;
                        else throw e;
                    }
                    catch
                    {
                        throw new FormatException($"Error parsing Math {{}} Line:{s} ERROR:{e.Message}");
                    }
                }
            }

            return new KeyValuePair<bool, string>(!BeforeModifications.Equals(s), s);
        }

        public static KeyValuePair<bool, string> ParseFuncs(string s)
        {
            StringFunction currentFunc = null;
            string BeforeModifications = s.Clone().ToString();
            string FuncStringInternals = null;
            BracketAnalyzer br = new BracketAnalyzer(s, '(', ')');
            try
            {
                foreach (var func in StringFunctions)
                {
                    currentFunc = func;
                    while (s.Contains(func.Name + "("))
                    {
                        br.FullLine = s;
                        br.FocusFirst(func.Name + "(");
                        FuncStringInternals = br.TextInsideOfBrackets;

                        string[] paramss = br.TextInsideOfBrackets.Split(',');
                        s = br.TextBeforeFocused.Substring(0, br.TextBeforeFocused.Length - func.Name.Length) + currentFunc.FunctionAction(new ModChart.ValuePair<string[], string>() { Main = paramss ,Extra = br.TextInsideOfBrackets }) + br.TextAfterFocused;
                    }
                }
            }
            catch (Exception e)
            {
                if (!BeforeModifications.Equals(s)) return new KeyValuePair<bool, string>(!BeforeModifications.Equals(s), s);
                else
                {
                    try
                    {
                        KeyValuePair<bool, string> Attempt = ParseFuncs(FuncStringInternals);
                        if (Attempt.Key) s = br.TextBeforeFocused + br.OpeningBracket + Attempt.Value + br.ClosingBracket + br.TextAfterFocused;
                        else throw e;
                    }
                    catch
                    {
                        throw new Exception($"Error executing internal function {currentFunc.Name} ERROR:{e.Message}");
                    }
                }
            }
            return new KeyValuePair<bool, string>(!BeforeModifications.Equals(s), s);
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
            foreach (var p in parameters)
            {
                p.InternalVariables = variables;
            }
        }
        public static void RefreshAllParameters(this Parameter[] ps)
        {
            foreach (var p in ps) p.Refresh();
        }
    }
    /// <summary>
    /// Holds instructions for parsing functions called from strings and replacing with values
    /// </summary>
    

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
