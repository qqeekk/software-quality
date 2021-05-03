using SetCalculations.Adapters;
using SetCalculations.Machine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SetCalculations
{
    public class StateMachine
    {
        private readonly Parser parser;

        public StateMachine(Parser parser)
        {
            this.parser = parser;
        }

        public IReadOnlySet<object> Calculate(IEnumerable<string> line)
        {
            var cursor = new LeftArgumentState(parser) as IContinuable<IReadOnlySet<object>>;

            foreach (var word in line)
            {
                switch (cursor.Next(word))
                {
                    case EndState state:
                        return state.State;

                    case ErrorState state:
                        var innerExceptions = state.Errors.Select(e => new ArgumentException(e));
                        throw new AggregateException(innerExceptions);

                    case IContinuable<IReadOnlySet<object>> state:
                        cursor = state;
                        continue;
                }
            }

            throw new InvalidOperationException(cursor.GetType().Name);
        }
    }
}
