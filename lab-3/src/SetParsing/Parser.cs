using SetParsing.Tokens;
using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace SetParsing
{
    /// <summary>
    /// Parser service.
    /// </summary>
    public static class Parser
    {
        private static readonly Regex complex = new Regex(@"(?<real>[+-]?\d+\.?\d*)(?<imaginary>[+-](\d+\.?\d*)?)i$",
            RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace);

        private static readonly Regex imaginary = new Regex(@"(?<imaginary>[+-]?(\d+\.?\d*)?)i$",
            RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace);

        private static readonly Regex real = new Regex(@"(?<real>[+-]?\d+\.?\d*)$",
            RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace);

        private static readonly Regex set = new Regex(@"\[(?<set>.*)\]$",
            RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace);


        /// <summary>
        /// Parse line. Generate token.
        /// </summary>
        /// <param name="line">Input.</param>
        /// <returns>Token.</returns>
        public static TokenBase ParseLine(string line)
        {
            return line switch
            {
                _ when real.Match(line) is Match m && m.Success =>
                    TryParseDecimal(m.Groups["real"].Value) switch
                    {
                        (true, var a) => new RealNumberToken(a),
                        _ => new ErrorToken("Real part is out of range."),
                    },

                _ when complex.Match(line) is Match m && m.Success => (
                    ParseLine(m.Groups["real"].Value),
                    TryParseDecimal(m.Groups["imaginary"].Value)) switch
                    {
                        (RealNumberToken real, (true, var b)) => real.AddImaginaryPart(b),
                        (RealNumberToken real, (false, _)) => new ErrorToken("Imaginary part is out of range."),
                        (var error, _) => error,
                    },

                _ when imaginary.Match(line) is Match m && m.Success =>
                    TryParseDecimal(m.Groups["imaginary"].Value) switch
                    {
                        (true, var b) => new ComplexNumberToken(0m, b),
                        _ => new ErrorToken("Imaginary part is out of range."),
                    },

                _ when set.Match(line) is Match m && m.Success =>
                    new SetToken(Array.ConvertAll(
                        m.Groups["set"].Value.Split(",", StringSplitOptions.RemoveEmptyEntries),
                        ParseLine)
                    ),

                { } => 
                    new ErrorToken("Unknown token"),
                
                null =>
                    throw new ArgumentNullException(nameof(line)),
            };
        }

        private static (bool, decimal) TryParseDecimal(string number) =>
            number switch
            {
                "-" => (true, -1m),
                "+" => (true, 1m),
                _ => (decimal.TryParse(number, 
                        NumberStyles.AllowLeadingSign | NumberStyles.Float, 
                        CultureInfo.InvariantCulture.NumberFormat, 
                        out var parsed), parsed)
            };
    }
}
