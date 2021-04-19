using ModChart;
using ModChart.Event;
using ModChart.Note;
using ModChart.Wall;
using System;
using System.Linq;
using System.Text.Json;
using static ScuffedWalls.ScuffedLogger.Default.ScuffedWorkspace.FunctionParser;

namespace ScuffedWalls.Functions
{
    [ScuffedFunction("AppendToAllWallsBetween","AppendWalls","AppendWall")]
    class AppendWalls : SFunction
    {
        public Parameter WallIndex;
        Parameter[] ps;
        public void SetParameters()
        {
            WallIndex = new Parameter("index","1");
            ps = new Parameter[] { WallIndex };
        }
        public void Run()
        {
            SetParameters();
            AppendTechnique type = GetParam("appendtechnique", AppendTechnique.NoOverwrites, p => (AppendTechnique)int.Parse(p));
            VariablePopulator internalvars = new VariablePopulator();
            float starttime = Time;
            float endtime = GetParam("tobeat", float.PositiveInfinity, p => float.Parse(p));
            string tracc = GetParam("ontrack", null, p => p);

            int i = 0;
            InstanceWorkspace.Walls = InstanceWorkspace.Walls.Select(obj =>
            {
                i++;

                internalvars.CurrentWall = obj;

                Parameters = Parameters.Select(p => { p.InternalVariables = internalvars.Properties.CombineWith(ps).ToArray(); return p; }).ToArray();
                
                if (obj._time.toFloat() >= starttime && obj._time.toFloat() <= endtime && (tracc == null || tracc.Equals(obj._customData._track)))
                {
                    WallIndex.StringData = i.ToString();
                    return obj.Append(Parameters.CustomDataParse(new BeatMap.Obstacle()), type);
                }
                else return obj;

            }).Cast<BeatMap.Obstacle>().ToList();

            Log($"Appended {i} walls from beats {starttime} to {endtime}");
            Parameter.ExternalVariables.RefreshAllParameters();
        }
    }
    [ScuffedFunction("AppendToAllNotesBetween", "AppendNotes", "AppendNote")]
    class AppendNotes : SFunction
    {
        public Parameter WallIndex;
        Parameter[] ps;
        public void SetParameters()
        {
            WallIndex = new Parameter("index", "1");
            ps = new Parameter[] { WallIndex };
        }
        public void Run()
        {
            SetParameters();
            int type = GetParam("appendtechnique",(int) AppendTechnique.NoOverwrites, p => int.Parse(p));
            VariablePopulator internalvars = new VariablePopulator();
            float starttime = Time;
            float endtime = GetParam("tobeat", float.PositiveInfinity, p => float.Parse(p));
            string tracc = GetParam("ontrack", null, p => p);
            int[] notetype = GetParam("notetype", new int[] { 0, 1, 2, 3 }, p => p.Split(",").Select(a => Convert.ToInt32(a)).ToArray());

            int i = 0;
            InstanceWorkspace.Notes = InstanceWorkspace.Notes.Select(obj =>
            {
                i++;
                internalvars.CurrentNote = obj;
                WallIndex.StringData = i.ToString();
                Parameters = Parameters.Select(p => { p.InternalVariables = internalvars.Properties.CombineWith(ps).ToArray(); return p; }).ToArray();
                if (obj._time.toFloat() >= starttime && obj._time.toFloat() <= endtime && (tracc == null || tracc.Equals(obj._customData._track))) return obj.Append(Parameters.CustomDataParse(new BeatMap.Note()), (AppendTechnique)type);
                else return obj;
            }).Cast<BeatMap.Note>().ToList();

            Log($"Appended {i} notes from beats {starttime} to {endtime}");
            Parameter.ExternalVariables.RefreshAllParameters();
        }
    }
    [ScuffedFunction("AppendToAllEventsBetween","AppendLights","AppendEvent","AppendEvents")]
    class AppendEvents : SFunction
    {
        public Parameter WallIndex;
        Parameter[] ps;
        public void SetParameters()
        {
            WallIndex = new Parameter("index", "1");
            ps = new Parameter[] { WallIndex };
        }
        public void Run()
        {
            SetParameters();
            int type = GetParam("appendtechnique", (int)AppendTechnique.NoOverwrites, p => int.Parse(p));
            int[] lightypes = GetParam("lighttype", new int[] { 0, 1, 2, 3,4,5,6,7,8 }, p => p.Split(",").Select(a => Convert.ToInt32(a)).ToArray());
            float starttime = Time;
            bool rainbow = GetParam("converttorainbow",false,p => bool.Parse(p));
            VariablePopulator internalvars = new VariablePopulator();
            float Rfactor = GetParam("rainbowfactor", 1, p => float.Parse(p));
            float endtime = GetParam("tobeat", float.PositiveInfinity, p => float.Parse(p));


            if (rainbow)
            {
                InstanceWorkspace.Lights = InstanceWorkspace.Lights.Select(obj =>
                {
                    if (obj._time.toFloat() >= starttime && obj._time.toFloat() <= endtime)
                    {
                        return (BeatMap.Event)obj.Append(new BeatMap.Event()
                        {
                            _customData = new BeatMap.CustomData()
                            {
                                _color = new object[]
                                {
                                    0.5f * Math.Sin(Math.PI * Rfactor * obj.GetTime()) + 0.5f,
                                    0.5f * Math.Sin((Math.PI * Rfactor * obj.GetTime()) - (Math.PI * (2f / 3f))) + 0.5f,
                                    0.5f * Math.Sin((Math.PI * Rfactor * obj.GetTime()) - (Math.PI * (4f / 3f))) + 0.5f,
                                    1
                                }
                            }
                        }, AppendTechnique.Overwrites);
                    }
                    else return obj;
                }).ToList();
            }
            int i = 0;
            InstanceWorkspace.Lights = InstanceWorkspace.Lights.Select(obj =>
            {
                i++;
                internalvars.CurrentEvent = obj;
                WallIndex.StringData = i.ToString();
                Parameters = Parameters.Select(p => { p.InternalVariables = internalvars.Properties.CombineWith(ps).ToArray(); return p; }).ToArray();
                
                if (obj._time.toFloat() >= starttime && obj._time.toFloat() <= endtime) return (BeatMap.Event)obj.Append(Parameters.CustomDataParse(new BeatMap.Event()), (AppendTechnique)type);
                else return obj;
            }).ToList();

            Log($"Appended {i} events from beats {starttime} to {endtime}");
            Parameter.ExternalVariables.RefreshAllParameters();
        }
    }
}
