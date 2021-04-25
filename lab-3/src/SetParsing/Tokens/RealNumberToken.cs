using System;

namespace SetParsing.Tokens
{
    /// <summary>
    /// Real number token.
    /// </summary>
    class RealNumberToken : TokenBase
    {
        /// <summary>
        /// Real value.
        /// </summary>
        public decimal Value { get; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="number">Number.</param>
        public RealNumberToken(decimal number)
        {
            this.Value = number;
        }

        /// <summary>
        /// Generate complex number from current token.
        /// </summary>
        /// <param name="imaginary">Imaginary part.</param>
        public ComplexNumberToken AddImaginaryPart(decimal imaginary)
            => new(Value, imaginary);

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return base.Equals(obj)
                && obj is RealNumberToken cplx
                && cplx.Value == Value;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), Value);
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"real {Value}";
        }
    }
}
