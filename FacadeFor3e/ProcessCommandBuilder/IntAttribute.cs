using System.Globalization;
using JetBrains.Annotations;

namespace FacadeFor3e.ProcessCommandBuilder
    {
    /// <summary>
    /// A 3E integer attribute value
    /// </summary>
    [PublicAPI]
    public sealed class IntAttribute : IAttribute
        {
        /// <summary>
        /// Gets/sets the value of this attribute
        /// </summary>
        public int? Value;

        /// <summary>
        /// Constructs a new 3E integer attribute value
        /// </summary>
        /// <param name="value">The initial value of the attribute</param>
        public IntAttribute(int? value)
            {
            this.Value = value;
            }

        /// <inheritdoc />
        public override string ToString()
            {
            return this.Value.HasValue ? this.Value.Value.ToString(CultureInfo.InvariantCulture) : string.Empty;
            }

        /// <summary>
        /// Implicitly converts an integer value to a IntAttribute
        /// </summary>
        /// <param name="value">The integer value to convert</param>
        public static implicit operator IntAttribute(int value)
            {
            return new IntAttribute(value);
            }

        /// <inheritdoc />
        object? IAttribute.Value => this.Value;

        /// <inheritdoc />
        public bool HasValue => this.Value.HasValue;
        }
    }