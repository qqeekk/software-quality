using SetCalculations.Adapters;
using System;
using System.Collections.Generic;

namespace SetCalculations.Machine
{
    public class RightArgumentState : IContinuable<IReadOnlySet<object>>
    {
        private readonly Parser parser;
        private readonly OpCodes op;
        public IReadOnlySet<object> State { get; }

        public RightArgumentState(Parser parser, IReadOnlySet<object> state, OpCodes op)
        {
            this.op = op;
            this.parser = parser;
            State = state;
        }

        public IState<IReadOnlySet<object>> Next(string param)
        {
            if (EndState.IsValidState(param))
            {
                return new ErrorState(State, new InvalidOperationException("Unexpected EOF."));
            }

            if (parser.TryParse(param, out var set, out var errors))
            {
                var next = Calculator.Apply(op, State, set);
                return new OperatorState(parser, next);
            }

            if (OperatorState.IsValidState(param))
            {
                return new ErrorState(State, new InvalidOperationException($"Unexpected operator: \"{param}\"."));
            }

            return new ErrorState(State, errors);
        }
    }
}
