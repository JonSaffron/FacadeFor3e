using System.Xml;

namespace FacadeFor3e.ProcessCommandBuilder
    {
    /// <summary>
    /// Provides a way to identify a row
    /// </summary>
    public abstract class IdentifyBase
        {
        /// <summary>
        /// Writes the key specification text
        /// </summary>
        /// <param name="writer">Specifies the object to write to</param>
        protected internal abstract void RenderKey(XmlWriter writer);
        }
    }
