using System;
using JetBrains.Annotations;

namespace FacadeFor3e
    {
    /// <summary>
    /// Used to specify how the running of a 3E application process should be altered
    /// </summary>
    [PublicAPI]
    public class ProcessExecutionRequestType
        {
        /// <summary>
        /// Returns the specified process execution request type
        /// </summary>
        public readonly string Type;

        /// <summary>
        /// Creates a custom process execution request type
        /// </summary>
        /// <param name="processExecutionRequestType"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ProcessExecutionRequestType(string processExecutionRequestType)
            {
            this.Type = processExecutionRequestType ?? throw new ArgumentNullException(nameof(processExecutionRequestType));
            }

        /// <summary>
        /// Creates a predefined process execution request type
        /// </summary>
        /// <param name="processExecutionRequestType">Specifies how the behaviour </param>
        /// <param name="suppressChildAutoGeneration"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public ProcessExecutionRequestType(ProcessExecutionRequestTypeEnum processExecutionRequestType, bool suppressChildAutoGeneration = false)
            {
            if (!Enum.IsDefined(typeof(ProcessExecutionRequestTypeEnum), processExecutionRequestType))
                {
                throw new ArgumentOutOfRangeException(nameof(processExecutionRequestType));
                }

            this.Type = $"{processExecutionRequestType:G}{(suppressChildAutoGeneration ? "_SuppressChildAutogeneration" : string.Empty)}";
            }
        }
    }
