using System.Xml;

namespace FacadeFor3e
    {
    /// <summary>
    /// Defines an operation to update a record
    /// </summary>
    public abstract class OperationEdit : OperationWithAttributesBase
        {
        protected abstract void RenderKey(XmlWriter writer);

        /// <summary>
        /// Outputs this operation
        /// </summary>
        /// <param name="writer">An XMLWriter to output to</param>
        /// <param name="objectName">The name of the parent data object</param>
        protected internal override void Render(XmlWriter writer, string objectName)
            {
            writer.WriteStartElement("Edit");
            writer.WriteStartElement(objectName);

            RenderKey(writer);

            RenderAttributes(writer);
            RenderChildren(writer);

            writer.WriteEndElement();
            writer.WriteEndElement();
            }
        }
    }
