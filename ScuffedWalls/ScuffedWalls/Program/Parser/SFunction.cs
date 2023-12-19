using ModChart;
using ScuffedWalls.Functions;
using System;
using System.Diagnostics;
using System.Linq;
using System.Numerics;

namespace ScuffedWalls
{
    public class SFunction
    {
        public Workspace InstanceWorkspace;
        public TreeDictionary Parameters;
        public float Time;
        public string Name;

        public virtual void Run() => ScuffedWalls.Print("Unimplimented Function", ScuffedWalls.LogSeverity.Notice);

        public void InstantiateSFunction(TreeDictionary parameters, Workspace instance, float time, string name)
        {
            Parameters = parameters;
            InstanceWorkspace = instance;
            Time = time;
            Name = name;
        }
        public Vector2 GetStartEndTime()
        {
            return new Vector2(Time, GetParam("tobeat", float.PositiveInfinity, FuncUtils.FloatConverter));
        }
        public void FunLog()
        {
            var param = GetParam("log", null, p => p);
            if (param != null) Console.WriteLine($"Log: {param}");
        }

        public void ConsoleOut(string Type, int Amount, float Beat, string Purpose)
        {
            ScuffedWalls.Print($"Added {Purpose} at beat {Beat} ({Amount} {Extensions.MakePlural(Type, Amount)})", Color: ConsoleColor.White, StackFrame: new StackTrace().GetFrame(1));
        }
        public T GetParam<T>(string Name, T DefaultValue, Func<string, T> Converter)
        {
            string param = Parameters.at<Parameter>(Name).StringData;
            return param == null ? DefaultValue : Converter(param);
        }
    }



}
