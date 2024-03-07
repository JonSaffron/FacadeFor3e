using JetBrains.Annotations;

namespace FacadeFor3e.ProcessCommandBuilder
    {
    /// <summary>
    /// A 3E Boolean attribute value
    /// </summary>
    /// <remarks>Boolean attributes don't support null values</remarks>
    [PublicAPI]
    public sealed class BoolAttribute : IAttribute
        {
        /// <summary>
        /// Gets/sets the value of this attribute
        /// </summary>
        public bool Value;

        /// <summary>
        /// Constructs a new 3E Boolean attribute value
        /// </summary>
        /// <param name="value">The initial value of the attribute</param>
        public BoolAttribute(bool value)
            {
            this.Value = value;
            }

        /// <inheritdoc />
        public override string ToString()
            {
            return this.Value.ToString().ToLowerInvariant();
            }

        /// <summary>
        /// Implicitly converts a Boolean value to a BooleanAttribute
        /// </summary>
        /// <param name="value">The Boolean value to convert</param>
        public static implicit operator BoolAttribute(bool value)
            {
            return new BoolAttribute(value);
            }

        /// <inheritdoc />
        object IAttribute.Value => this.Value;


        /// <inheritdoc />
        public bool HasValue => true;
        }
    }