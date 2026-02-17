using System;
using JetBrains.Annotations;

namespace FacadeFor3e.ProcessCommandBuilder
    {
    /// <summary>
    /// A 3E guid attribute value
    /// </summary>
    [PublicAPI]
    public readonly struct GuidAttribute : IAttribute
        {
        /// <summary>
        /// Gets/sets the value of this attribute
        /// </summary>
        public readonly Guid? Value;

        /// <summary>
        /// Constructs a new 3E guid attribute value
        /// </summary>
        /// <param name="value">The initial value of the attribute</param>
        public GuidAttribute(Guid? value)
            {
            this.Value = value;
            }

        /// <inheritdoc />
        public override string ToString()
            {
            return this.Value.HasValue ? this.Value.Value.ToString("D") : string.Empty;
            }

        /// <summary>
        /// Implicitly converts a guid value to a GuidAttribute
        /// </summary>
        /// <param name="value">The guid value to convert</param>
        public static implicit operator GuidAttribute(Guid value)
            {
            return new GuidAttribute(value);
            }

        /// <inheritdoc />
        object? IAttribute.Value => this.Value;

        /// <inheritdoc />
        public bool HasValue => this.Value.HasValue;
        }
    }