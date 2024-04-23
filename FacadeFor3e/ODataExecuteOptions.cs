using System;
using JetBrains.Annotations;

namespace FacadeFor3e
    {
    /// <summary>
    /// Provides options to be used with the Execute method of <see cref="ODataServices"/>
    /// </summary>
    [PublicAPI]
    public sealed class ODataExecuteOptions
        {
        /// <summary>
        /// Specifies the default options
        /// </summary>
        public static ODataExecuteOptions Default { get; } = new ODataExecuteOptions
            {
            ThrowExceptionIfProcessFails = true,
            ReleaseProcess = true,
            CleanupProcessOnFailure = true
            };

        /// <summary>
        /// When set to False, an 
        /// </summary>
        public bool ThrowExceptionIfProcessFails;

        /// <summary>
        /// When set to False, a process created during Post or Patch requests handling will not be released
        /// </summary>
        public bool ReleaseProcess;

        /// <summary>
        /// When set to False, in case of a validation error or 3E exception during Post or Patch requests handling, the created process will not be cancelled
        /// </summary>
        public bool CleanupProcessOnFailure;

        /// <summary>
        /// When set to False, a process created during Post or Patch requests handling will not be released.
        /// </summary>
        public string? OutputStepOverride;

        /// <summary>
        /// Specifies the owner of the process
        /// </summary>
        public Guid? RoleId;
        }
    }
