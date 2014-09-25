using System.Xml;

namespace FacadeFor3e
    {
    /// <summary>
    /// Defines an operation to add a record
    /// </summary>
    public class OperationAdd : OperationWithAttributesBase
        {
        /// <summary>
        /// Outputs this operation
        /// </summary>
        /// <param name="writer">An XMLWriter to output to</param>
        /// <param name="objectName">The name of the parent data object</param>
        protected internal override void Render(XmlWriter writer, string objectName)
            {
            writer.WriteStartElement("Add");
            writer.WriteStartElement(objectName);

            if (this.Attributes.Count > 0)
                {
                writer.WriteStartElement("Attributes");
                foreach (AttributeValue a in this.Attributes)
                    a.Render(writer);
                writer.WriteEndElement();
                }

            if (this.Children.Count > 0)
                {
                writer.WriteStartElement("Children");
                foreach (DataObject a in this.Children)
                    a.Render(writer);
                writer.WriteEndElement();
                }

            writer.WriteEndElement();
            writer.WriteEndElement();
            }
        }
    }
