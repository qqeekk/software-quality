using System;

namespace SetParsing
{
    /// <summary>
    /// Abstract token.
    /// </summary>
    public abstract class TokenBase
    {
        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return GetType().Equals(obj?.GetType());
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return HashCode.Combine(GetType(), 40056);
        }
    }
}
