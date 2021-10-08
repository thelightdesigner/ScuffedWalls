using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ScuffedWalls
{
    public class ScuffedFunction
    {
        public Workspace InstanceWorkspace { get; private set; }
        public List<Parameter> UnderlyingParameters { get; private set; }
        public Parameter DefiningParameter { get; private set; }
        public float Time { get; set; }
        public virtual void Run() => ScuffedWalls.Print("Unimplimented Function", ScuffedWalls.LogSeverity.Warning);
        public void InstantiateSFunction(List<Parameter> parameters, Parameter defining, Workspace instance, float time)
        {
            UnderlyingParameters = parameters;
            DefiningParameter = defining;
            InstanceWorkspace = instance;
            Time = time;
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
            ScuffedWalls.Print($"Added {Purpose} at beat {Beat} ({Amount} {Type.MakePlural(Amount)})", Color: ConsoleColor.White, StackFrame: new StackTrace().GetFrame(1));
        }
        public T GetParam<T>(string Name, T DefaultValue, Func<string,T> Converter)
        {
            var filteredparams = UnderlyingParameters.Where(p => p.Name.ToLower() == Name.ToLower());
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
