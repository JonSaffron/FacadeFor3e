using System;
using System.Linq;
using System.Xml;
using JetBrains.Annotations;

namespace FacadeFor3e.ProcessCommandBuilder
    {
    /// <summary>
    /// Defines the interface for an operation upon an object which requires a collection of attributes
    /// </summary>
    [PublicAPI]
    public abstract class OperationWithAttributesBase : OperationBase
        {
        /// <summary>
        /// Constructs a new OperationWithAttributesBase object
        /// </summary>
        protected OperationWithAttributesBase()
            {
            this.Attributes = new AttributeCollection();
            this.Children = new ChildObjectCollection();
            }

        /// <summary>
        /// Gets the attributes to set with this operation
        /// </summary>
        public AttributeCollection Attributes { get; }

        /// <summary>
        /// Gets the collection of children
        /// </summary>
        public ChildObjectCollection Children { get; }

        /// <summary>
        /// Appends a new attribute to the operation
        /// </summary>
        /// <param name="name">The column name</param>
        /// <param name="value">The value to assign</param>
        /// <returns>A reference to this operation object</returns>
        public OperationWithAttributesBase AddAttribute(string name, string? value)
            {
            var a = new NamedAttributeValue(name, new StringAttribute(value));
            this.Attributes.Add(a);
            return this;
            }

        /// <summary>
        /// Appends a new attribute to the operation
        /// </summary>
        /// <param name="name">The column name</param>
        /// <param name="value">The value to assign</param>
        /// <returns>A reference to this operation object</returns>
        public OperationWithAttributesBase AddAttribute(string name, bool value)
            {
            var a = new NamedAttributeValue(name, new BoolAttribute(value));
            this.Attributes.Add(a);
            return this;
            }

        /// <summary>
        /// Appends a new attribute to the operation
        /// </summary>
        /// <param name="name">The column name</param>
        /// <param name="value">The value to assign</param>
        /// <returns>A reference to this operation object</returns>
        public OperationWithAttributesBase AddAttribute(string name, int? value)
            {
            var a = new NamedAttributeValue(name, new IntAttribute(value));
            this.Attributes.Add(a);
            return this;
            }

        /// <summary>
        /// Appends a new attribute to the operation
        /// </summary>
        /// <param name="name">The column name</param>
        /// <param name="value">The value to assign</param>
        /// <returns>A reference to this operation object</returns>
        public OperationWithAttributesBase AddAttribute(string name, decimal? value)
            {
            var a = new NamedAttributeValue(name, new DecimalAttribute(value));
            this.Attributes.Add(a);
            return this;
            }

        /// <summary>
        /// Appends a new attribute to the operation
        /// </summary>
        /// <param name="name">The column name</param>
        /// <param name="value">The value to assign</param>
        /// <returns>A reference to this operation object</returns>
        public OperationWithAttributesBase AddDateAttribute(string name, DateTime? value)
            {
            var a = new NamedAttributeValue(name, new DateAttribute(value));
            this.Attributes.Add(a);
            return this;
            }

        /// <summary>
        /// Appends a new attribute to the operation
        /// </summary>
        /// <param name="name">The column name</param>
        /// <param name="value">The value to assign</param>
        /// <returns>A reference to this operation object</returns>
        public OperationWithAttributesBase AddDateTimeAttribute(string name, DateTime? value)
            {
            var a = new NamedAttributeValue(name, new DateTimeAttribute(value));
            this.Attributes.Add(a);
            return this;
            }

        /// <summary>
        /// Appends a new attribute to the operation
        /// </summary>
        /// <param name="name">The column name</param>
        /// <param name="value">The value to assign</param>
        /// <returns>A reference to this operation object</returns>
        public OperationWithAttributesBase AddAttribute(string name, Guid value)
            {
            var a = new NamedAttributeValue(name, new GuidAttribute(value));
            this.Attributes.Add(a);
            return this;
            }

        /// <summary>
        /// Appends a new attribute to the operation
        /// </summary>
        /// <param name="name">The column name</param>
        /// <param name="alias">The name of the alias column in a foreign key relationship</param>
        /// <param name="value">The value to assign</param>
        /// <returns>A reference to this operation object</returns>
        public OperationWithAttributesBase AddAliasedAttribute(string name, string alias, string value)
            {
            var a = new AliasAttribute(name, alias, new StringAttribute(value));
            this.Attributes.Add(a);
            return this;
            }

        /// <summary>
        /// Appends a new child object. Children must be added in the correct order. Check the 3E object schemas in TE_3E_Share\(instance)\Inetpub\XML\Object\Schema\Write
        /// </summary>
        /// <param name="name">The name of the related child object</param>
        /// <returns>The new DataObject</returns>
        public DataObject AddChild(string name)
            {
            var a = new DataObject(name);
            this.Children.Add(a);
            return a;
            }

        /// <summary>
        /// Outputs this operation's attributes
        /// </summary>
        /// <param name="writer">An XMLWriter to output to</param>
        protected void RenderAttributes(XmlWriter writer)
            {
            if (this.Attributes.Any())
                {
                writer.WriteStartElement("Attributes");
                foreach (NamedAttributeValue a in this.Attributes)
                    {
                    // ReSharper disable once PossibleNullReferenceException
                    a.Render(writer);
                    }
                writer.WriteEndElement();
                }
            }

        /// <summary>
        /// Outputs this operation's children
        /// </summary>
        /// <param name="writer">An XMLWriter to output to</param>
        protected void RenderChildren(XmlWriter writer)
            {
            if (this.Children.Any())
                {
                writer.WriteStartElement("Children");
                foreach (DataObject a in this.Children)
                    {
                    // ReSharper disable once PossibleNullReferenceException
                    a.Render(writer);
                    }
                writer.WriteEndElement();
                }
            }
        }
    }
