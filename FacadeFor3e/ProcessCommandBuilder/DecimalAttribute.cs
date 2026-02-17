using System.Globalization;
using JetBrains.Annotations;

namespace FacadeFor3e.ProcessCommandBuilder
    {
    /// <summary>
    /// A 3E decimal attribute value
    /// </summary>
    [PublicAPI]
    public readonly struct DecimalAttribute : IAttribute
        {
        /// <summary>
        /// Gets/sets the value of this attribute
        /// </summary>
        public readonly decimal? Value;

        /// <summary>
        /// Constructs a new 3E decimal attribute value
        /// </summary>
        /// <param name="value">The initial value of the attribute</param>
        public DecimalAttribute(decimal? value)
            {
            this.Value = value;
            }

        /// <inheritdoc />
        public override string ToString()
            {
            return this.Value.HasValue ? this.Value.Value.ToString(CultureInfo.InvariantCulture) : string.Empty;
            }

        /// <summary>
        /// Implicitly converts a decimal value to a DecimalAttribute
        /// </summary>
        /// <param name="value">The decimal value to convert</param>
        public static implicit operator DecimalAttribute(decimal value)
            {
            return new DecimalAttribute(value);
            }

        /// <inheritdoc />
        object? IAttribute.Value => this.Value;

        /// <inheritdoc />
        public bool HasValue => this.Value.HasValue;
        }
    }