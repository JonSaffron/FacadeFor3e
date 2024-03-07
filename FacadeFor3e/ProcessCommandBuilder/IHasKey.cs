namespace FacadeFor3e.ProcessCommandBuilder
    {
    /// <summary>
    /// Describes an operation that needs to identify a particular row
    /// </summary>
    public interface IHasKey
        {
        /// <summary>
        /// Gets or sets the object identifying which row to act on during an edit or delete operation
        /// </summary>
        public IdentifyBase KeySpecification { get; set; }
        }
    }