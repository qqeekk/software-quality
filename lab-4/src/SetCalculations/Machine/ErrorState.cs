using System;
using System.Collections.Generic;

namespace SetCalculations.Machine
{
    public class ErrorState : IState<IReadOnlySet<object>>
    {
        public IReadOnlySet<object> State { get; }

        public IEnumerable<Exception> Errors { get; }

        internal ErrorState(
            IReadOnlySet<object> previousState,
            Exception message) : this(previousState, new[] { message })
        { 
        }

        public ErrorState(
            IReadOnlySet<object> previousState,
            IEnumerable<Exception> errors)
        {
            State = previousState;
            Errors = errors;
        }
    }
}
