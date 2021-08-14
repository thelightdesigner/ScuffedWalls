using System;
using System.Linq;

namespace ScuffedWalls
{
    public class SFunction
    {
        public Workspace InstanceWorkspace;
        public Parameter[] Parameters;
        public float Time;

        public virtual void Run() => ScuffedLogger.Error.Log("Unimplimented Function");

        public void InstantiateSFunction(Parameter[] parameters, Workspace instance, float time)
        {
            Parameters = parameters;
            InstanceWorkspace = instance;
            Time = time;
        }
        public void FunLog()
        {
            var param = GetParam("log", null, p => p);
            if (param != null) Console.WriteLine($"Log: {param}");
        }
        
        public void ConsoleOut(string Type, int Amount, float Beat, string Purpose)
        {
            Console.ForegroundColor = ConsoleColor.White;
            ScuffedLogger.Default.ScuffedWorkspace.FunctionParser.Log($"Added {Purpose} at beat {Beat} ({Amount} {Internal.MakePlural(Type, Amount)})");
            Console.ResetColor();
        }
        public T GetParam<T>(string Name, T DefaultValue, Func<string,T> Converter)
        {
            var filteredparams = Parameters.Where(p => p.Name.ToLower() == Name.ToLower());
            if (filteredparams != null && filteredparams.Any())
            {
                var converted = Converter(filteredparams.First().StringData);
                filteredparams.First().WasUsed = true;
                return converted;
            }
            else return DefaultValue;
        }
    }



}
