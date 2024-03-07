using JetBrains.Annotations;

namespace FacadeFor3e.ProcessCommandBuilder
    {
    /// <summary>
    /// An untyped 3E attribute value
    /// </summary>
    [PublicAPI]
    public interface IAttribute
        {
        /// <summary>
        /// Gets the untyped attribute value
        /// </summary>
        object? Value { get; }

        /// <summary>
        /// Returns whether the attribute has a non-null value
        /// </summary>
        bool HasValue { get; }
        }
    }