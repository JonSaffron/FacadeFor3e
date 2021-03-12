using System.Xml;
using JetBrains.Annotations;

namespace FacadeFor3e
    {
    /// <summary>
    /// Defines an operation to delete a record
    /// </summary>
    public abstract class OperationDelete : OperationBase
        {
        protected abstract void RenderKey(XmlWriter writer);

        /// <summary>
        /// Outputs this operation
        /// </summary>
        /// <param name="writer">An XMLWriter to output to</param>
        /// <param name="objectSuperclassName">The name of the parent data object for when SubClass is not specified</param>
        protected internal override void Render(XmlWriter writer, string objectSuperclassName)
            {
            writer.WriteStartElement("Delete");
            writer.WriteStartElement(this.SubClass ?? objectSuperclassName);

            RenderKey(writer);

            writer.WriteEndElement();
            writer.WriteEndElement();
            }
        }
    }
