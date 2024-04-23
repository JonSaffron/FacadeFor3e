using System;
using JetBrains.Annotations;

namespace FacadeFor3e
    {
    /// <summary>
    /// Allows the process priority to be specified
    /// </summary>
    [PublicAPI]
    public sealed class ProcessPriority
        {
        /// <summary>
        /// Returns the specified process priority
        /// </summary>
        public readonly string Priority;

        /// <summary>
        /// Creates a custom priority for the process
        /// </summary>
        /// <param name="priority">The priority to assign</param>
        /// <exception cref="ArgumentNullException">If the priority specified is null</exception>
        /// <exception cref="ArgumentOutOfRangeException">If the priority specified is too long</exception>
        public ProcessPriority(string priority)
            {
            if (priority == null) throw new ArgumentNullException(nameof(priority));
            if (priority.Length > 50)
                throw new ArgumentOutOfRangeException(nameof(priority), "Maximum length of priority is 50 characters");
            this.Priority = priority;
            }

        /// <summary>
        /// Low priority
        /// </summary>
        public static ProcessPriority Low => new ProcessPriority("LOW");

        /// <summary>
        /// Medium priority
        /// </summary>
        public static ProcessPriority Medium => new ProcessPriority("MEDIUM");

        /// <summary>
        /// High priority
        /// </summary>
        public static ProcessPriority High => new ProcessPriority("HIGH");
        }
    }
