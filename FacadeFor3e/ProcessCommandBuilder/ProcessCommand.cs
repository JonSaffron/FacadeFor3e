using System;
using JetBrains.Annotations;

namespace FacadeFor3e.ProcessCommandBuilder
    {
    /// <summary>
    /// Defines a process for performing one or more operations on an object
    /// </summary>
    [PublicAPI]
    public class ProcessCommand : DataObject
        {
        /// <summary>
        /// Constructs a new process
        /// </summary>
        /// <param name="processCode">Specifies the process to run</param>
        /// <param name="objectName">Name of the object to affect</param>
        public ProcessCommand(string processCode, string objectName)
            : base(objectName)
            {
            this.ProcessCode = processCode ?? throw new ArgumentNullException(nameof(processCode));
            CommonLibrary.EnsureValid(processCode);
            }

        /// <summary>
        /// Returns the process code.
        /// </summary>
        public string ProcessCode { get; }

        /// <summary>
        /// Sets or returns the process name displayed in a user's action list.
        /// </summary>
        /// <remarks>Value is stored in NxFwkProcessItem.Name</remarks>
        public string? ProcessName { get; set; }

        /// <summary>
        /// Sets or returns the process description displayed in a user's action list.
        /// </summary>
        /// <remarks>Value is stored in NxFwkProcessItem.Description</remarks>
        public string? Description { get; set; }

        /// <summary>
        /// Sets or returns the process priority.
        /// </summary>
        /// <remarks>Value is stored in NxFwkProcessItem.Priority</remarks>
        public PriorityEnum? Priority { get; set; }

        /// <summary>
        /// Gets or returns the Operating Unit to use whilst running the process.
        /// Setting this can be useful when dealing with vouchers. Its front-end equivalent is the Operating Unit setting in a process's Folder fields.
        /// </summary>
        /// <remarks>Value is stored in NxFwkProcessItem.Unit</remarks>
        public string? OperatingUnit { get; set; }

        /// <summary>
        /// Gets or returns the process CheckSum.
        /// </summary>
        /// <remarks>Value is stored in NxFwkProcessItem.CheckSum</remarks>
        public float? CheckSum { get; set; }

        /// <summary>
        /// Gets or sets the proxy user.
        /// </summary>
        public string? ProxyUser { get; set; }

        /// <summary>
        /// Gets or sets the proxy user id.
        /// </summary>
        public string? ProxyUserId { get; set; }

        /// <summary>
        /// Gets or sets how the execution of the process should be altered from the default.
        /// </summary>
        public ProcessExecutionRequestTypeEnum? ProcessRequestType { get; set; }

        /// <summary>
        /// Gets or sets the user role to use for the first step when ProcessRequestType is not set to Default.
        /// </summary>
        public string? ProcessAutomationRoleAfterFirstStep { get; set; }

        /// <summary>
        /// Validates request is coming from an authorized external application.
        /// </summary>
        public string? ProcessRequestSignature { get; set; }

        /// <summary>
        /// Generates the namespace required for the process
        /// </summary>
        /// <returns>The namespace that applies for the specified process</returns>
        protected internal string ProcessNameSpace => $@"http://elite.com/schemas/transaction/process/write/{this.ProcessCode}";

        /// <summary>
        /// Generates the namespace required for the object
        /// </summary>
        /// <returns>The namespace that applies for the specified object</returns>
        protected internal string ObjectNameSpace => $@"http://elite.com/schemas/transaction/object/write/{this.ObjectName}";
        }
    }
