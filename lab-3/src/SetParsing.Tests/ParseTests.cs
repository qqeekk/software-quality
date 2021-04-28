using System;
using System.Collections;
using System.Numerics;
using Xunit;

namespace SetParsing.Tests
{
    /// <summary>
    /// Test fixture.
    /// </summary>
    public partial class ParseTests
    {
        [Theory]
        [MemberData(nameof(TokenEqualityData))]
        public void TokenEqualityTest(TokenBase a, TokenBase b, bool equal)
        {
            if (equal)
            {
                Assert.Equal(a, b);
                Assert.Equal(a.GetHashCode(), b.GetHashCode());
            }
            else
            {
                Assert.NotEqual(a, b);
            }
        }

        [Theory]
        [MemberData(nameof(ParseAtomicData))]
        [MemberData(nameof(ParseSetData))]
        public void ProcessLineTest(string atomic, TokenBase result)
        {
            Assert.Equal(expected: result, actual: Parser.ProcessLine(atomic));
        }

        [Theory]
        [MemberData(nameof(TranslateTreeData))]
        public void TranslateTest(TokenBase token, object result)
        {
            Assert.Equal(expected: result, actual: Parser.Translate(token, null), new StructuralEqualityComparer());
        }

        [Fact]
        public void ParseTest()
        {
            Assert.Equal(expected: 1.5M,
                         actual: Parser.Parse<decimal>("1.5"));

            Assert.Equal(expected: new Complex(5, -1),
                         actual: Parser.Parse<Complex>("5-i"));

            Assert.Equal(expected: new object[] { 5M, new object[] { 3M } },
                         actual: Parser.Parse<IEnumerable>("[5, [3]]"),
                         comparer: new StructuralEqualityComparer());

            var exception = Assert.Throws<AggregateException>(() => Parser.Parse<object>($"[1, *, {Large}]"));
            Assert.Equal(2, exception.InnerExceptions.Count);
        }

        [Fact]
        public void NullToleranceTest()
        {
            Assert.Throws<ArgumentNullException>(() => Parser.ProcessLine(null));
            Assert.Throws<ArgumentNullException>(() => Parser.Translate(null, null));
            Assert.Throws<ArgumentNullException>(() => Parser.Parse<object>(null));

            Assert.Throws<NotImplementedException>(() => Parser.Translate(new FakeToken(), null));
        }
        private class FakeToken : TokenBase { }
    }
}
