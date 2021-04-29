using System;
using System.Linq;

namespace SetParsing.Tokens
{
    /// <summary>
    /// Set token.
    /// </summary>
    public class SetToken : TokenBase
    {
        /// <summary>
        /// Element tokens.
        /// </summary>
        public TokenBase[] Tokens { get; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public SetToken(params TokenBase[] tokens)
        {
            this.Tokens = tokens;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return base.Equals(obj)
                && obj is SetToken set
                && Enumerable.SequenceEqual(Tokens, set.Tokens);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return Tokens.Aggregate(base.GetHashCode(), HashCode.Combine);
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"set {string.Join(" ", Tokens.Select(t => $"({t})"))}";
        }
    }
}
