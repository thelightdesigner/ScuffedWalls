using System;
using System.Collections.Generic;
using System.Text;

namespace ScuffedWalls
{
    public class SFunction
    {
        public Workspace InstanceWorkspace;
        public Parameter[] Parameters;
        public float Time;
        
        public void InstantiateSFunction(Parameter[] parameters, Workspace instance, float time)
        {
            Parameters = parameters;
            InstanceWorkspace = instance;
            Time = time;
            //foreach (var s in parameters) Console.WriteLine(s);
        }
        public void ConsoleOut(string Type, int Amount, float Beat, string Purpose)
        {
            Console.ForegroundColor = ConsoleColor.White;
            ScuffedLogger.ScuffedWorkspace.FunctionParser.Log($"Added {Purpose} at beat {Beat} ({Amount} {Type}s)");
            Console.ResetColor();
        }
    }



}
