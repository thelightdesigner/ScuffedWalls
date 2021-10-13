using ModChart;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ScuffedWalls
{
    class VariablePopulator
    {
        public VariablePopulator()
        {
            Properties = new TreeList<AssignableInlineVariable>(AssignableInlineVariable.Exposer);
        }
        public void UpdateProperties(ICustomDataMapObject obj)
        {
            SetProperties(Properties, obj);
        }
        public TreeList<AssignableInlineVariable> Properties { get; }
        public static void SetProperties(TreeList<AssignableInlineVariable> properties, ICustomDataMapObject mapObject)
        {
            List<AssignableInlineVariable> propVars = new List<AssignableInlineVariable>();

            foreach (var prop in mapObject.GetType().GetProperties())
            {
                object val = prop.GetValue(mapObject);
                if (val != null) propVars.Add(new AssignableInlineVariable(prop.Name, getNumberFromEnum(val).ToString()));
            }

            if (mapObject._customData != null)
            {
                mapObject._customData.DeleteNullValues();
                PopulateParts(mapObject._customData);
            }


            void PopulateParts(TreeDictionary dict, string prefix = "")
            {
                foreach (KeyValuePair<string, object> Property in dict)
                {
                    if (Property.Value is TreeDictionary dictionary) PopulateParts(dictionary, Property.Key + ".");
                    else if (Property.Value is IEnumerable<object> Array) propVars.AddRange(GetArrayVars(Array, prefix + Property.Key));
                    else propVars.Add(new AssignableInlineVariable(Property.Key, prefix + Property.Value.ToString()));
                }
            }

            properties.Clear();
            properties.AddRange(propVars);
          //  ScuffedWalls.Print(string.Join(',', Properties.Select(p => p.Name + " " + p.StringData)));
        }
        private static object getNumberFromEnum(object val)
        {
            if (val is Enum) return (int)val;
            else return val;
        }
        public static TreeList<AssignableInlineVariable> GetArrayVars(IEnumerable<object> Array, string Name)
        {
            var Vars = new TreeList<AssignableInlineVariable>(AssignableInlineVariable.Exposer);

            for (int Index = 0; Index < Array.Count(); Index++)
            {
                if (Array.ElementAt(Index) is IEnumerable<object> NestedArray)
                    Vars.AddRange(
                        GetArrayVars(
                            NestedArray,
                            $"{Name}({Index})"));

                else Vars.Add(
                    new AssignableInlineVariable(
                        $"{Name}({Index})",
                        Array.ElementAt(Index).ToString()
                        ));
            }

            return Vars;
        }
    }
}
