using System;
using System.Collections.Generic;
using System.Linq;

namespace ScuffedWalls
{
    public class ScuffedFunction
    {
        public Workspace InstanceWorkspace { get; private set; }
        public FunctionParser InstanceParser { get; private set; }
        public List<Parameter> Parameters { get; private set; }
        public float Time { get; set; }

        public virtual void Run() => ScuffedWalls.Print("Unimplimented Function", ScuffedWalls.LogSeverity.Warning);
        public virtual void Repeat() { }
        
        
        public void InstantiateSFunction(List<Parameter> parameters, Workspace instance, float time, FunctionParser parser)
        {
            Parameters = parameters;
            InstanceWorkspace = instance;
            Time = time;
            InstanceParser = parser;
        }
        public void FunLog()
        {
            var param = GetParam("log", null, p => p);
            if (param != null) Console.WriteLine($"Log: {param}");
        }

        public void AddRefresh(string file)
        {
            if (GetParam("refreshonsave", false, p => bool.Parse(p)))
            {
                Utils.FilesToChange.Add(
                    new FileChangeDetector(new System.IO.FileInfo(file)));
            }
        }
        
        public void ConsoleOut(string Type, int Amount, float Beat, string Purpose)
        {
            ScuffedWalls.Print($"Added {Purpose} at beat {Beat} ({Amount} {Internal.MakePlural(Type, Amount)})", Color: ConsoleColor.White);
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
