using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace SetParsing
{
    public class StructuralEqualityComparer : IEqualityComparer<object>
    {
        public new bool Equals(object x, object y)
        {
            return object.Equals(x, y) || (
                x is IEnumerable<object> xseq
                && y is IEnumerable<object> yseq
                && Enumerable.SequenceEqual(xseq, yseq, this));
        }

        public int GetHashCode([DisallowNull] object obj)
        {
            return obj is IEnumerable<object> seq
                ? seq.Aggregate(47587485, HashCode.Combine)
                : obj.GetHashCode();
        }
    }
}
