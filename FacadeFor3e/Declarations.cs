using System.Diagnostics.CodeAnalysis;

namespace FacadeFor3e
    {
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public enum PriorityEnum
        {
        LOW,
        MEDIUM,
        HIGH
        }

    /// <summary>
    /// Indicates how the execution of a 3e application process should be altered 
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public enum ProcessExecutionRequestTypeEnum
        {
        /// <summary>
        ///  Will leave the original process behavior unaffected and is the expected behavior if this value is not passed.
        /// </summary>
        Default,

        /// <summary>
        /// Calls the 3E Save event for the object collection and if there are no validation errors in the data, will exit.
        /// If there are errors it will route to the original first step in the process and continue with the defined behavior from that point.
        /// </summary>
        SaveFirstEndOnNoErrors,

        /// <summary>
        /// Calls the 3E Save event for the object collection will route to the original first step in the process and continue with the defined behavior from that point
        /// whether there are validation errors or not.
        /// </summary>
        RouteFirstExecuteStandardProcess,

        Default_SuppressChildAutogeneration,
        SaveFirstEndOnNoErrors_SuppressChildAutogeneration,
        RouteFirstExecuteStandardProcess_SuppressChildAutogeneration
        }
    }
