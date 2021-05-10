using SetCalculations.Adapters;
using System;
using System.Collections.Generic;

namespace SetCalculations.Machine
{
    public enum OpCodes
    {
        Intersection, // &
        Union, // +
        Difference, // /
        SymmetricDifference, // //
    }

    public class OperatorState : IContinuable<IReadOnlySet<object>>
    {
        private readonly Parser argumentParser;

        public IReadOnlySet<object> State { get; }

        public OperatorState(Parser parser, IReadOnlySet<object> state)
        {
            this.argumentParser = parser;
            State = state;
        }

        public IState<IReadOnlySet<object>> Next(string param)
        {
            if (EndState.IsValidState(param))
            {
                return new EndState(State);
            }

            if (TryParse(param, out var op))
            {
                return new RightArgumentState(argumentParser, State, op);
            }

            if (argumentParser.TryParse(param, out _, out _))
            {
                return new ErrorState(State, new InvalidOperationException("Infix operator missing."));
            }

            return new ErrorState(State, new InvalidOperationException($"Unknown operator: \"{param}\"."));
        }

        internal static bool IsValidState(string param)
        {
            return TryParse(param, out _);
        }

        private static bool TryParse(string param, out OpCodes op)
        {
            var code = param.Trim() switch
            {
                "&" => OpCodes.Intersection,
                "+" => OpCodes.Union,
                "//" => OpCodes.SymmetricDifference,
                "/" => OpCodes.Difference,
                _ => (OpCodes?)null,
            };

            op = code ?? default;
            return code.HasValue;
        }
    }
}
