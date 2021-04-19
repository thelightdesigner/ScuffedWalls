using System;
using System.Collections.Generic;
using ModChart;
using System.Text;
using System.Linq;
using System.Text.Json;
using System.Runtime.Remoting;

namespace ScuffedWalls
{
    class VariablePopulator
    {
        private BeatMap.Obstacle _wall;
        public BeatMap.Obstacle CurrentWall { set
            {
                _wall = value;
                SetProperties();
            }
        }
        private BeatMap.Note _note;
        public BeatMap.Note CurrentNote
        {
            set
            {
                _note = value;
                SetProperties();
            }
        }
        private BeatMap.Event _event;
        public BeatMap.Event CurrentEvent
        {
            set
            {
                _event = value;
                SetProperties();
            }
        }
        public Parameter[] Properties { get; set; }
        string[] ExcludedProps = { "_customData", "_animation" };
        public void SetProperties()
        {
            List<Parameter> propVars = new List<Parameter>(0);

            dynamic mapobj = _wall;
            if(_note != null) mapobj = _note;
            if (_event != null) mapobj = _event;

            if (mapobj != null)
            {
                foreach (var baseProp in ((object)mapobj).GetType().GetProperties().Where(p => ExcludedProps.All(n => p.Name != n)))
                {
                    object value = baseProp.GetValue(mapobj);
                    if (value != null)
                    {
                        propVars.Add(new Parameter(baseProp.Name,value.ToString()));
                    }
                }
                if(mapobj._customData != null)
                {
                    foreach (var baseProp in typeof(BeatMap.CustomData).GetProperties().Where(p => ExcludedProps.All(n => p.Name != n)))
                    {
                        object value = baseProp.GetValue(mapobj._customData);
                        if (value != null)
                        {
                            propVars.AddRange(DynamicToVar(value,baseProp.Name));
                        }
                    }
                    if((mapobj is BeatMap.Note || mapobj is BeatMap.Obstacle) && mapobj._customData._animation != null)
                    {
                        foreach (var baseProp in typeof(BeatMap.CustomData.Animation).GetProperties().Where(p => ExcludedProps.All(n => p.Name != n)))
                        {
                            object value = baseProp.GetValue(mapobj._customData._animation);
                            if (value != null)
                            {
                                propVars.AddRange(DynamicToVar(value, "_animation." + baseProp.Name));
                            }
                        }
                    }
                }
            }
            //foreach(var f in propVars)  Console.WriteLine(f.ToString());

            Properties = propVars.ToArray();
        }
        public Parameter[] EachToVar(object[] array, string outerPropName)
        {
            List<Parameter> vars = new List<Parameter>();
            for(int i = 0; i < array.Length; i++)
            {
                vars.Add(new Parameter($"{outerPropName}({i})",array[i].ToString()));
               // Console.WriteLine(array[i]);
            }
            return vars.ToArray();
        }
        public Parameter[] EachToVar(object[][] array, string outerPropName)
        {
            List<Parameter> vars = new List<Parameter>();
            for(int i = 0; i < array.Length; i++)
            {
                vars.AddRange(EachToVar(array[i],$"{outerPropName}({i})"));
            }
            return vars.ToArray();
        }
        public Parameter[] DynamicToVar(dynamic Dyno,string name)
        {
            try
            {
                return EachToVar(JsonSerializer.Deserialize<object[][]>(JsonSerializer.Serialize((object)Dyno)), name);
            }
            catch { }
            try
            {
                return EachToVar(JsonSerializer.Deserialize<object[]>(JsonSerializer.Serialize((object)Dyno)), name);
            }
            catch { }
            return new Parameter[] { new Parameter(name,((object)Dyno).ToString())  };
        }
    }
}
