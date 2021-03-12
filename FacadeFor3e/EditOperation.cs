using System;
using System.Xml;

namespace FacadeFor3e
    {
    public class EditOperation : OperationWithAttributesBase
        {
        private IKeySpecification _keySpecification;

        public EditOperation(IKeySpecification keySpecification, string subClass = null)
            {
            this._keySpecification = keySpecification ?? throw new ArgumentNullException(nameof(keySpecification));
            this.SubClass = subClass;
            }

        public IKeySpecification KeySpecification
            {
            get => this._keySpecification;

            set
                {
                this._keySpecification = value ?? throw new ArgumentNullException(nameof(value));
                }
            }

        /// <summary>
        /// Outputs this operation
        /// </summary>
        /// <param name="writer">An XMLWriter to output to</param>
        /// <param name="objectSuperclassName">The name of the parent data object for when SubClass is not specified</param>
        protected internal override void Render(XmlWriter writer, string objectSuperclassName)
            {
            writer.WriteStartElement("Edit");
            writer.WriteStartElement(this.SubClass ?? objectSuperclassName);

            this.KeySpecification.RenderKey(writer);

            writer.WriteEndElement();
            writer.WriteEndElement();
            }
        }
    }