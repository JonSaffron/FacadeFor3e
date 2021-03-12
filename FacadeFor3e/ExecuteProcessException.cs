using System;
using System.Runtime.Serialization;
using JetBrains.Annotations;

namespace FacadeFor3e
    {
    /// <summary>
    /// Represents an error that occurred during execution of a process
    /// </summary>
    [Serializable]
    [PublicAPI]
    public class ExecuteProcessException : Exception
        {
        private const string StandardMessage = "An error occurred whilst running a 3E process.";
        
        /// <summary>
        /// Where available, this returns an object providing information on the response returned from the Transaction Service 
        /// </summary>
        public ExecuteProcessResult? ExecuteProcessResult { get; private set; }

        /// <summary>
        /// Constructs a new ProcessException
        /// </summary>
        public ExecuteProcessException() : base(StandardMessage)
            {
            }

        /// <summary>
        /// Constructs a new ProcessException
        /// </summary>
        /// <param name="message">The message that describes the error</param>
        public ExecuteProcessException(string message) : base(message)
            {
            }

        /// <summary>
        /// Constructs a new ProcessException
        /// </summary>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified</param>
        public ExecuteProcessException(Exception innerException) : base(StandardMessage, innerException)
            {
            }

        /// <summary>
        /// Constructs a new ProcessException
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified</param>
        public ExecuteProcessException(string message, Exception innerException) : base(message, innerException)
            {
            }

        /// <summary>
        /// Constructs a new ProcessException
        /// </summary>
        /// <param name="info">The SerializationInfo that holds the serialized object data about the exception being thrown</param>
        /// <param name="context">The StreamingContext that contains contextual information about the source or destination</param>
        protected ExecuteProcessException(SerializationInfo info, StreamingContext context) : base(info, context)
            {
            }

        /// <summary>
        /// Constructs a new ProcessException
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception</param>
        /// <param name="executeProcessResult">The run process result</param>
        public ExecuteProcessException(string message, ExecuteProcessResult executeProcessResult) : base(message)
            {
            this.ExecuteProcessResult = executeProcessResult;
            }

        /// <summary>
        /// Constructs a new ProcessException
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified</param>
        /// <param name="executeProcessResult">The run process result</param>
        public ExecuteProcessException(string message, Exception innerException, ExecuteProcessResult executeProcessResult) : base(message, innerException)
            {
            this.ExecuteProcessResult = executeProcessResult;
            }
        }
    }
