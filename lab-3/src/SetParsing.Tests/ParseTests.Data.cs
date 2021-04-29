using SetParsing.Tokens;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace SetParsing.Tests
{
    public partial class ParseTests
    {
        private static readonly string Large = new(c: '5', count: 50);

        public static IEnumerable<object[]> TokenEqualityData()
        {
            yield return new object[] { R(2), R(2), true };
            yield return new object[] { R(2), R(1.99999999999999999999999999999999M), true };
            yield return new object[] { C(0, 3), C(0, 3), true };
            yield return new object[] { C(1, 3), C(1, 3), true };
            yield return new object[] { C(1, 0), R(1), false };
            yield return new object[] { R(1), C(1, 0), false };
            yield return new object[] { R(2), null, false };
            yield return new object[] { C(2, 5), null, false };

            yield return new object[] { E(ErrorCode.Unknown), E(ErrorCode.Unknown), true };
            yield return new object[] { E(ErrorCode.Unknown), E(ErrorCode.OutOfRange), false };
            yield return new object[] { E(ErrorCode.Unknown), S(), false };
            yield return new object[] { E(ErrorCode.Unknown), null, false };

            yield return new object[] { S(), S(), true };
            yield return new object[] { S(R(1)), S(R(1)), true };
            yield return new object[] { R(1), S(R(1)), false };
            yield return new object[] { S(R(1)), R(1), false };
            yield return new object[] { S(), S(E(ErrorCode.Unknown)), false };
            yield return new object[] { S(E(ErrorCode.Unknown)), S(E(ErrorCode.OutOfRange)), false };
            yield return new object[] { S(), null, false };
        }

        public static IEnumerable<object[]> ParseAtomicData()
        {
            yield return new object[] { "", E(ErrorCode.Unknown) };

            // Real numbers.
            yield return new object[] { "0", R(0) };
            yield return new object[] { "1", R(1) };
            yield return new object[] { "2.", R(2) };
            yield return new object[] { "19.98", R(19.98M) };
            yield return new object[] { "+1", R(1) };
            yield return new object[] { "+2.5", R(2.5M) };
            yield return new object[] { "-2.5", R(-2.5M) };
            yield return new object[] { "-32.9999999999999999999999999999999999999999999999999", R(-33M) };
            yield return new object[] { ".2", E(ErrorCode.Unknown) }; // not supported
            yield return new object[] { "-", E(ErrorCode.Unknown) }; // no-number
            yield return new object[] { "+", E(ErrorCode.Unknown) };
            yield return new object[] { "+ 1", E(ErrorCode.Unknown) }; // whitespace
            yield return new object[] { "- 2", E(ErrorCode.Unknown) };
            yield return new object[] { Large, E(ErrorCode.OutOfRange) };

            // Complex numbers.
            yield return new object[] { "i", C(0, 1) };
            yield return new object[] { "I", C(0, 1) };
            yield return new object[] { "-I", C(0, -1) };
            yield return new object[] { "+5i", C(0, 5) };
            yield return new object[] { "+5.i", C(0, 5) };
            yield return new object[] { "5.67i", C(0, 5.67M) };
            yield return new object[] { "1-5.67i", C(1, -5.67M) };
            yield return new object[] { "1.3-5.67i", C(1.3M, -5.67M) };
            yield return new object[] { "+1.3-5.67i", C(1.3M, -5.67M) };
            yield return new object[] { "-3+i", C(-3, 1) };
            yield return new object[] { "i-3", E(ErrorCode.Unknown) }; // not supported
            yield return new object[] { "-3 + i", E(ErrorCode.Unknown) }; // whitespace
            yield return new object[] { Large + "I", E(ErrorCode.OutOfRange) }; // too large imaginary
            yield return new object[] { $"1+{Large}I", E(ErrorCode.OutOfRange) }; // too large imaginary
            yield return new object[] { $"{Large}-I", E(ErrorCode.OutOfRange) }; // too large real
        }

        public static IEnumerable<object[]> ParseSetData()
        {
            yield return new object[] { "[]", S() };
            yield return new object[] { "[1]", S(R(1)) };
            yield return new object[] { "[1, 2]", S(R(1), R(2)) };
            yield return new object[] { "[1+2i, 2,]", S(C(1, 2), R(2)) };
            yield return new object[] { "[1, []]", S(R(1), S()) };
            yield return new object[] { "[[], 1+2i]", S(S(), C(1, 2)) };
            yield return new object[] { "[[]]", S(S()) };
            yield return new object[] { "[*]", S(E(ErrorCode.Unknown)) };
            yield return new object[] { "[6.5, *]", S(R(6.5M), E(ErrorCode.Unknown)) };
            yield return new object[] { "[6.5, *, 1]", S(R(6.5M), E(ErrorCode.Unknown), R(1)) };
        }

        public static IEnumerable<object[]> TranslateTreeData()
        {
            yield return new object[] { R(-1), -1M };
            yield return new object[] { C(0, 5), new Complex(0, 5) };
            yield return new object[] { E(ErrorCode.OutOfRange), null };

            yield return new object[] { S(), Array.Empty<object>() };
            yield return new object[] { S(R(1)), new object[] { 1M } };
            yield return new object[] { S(C(-1, 6.5M)), new object[] { new Complex(-1, 6.5) } };
            yield return new object[] { S(E(ErrorCode.Unknown)), new object[] { null } };
            yield return new object[] { S(S()), new object[] { Array.Empty<object>() } };

            yield return new object[] { S(R(1), C(1, 2)), new object[] { 1M, new Complex(1, 2) } };
            yield return new object[] { S(R(1), S(C(1, 2))), new object[] { 1M, new object[] { new Complex(1, 2) } } };
            yield return new object[] { S(R(1), E(ErrorCode.Unknown), R(2.5M)), new object[] { 1M, null, 2.5M } };
        }

        private static RealNumberToken R(decimal num) => new(num);
        private static ComplexNumberToken C(decimal real, decimal imaginary) => new(real, imaginary);
        private static ErrorToken E(ErrorCode code) => new(code);
        private static SetToken S(params TokenBase[] tokens) => new(tokens);
    }
}
