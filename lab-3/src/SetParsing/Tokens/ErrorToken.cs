namespace SetParsing.Tokens
{
    class ErrorToken : TokenBase
    {
        private readonly string message;

        public ErrorToken(string message)
        {
            this.message = message;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"error \"{message}\"";
        }
    }
}
