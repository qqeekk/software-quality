using SetCalculations.Machine;
using SetOperations;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SetCalculations
{
    static class Calculator
    {
        public static IReadOnlySet<object> Apply(OpCodes op, IEnumerable left, IEnumerable right)
        {
            return new HashSet<object>(op switch
            {
                OpCodes.Intersection => left.GetIntersection(right),
                OpCodes.Union => left.GetUnion(right),
                OpCodes.Difference => left.GetDifference(right),
                OpCodes.SymmetricDifference => left.GetSymmetricDifference(right),
                _ => throw new NotImplementedException(),
            });
        }
    }
}
