using SetCalculations.Adapters;
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
        private readonly Parser parser;

        public IReadOnlySet<object> State { get; }

        public OperatorState(Parser parser, IReadOnlySet<object> state)
        {
            this.parser = parser;
            State = state;
        }

        public IState<IReadOnlySet<object>> Next(string param)
        {
            if (string.IsNullOrEmpty(param))
            {
                return new EndState(State);
            }

            var code = param.Trim() switch
            {
                "&" => OpCodes.Intersection,
                "+" => OpCodes.Union,
                "//" => OpCodes.SymmetricDifference,
                "/" => OpCodes.Difference,
                _ => (OpCodes?)null,
            };

            if (code is OpCodes op)
            {
                return new RightArgumentState(parser, State, op);
            }

            return new ErrorState(State, new[] { $"Unknown operator: \"{param}\"." });
        }
    }
}
