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
            IContinuable<IReadOnlySet<object>> cursor = new LeftArgumentState(parser);
            foreach (var word in line)
            {
                switch (cursor.Next(word))
                {
                    case IContinuable<IReadOnlySet<object>> state:
                        cursor = state;
                        continue;

                    case EndState state:
                        return state.State;

                    case ErrorState state when state.Errors.Count() == 1 && state.Errors.First() is InvalidOperationException ex:
                        throw new InvalidOperationException(cursor.GetType().Name, ex);

                    case ErrorState state:
                        throw new AggregateException(cursor.GetType().Name, state.Errors);
                }
            }

            throw new InvalidOperationException(cursor.GetType().Name);
        }
    }
}
