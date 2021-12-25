using ModChart;
using ModChart.Wall;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace ScuffedWalls
{
    public class StringFunction
    {
        public static Random RandomInstance = new Random();
        public string Name { get; set; } //name of the func
        public Func<string, string> FunctionAction { get; set; } //convert from params to output string
        public static Func<StringFunction, string> Exposer => sfunc => sfunc.Name;
        public static StringFunction[] Functions => new StringFunction[]
            {
                new StringFunction()
                {
                    Name = "RepeatPointDefinition",
                    FunctionAction = InputArgs =>
                    {
                        int indexoflast = InputArgs.LastIndexOf(",");

                        string pd = InputArgs.Substring(0,indexoflast);

                        int repcount = int.Parse(InputArgs.Substring(indexoflast + 1,InputArgs.Length - indexoflast - 1));

                        TreeList<AssignableInlineVariable> vars = new TreeList<AssignableInlineVariable>(AssignableInlineVariable.Exposer);
                        AssignableInlineVariable repeat = new AssignableInlineVariable("reppd", "0");
                        vars.Add(repeat);
                        StringComputationExcecuter computer = new StringComputationExcecuter(vars, true);

                        List<string> points = new List<string>();
                        for (int i = 0; i < repcount; i++)
                        {
                            repeat.StringData = i.ToString();
                            points.Add(computer.Parse(pd));
                            AssignableInlineVariable.Ping();
                        }

                        return string.Join(',',points);
                    }
                },
                new StringFunction()
                {
                    Name = "MultPointDefinition",
                    FunctionAction = InputArgs =>
                    {
                        string[] spli = InputArgs.Split("],",2);
                        string pd = spli[0] + "]";
                        float val = spli[1].ToFloat();

                        object[] PointDefinition = JsonSerializer.Deserialize<object[]>(pd);
                        for(int i = 0; i < PointDefinition.Length; i++) if(float.TryParse(PointDefinition[i].ToString(),out float result)) PointDefinition[i] = result * val;

                        return JsonSerializer.Serialize(PointDefinition);
                    }
                },
                new StringFunction()
                {
                    Name = "HSLtoRGB",
                    FunctionAction = InputArgs =>
                    {
                        string[] parameters = InputArgs.Split(',');
                        float H = parameters[0].ToFloat();
                        float S = parameters.Length > 1 ? parameters[1].ToFloat() : 1f;
                        float L = parameters.Length > 2 ? parameters[2].ToFloat() : 0.5f;
                        float A = parameters.Length > 3 ? parameters[3].ToFloat() : 1f;
                        string AdditionalValues = parameters.Length > 4 ? ","+ string.Join(',', parameters.Slice(4,parameters.Length)) : string.Empty;

                        Color p = Color.HslToRGB(H,S,L);

                        return $"[{p.R},{p.G},{p.B},{A}{AdditionalValues}]";
                    }
                },
                new StringFunction()
                {
                    Name = "OrderPointDefinitions",
                    FunctionAction = InputArgs =>
                    {
                        object[][] PointDefinition = JsonSerializer.Deserialize<object[][]>($"[{InputArgs}]");
                        string serial = JsonSerializer.Serialize( PointDefinition.OrderBy(p => gettimevalue(p)));
                        return serial.Substring(1,serial.Length-2);

                        float gettimevalue(object[] point)
                        {
                            float last = 0;
                            foreach(var val in point) if(float.TryParse(val.ToString(),out float result)) last = result;
                            return last;
                        }
                    }
                },
                new StringFunction()
                {
                    Name = "Random",
                    FunctionAction = InputArgs =>
                    {
                        string[] parameters = InputArgs.Split(',');
                        float first = parameters[0].ToFloat();
                        float last = parameters[1].ToFloat();
                         if (parameters.Length > 2 && last < first)
                         {
                             float f = first;
                             float l = last;
                             first = l;
                             last = f;
                         }

                         return (RandomInstance.NextDouble() * (last - first) + first).ToString();

                    }
                },
                new StringFunction()
                {
                    Name = "RandomInt",
                    FunctionAction = InputArgs =>
                    { 
                        string[] parameters = InputArgs.Split(',');
                        Random rnd = new Random();
                        int first = int.Parse(parameters[0]);
                        int last = int.Parse(parameters[1]);
                        if (last < first)
                        {
                            int f = first;
                            int l = last;
                            first = l;
                            last = f;
                        }

                        return rnd.Next(first,last).ToString();
                    }
                }
        };

    }
    public class BracketAnalyzer
    {
        public string TextBeforeFocused;
        public string TextAfterFocused;
        public string TextInsideOfBrackets;
        public string TextInsideWithBrackets;
        public char OpeningBracket;
        public char ClosingBracket;
        public string FullLine;

        public BracketAnalyzer(string Line, char Opening, char Closing)
        {
            FullLine = TextInsideOfBrackets = TextInsideWithBrackets = Line;
            OpeningBracket = Opening;
            ClosingBracket = Closing;
        }
        public void Focus(int Index)
        {
            int closing = GetPosOfClosingSymbol(Index);
            var splits = SplitAt2(FullLine, Index, closing);
            TextInsideWithBrackets = splits[1];
            TextInsideOfBrackets = TextInsideWithBrackets.Substring(1, TextInsideWithBrackets.Length - 2);
            TextBeforeFocused = splits[0];
            TextAfterFocused = splits[2];
        }
        public void FocusFirst()
        {
            Focus(FullLine.IndexOf(OpeningBracket));
        }
        public void FocusFirst(string Name)
        {
            Focus(FullLine.IndexOf(Name) + (Name.Length - 1));
        }
        public bool IsOpeningBracket(int i) => FullLine[i] == OpeningBracket;
        public int GetPosOfClosingSymbol(int indexofparenthesis)
        {
            char[] characters = FullLine.ToCharArray();
            int depth = 0;
            for (int i = indexofparenthesis; i < characters.Length; i++)
            {
                if (characters[i] == OpeningBracket) depth++;
                else if (characters[i] == ClosingBracket) depth--;

                if (depth == 0) return i;
            }
            throw new Exception("No closing of brackets/paranthesis!");
        }
        public static string[] SplitAt2(string s, int argpos, int argpos2) => new string[] { s.Substring(0, argpos), s.Substring(argpos, argpos2 - argpos + 1), s.Substring(argpos2 + 1) };
    }
}
