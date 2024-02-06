namespace FacadeFor3e.ProcessCommandBuilder
    {
    /// <summary>
    /// Identifies a row during an edit or delete operation
    /// </summary>
    public abstract class IdentifyBase
        {
        }

    public interface IHasKey
        {
        public IdentifyBase KeySpecification { get; set; }
        }
    }
