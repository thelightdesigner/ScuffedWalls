using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ScuffedWalls
{
    public class ContainerRequest : Request
    {
        public const string WorkspaceKeyword = "workspace";
        public const string DefineKeyword = "function";
        public string Name { get; private set; }
        public List<FunctionRequest> FunctionRequests { get; private set; } = new List<FunctionRequest>();

        private readonly TreeList<VariableRequest> _varRequestContainer = new TreeList<VariableRequest>(VariableRequest.Exposer);
        private readonly TreeList<VariableRequest> _primaryRequests = new TreeList<VariableRequest>(VariableRequest.Exposer);
        protected readonly TreeList<VariableRequest> _customVariables = new TreeList<VariableRequest>(VariableRequest.Exposer);

        public List<VariableRequest> VariableRequests => _varRequestContainer.Values;

        private CacheableScanner<Parameter> _paramScanner;
        public ContainerRequest()
        {
            _varRequestContainer.Register(_primaryRequests);
            _varRequestContainer.Register(_customVariables);
        }
        public void ResetDefaultValues()
        {
            _customVariables.Clear();
            foreach (var Var in VariableRequests) Var.ResetDefaultValue();
            foreach (var param in Parameters) param.Variables.Clear();
        }
        public void RegisterCallTime(float call)
        {
            foreach (var Fun in FunctionRequests) Fun.SetCallTime(call);
        }
        public void RegisterCustomVariables(IEnumerable<VariableRequest> cvs, bool affectPublicVariablesOnly)
        {
            foreach (var _var in cvs)
            {
                VariableRequest primaryRequest = _primaryRequests.Get(_var.Name);
                if (affectPublicVariablesOnly && primaryRequest == null) continue;
                if (primaryRequest != null && primaryRequest.Public) primaryRequest.Data = _var.Data;
                else _customVariables.Add(_var);
            }
        }
        public override Request SetupFromLines(List<Parameter> Lines)
        {
            Parameters = new TreeList<Parameter>(Lines, Parameter.Exposer);
            DefiningParameter = Lines.First();
            UnderlyingParameters = new TreeList<Parameter>(Lines.Lasts(), Parameter.Exposer);
            Name = DefiningParameter.StringData?.Trim();

            _paramScanner = new CacheableScanner<Parameter>(UnderlyingParameters);
            Type previous = Type.None;

            while (_paramScanner.MoveNext())
            {
                bool varIs = VariableRequest.IsName(_paramScanner.Current.Clean.Name);
                bool funIs = FunctionRequest.IsName(_paramScanner.Current.Clean.Name);
                if (varIs || funIs)
                {
                    addLastRequest();
                    previous = varIs ? Type.VariableRequest : funIs ? Type.FunctionRequest : Type.None;
                }
                _paramScanner.AddToCache();
            }
            addLastRequest();
            return this;

            void addLastRequest()
            {
                switch (previous)
                {
                    case Type.FunctionRequest:
                        if (_paramScanner.AnyCached)
                            FunctionRequests.Add((FunctionRequest)new FunctionRequest().SetupFromLines(_paramScanner.GetAndResetCache()));
                        break;
                    case Type.VariableRequest:
                        if (_paramScanner.AnyCached)
                            _primaryRequests.Add((VariableRequest)new VariableRequest().SetupFromLines(_paramScanner.GetAndResetCache()));
                        break;
                }
            }
        }
    }
}
