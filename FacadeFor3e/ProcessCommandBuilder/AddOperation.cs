using System;
using System.Xml;
using JetBrains.Annotations;

namespace FacadeFor3e.ProcessCommandBuilder
    {
    /// <summary>
    /// Defines an operation to add a record
    /// </summary>
    [PublicAPI]
    public class AddOperation : OperationWithAttributesBase
        {
        /// <summary>
        /// Creates a new Add operation
        /// </summary>
        public AddOperation()
            {
            }

        /// <summary>
        /// Creates a new Add operation for the specified subclass
        /// </summary>
        /// <param name="subClass">The id of the subclass of object to create</param>
        public AddOperation(string subClass)
            {
            this.SubClass = subClass ?? throw new ArgumentNullException(nameof(subClass));
            CommonLibrary.IsValidId(subClass);
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
