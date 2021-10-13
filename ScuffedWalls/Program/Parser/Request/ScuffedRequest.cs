using System;
using System.Collections.Generic;
using System.Linq;
using static ScuffedWalls.ScuffedRequestParser;

namespace ScuffedWalls
{
    public class Request
    {
        public enum Type
        {
            ScuffedRequest,
            ContainerRequest,
            FunctionRequest,
            VariableRequest
        }
        public TreeList<Parameter> Parameters { get; protected set; }
        public Parameter DefiningParameter { get; protected set; }
        public TreeList<Parameter> UnderlyingParameters { get; protected set; }
        public virtual Request Setup(List<Parameter> Lines)
        {
            Parameters = new TreeList<Parameter>(Lines, Parameter.Exposer);
            DefiningParameter = Lines.First();
            UnderlyingParameters = new TreeList<Parameter>(Lines.Lasts(), Parameter.Exposer);
            return this;
        }
    }
    public class ScuffedRequest : Request
    {
        private CacheableScanner<Parameter> _paramScanner;
        public List<ContainerRequest> WorkspaceRequests { get; private set; } = new List<ContainerRequest>();

        public override Request Setup(List<Parameter> Lines)
        {
            Parameters = new TreeList<Parameter>(Lines, Parameter.Exposer);
            DefiningParameter = null;
            UnderlyingParameters = null;

            _paramScanner = new CacheableScanner<Parameter>(Parameters);

            while (_paramScanner.MoveNext())
            {
                addWspc();
                _paramScanner.AddToCache();
            }
            addWspc();

            return this;
            void addWspc()
            {
                if ((_paramScanner.Current == null || ContainerRequest.IsName(_paramScanner.Current.Clean.Name)) && _paramScanner.AnyCached)
                {
                    WorkspaceRequests.Add((ContainerRequest)new ContainerRequest().Setup(_paramScanner.GetAndResetCache()));
                }
            }
        }
    }
}
