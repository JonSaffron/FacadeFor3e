using System;
using System.Xml;

namespace FacadeFor3e
    {
    public class DeleteOperation : OperationBase
        {
        private IKeySpecification _keySpecification;

        public DeleteOperation(IKeySpecification keySpecification, string subClass = null)
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
            writer.WriteStartElement("Delete");
            writer.WriteStartElement(this.SubClass ?? objectSuperclassName);

            this.KeySpecification.RenderKey(writer);

            writer.WriteEndElement();
            writer.WriteEndElement();
            }


        public DeleteOperation(int primaryKey)
            {
            this.KeySpecification = new IdentifyByPrimaryKey<IntAttribute>(primaryKey);
            }

        public DeleteOperation(string primaryKey)
            {
            this.KeySpecification = new IdentifyByPrimaryKey<StringAttribute>(primaryKey);
            }

        public DeleteOperation(Guid primaryKey)
            {
            this.KeySpecification = new IdentifyByPrimaryKey<GuidAttribute>(primaryKey);
            }
        }
    }
