namespace SetParsing.Tokens
{
    /// <summary>
    /// Error token.
    /// </summary>
    public class ErrorToken : TokenBase
    {
        /// <summary>
        /// Error message.
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public ErrorToken(string message)
        {
            this.Message = message;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"error \"{Message}\"";
        }
    }
}
