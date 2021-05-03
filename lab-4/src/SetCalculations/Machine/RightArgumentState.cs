using SetCalculations.Adapters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SetCalculations.Machine
{
    class RightArgumentState : IContinuable<IReadOnlySet<object>>
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
            try
            {
                var arg = parser.Parse(param);
                var set = arg as IReadOnlySet<object> ?? new[] { arg }.ToHashSet();

                var next = Calculator.Apply(op, State, set);
                return new OperatorState(parser, next);
            }
            catch (AggregateException agg)
            {
                return new ErrorState(State, agg.InnerExceptions.Select(e => e.Message).ToList());
            }
        }
    }
}
