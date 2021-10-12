using ModChart;
using System;
using System.Linq;
using System.Text.Json;

namespace ScuffedWalls.Functions
{
    [SFunction("AppendToAllWallsBetween", "AppendWalls", "AppendWall")]
    class AppendWalls : ScuffedFunction
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
            Parameter select = UnderlyingParameters.FirstOrDefault(p => p.Name == "select");
            if (select != null) select.WasUsed = true;
            bool selectable() => select == null || bool.Parse(select.StringData);

            int i = 0;
            InstanceWorkspace.Walls = InstanceWorkspace.Walls.Select(obj =>
            {

                internalvars.CurrentWall = obj;

                foreach (var param in UnderlyingParameters) param.InternalVariables = internalvars.Properties.CombineWith(ps).ToArray();

                if (obj._time.ToFloat() >= starttime && obj._time.ToFloat() <= endtime && AppendNotes.isOnTrack(obj._customData, tracc) && lineindex.Any(num => num == obj._lineIndex.Value) && selectable())
                {
                    WallIndex.StringData = i.ToString();

                    FunLog();

                    var r = obj.Append(UnderlyingParameters.CustomDataParse(new BeatMap.Obstacle()), type);
                    i++;
                    return r;
                }
                else return obj;

            }).Cast<BeatMap.Obstacle>().ToList();

            ScuffedWalls.Print($"Appended {i} walls from beats {starttime} to {endtime}");
            Parameter.ExternalVariables.RefreshAllParameters();
        }
    }
    [SFunction("AppendToAllNotesBetween", "AppendNotes", "AppendNote")]
    class AppendNotes : ScuffedFunction
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

            Parameter select = UnderlyingParameters.FirstOrDefault(p => p.Name == "select");
            if (select != null) select.WasUsed = true;
            bool selectable() => select == null || bool.Parse(select.StringData);
            float starttime = Time;
            float endtime = GetParam("tobeat", float.PositiveInfinity, p => float.Parse(p));
            string tracc = GetParam("ontrack", null, p => p);
            int[] notetype = GetParam("selecttype", new int[] { 0, 1, 2, 3 }, p => p.Split(",").Select(a => Convert.ToInt32(a)).ToArray());
            int i = 0;
            InstanceWorkspace.Notes = InstanceWorkspace.Notes.Select(obj =>
            {
                internalvars.CurrentNote = obj;
                foreach (var param in UnderlyingParameters) param.InternalVariables = internalvars.Properties.CombineWith(ps).ToArray();
                if (obj._time.Value >= starttime && obj._time.Value <= endtime && isOnTrack(obj._customData, tracc) && notetype.Any(t => t == (int)obj._type) && selectable())
                {
                    WallIndex.StringData = i.ToString();

                    FunLog();

                    var r = obj.Append(UnderlyingParameters.CustomDataParse(new BeatMap.Note()), type);
                    i++;
                    return r;
                }
                else return obj;
            }).Cast<BeatMap.Note>().ToList();

            ScuffedWalls.Print($"Appended {i} notes from beats {starttime} to {endtime}");
            Parameter.ExternalVariables.RefreshAllParameters();
        }

        public static bool isOnTrack(TreeDictionary _customData, string track)
        {
            if (string.IsNullOrEmpty(track) || (_customData.ContainsKey("_track") && track.Equals(_customData["_track"]))) return true;
            else return false;
        }
    }
    [SFunction("AppendToAllEventsBetween", "AppendLights", "AppendEvent", "AppendEvents")]
    class AppendEvents : ScuffedFunction
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
            Parameter select = UnderlyingParameters.FirstOrDefault(p => p.Name == "select");
            if (select != null) select.WasUsed = true;
            bool selectable() => select == null || bool.Parse(select.StringData);

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

                WallIndex.StringData = i.ToString();
                foreach (var param in UnderlyingParameters) param.InternalVariables = internalvars.Properties.CombineWith(ps).ToArray();

                if (obj._time.ToFloat() >= starttime && obj._time.ToFloat() <= endtime && selectable())
                {
                    internalvars.CurrentEvent = obj;

                    FunLog();

                    var r = (BeatMap.Event)obj.Append(UnderlyingParameters.CustomDataParse(new BeatMap.Event()), type);
                    i++;
                    return r;
                }
                else return obj;
            }).ToList();

            ScuffedWalls.Print($"Appended {i} events from beats {starttime} to {endtime}");
            Parameter.ExternalVariables.RefreshAllParameters();
        }
    }
}
