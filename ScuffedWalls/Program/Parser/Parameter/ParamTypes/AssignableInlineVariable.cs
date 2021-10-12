using System;
using System.Collections.Generic;
using System.Text;

namespace ScuffedWalls
{
    public enum VariableRecomputeSettings
    {
        AllReferences,
        AllRefreshes,
        OnCreationOnly
    }
    public  class AssignableInlineVariable : INameStringDataPair
    {
        private Variable _raw;
        private Variable _instance;
        private Variable _creation;
        private VariableRecomputeSettings _variableComputeSettings;
        public string Name { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string StringData { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public static Func<AssignableInlineVariable, string> Exposer => var => var.Name;
        public static StringComputationExcecuter Computer = new StringComputationExcecuter(new Lookup<AssignableInlineVariable>(Exposer));
        public string GetName() => _raw.Name;
        public string GetStringData()
        {
            switch (_variableComputeSettings)
            {
                case VariableRecomputeSettings.AllRefreshes:
                    return _instance.StringData;
                case VariableRecomputeSettings.OnCreationOnly:
                    return _creation.StringData;
                default:
                    return Computer.Parse(_raw.StringData);
            }
            
        }

        public AssignableInlineVariable(string name, string value, VariableRecomputeSettings recompute = VariableRecomputeSettings.AllReferences)
        {
            _raw = new Variable(name, value);
            _variableComputeSettings = recompute;
        }

        /// <summary>
        /// Recomputes the internal variables if Recompute Settings is set to do so.
        /// </summary>
        public void Refresh()
        {
            if (_variableComputeSettings == VariableRecomputeSettings.AllRefreshes)
            {
                _instance = null//recompute
            }
        }
    }
}
