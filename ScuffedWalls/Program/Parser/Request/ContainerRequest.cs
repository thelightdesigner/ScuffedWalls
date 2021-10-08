using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ScuffedWalls
{
    public class ContainerRequest : Request
    {
        public static string[] Keywords => new string[] { "workspace", "function" };
        public static bool IsName(string name)
        {
            if (Keywords.Any(keyword => name.Equals(keyword))) return true;
            return false;
        }
        public string Name { get; private set; }
        /// <summary>
        /// Indicates if this workspace should be populated as a callable scuffed function
        /// </summary>
        public bool IsFunction { get; private set; }
        public List<FunctionRequest> FunctionRequests { get; private set; } = new List<FunctionRequest>();
        public List<VariableRequest> VariableRequests { get; private set; } = new List<VariableRequest>();

        private CacheableScanner<Parameter> _paramScanner;
        public override Request Setup(List<Parameter> Lines)
        {
            Parameters = new Lookup<Parameter>(Lines, Parameter.Exposer);
            DefiningParameter = Lines.First();
            UnderlyingParameters = new Lookup<Parameter>(Lines.Lasts(), Parameter.Exposer);
            IsFunction = DefiningParameter.Clean.Name == "function";
            Name = DefiningParameter.StringData;

            //     foreach(var p in UnderlyingParameters) Console.WriteLine(p.ToString());

            _paramScanner = new CacheableScanner<Parameter>(UnderlyingParameters);
            Type previous = Type.ContainerRequest;

            while (_paramScanner.MoveNext())
            {
                bool varIs = VariableRequest.IsName(_paramScanner.Current.Clean.Name);
                bool funIs = FunctionRequest.IsName(_paramScanner.Current.Clean.Name);
                if (varIs || funIs)
                {
                    addLastRequest();
                    previous = varIs ? Type.VariableRequest : funIs ? Type.FunctionRequest : Type.ContainerRequest;
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
                            FunctionRequests.Add((FunctionRequest)new FunctionRequest().Setup(_paramScanner.GetAndResetCache()));
                        break;
                    case Type.VariableRequest:
                        if (_paramScanner.AnyCached)
                            VariableRequests.Add((VariableRequest)new VariableRequest().Setup(_paramScanner.GetAndResetCache()));
                        break;
                }
            }
        }
    }
}
