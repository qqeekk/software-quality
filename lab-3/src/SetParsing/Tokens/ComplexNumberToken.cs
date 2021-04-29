using System;

namespace SetParsing.Tokens
{
    /// <summary>
    /// Complex number.
    /// </summary>
    public class ComplexNumberToken : TokenBase
    {
        /// <summary>
        /// Real part.
        /// </summary>
        public decimal Real { get; }

        /// <summary>
        /// Imaginary part.
        /// </summary>
        public decimal Imaginary { get; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="real">Real part.</param>
        /// <param name="imaginary">Imagionary part.</param>
        public ComplexNumberToken(decimal real, decimal imaginary)
        {
            this.Real = real;
            this.Imaginary = imaginary;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return base.Equals(obj)
                && obj is ComplexNumberToken cplx
                && cplx.Real == Real
                && cplx.Imaginary == Imaginary;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), Real, Imaginary);
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"complex {Real}{Imaginary:+;;+}{Imaginary}i";
        }
    }
}
