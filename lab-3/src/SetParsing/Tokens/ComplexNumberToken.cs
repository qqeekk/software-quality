using System;

namespace SetParsing.Tokens
{
    /// <summary>
    /// Complex number.
    /// </summary>
    class ComplexNumberToken : TokenBase
    {
        protected readonly decimal Real;
        protected readonly decimal Imaginary;

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
