using ModChart;
using System;
using System.Collections.Generic;
using static ScuffedWalls.StringParser;
using static ModChart.DifficultyV3;

namespace ScuffedWalls
{
    public class CustomDataParser
    {

        public static readonly CustomDataParser Instance = new CustomDataParser();

        //see this, convert to that 
        private static readonly Dictionary<string, Func<string, object>> heckStringToTypeParsers = new()
        {
            [coordinates] = ParseAs<float[]>,
            [worldRotation] = ParseAs<float[]>,
            //...
        };

        public SDictionary ParseParameters(IEnumerable<Parameter> parameters)
        {
            SDictionary customData = new();
            foreach (var parameter in parameters)
            {
                if (heckStringToTypeParsers.TryGetValue(parameter.Name, out Func<string, object>? converter))
                {
                    customData[correctCaseHeckKeyword(parameter.Name)] = converter(parameter.StringData);
                }
            }
        }

    }
}