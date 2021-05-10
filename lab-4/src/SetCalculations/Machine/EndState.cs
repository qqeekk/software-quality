using System.Collections.Generic;

namespace SetCalculations.Machine
{
    public class EndState : IState<IReadOnlySet<object>>
    {
        public IReadOnlySet<object> State { get; }

        public EndState(IReadOnlySet<object> state)
        {
            State = state;
        }

        internal static bool IsValidState(string param)
        {
            return string.IsNullOrEmpty(param);
        }
    }
}
