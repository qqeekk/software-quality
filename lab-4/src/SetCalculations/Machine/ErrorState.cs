using System.Collections.Generic;

namespace SetCalculations.Machine
{
    public class ErrorState : IState<IReadOnlySet<object>>
    {
        public IReadOnlySet<object> State { get; }

        public IEnumerable<string> Errors { get; }

        public ErrorState(IReadOnlySet<object> previousState, IEnumerable<string> errors)
        {
            State = previousState;
            Errors = errors;
        }
    }
}
