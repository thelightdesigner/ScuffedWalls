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
        protected Workspace InstanceWorkspace { get; private set; }
        protected FunctionRequest Request { get; private set; }
        protected TreeList<Parameter> UnderlyingParameters => Request.UnderlyingParameters;
        protected Parameter DefiningParameter => Request.DefiningParameter;
        protected TreeList<AssignableInlineVariable> Variables { get; }
        protected int RepeatCount { get; private set; }
        protected float Time { get; set; }
        /// <summary>
        /// Called once.
        /// </summary>
        protected virtual void Init() { }
        /// <summary>
        /// Called many times based on "repeat". The code here may be called 1000+ times, keep it lite.
        /// </summary>
        protected virtual void Update() { }
        /// <summary>
        /// Called once on function end.
        /// </summary>
        protected virtual void Finish() { }
        public MapStats Stats { get; private set; }
        public void SetTime(float time)
        {
            Time = time;
        }
        public void InstantiateSFunction(FunctionRequest request, Workspace instance, float time, int repeat)
        {
            Request = request;
            InstanceWorkspace = instance;
            Time = time;
            Stats = new MapStats();
            RepeatCount = repeat;
            if (UnderlyingParameters != null) foreach (var param in UnderlyingParameters) param.Variables.Register(Variables);
            Init();
        }
        public void Repeat()
        {
            FunLog();
            Update();
        }
        public void Terminate()
        {
            Finish();
        }
        private void FunLog()
        {
            var param = GetParam("log", null, p => p);
            if (param != null) Console.WriteLine($"Log: {param}");
        }
        protected void AddRefresh(string file)
        {
            if (GetParam("refreshonsave", false, p => bool.Parse(p)))
            {
                Utils.FilesToChange.Add(
                    new FileChangeDetector(new System.IO.FileInfo(file)));
            }
        }
        protected void RegisterChanges(string Type, int Amount)
        {
            Stats.AddStat(Type, Amount);
           // ScuffedWalls.Print($"Added {Purpose} at beat {Beat} ({Amount} {Type.MakePlural(Amount)})", Color: ConsoleColor.White, StackFrame: new StackTrace().GetFrame(1));
        }
        protected T GetParam<T>(string Name, T DefaultValue, Func<string,T> Converter)
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
