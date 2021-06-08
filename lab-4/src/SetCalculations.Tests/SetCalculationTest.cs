using Moq;
using SetCalculations.Adapters;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SetCalculations.Tests
{
    public partial class SetCalculationTest
    {
        [Theory]
        [MemberData(nameof(CalculationTestData))]
        public void CalculationTest(IReadOnlySet<object> result, string expression, Parser parser)
        {
            var tokens = expression.Split().Append(string.Empty);
            var set = new StateMachine(parser).Calculate(tokens);

            Assert.True(result.SetEquals(set));
        }

        [Theory]
        [MemberData(nameof(ExceptionTestData))]
        public void ExceptionTest(string[] expression, Type exceptionType, Type state)
        {
            var parser = MockParser(Set());
            var machine = new StateMachine(parser);

            var ex = Assert.Throws(exceptionType, () => machine.Calculate(expression));
            Assert.StartsWith(state.Name, ex.Message);
        }

        private static Parser MockParser(params object[] setups)
        {
            var parser = new Mock<Parser>(MockBehavior.Strict);
            for (var i = 0; i < setups.Length; i++)
            {
                parser.Setup(p => p.Parse("$" + i)).Returns(setups[i]);
            }

            parser.Setup(p => p.Parse(It.Is<string>(s => !s.StartsWith("$"))))
                .Throws(new AggregateException(new ArgumentException("Error")));

            return parser.Object;
        }
    }
}
