using System;
using System.Xml;
using JetBrains.Annotations;

namespace FacadeFor3e.ProcessCommandBuilder
    {
    /// <summary>
    /// Defines an operation to edit a record
    /// </summary>
    [PublicAPI]
    public class EditOperation : OperationWithAttributesBase
        {
        private IdentifyBase _keySpecification;

        /// <summary>
        /// Creates a new Edit operation
        /// </summary>
        /// <param name="keySpecification">Defines which row should be edited</param>
        // ReSharper disable once NotNullMemberIsNotInitialized
        public EditOperation(IdentifyBase keySpecification)
            {
            this._keySpecification = keySpecification ?? throw new ArgumentNullException(nameof(keySpecification));
            }

        /// <summary>
        /// Creates a new Edit operation
        /// </summary>
        /// <param name="keySpecification">Defines which row should be edited</param>
        /// <param name="subClass">The id of the subclass of object to edit</param>
        // ReSharper disable once NotNullMemberIsNotInitialized
        public EditOperation(IdentifyBase keySpecification, string subClass) : this(keySpecification)
            {
            this.SubClass = subClass ?? throw new ArgumentNullException(nameof(subClass));
            }

        /// <summary>
        /// Gets or sets the specification of which row to edit
        /// </summary>
        public IdentifyBase KeySpecification
            {
            get => this._keySpecification;

            set => this._keySpecification = value ?? throw new ArgumentNullException(nameof(value));
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

            RenderAttributes(writer);
            RenderChildren(writer);

            writer.WriteEndElement();
            writer.WriteEndElement();
            }

        /// <summary>
        /// Returns an EditOperation object upon a row identified by primary key
        /// </summary>
        /// <param name="primaryKey">Specifies the primary key</param>
        /// <returns>An EditOperation object that targets a row by primary key</returns>
        public static EditOperation ByPrimaryKey(int primaryKey)
            {
            return new EditOperation(new IdentifyByPrimaryKey(primaryKey));
            }

        /// <summary>
        /// Returns an EditOperation object upon a row identified by primary key
        /// </summary>
        /// <param name="primaryKey">Specifies the primary key</param>
        /// <returns>An EditOperation object that targets a row by primary key</returns>
        public static EditOperation ByPrimaryKey(Guid primaryKey)
            {
            return new EditOperation(new IdentifyByPrimaryKey(primaryKey));
            }

        /// <summary>
        /// Returns an EditOperation object upon a row identified by primary key
        /// </summary>
        /// <param name="primaryKey">Specifies the primary key</param>
        /// <returns>An EditOperation object that targets a row by primary key</returns>
        public static EditOperation ByPrimaryKey(string primaryKey)
            {
            return new EditOperation(new IdentifyByPrimaryKey(primaryKey));
            }

        /// <summary>
        /// Returns an EditOperation object upon a row identified by a unique value found on the specified attribute
        /// </summary>
        /// <param name="alias">Specifies the attribute that contains the value to search for</param>
        /// <param name="value">Specifies the value to search for</param>
        /// <returns>An EditOperation object that targets a row by primary key</returns>
        public static EditOperation ByUniqueAlias(string alias, string value)
            {
            return new EditOperation(new IdentifyByAlias(alias, new StringAttribute(value)));
            }

        /// <summary>
        /// Returns an EditOperation object upon a row in a child collection identified by its position
        /// </summary>
        /// <param name="position">Specifies the row's position in the child collection</param>
        /// <returns>An EditOperation object that targets a row by its position in a child collection</returns>
        public static EditOperation ByPosition(int position)
            {
            return new EditOperation(new IdentifyByPosition(position));
            }
        }
    }
