using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace ScuffedWalls.Functions
{
    class FuncUtils
    {
        public static Func<string, float> FloatConverter => value => float.Parse(value);
        public static Func<string, bool> BoolConverter => value => bool.Parse(value);
        public static Func<string, int> IntConverter => value => int.Parse(value);
        public static Func<string, string> StringConverter => value => value;

    }
}
