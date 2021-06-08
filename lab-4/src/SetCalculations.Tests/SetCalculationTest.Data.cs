using SetCalculations.Machine;
using System;
using System.Collections.Generic;

namespace SetCalculations.Tests
{
    public partial class SetCalculationTest
    {
        public static IEnumerable<object[]> CalculationTestData()
        {
            yield return new object[]
            {
                Set(),
                string.Empty,
                MockParser(),
            };

            yield return new object[]
            {
                Set(5M),
                "$0",
                MockParser(5M)
            };
            yield return new object[]
            {
                Set(Set(4M, 5M), 8M),
                "$0 + $1",
                MockParser(
                    Set(8M, Set(4M, 5M)),
                    Set(Set(4M, 5M))
                )
            };

            yield return new object[]
            {
                Set(Set(4M, 5M), 8M, 3M),
                "$0 + $1 + $2",
                MockParser(
                    Set(3M, 8M),
                    Set(8M, Set(4M, 5M)),
                    Set(Set(4M, 5M))
                )
            };

            yield return new object[]
            {
                Set(Set(4M, 5M)),
                "$0 & $1",
                MockParser(
                    Set(8M, Set(4M, 5M)),
                    Set(Set(4M, 5M))
                )
            };

            yield return new object[]
            {
                Set(8M, Set(4M, 5M)),
                "$0 & $1 + $2",
                MockParser(
                    Set(3M, 8M),
                    Set(8M, Set(4M, 5M)),
                    Set(Set(4M, 5M))
                )
            };
            yield return new object[]
            {
                Set(8M),
                "$0 / $1",
                MockParser(
                    Set(8M, Set(4M, 5M)),
                    Set(Set(4M, 5M))
                )
            };
            yield return new object[]
            {
                Set(8M, Set(1M)),
                "$0 // $1",
                MockParser(
                    Set(8M, Set(4M, 5M)),
                    Set(Set(1M), Set(4M, 5M))
                )
            };
        }

        public static IEnumerable<object[]> ExceptionTestData()
        {
            yield return new object[]
            {
                new[] { "+", null },
                typeof(InvalidOperationException),
                typeof(LeftArgumentState),
            };

            yield return new object[]
            {
                new[] { "$0", "+", null },
                typeof(InvalidOperationException),
                typeof(RightArgumentState),
            };

            yield return new object[]
            {
                new[] { "##", null },
                typeof(AggregateException),
                typeof(LeftArgumentState),
            };

            yield return new object[]
            {
                new[] { "##", "+", null },
                typeof(AggregateException),
                typeof(LeftArgumentState),
            };

            yield return new object[]
            {
                new[] { "$0", "x", null },
                typeof(InvalidOperationException),
                typeof(OperatorState),
            };

            yield return new object[]
            {
                new[] { "$0", "$0", null },
                typeof(InvalidOperationException),
                typeof(OperatorState),
            };

            yield return new object[]
            {
                new[] { "$0", "/", "/", null },
                typeof(InvalidOperationException),
                typeof(RightArgumentState),
            };

            yield return new object[]
            {
                new[] { "$0", "/", "##", null },
                typeof(AggregateException),
                typeof(RightArgumentState),
            };
            
            yield return new object[]
            {
                new[] { "$0" },
                typeof(InvalidOperationException),
                typeof(OperatorState),
            };

            yield return new object[]
            {
                new[] { "$0", "+" },
                typeof(InvalidOperationException),
                typeof(RightArgumentState),
            };
        }

        private static IReadOnlySet<object> Set(params object[] p)
        {
            return new HashSet<object>(p, new SetParsing.StructuralEqualityComparer());
        }
    }
}
