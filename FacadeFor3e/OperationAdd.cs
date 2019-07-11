using System;
using System.Xml;

namespace FacadeFor3e
    {
    /// <summary>
    /// Defines an operation to add a record
    /// </summary>
    public class OperationAdd : OperationWithAttributesBase
        {
        /// <summary>
        /// The ID of the template to use for the Add operation
        /// </summary>
        public Guid? TemplateId { get; set; }

        /// <summary>
        /// The ID of the existing row to clone
        /// </summary>
        public Guid? ModelRowId { get; set; }

        /// <summary>
        /// The subclass of object to create
        /// </summary>
        /// <remarks>If null, this defaults to the parent data object</remarks>
        public string SubClass { get; set; }

        /// <summary>
        /// Creates a new Add operation for the specified subclass
        /// </summary>
        /// <param name="subClass">The name of the subclass of object to create</param>
        /// <returns>An Add operation</returns>
        public static OperationAdd AddSubclass(string subClass)
            {
            var result = new OperationAdd { SubClass = subClass };
            return result;
            }

        /// <summary>
        /// Creates a new Add operation using a template
        /// </summary>
        /// <param name="modelId">The ID of the template to use</param>
        /// <returns>An Add operation</returns>
        public static OperationAdd AddFromModel(Guid modelId)
            {
            var result = new OperationAdd {TemplateId = modelId};
            return result;
            }

        /// <summary>
        /// Creates a new Add operation that clones an existing row
        /// </summary>
        /// <param name="modelRowId">The ID of the existing row</param>
        /// <returns>An Add operation</returns>
        public static OperationAdd AddModelFromDb(Guid modelRowId)
            {
            var result = new OperationAdd {ModelRowId = modelRowId};
            return result;
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
