using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace FacadeFor3e
    {
    /// <summary>
    /// Provides options to be used with <see cref="ExecuteProcessService"/>
    /// </summary>
    [PublicAPI]
    public sealed class ExecuteProcessOptions
        {
        /// <summary>
        /// Default list of process step IDs that indicate failure
        /// </summary>
        public static IList<string> DefaultOutputIdsThatIndicateFailure { get; } = new List<string> { "Failure" };

        /// <summary>
        /// Asks 3E to return the primary key of any new records created
        /// </summary>
        /// <remarks>Only records created at the top level will have their primary keys returned</remarks>
        public bool GetKeys;

        /// <summary>
        /// When true, the <see cref="ExecuteProcessService"/> will throw an exception if the process stops at a page step
        /// </summary>
        public bool ThrowExceptionIfProcessDoesNotComplete;

        /// <summary>
        /// When true, the <see cref="ExecuteProcessService"/> will throw an exception if data validation errors are returned
        /// </summary>
        public bool ThrowExceptionIfDataErrorsFound;

        /// <summary>
        /// Gets or sets the proxy user.
        /// </summary>
        public Guid? ProxyUser { get; set; }

        /// <summary>
        /// Gets or returns the process CheckSum.
        /// </summary>
        /// <remarks>Value is stored in NxFwkProcessItem.CheckSum</remarks>
        public decimal? CheckSum { get; set; }

        /// <summary>
        /// Gets or sets how the execution of the process should be altered from the default.
        /// </summary>
        /// <remarks>TransactionServices only, and it didn't seem to work even then</remarks>
        public ProcessExecutionRequestType? ProcessExecutionRequestType { get; set; }

        /// <summary>
        /// Gets or sets the user role to use for the first step when ProcessRequestType is not set to Default.
        /// </summary>
        public string? ProcessAutomationRoleAfterFirstStep { get; set; }

        /// <summary>
        /// Validates request is coming from an authorized external application.
        /// </summary>
        public string? ProcessRequestSignature { get; set; }

        /// <summary>
        /// List of process step IDs that indicate failure
        /// </summary>
        public IList<string>? OutputIdsThatIndicateFailure { get; set; } = DefaultOutputIdsThatIndicateFailure;

        /// <summary>
        /// Specifies the default options of throwing errors if the process does not complete or if data errors are returned
        /// </summary>
        /// <remarks>Equivalent to setting <see cref="ThrowExceptionIfProcessDoesNotComplete"/> and <see cref="ThrowExceptionIfDataErrorsFound"/> to true</remarks>
        public static ExecuteProcessOptions Default { get; } = new ExecuteProcessOptions
            {
            GetKeys = false,
            ThrowExceptionIfDataErrorsFound = true,
            ThrowExceptionIfProcessDoesNotComplete = true
            };

        /// <summary>
        /// Specifies the default options of throwing errors if the process does not complete or if data errors are returned and returning the keys of new records
        /// </summary>
        /// <remarks>Equivalent to setting <see cref="ThrowExceptionIfProcessDoesNotComplete"/>, <see cref="ThrowExceptionIfDataErrorsFound"/>, and <see cref="GetKeys"/> to true</remarks>
        public static ExecuteProcessOptions DefaultWithKeys { get; } = new ExecuteProcessOptions
            {
            GetKeys = true,
            ThrowExceptionIfDataErrorsFound = true,
            ThrowExceptionIfProcessDoesNotComplete = true
            };
        }
    }
