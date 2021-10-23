using System;
using System.Collections.Generic;
using System.Text;

namespace ScuffedWalls
{
    class VariableRequestParser : IRequestParser<VariableRequest, AssignableInlineVariable>
    {
        public AssignableInlineVariable Result => _result;
        public VariableRequest CurrentRequest => _request;

        private readonly VariableRequest _request;
        private AssignableInlineVariable _result;

        public VariableRequestParser(VariableRequest request)
        {
            _request = request;
        }
        public AssignableInlineVariable GetResult()
        {
            AssignableInlineVariable variable = null;
            Debug.TryAction(() =>
            {
                variable = new AssignableInlineVariable(_request.Name, _request.Data, _request.VariableRecomputeSettings);
                _result = variable;

                ScuffedWalls.Print($"Added Variable \"{variable.Name}\" Val:{variable.StringData}", ShowStackFrame: false);
            }, e =>
            {
                ScuffedWalls.Print($"Error adding global variable {_request.Name} ERROR:{e.Message} ", ScuffedWalls.LogSeverity.Error);
            });
            _result = variable;
            return variable;
        }
    }
}
