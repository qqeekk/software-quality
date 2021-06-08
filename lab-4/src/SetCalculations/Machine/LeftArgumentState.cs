using SetCalculations.Adapters;
using System;
using System.Collections.Generic;

namespace SetCalculations.Machine
{
    public class LeftArgumentState : IContinuable<IReadOnlySet<object>>
    {
        private readonly Parser parser;

        public IReadOnlySet<object> State { get; } = new HashSet<object>();

        public LeftArgumentState(Parser parser)
        {
            this.parser = parser;
        }

        public IState<IReadOnlySet<object>> Next(string param)
        {
            if (EndState.IsValidState(param))
            {
                return new EndState(State);
            }

            if (OperatorState.IsValidState(param))
            {
                return new ErrorState(State, new InvalidOperationException($"Unexpected operator: \"{param}\"."));
            }

            return parser.TryParse(param, out var set, out var errors)
                ? new OperatorState(parser, set)
                : new ErrorState(State, errors);
        }
    }
}
