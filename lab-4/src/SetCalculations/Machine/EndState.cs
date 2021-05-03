using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SetCalculations.Machine
{
    class EndState : IState<IReadOnlySet<object>>
    {
        public IReadOnlySet<object> State { get; }

        public EndState(IReadOnlySet<object> state)
        {
            State = state;
        }
    }
}
