using ModChart;
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
            List<Parameter> propVars = new List<Parameter>(0);

            ICustomDataMapObject mapobj = _wall;
            if (_note != null) mapobj = _note;
            if (_event != null) mapobj = _event;

            if (mapobj != null) PopulateParts(mapobj._customData);

            void PopulateParts(TreeDictionary dict)
            {
                foreach (KeyValuePair<string, object> Property in dict)
                {
                    if (Property.Value is TreeDictionary dictionary) PopulateParts(dictionary);
                    else if (Property.Value is IEnumerable<object> Array) propVars.AddRange(GetArrayVars(Array, Property.Key));
                    else propVars.Add(new Parameter(Property.Key, Property.Value.ToString()));
                }
            }

            Properties = propVars.ToArray();
        }

        //old
        /*
        public Parameter[] EachToVar(object[] array, string outerPropName)
        {
            List<Parameter> vars = new List<Parameter>();
            for(int i = 0; i < array.Length; i++)
            {
                vars.Add(new Parameter($"{outerPropName}({i})",array[i].ToString()));
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
        public Parameter[] GetArrayVars(KeyValuePair<string, object> Item)
        {
            try
            {
                return EachToVar(JsonSerializer.Deserialize<object[][]>(JsonSerializer.Serialize(Item.Value)), Item.Key);
            }
            catch { }
            try
            {
                return EachToVar(JsonSerializer.Deserialize<object[]>(JsonSerializer.Serialize(Item.Value)), Item.Key);
            }
            catch { }
            return new Parameter[] { new Parameter(Item.Key, Item.Value.ToString())  };
        }
        */

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
                        Array.ElementAt(Index).ToString(),
                        $"{Name}({Index})"));
            }

            return Vars.ToArray();
        }
    }
}
