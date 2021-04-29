using System;

namespace SetParsing.Tokens
{
    /// <summary>
    /// Error code.
    /// </summary>
    public enum ErrorCode : short
    {
        OutOfRange,
        Unknown,
    }

    /// <summary>
    /// Error token.
    /// </summary>
    public class ErrorToken : TokenBase
    {
        public ErrorCode Code { get; }

        /// <summary>
        /// Error message.
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public ErrorToken(ErrorCode code, string message = null)
        {
            Code = code;
            this.Message = message ?? string.Empty;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"error \"{Message}\"";
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return base.Equals(obj)
                && obj is ErrorToken token
                && Code == token.Code;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), Code);
        }
    }
}
