using System;

namespace SetParsing.Tokens
{
    /// <summary>
    /// Real number token.
    /// </summary>
    class RealNumberToken : TokenBase
    {
        private readonly decimal number;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="number">Number.</param>
        public RealNumberToken(decimal number)
        {
            this.number = number;
        }

        /// <summary>
        /// Generate complex number from current token.
        /// </summary>
        /// <param name="imaginary">Imaginary part.</param>
        public ComplexNumberToken AddImaginaryPart(decimal imaginary)
            => new ComplexNumberToken(number, imaginary);

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return base.Equals(obj)
                && obj is RealNumberToken cplx
                && cplx.number == number;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), number);
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"real {number}";
        }
    }
}
