using ModChart;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ScuffedWalls
{
    class VariablePopulator
    {
        private BeatMap.Obstacle _wall;
        public BeatMap.Obstacle CurrentWall
        {
            set
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
        public void SetProperties()
        {
            List<AssignableInlineVariable> propVars = new List<Parameter>(0);

            ICustomDataMapObject mapobj = _wall;
            if (_note != null) mapobj = _note;
            if (_event != null) mapobj = _event;
            if (mapobj == null) return;

            foreach (var prop in mapobj.GetType().GetProperties())
            {
                object val = prop.GetValue(mapobj);
                if (val != null) propVars.Add(new AssignableInlineVariable(prop.Name, getNumberFromEnum(val).ToString()));
            }

            if (mapobj._customData != null)
            {
                mapobj._customData.DeleteNullValues();
                PopulateParts(mapobj._customData);
            }


            void PopulateParts(TreeDictionary dict, string prefix = "")
            {
                foreach (KeyValuePair<string, object> Property in dict)
                {
                    if (Property.Value is TreeDictionary dictionary) PopulateParts(dictionary, Property.Key + ".");
                    else if (Property.Value is IEnumerable<object> Array) propVars.AddRange(GetArrayVars(Array, prefix + Property.Key));
                    else propVars.Add(new Parameter(Property.Key, prefix + Property.Value.ToString()));
                }
            }

            Properties = propVars.ToArray();
            ScuffedWalls.Print(string.Join(',', Properties.Select(p => p.Name + " " + p.StringData)));
        }
        private object getNumberFromEnum(object val)
        {
            if (val is Enum) return (int)val;
            else return val;
        }
        public Parameter[] GetArrayVars(IEnumerable<object> Array, string Name)
        {
            var Vars = new List<Parameter>();

            for (int Index = 0; Index < Array.Count(); Index++)
            {
                if (Array.ElementAt(Index) is IEnumerable<object> NestedArray)
                    Vars.AddRange(
                        GetArrayVars(
                            NestedArray,
                            $"{Name}({Index})"));

                else Vars.Add(
                    new Parameter(
                        $"{Name}({Index})",
                        Array.ElementAt(Index).ToString()
                        ));
            }

            return Vars.ToArray();
        }
    }
}
