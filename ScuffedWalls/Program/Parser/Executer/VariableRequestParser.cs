using System;
using System.Collections.Generic;
using System.Text;

namespace ScuffedWalls
{
    class VariableRequestParser : IRequestParser<VariableRequest, IEnumerable<AssignableInlineVariable>>
    {
        public static readonly char[] VectorIndex = { 'x', 'y', 'z', 't', 'e', 's' };
        public IEnumerable<AssignableInlineVariable> Result => _result;
        public VariableRequest CurrentRequest => _request;
        public bool HideLogs { get; set; }

        private readonly VariableRequest _request;
        private IEnumerable<AssignableInlineVariable> _result;

        public VariableRequestParser(VariableRequest request, bool hideLogs)
        {
            HideLogs = hideLogs;
            _request = request;
        }
        public IEnumerable<AssignableInlineVariable> GetResult()
        {
            List<AssignableInlineVariable> variables = new List<AssignableInlineVariable>();
            Debug.TryAction(() =>
            {
                if (_request.ContentsType == VariableEnumType.Array || _request.ContentsType == VariableEnumType.Vector)
                {
                    string[] values = _request.Data.ParseSWArray();
                    for (int i = 0; i < values.Length; i++)
                    {
                        string indexer = _request.ContentsType switch
                        {
                            VariableEnumType.Array => i.ToString(),
                            VariableEnumType.Vector => VectorIndex[i].ToString(),
                            _ => throw new Exception("iswimfly")
                        };
                        variables.Add(new AssignableInlineVariable(_request.Name + $"({indexer})", values[i], _request.VariableRecomputeSettings));
                    }
                }
                else
                {
                    variables.Add(new AssignableInlineVariable(_request.Name, _request.Data, _request.VariableRecomputeSettings));
                    _result = variables;
                }

                if (!HideLogs) foreach(var x in variables) ScuffedWalls.Print($"Added Variable \"{x.Name}\" Val:{x.StringData}", ShowStackFrame: false);
            }, e =>
            {
                ScuffedWalls.Print($"Error adding global variable {_request.Name} ERROR:{e.Message} ", ScuffedWalls.LogSeverity.Error);
            });
            _result = variables;
            return variables;
        }
    }
}
