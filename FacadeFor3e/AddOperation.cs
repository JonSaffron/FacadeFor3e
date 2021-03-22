using System.Xml;

namespace FacadeFor3e
    {
    /// <summary>
    /// Defines an operation to add a record
    /// </summary>
    public class AddOperation : OperationWithAttributesBase
        {
        /// <summary>
        /// Creates a new Add operation for the specified subclass
        /// </summary>
        /// <param name="subClass">The name of the subclass of object to create</param>
        public AddOperation(string subClass = null)
            {
            this.SubClass = subClass;
            }

        /// <summary>
        /// Outputs this operation
        /// </summary>
        /// <param name="writer">An XMLWriter to output to</param>
        /// <param name="objectSuperclassName">The name of the parent data object for when SubClass is not specified</param>
        protected internal override void Render(XmlWriter writer, string objectSuperclassName)
            {
            writer.WriteStartElement("Add");
            writer.WriteStartElement(this.SubClass ?? objectSuperclassName);

            RenderAttributes(writer);
            RenderChildren(writer);

            writer.WriteEndElement();
            writer.WriteEndElement();
            }
        }
    }
