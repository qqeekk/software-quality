using SetParsing.Tokens;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;

namespace SetParsing
{
    /// <summary>
    /// Parser service.
    /// </summary>
    public static class Parser
    {
        private static readonly Regex complex = new(@"^(?<real>[+-]?\d+\.?\d*)(?<imaginary>[+-](\d+\.?\d*)?)[Ii]$",
            RegexOptions.Compiled | RegexOptions.Singleline);

        private static readonly Regex imaginary = new(@"^(?<imaginary>[+-]?(\d+\.?\d*)?)[Ii]$",
            RegexOptions.Compiled | RegexOptions.Singleline);

        private static readonly Regex real = new(@"^(?<real>[+-]?\d+\.?\d*)$",
            RegexOptions.Compiled | RegexOptions.Singleline);

        private static readonly Regex set = new(@"^\[(?<set>.*)\]$",
            RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace);

        /// <summary>
        /// Parse string into .NET object.
        /// <para>
        /// See also: <seealso cref="ProcessLine(string)"/>,
        /// <seealso cref=" Translate(TokenBase, ICollection{ArgumentException})"/>
        /// </para>
        /// </summary>
        /// <typeparam name="T">Target type.</typeparam>
        /// <param name="input">Input.</param>
        /// <exception cref="AggregateException" />
        /// <exception cref="InvalidCastException" />
        public static T Parse<T>(string input)
        {
            var token = ProcessLine(input);

            var errors = new List<ArgumentException>();
            var value = Translate(token, errors);

            if (!errors.Any())
            {
                return (T)value;
            }

            throw new AggregateException(errors);
        }

        /// <summary>
        /// Translate token tree to an object.
        /// List of objects:
        /// <list type="number">
        /// <item><see cref="decimal"/></item>
        /// <item><see cref="Complex"/></item>
        /// <item><see cref="Array"/></item>
        /// </list>
        /// </summary>
        /// <param name="token">Token tree root.</param>
        /// <param name="errors">Errors aggregate.</param>
        /// <returns>Object representation.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="NotImplementedException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public static object Translate(TokenBase token, ICollection<ArgumentException> errors)
        {
            return token switch
            {
                RealNumberToken num => num.Value,
                ComplexNumberToken cnum => new Complex(real: (double)cnum.Real, imaginary: (double)cnum.Imaginary),
                SetToken set => Array.ConvertAll(set.Tokens, t => Translate(t, errors)),
                ErrorToken val => SetError(val.Message),
                { } => throw new NotImplementedException(),
                null => throw new ArgumentNullException(nameof(token)),
            };

            object SetError(string message)
            {
                errors?.Add(new ArgumentException(message));
                return null;
            }
        }

        /// <summary>
        /// Parse line. Generate token.
        /// </summary>
        /// <param name="line">Input.</param>
        /// <returns>Token.</returns>
        public static TokenBase ProcessLine(string line)
        {
            return line switch
            {
                _ when real.Match(line) is Match m && m.Success =>
                    TryParseDecimal(m.Groups["real"].Value) switch
                    {
                        (true, var a) => new RealNumberToken(a),
                        _ => new ErrorToken(ErrorCode.OutOfRange, "Real part is out of range."),
                    },

                _ when complex.Match(line) is Match m && m.Success
                    && m.Groups["real"].Value is var mReal
                    && m.Groups["imaginary"].Value is var mImaginary =>
                    (ProcessLine(mReal), TryParseDecimal(mImaginary)) switch
                    {
                        (RealNumberToken real, (true, var b)) => real.AddImaginaryPart(b),
                        (RealNumberToken real, (false, _)) => new ErrorToken(ErrorCode.OutOfRange, "Imaginary part is out of range."),
                        (var error, _) => error,
                    },

                _ when imaginary.Match(line) is Match m && m.Success =>
                    TryParseDecimal(m.Groups["imaginary"].Value) switch
                    {
                        (true, var b) => new ComplexNumberToken(0m, b),
                        _ => new ErrorToken(ErrorCode.OutOfRange, "Imaginary part is out of range."),
                    },

                _ when set.Match(line) is Match m && m.Success =>
                    new SetToken(Array.ConvertAll(
                        m.Groups["set"].Value.Split(",", StringSplitOptions.RemoveEmptyEntries),
                        converter: v => ProcessLine(v.Trim()))
                    ),

                _ => new ErrorToken(ErrorCode.Unknown, "Unknown token"),
            };
        }

        private static (bool, decimal) TryParseDecimal(string number)
        {
            return number switch
            {
                "-" => (true, -1m),
                "+" or "" => (true, 1m),
                _ => (decimal.TryParse(number,
                    style: NumberStyles.AllowLeadingSign | NumberStyles.Float,
                    provider: CultureInfo.InvariantCulture.NumberFormat,
                    result: out var parsed), parsed)
            };
        }
    }
}
