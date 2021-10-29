using ModChart;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ScuffedWalls
{
    public class ScuffedFunction
    {
        public ScuffedFunction()
        {
            Variables = new TreeList<AssignableInlineVariable>(AssignableInlineVariable.Exposer);
        }
        public Workspace InstanceWorkspace { get; private set; }
        public TreeList<Parameter> UnderlyingParameters { get; private set; }
        public Parameter DefiningParameter { get; private set; }
        public TreeList<AssignableInlineVariable> Variables { get; }
        public float Time { get; set; }
        public virtual void Run() => ScuffedWalls.Print("Unimplimented Function", ScuffedWalls.LogSeverity.Warning);
        public MapStats Stats { get; private set; }
        public void InstantiateSFunction(TreeList<Parameter> parameters, Parameter defining, Workspace instance, float time)
        {
            UnderlyingParameters = parameters;
            DefiningParameter = defining;
            InstanceWorkspace = instance;
            Time = time;
            Stats = new MapStats();
            if (UnderlyingParameters != null) foreach (var param in UnderlyingParameters) param.Variables.Register(Variables);
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
        public void RegisterChanges(string Type, int Amount)
        {
            Stats.AddStat(Type, Amount);
           // ScuffedWalls.Print($"Added {Purpose} at beat {Beat} ({Amount} {Type.MakePlural(Amount)})", Color: ConsoleColor.White, StackFrame: new StackTrace().GetFrame(1));
        }
        public T GetParam<T>(string Name, T DefaultValue, Func<string,T> Converter)
        {
            Parameter result = UnderlyingParameters.Get(Name.ToLower());
            if (result != null)
            {
                result.WasUsed = true;
                return Converter(result.StringData);
            }
            return DefaultValue;
        } 
    }



}
