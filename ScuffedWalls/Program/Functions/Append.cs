using ModChart;
using ModChart.Event;
using ModChart.Note;
using ModChart.Wall;
using System;
using System.Linq;

namespace ScuffedWalls.Functions
{
    [ScuffedFunction("AppendToAllWallsBetween","AppendWalls","AppendWall")]
    class AppendWalls : SFunction
    {
        public Variable WallIndex;
        public void SetParameters()
        {
            WallIndex = new Variable { Name = "index", Data = "1" };
            Parameters = Parameters.AddVariables(new Variable[] { WallIndex });
        }
        public void Run()
        {
            SetParameters();
            AppendTechnique type = 0;
            VariablePopulator internalvars = new VariablePopulator();
            float starttime = Time;
            float endtime = float.PositiveInfinity;

            foreach (var p in Parameters)
            {
                if (p.Name == "appendtechnique") type = (AppendTechnique)Convert.ToInt32(p.Data);
                else if (p.Name == "tobeat") { endtime = Convert.ToSingle(p.Data); }
            }

            int i = 0;
            InstanceWorkspace.Walls = InstanceWorkspace.Walls.Select(obj =>
            {
                i++;

                internalvars.CurrentWall = obj;

                Parameters = Parameters.Select(p => { p.InternalVariables = internalvars.Properties; return p; }).ToArray();

                if (obj._time.toFloat() >= starttime && obj._time.toFloat() <= endtime)
                {
                    WallIndex.Data = i.ToString();
                    return obj.Append(Parameters.CustomDataParse(new BeatMap.Obstacle()), type);
                }
                else return obj;

            }).Cast<BeatMap.Obstacle>().ToList();

            ScuffedLogger.ScuffedWorkspace.FunctionParser.Log($"Appended {i} walls from beats {starttime} to {endtime}");
        }
    }
    [ScuffedFunction("AppendToAllNotesBetween", "AppendNotes", "AppendNote")]
    class AppendNotes : SFunction
    {
        public Variable WallIndex;
        public void SetParameters()
        {
            WallIndex = new Variable { Name = "index", Data = "1" };
            Parameters = Parameters.AddVariables(new Variable[] { WallIndex });
        }
        public void Run()
        {
            SetParameters();
            int type = 0;
            VariablePopulator internalvars = new VariablePopulator();
            bool b = false;
            float starttime = Time;
            float endtime = float.PositiveInfinity;
            int[] notetype = { 0, 1, 2, 3 };

            foreach (var p in Parameters)
            {
                switch (p.Name)
                {
                    case "appendtechnique":
                        type = Convert.ToInt32(p.Data);
                        break;
                    case "tobeat":
                        endtime = Convert.ToSingle(p.Data); b = true;
                        break;
                    case "notecolor":
                        if (p.Data == "red") notetype = new int[] { 0 };
                        else if (p.Data == "blue") notetype = new int[] { 1 };
                        else if (p.Data == "bomb") notetype = new int[] { 2 };
                        else notetype = p.Data.Split(",").Select(a => { return Convert.ToInt32(a); }).ToArray();
                        break;
                }
            }
            int i = 0;
            InstanceWorkspace.Notes = InstanceWorkspace.Notes.Select(obj =>
            {
                i++;
                internalvars.CurrentNote = obj;
                WallIndex.Data = i.ToString();
                Parameters = Parameters.Select(p => { p.InternalVariables = internalvars.Properties; return p; }).ToArray();
                if (obj._time.toFloat() >= starttime && obj._time.toFloat() <= endtime) return obj.Append(Parameters.CustomDataParse(new BeatMap.Note()), (AppendTechnique)type);
                else return obj;
            }).Cast<BeatMap.Note>().ToList();

            ScuffedLogger.ScuffedWorkspace.FunctionParser.Log($"Appended {i} notes from beats {starttime} to {endtime}");
        }
    }
    [ScuffedFunction("AppendToAllEventsBetween","AppendLights","AppendEvent","AppendEvents")]
    class AppendEvents : SFunction
    {
        public Variable WallIndex;
        public void SetParameters()
        {
            WallIndex = new Variable { Name = "index", Data = "1" };
            Parameters = Parameters.AddVariables(new Variable[] { WallIndex });
        }
        public void Run()
        {
            SetParameters();
            int type = 0;
            int[] lightypes = { 1, 2, 3, 4, 5, 6, 7, 8 };
            float starttime = Time;
            bool rainbow = false;
            bool props = false;
            VariablePopulator internalvars = new VariablePopulator();
            float Rfactor = 1f;
            float Pfactor = 1f;
            float endtime = float.PositiveInfinity;

            foreach (var p in Parameters)
            {
                if (p.Name == "appendtechnique") type = Convert.ToInt32(p.Data);
                else if (p.Name == "tobeat") { endtime = Convert.ToSingle(p.Data); }
                else if (p.Name == "converttoprops") props = bool.Parse(p.Data);
                else if (p.Name == "converttorainbow") rainbow = bool.Parse(p.Data);
                else if (p.Name == "rainbowfactor") Rfactor = Convert.ToSingle(p.Data);
                else if (p.Name == "propfactor") Pfactor = Convert.ToSingle(p.Data);
                else if (p.Name == "lighttype") lightypes = p.Data.Split(",").Select(a => { return Convert.ToInt32(a); }).ToArray();
            }

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
                WallIndex.Data = i.ToString();
                Parameters = Parameters.Select(p => { p.InternalVariables = internalvars.Properties; return p; }).ToArray();
                if (obj._time.toFloat() >= starttime && obj._time.toFloat() <= endtime) return (BeatMap.Event)obj.Append(Parameters.CustomDataParse(new BeatMap.Event()), (AppendTechnique)type);
                else return obj;
            }).ToList();

            ScuffedLogger.ScuffedWorkspace.FunctionParser.Log($"Appended {i} events from beats {starttime} to {endtime}");
        }
    }


}
