using System.Xml;

namespace FacadeFor3e
    {
    /// <summary>
    /// Defines the interface for an operation upon an object
    /// </summary>
    public abstract class OperationBase
        {
        /// <summary>
        /// Output the operation
        /// </summary>
        /// <param name="writer">An XMLWriter to output to</param>
        /// <param name="objectName">The name of the parent data object</param>
        protected internal abstract void Render(XmlWriter writer, string objectName);
        }
    }
