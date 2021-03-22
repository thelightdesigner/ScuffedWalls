using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace ScuffedWalls.Functions
{
    [ScuffedFunction("Test")]
    class Test : SFunction
    {
        public int SomeVariable;
        public string SomeOtherThing;
        public object[] ArrayTest;
        public object[] ArrayTest2;
        public void Run()
        {
            Console.WriteLine($"{SomeVariable} {SomeOtherThing} {JsonSerializer.Serialize(ArrayTest)} {JsonSerializer.Serialize(ArrayTest2)}");
        }
    }
}
