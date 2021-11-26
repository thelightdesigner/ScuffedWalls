using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ScuffedWalls
{
    public class Request
    {
        public enum Type
        {
            ScuffedRequest,
            DefineRequest,
            WorkspaceRequest,
            FunctionRequest,
            VariableRequest,
            None
        }
        public TreeList<Parameter> Parameters { get; protected set; }
        public Parameter DefiningParameter { get; protected set; }
        public TreeList<Parameter> UnderlyingParameters { get; protected set; }
        public virtual Request SetupFromLines(List<Parameter> Lines)
        {
            Parameters = new TreeList<Parameter>(Lines, Parameter.Exposer);
            DefiningParameter = Lines.First();
            UnderlyingParameters = new TreeList<Parameter>(Lines.Lasts(), Parameter.Exposer);
            return this;
        }
    }
    public class ScuffedRequest : Request
    {
        public ContainerRequest GetCustomFunction(string name) => CustomFunctionRequests.FirstOrDefault(p => p.Name.ToLower().RemoveWhiteSpace() == name.ToLower().RemoveWhiteSpace());
        public bool CustomFunctionExists(string name) => CustomFunctionRequests.Any(p => p.Name.ToLower().RemoveWhiteSpace() == name.ToLower().RemoveWhiteSpace());
        private CacheableScanner<Parameter> _paramScanner;
        public List<ContainerRequest> WorkspaceRequests { get; private set; } = new List<ContainerRequest>();
        public List<ContainerRequest> CustomFunctionRequests { get; private set; } = new List<ContainerRequest>();
        public override Request SetupFromLines(List<Parameter> Lines)
        {
            Parameters = new TreeList<Parameter>(Lines, Parameter.Exposer);
            _paramScanner = new CacheableScanner<Parameter>(Parameters);
            List<FileInfo> includes = new List<FileInfo>();


            Type previousType = Type.ScuffedRequest;
            while (_paramScanner.MoveNext())
            {
                addWspc();
                _paramScanner.AddToCache();
            }
            addWspc();

            foreach (var include in includes)
            {
                ScuffedWalls.Print($"Included {include.Name}");

                ScuffedRequest requestToInclude = (ScuffedRequest)new ScuffedRequest().SetupFromLines(new ScuffedWallFile(include.FullName).Parameters);

                WorkspaceRequests.AddRange(requestToInclude.WorkspaceRequests);
                CustomFunctionRequests.AddRange(requestToInclude.CustomFunctionRequests); 
                
            }

            return this;

            void addWspc()
            {

                if (_paramScanner.Current == null || _paramScanner.Current.Clean.Name == ContainerRequest.WorkspaceKeyword || _paramScanner.Current.Clean.Name == ContainerRequest.DefineKeyword)
                {
                    switch (previousType)
                    {
                        case Type.WorkspaceRequest:
                            WorkspaceRequests.Add((ContainerRequest)new ContainerRequest().SetupFromLines(_paramScanner.GetAndResetCache()));
                            break;
                        case Type.DefineRequest:
                            ContainerRequest def = (ContainerRequest)new ContainerRequest().SetupFromLines(_paramScanner.GetAndResetCache());
                            CustomFunctionRequests.Add(def);

                            string param = string.Join(", ", def.VariableRequests.Where(v => v.Public).Select(v => v.Name));

                            ScuffedWalls.Print($"Registered Custom Function \"{def.Name}\" {(string.IsNullOrEmpty(param) ? "" : $"Params: {{{param}}}")}");
                            break;
                        case Type.ScuffedRequest:
                            addDefaults();
                            break;
                    }
                    previousType =
                        _paramScanner.Current?.Clean.Name == ContainerRequest.WorkspaceKeyword ? Type.WorkspaceRequest :
                        _paramScanner.Current?.Clean.Name == ContainerRequest.DefineKeyword ? Type.DefineRequest :
                        Type.ScuffedRequest;
                }

                

               /* if (_paramScanner.AnyCached)
                {
                    if (_paramScanner.Cache.First().Clean.Name == ContainerRequest.WorkspaceKeyword) ;
                    else if ()
                    else addDefaults();
                } */
            }
            void addDefaults()
            {
                var cache = _paramScanner.GetAndResetCache();
                foreach (var line in cache)
                {
                    switch (line.Clean.Name)
                    {
                        case "include":
                            includes.Add(new FileInfo(Path.Combine(Utils.ScuffedConfig.MapFolderPath, line.StringData.Trim())));
                            break;
                        case "hidemapinrpc":
                            Utils.ScuffedConfig.HideMapInRPC = bool.Parse(line.StringData);
                            break;
                        case "debug":
                            Utils.ScuffedConfig.Debug = bool.Parse(line.StringData);
                            break;
                        case "prettyprintjson":
                            Utils.ScuffedConfig.PrettyPrintJson = bool.Parse(line.StringData);
                            break;
                    }
                    
                }

            }

        }
    }
}
