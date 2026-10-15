using JetBrains.Annotations;

namespace FacadeFor3e.ProcessCommandBuilder
    {
    /// <summary>
    /// A 3E null attribute value
    /// </summary>
    [PublicAPI]
    public readonly struct NullAttribute : IAttribute
        {
        /// <summary>
        /// Gets/sets the value of this attribute
        /// </summary>
        public object? Value => null;

        /// <summary>
        /// Constructs a new 3E null attribute value
        /// </summary>
        public NullAttribute()
            {
            }

        /// <inheritdoc />
        public override string ToString()
            {
            return string.Empty;
            }

        /// <inheritdoc />
        object? IAttribute.Value => this.Value;

        /// <inheritdoc />
        public bool HasValue => false;
        }
    }
