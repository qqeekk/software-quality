using System;
using System.Linq;

namespace SetParsing.Tokens
{
    /// <summary>
    /// Set token.
    /// </summary>
    class SetToken : TokenBase
    {
        private readonly TokenBase[] tokens;

        /// <summary>
        /// Constructor.
        /// </summary>
        public SetToken(params TokenBase[] tokens)
        {
            this.tokens = tokens;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return base.Equals(obj)
                && obj is SetToken set
                && Enumerable.SequenceEqual(tokens, set.tokens);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return tokens.Aggregate(base.GetHashCode(), HashCode.Combine);
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"set {string.Join(" ", tokens.Select(t => $"({t})"))}";
        }
    }
}
