﻿using ModChart;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ScuffedWalls
{
    /// <summary>
    /// Holds objects related to the current sw file being parsed
    /// </summary>
    /// 
    public interface IRequestParser<RequestType, ReturnType>
    {
        public ReturnType GetResult();
        public RequestType CurrentRequest { get; }
        public ReturnType Result { get; }
    }
    public class ScuffedRequestParser : IRequestParser<ScuffedRequest, DifficultyV2>
    {
        public static ScuffedRequestParser Instance { get; private set; }
        public static Rainbow WorkspaceRainbow = new Rainbow();
        public DifficultyV2 Result => _latestRunBeatMap;
        public ScuffedRequest CurrentRequest => _request;
        public List<ContainerRequest> CustomFunctions => CurrentRequest.CustomFunctionRequests;
        public List<Workspace> Workspaces => _workspaces;

        private DifficultyV2 _latestRunBeatMap;
        private ScuffedRequest _request;
        private List<Workspace> _workspaces;
        private IEnumerator<ContainerRequest> _workspaceRequestEnumerator;

        public ScuffedRequestParser(ScuffedRequest request)
        {
            Instance = this;
            _request = request;
        } 
        
        
        /*
          
        public static T GetParam<T>(string name, T defaultval, Func<string, T> converter, Lookup<Parameter> parameters)
        {
            if (!parameters.Any(p => p.Clean.Name.Equals(name))) return defaultval;
            return converter(parameters.Where(p => p.Clean.Name.Equals(name)).First().StringData);
        }
        */
        public DifficultyV2 GetResult()
        {

            _workspaceRequestEnumerator = CurrentRequest.WorkspaceRequests.GetEnumerator();
            _workspaces = new List<Workspace>();

            while (_workspaceRequestEnumerator.MoveNext())
            {
                var workreq = _workspaceRequestEnumerator.Current;

                if (workreq.Name != null && workreq.Name != string.Empty)
                    ScuffedWalls.Print($"Workspace {Workspaces.Count()} : \"{workreq.Name}\"", Color: WorkspaceRainbow.Next());
                else
                    ScuffedWalls.Print($"Workspace {Workspaces.Count()}", Color: WorkspaceRainbow.Next());

                Workspaces.Add(new WorkspaceRequestParser(workreq).GetResult());
            }
            DifficultyV2 map = Workspace.Combine(Workspaces);
            map.Prune();
            if (ScuffedWallsContainer.ScuffedConfig.IsAutoSimplifyPointDefinitionsEnabled)
            {
                try
                {
                    ScuffedWalls.Print("Simplifying Point Definitions...");
                    BeatmapCompressor.SimplifyAllPointDefinitions(map);
                }
                catch (IndexOutOfRangeException e)
                {
                    ScuffedWalls.Print($"Failed to simplify Point Definitions, A point definition may be missing a value? ERROR:{e.Message}", ScuffedWalls.LogSeverity.Warning);
                }
                catch (Exception e)
                {
                    ScuffedWalls.Print($"Failed to simplify Point Definitions ERROR:{e.Message}", ScuffedWalls.LogSeverity.Warning);
                }
            }
            _latestRunBeatMap = map;
            return map;
        }
    }



}
