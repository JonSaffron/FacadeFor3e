using JetBrains.Annotations;

namespace FacadeFor3e
    {
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
        SaveFirstEndOnNoErrors = 1,

        /// <summary>
        /// Calls the 3E Save event for the object collection will route to the original first step in the process and continue with the defined behavior from that point
        /// whether there are validation errors or not.
        /// </summary>
        RouteFirstExecuteStandardProcess = 2,
        }
    }
