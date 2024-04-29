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
            CleanupProcessOnFailure = true,
            AliasAttributesSupportBeingNamed = true
            };

        /// <summary>
        /// When set to False, an exception will be thrown if the call to OData returns an error message
        /// </summary>
        public bool ThrowExceptionIfProcessFails = true;

        /// <summary>
        /// When set to False, a process created during Post or Patch requests handling will not be released
        /// </summary>
        public bool ReleaseProcess = true;

        /// <summary>
        /// When set to False, in case of a validation error or 3E exception during Post or Patch requests handling, the created process will not be cancelled
        /// </summary>
        public bool CleanupProcessOnFailure = true;

        /// <summary>
        /// When set to False, a process created during Post or Patch requests handling will not be released.
        /// </summary>
        public string? OutputStepOverride;

        /// <summary>
        /// Specifies the owner of the process
        /// </summary>
        public Guid? RoleId;

        /// <summary>
        /// Specifies that alias attributes can be rendered to JSON with explicit names
        /// </summary>
        /// <remarks>In older versions of the 3E OData service you cannot specify which attribute is being used when specifying an attribute value via an alias such as Timekeeper.Number.
        /// If you need to revert to the older rendering then set this to False</remarks>
        public bool AliasAttributesSupportBeingNamed = true;
        }
    }
