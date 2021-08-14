using ModChart;
using System;
using System.Linq;
using System.Text.Json;
using static ScuffedWalls.ScuffedLogger.Default.ScuffedWorkspace.FunctionParser;

namespace ScuffedWalls.Functions
{
    [ScuffedFunction("AppendToAllWallsBetween", "AppendWalls", "AppendWall")]
    class AppendWalls : SFunction
    {
        public Parameter WallIndex;
        Parameter[] ps;
        public void SetParameters()
        {
            WallIndex = new Parameter("index", "0");
            ps = new Parameter[] { WallIndex };
        }
        public override void Run()
        {
            SetParameters();

            AppendPriority type = GetParam("appendtechnique", AppendPriority.Low, p => (AppendPriority)int.Parse(p));
            VariablePopulator internalvars = new VariablePopulator();
            float starttime = Time;
            float endtime = GetParam("tobeat", float.PositiveInfinity, p => float.Parse(p));
            string tracc = GetParam("ontrack", null, p => p);
            var lineindex = GetParam("selectlineindex", new int[] { 0, 1, 2, 3, 4 }, p => p.Split(',').Select(val => int.Parse(val)));
            

            int i = 0;
            InstanceWorkspace.Walls = InstanceWorkspace.Walls.Select(obj =>
            {

                internalvars.CurrentWall = obj;

                Parameters = Parameters.Select(p => { p.InternalVariables = internalvars.Properties.CombineWith(ps).ToArray(); return p; }).ToArray();


                if (obj._time.ToFloat() >= starttime && obj._time.ToFloat() <= endtime && AppendNotes.isOnTrack(obj._customData, tracc) && lineindex.Any(num => num == obj._lineIndex.Value))
                {
                    WallIndex.StringData = i.ToString();

                    FunLog();

                    var r = obj.Append(Parameters.CustomDataParse(new BeatMap.Obstacle()), type);
                    i++;
                    return r;
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
            WallIndex = new Parameter("index", "0");
            ps = new Parameter[] { WallIndex };
        }
        public override void Run()
        {
            SetParameters();
            AppendPriority type = GetParam("appendtechnique", AppendPriority.Low, p => (AppendPriority)int.Parse(p));
            VariablePopulator internalvars = new VariablePopulator();
            float starttime = Time;
            float endtime = GetParam("tobeat", float.PositiveInfinity, p => float.Parse(p));
            string tracc = GetParam("ontrack", null, p => p);
            int[] notetype = GetParam("selecttype", new int[] { 0, 1, 2, 3 }, p => p.Split(",").Select(a => Convert.ToInt32(a)).ToArray());
            int i = 0;
            InstanceWorkspace.Notes = InstanceWorkspace.Notes.Select(obj =>
            {


                if (obj._time.ToFloat() >= starttime && obj._time.ToFloat() <= endtime && isOnTrack(obj._customData, tracc) && notetype.Any(t => t == (int)obj._type))
                {
                    WallIndex.StringData = i.ToString();
                    internalvars.CurrentNote = obj;
                    Parameters = Parameters.Select(p => { p.InternalVariables = internalvars.Properties.CombineWith(ps).ToArray(); return p; }).ToArray();

                    FunLog();

                    var r = obj.Append(Parameters.CustomDataParse(new BeatMap.Note()), type);
                    i++;
                    return r;
                }
                else return obj;
            }).Cast<BeatMap.Note>().ToList();

            Log($"Appended {i} notes from beats {starttime} to {endtime}");
            Parameter.ExternalVariables.RefreshAllParameters();
        }

        public static bool isOnTrack(TreeDictionary _customData, string track)
        {
            if (string.IsNullOrEmpty(track) || (_customData.ContainsKey("_track") && track.Equals(_customData["_track"]))) return true;
            else return false;
        }
    }
    [ScuffedFunction("AppendToAllEventsBetween", "AppendLights", "AppendEvent", "AppendEvents")]
    class AppendEvents : SFunction
    {
        public Parameter WallIndex;
        Parameter[] ps;
        public void SetParameters()
        {
            WallIndex = new Parameter("index", "0");
            ps = new Parameter[] { WallIndex };
        }
        public override void Run()
        {
            SetParameters();
            AppendPriority type = GetParam("appendtechnique", AppendPriority.Low, p => (AppendPriority)int.Parse(p));
            int[] lightypes = GetParam("selecttype", new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 }, p => p.Split(",").Select(a => Convert.ToInt32(a)).ToArray());
            float starttime = Time;
            bool rainbow = GetParam("converttorainbow", false, p => bool.Parse(p));
            VariablePopulator internalvars = new VariablePopulator();
            float Rfactor = GetParam("rainbowfactor", 1, p => float.Parse(p));
            float endtime = GetParam("tobeat", float.PositiveInfinity, p => float.Parse(p));


            if (rainbow)
            {
                InstanceWorkspace.Lights = InstanceWorkspace.Lights.Select(obj =>
                {
                    if (obj._time.ToFloat() >= starttime && obj._time.ToFloat() <= endtime)
                    {
                        return (BeatMap.Event)obj.Append(new BeatMap.Event()
                        {
                            _customData = new TreeDictionary()
                            {
                                ["_color"] = new object[]
                                {
                                    0.5f * Math.Sin(Math.PI * Rfactor * obj.GetTime()) + 0.5f,
                                    0.5f * Math.Sin((Math.PI * Rfactor * obj.GetTime()) - (Math.PI * (2f / 3f))) + 0.5f,
                                    0.5f * Math.Sin((Math.PI * Rfactor * obj.GetTime()) - (Math.PI * (4f / 3f))) + 0.5f,
                                    1
                                }
                            }
                        }, AppendPriority.High);
                    }
                    else return obj;
                }).ToList();
            }
            int i = 0;
            InstanceWorkspace.Lights = InstanceWorkspace.Lights.Select(obj =>
            {


                if (obj._time.ToFloat() >= starttime && obj._time.ToFloat() <= endtime)
                {
                    internalvars.CurrentEvent = obj;
                    WallIndex.StringData = i.ToString();
                    Parameters = Parameters.Select(p => { p.InternalVariables = internalvars.Properties.CombineWith(ps).ToArray(); return p; }).ToArray();

                    FunLog();

                    string log = GetParam("log", null, p => p);
                    if (log != null) Log(log);

                    var r = (BeatMap.Event)obj.Append(Parameters.CustomDataParse(new BeatMap.Event()), type);
                    i++;
                    return r;
                }
                else return obj;
            }).ToList();

            Log($"Appended {i} events from beats {starttime} to {endtime}");
            Parameter.ExternalVariables.RefreshAllParameters();
        }
    }
}
