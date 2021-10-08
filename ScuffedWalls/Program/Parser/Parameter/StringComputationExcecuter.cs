using NCalc;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScuffedWalls
{
    public class StringComputationExcecuter
    {
        private static readonly Lookup<StringFunction> _stringFunctions = new Lookup<StringFunction>(StringFunction.Functions, StringFunction.Exposer);
        public Lookup<AssignableInlineVariable> Variables { get; }
        public Lookup<AssignableInlineVariable> Constants { get; }
        private Lookup<AssignableInlineVariable> _allVars => new Lookup<AssignableInlineVariable>(Variables.CombineWith(Constants), AssignableInlineVariable.Exposer);
        public bool HandleExceptions { get; private set; }
        public StringComputationExcecuter(Lookup<AssignableInlineVariable> vars, Lookup<AssignableInlineVariable> consts = null, bool handleExceptions = false)
        {
            Variables = vars;
            Constants = consts ?? new Lookup<AssignableInlineVariable>(AssignableInlineVariable.Exposer);
            HandleExceptions = handleExceptions;
        }
        public string Parse(string Line)
        {
            string LastAttempt = string.Empty;
            string ThisAttempt = Line.Clone().ToString();
            Exception MostRecentError = null;

            while (!LastAttempt.Equals(ThisAttempt)) //if we break from this, nothing changed so there is nothing more to do
            {
                LastAttempt = ThisAttempt.Clone().ToString();

                try //Variables
                {
                    KeyValuePair<bool, string> Modified = ParseVar(ThisAttempt.Clone().ToString(), _allVars);
                    if (Modified.Key) //CASE 1: string was modified with no error; last error doesnt count because it was resolved
                    {
                        ThisAttempt = Modified.Value;
                        MostRecentError = null;
                    }
                }
                catch (Exception e) { MostRecentError = e; } //CASE 3: string wasnt modified with an error; cache error, we try again later

                try //Math
                {
                    KeyValuePair<bool, string> Modified = ParseMath(ThisAttempt.Clone().ToString());
                    if (Modified.Key)
                    {
                        ThisAttempt = Modified.Value;
                        MostRecentError = null;
                    }
                }
                catch (Exception e) { MostRecentError = e; }

                try //Functions
                {
                    KeyValuePair<bool, string> Modified = ParseFuncs(ThisAttempt.Clone().ToString());
                    if (Modified.Key)
                    {
                        ThisAttempt = Modified.Value;
                        MostRecentError = null;
                    }
                }
                catch (Exception e) { MostRecentError = e; }

            }
            if (MostRecentError != null && !HandleExceptions) throw MostRecentError; //if there is still an error, one of the steps couldnt ever continue 

            return ThisAttempt;
        }
        public static KeyValuePair<bool, string> ParseVar(string s, Lookup<AssignableInlineVariable> variables)
        {
            string currentvar = "";
            string BeforeModifications = s.Clone().ToString();
            try
            {
                foreach (var v in variables)
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
                foreach (var func in _stringFunctions)
                {
                    currentFunc = func;
                    while (s.Contains(func.Name + "("))
                    {
                        br.FullLine = s;
                        br.FocusFirst(func.Name + "(");
                        FuncStringInternals = br.TextInsideOfBrackets;

                        string[] paramss = br.TextInsideOfBrackets.Split(',');
                        s = br.TextBeforeFocused.Substring(0, br.TextBeforeFocused.Length - func.Name.Length) + currentFunc.FunctionAction(new ModChart.ValuePair<string[], string>() { Main = paramss, Extra = br.TextInsideOfBrackets }) + br.TextAfterFocused;
                    }
                }
            }
            catch (Exception e)
            {
                if (!BeforeModifications.Equals(s)) return new KeyValuePair<bool, string>(true, s);
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

    }
}
