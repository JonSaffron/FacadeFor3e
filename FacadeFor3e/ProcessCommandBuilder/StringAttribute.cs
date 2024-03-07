using JetBrains.Annotations;

namespace FacadeFor3e.ProcessCommandBuilder
    {
    /// <summary>
    /// A 3E string attribute value
    /// </summary>
    [PublicAPI]
    public sealed class StringAttribute : IAttribute
        {
        /// <summary>
        /// Gets/sets the value of this attribute
        /// </summary>
        public string? Value;

        /// <summary>
        /// Constructs a new 3E string attribute value
        /// </summary>
        /// <param name="value">The initial value of the attribute</param>
        public StringAttribute(string? value)
            {
            this.Value = value;
            }

        /// <inheritdoc />
        public override string ToString()
            {
            return this.Value ?? string.Empty;
            }

        /// <summary>
        /// Implicitly converts a string value to a StringAttribute
        /// </summary>
        /// <param name="value">The string value to convert</param>
        public static implicit operator StringAttribute(string value)
            {
            return new StringAttribute(value);
            }

        /// <inheritdoc />
        object? IAttribute.Value => this.Value;

        /// <inheritdoc />
        public bool HasValue => !string.IsNullOrWhiteSpace(this.Value);
        }
    }