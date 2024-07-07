using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScuffedWalls
{
    public static class StringParser
    {
        public static object ParseAny(string data)
        {
            //if (float.TryParse(data, out float parsedData)) return parsedData; //ddddd.dddd
            //else if ()
            return null;
        }

        public static T ParseAs<T>(string data)
        {
            return default(T);
        }
        public static object ParseAsEither<T,K>(string data)
        {
            return default(T);
        }
    }
}
