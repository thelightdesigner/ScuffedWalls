using ModChart;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScuffedWalls.Functions
{
    [ScuffedFunction("Note")]
    class Note : SFunction
    {
        public Variable Repeat;
        public Variable Beat;
        public void SetParameters()
        {
            Repeat = new Variable { Name = "repeat", Data = "1" };
            Beat = new Variable { Name = "time", Data = Time.ToString() };
            Parameters = Parameters.AddVariables(new Variable[] { Repeat, Beat });
        }
        public void Run()
        {
            SetParameters();
            int repeatcount = 1;
            float repeatTime = 0;
            int type = 1;
            int cutdirection = 1;
            //parse special parameters
            foreach (var p in Parameters)
            {
                switch (p.Name)
                {
                    case "type":
                        type = Convert.ToInt32(p.Data);
                        break;
                    case "cutdirection":
                        cutdirection = Convert.ToInt32(p.Data);
                        break;
                    case "repeat":
                        repeatcount = Convert.ToInt32(p.Data);
                        break;
                    case "repeataddtime":
                        repeatTime = Convert.ToSingle(p.Data);
                        break;
                }
            }
            for (float i = 0; i < repeatcount; i++)
            {
                InstanceWorkspace.Notes.Add((BeatMap.Note)new BeatMap.Note()
                {
                    _time = Time + (i * repeatTime),
                    _lineIndex = 0,
                    _lineLayer = 0,
                    _cutDirection = cutdirection,
                    _type = type
                }.Append(Parameters.CustomDataParse(new BeatMap.Note()), AppendTechnique.Overwrites));

                Repeat.Data = i.ToString();
                Beat.Data = (Time + (i * repeatTime)).ToString();
            }
            ConsoleOut("Note", repeatcount, Time, "Note");
        }
    }


}
