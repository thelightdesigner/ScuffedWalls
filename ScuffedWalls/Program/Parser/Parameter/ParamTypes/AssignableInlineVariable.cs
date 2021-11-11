using System;

namespace ScuffedWalls
{
    public enum VariableRecomputeSettings
    {
        AllReferences,
        AllRefreshes,
        OnCreationOnly
    }
    public class AssignableInlineVariable : INameStringDataPair
    {
        private readonly Variable _raw;
        private readonly Variable _instance;
        private readonly Variable _creation;
        private int _ping;
        private readonly VariableRecomputeSettings _variableComputeSettings;
        public static int RefreshPing { get; private set; }
        public static void Ping()
        {
            RefreshPing++;
        }
        public string Name { get => GetName(); set { _raw.Name = value; } }
        public string StringData { get => GetStringData(); set { _raw.StringData = value; } }
        public static Func<AssignableInlineVariable, string> Exposer => var => var.Name;
        public static readonly StringComputationExcecuter Computer = new StringComputationExcecuter(new TreeList<AssignableInlineVariable>(Exposer));
        public string GetName() => _raw.Name;
        public string GetStringData()
        {
            switch (_variableComputeSettings)
            {
                case VariableRecomputeSettings.AllRefreshes:
                    {
                        if (_ping != RefreshPing)
                        {
                            _instance.StringData = Computer.Parse(_raw.StringData);
                            _ping = RefreshPing;
                        }
                        return _instance.StringData;
                    }
                case VariableRecomputeSettings.OnCreationOnly:
                    return _creation.StringData;
                case VariableRecomputeSettings.AllReferences:
                    return Computer.Parse(_raw.StringData);
                default:
                    return Computer.Parse(_raw.StringData);
            }

        }

        public AssignableInlineVariable(string name, string value, VariableRecomputeSettings recompute = VariableRecomputeSettings.AllReferences)
        {
            _raw = new Variable(name, value);
            _creation = new Variable(name, Computer.Parse(_raw.StringData));
            _instance = (Variable)_creation.Clone();
            _variableComputeSettings = recompute;
        }
        public override string ToString()
        {
            return $"{Name}:{StringData}";
        }
        /// <summary>
        /// Recomputes the internal variables if Recompute Settings is set to do so.
        /// </summary>

    }
}
