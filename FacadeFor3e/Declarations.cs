using JetBrains.Annotations;

// ReSharper disable InconsistentNaming
namespace FacadeFor3e
    {
    /// <summary>
    /// Process priority
    /// </summary>
    [PublicAPI]
    public enum PriorityEnum
        {
        /// <summary>
        /// Low priority
        /// </summary>
        LOW,

        /// <summary>
        /// Medium priority
        /// </summary>
        MEDIUM,

        /// <summary>
        /// High priority
        /// </summary>
        HIGH
        }

    /// <summary>
    /// Indicates how the execution of a 3E application process should be altered 
    /// </summary>
    /// <remarks>Only place that I can currently see where SuppressChildAutoGeneration would have any effect is the FAAsset object</remarks>
    [PublicAPI]
    public enum ProcessExecutionRequestTypeEnum
        {
        /// <summary>
        ///  Will leave the original process behavior unaffected and is the expected behavior if this value is not passed.
        /// </summary>
        Default = 0,

        /// <summary>
        /// Calls the 3E Save event for the object collection and if there are no validation errors in the data, will exit.
        /// If there are errors it will route to the original first step in the process and continue with the defined behavior from that point.
        /// </summary>
        /// <remarks>Cannot be used in conjunction with <see cref="RouteFirstExecuteStandardProcess" /></remarks>
        SaveFirstEndOnNoErrors = 1,

        /// <summary>
        /// Calls the 3E Save event for the object collection will route to the original first step in the process and continue with the defined behavior from that point
        /// whether there are validation errors or not.
        /// </summary>
        /// <remarks>Cannot be used in conjunction with <see cref="SaveFirstEndOnNoErrors" /></remarks>
        RouteFirstExecuteStandardProcess = 2,

        /// <summary>
        /// Leaves the original process behaviour unaffected, but sets the Process.IsRequestingChildObjectAutogenSuppression flag.
        /// </summary> 
        Default_SuppressChildAutogeneration = 0x100,

        /// <summary>
        /// Same as <see cref="SaveFirstEndOnNoErrors"/>, but sets the Process.IsRequestingChildObjectAutogenSuppression flag.
        /// </summary>
        SaveFirstEndOnNoErrors_SuppressChildAutogeneration = 0x101,

        /// <summary>
        /// Same as <see cref="RouteFirstExecuteStandardProcess"/>, but sets the Process.IsRequestingChildObjectAutogenSuppression flag.
        /// </summary>
        RouteFirstExecuteStandardProcess_SuppressChildAutogeneration = 0x102
        }

    public class ExecuteProcessParams
        {
        private static readonly ExecuteProcessParams _default = new ExecuteProcessParams
            {
            GetKeys = false,
            ThrowExceptionIfDataErrorsFound = true,
            ThrowExceptionIfProcessDoesNotComplete = true
            };

        private static readonly ExecuteProcessParams _defaultWithKeys = new ExecuteProcessParams
            {
            GetKeys = true,
            ThrowExceptionIfDataErrorsFound = true,
            ThrowExceptionIfProcessDoesNotComplete = true
            };

        public bool GetKeys;

        public bool ThrowExceptionIfProcessDoesNotComplete;

        public bool ThrowExceptionIfDataErrorsFound;

        public static ExecuteProcessParams Default => _default;

        public static ExecuteProcessParams DefaultWithKeys => _defaultWithKeys;
        }
    }
