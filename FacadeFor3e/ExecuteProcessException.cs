using System;
using JetBrains.Annotations;

namespace FacadeFor3e
    {
    /// <summary>
    /// Represents an error that occurred during execution of a process
    /// </summary>
    [PublicAPI]
    public class ExecuteProcessException : Exception
        {
        /// <summary>
        /// Where available, this returns an object providing information on the response returned from the Transaction Service 
        /// </summary>
        public ExecuteProcessResult? ExecuteProcessResult { get; }

        /// <summary>
        /// Constructs a new ExecuteProcessException
        /// </summary>
        /// <param name="message">The message that describes the error</param>
        public ExecuteProcessException(string message) : base(message)
            {
            }

        /// <summary>
        /// Constructs a new ExecuteProcessException
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception</param>
        /// <param name="executeProcessResult">The run process result</param>
        public ExecuteProcessException(string message, ExecuteProcessResult executeProcessResult) : base(message)
            {
            this.ExecuteProcessResult = executeProcessResult ?? throw new ArgumentNullException(nameof(executeProcessResult));
            }
        }
    }
