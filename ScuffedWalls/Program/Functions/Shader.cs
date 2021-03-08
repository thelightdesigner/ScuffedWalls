using System;
using System.Collections.Generic;
using System.Text;

namespace ScuffedWalls.Functions
{
    [ScuffedFunction("Shader")]
    class Shader : SFunction
    {
        public void Run()
        {
            //your custom scripts can go here
            ConsoleOut("Shader",0,Time,"Shader");
        }
    }
}
