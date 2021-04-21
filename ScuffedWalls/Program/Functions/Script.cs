using ModChart;
using ModChart.Wall;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScuffedWalls.Functions
{
    [ScuffedFunction("Script")]
    class Script : SFunction
    {
        public void Run() 
        {
            //put scripts here

            /*
            foreach (var light in InstanceWorkspace.Lights) light._time = light._time.toFloat() * 2f + 24 + 7.25 + (1f/16f);

            float time = 0;
            List<BeatMap.Event> lights = new List<BeatMap.Event>();

            foreach (var light in InstanceWorkspace.Lights)
            {
                float lgtim = light._time.toFloat();

                if (lgtim - time < 0.26f && lgtim - time > 0.005f)
                {
                    lights.Add(new BeatMap.Event()
                    {
                        _time = light._time,
                        _type = 8,
                        _value = 5
                    });
                }

                time = lgtim;
            }

            Console.WriteLine(lights.Count);
            InstanceWorkspace.Lights.AddRange(lights);

            Console.WriteLine("hai");

            */

        }

    }
}
