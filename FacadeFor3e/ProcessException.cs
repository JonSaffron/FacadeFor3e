using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using JetBrains.Annotations;

namespace FacadeFor3e
    {
    /// <summary>
    /// Represents an error that occurred during execution of a process
    /// </summary>
    [Serializable]
    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public class ProcessException : Exception
        {
        private const string StandardMessage = "An error occurred whilst running a 3e process.";
        public RunProcessParameters RunProcessParameters { get; private set; }
        public RunProcessResult RunProcessResult { get; private set; }
        private readonly List<string> _errorMessages;

        public IEnumerable<string> Errors
            {
            get
                {
                var result = (this._errorMessages == null) ? new List<string>() : new List<string>(this._errorMessages);
                return result;
                }
            }

        /// <summary>
        /// Constructs a new ProcessException
        /// </summary>
        public ProcessException() : base(StandardMessage)
            {
            }

        /// <summary>
        /// Constructs a new ProcessException
        /// </summary>
        /// <param name="message">The message that describes the error</param>
        public ProcessException(string message) : base(message)
            {
            }

        /// <summary>
        /// Constructs a new ProcessException
        /// </summary>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified</param>
        public ProcessException(Exception innerException) : base(StandardMessage, innerException)
            {
            }

        /// <summary>
        /// Constructs a new ProcessException
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified</param>
        public ProcessException(string message, Exception innerException) : base(message, innerException)
            {
            }

        /// <summary>
        /// Constructs a new ProcessException
        /// </summary>
        /// <param name="info">The SerializationInfo that holds the serialized object data about the exception being thrown</param>
        /// <param name="context">The StreamingContext that contains contextual information about the source or destination</param>
        protected ProcessException(SerializationInfo info, StreamingContext context) : base(info, context)
            {
            }

        /// <summary>
        /// Constructs a new ProcessException
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception</param>
        /// <param name="p">The run process parameters</param>
        /// <param name="r">The run process result</param>
        public ProcessException(string message, RunProcessParameters p, RunProcessResult r) : base(message)
            {
            this.RunProcessParameters = p;
            this.RunProcessResult = r;
            }

        /// <summary>
        /// Constructs a new ProcessException
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified</param>
        /// <param name="p">The run process parameters</param>
        /// <param name="r">The run process result</param>
        public ProcessException(string message, Exception innerException, RunProcessParameters p, RunProcessResult r) : base(message, innerException)
            {
            this.RunProcessParameters = p;
            this.RunProcessResult = r;
            }

        /// <summary>
        /// Constructs a new ProcessException
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception</param>
        /// <param name="p">The run process parameters</param>
        /// <param name="r">The run process result</param>
        /// <param name="errorMessages">The list of error messages from 3e</param>
        public ProcessException(string message, RunProcessParameters p, RunProcessResult r, IEnumerable<string> errorMessages) : base(message)
            {
            this.RunProcessParameters = p;
            this.RunProcessResult = r;
            this._errorMessages = (errorMessages == null) ? null : new List<string>(errorMessages); 
            }
        }
    }
