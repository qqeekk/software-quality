using SetCalculations.Adapters;
using System;
using System.Collections.Generic;
using System.Linq;

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
            try
            {
                var arg = parser.Parse(param);
                var set = arg as IReadOnlySet<object> ?? new[] { arg }.ToHashSet();
                return new OperatorState(parser, set);
            }
            catch (AggregateException agg)
            {
                return new ErrorState(State, agg.InnerExceptions.Select(e => e.Message).ToList());
            }
        }
    }
}
