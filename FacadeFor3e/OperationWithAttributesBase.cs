using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using JetBrains.Annotations;

namespace FacadeFor3e
    {
    /// <summary>
    /// Defines the interface for an operation upon an object which requires a collection of attributes
    /// </summary>
    [PublicAPI]
    public abstract class OperationWithAttributesBase : OperationBase
        {
        private readonly AttributeCollection _attributes;
        private readonly ChildObjectCollection _children;

        protected OperationWithAttributesBase()
            {
            this._attributes = new AttributeCollection();
            this._children = new ChildObjectCollection();
            }

        /// <summary>
        /// Gets the attributes to set with this operation
        /// </summary>
        public ICollection<NamedAttribute> Attributes => this._attributes;

        /// <summary>
        /// Gets the collection of children
        /// </summary>
        public ICollection<DataObject> Children => this._children;

        /// <summary>
        /// Appends a new attribute to the operation
        /// </summary>
        /// <param name="name">The column name</param>
        /// <param name="value">The value to assign</param>
        /// <returns>The new attribute</returns>
        public OperationWithAttributesBase AddAttribute(string name, string value)
            {
            var a = new NamedAttributeValue(name, new StringAttribute(name));
            this._attributes.Add(a);
            return this;
            }

        /// <summary>
        /// Appends a new attribute to the operation
        /// </summary>
        /// <param name="name">The column name</param>
        /// <param name="value">The value to assign</param>
        /// <returns>The new attribute</returns>
        public OperationWithAttributesBase AddAttribute(string name, bool value)
            {
            var a = new NamedAttributeValue(name, new BoolAttribute(value));
            this._attributes.Add(a);
            return this;
            }

        /// <summary>
        /// Appends a new attribute to the operation
        /// </summary>
        /// <param name="name">The column name</param>
        /// <param name="value">The value to assign</param>
        /// <returns>The new attribute</returns>
        public OperationWithAttributesBase AddAttribute(string name, int? value)
            {
            var a = new NamedAttributeValue(name, new IntAttribute(value));
            this._attributes.Add(a);
            return this;
            }

        /// <summary>
        /// Appends a new attribute to the operation
        /// </summary>
        /// <param name="name">The column name</param>
        /// <param name="value">The value to assign</param>
        /// <returns>The new attribute</returns>
        public OperationWithAttributesBase AddAttribute(string name, decimal? value)
            {
            var a = new NamedAttributeValue(name, new DecimalAttribute(value));
            this._attributes.Add(a);
            return this;
            }

        /// <summary>
        /// Appends a new attribute to the operation
        /// </summary>
        /// <param name="name">The column name</param>
        /// <param name="value">The value to assign</param>
        /// <returns>The new attribute</returns>
        public OperationWithAttributesBase AddAttribute(string name, DateTime? value)
            {
            NamedAttribute a;
            if (value == null || value.Value.TimeOfDay == TimeSpan.Zero)
                {
                a = new NamedAttributeValue(name, new DateAttribute(value));
                }
            else
                {
                a = new NamedAttributeValue(name, new DateTimeAttribute(value));
                }
            this._attributes.Add(a);
            return this;
            }

        /// <summary>
        /// Appends a new attribute to the operation
        /// </summary>
        /// <param name="name">The column name</param>
        /// <param name="value">The value to assign</param>
        /// <returns>The new attribute</returns>
        public OperationWithAttributesBase AddAttribute(string name, Guid value)
            {
            var a = new NamedAttributeValue(name, new GuidAttribute(value));
            this._attributes.Add(a);
            return this;
            }

        /// <summary>
        /// Appends a new attribute to the operation
        /// </summary>
        /// <param name="name">The column name</param>
        /// <param name="alias">The name of the alias column in a foreign key relationship</param>
        /// <param name="value">The value to assign</param>
        /// <returns>The new attribute</returns>
        public OperationWithAttributesBase AddAttribute(string name, string alias, string value)
            {
            var a = new AliasAttribute(name, alias, new StringAttribute(value));
            this._attributes.Add(a);
            return this;
            }

        /// <summary>
        /// Appends a new attribute to the operation
        /// </summary>
        /// <param name="name">The column name</param>
        /// <param name="alias">The name of the alias column in a foreign key relationship</param>
        /// <param name="value">The value to assign</param>
        /// <returns>The new attribute</returns>
        public OperationWithAttributesBase AddAttribute(string name, string alias, int value)
            {
            var a = new AliasAttribute(name, alias, new IntAttribute(value));
            this._attributes.Add(a);
            return this;
            }

        /// <summary>
        /// Appends a new attribute to the operation
        /// </summary>
        /// <param name="name">The column name</param>
        /// <param name="alias">The name of the alias column in a foreign key relationship</param>
        /// <param name="value">The value to assign</param>
        /// <returns>The new attribute</returns>
        public OperationWithAttributesBase AddAttribute(string name, string alias, decimal value)
            {
            var a = new AliasAttribute(name, alias, new DecimalAttribute(value));
            this._attributes.Add(a);
            return this;
            }

        /// <summary>
        /// Appends a new attribute to the operation
        /// </summary>
        /// <param name="name">The column name</param>
        /// <param name="alias">The name of the alias column in a foreign key relationship</param>
        /// <param name="value">The value to assign</param>
        /// <returns>The new attribute</returns>
        public OperationWithAttributesBase AddAttribute(string name, string alias, DateTime value)
            {
            AliasAttribute a;
            if (value.TimeOfDay == TimeSpan.Zero)
                {
                a = new AliasAttribute(name, alias, new DateAttribute(value));
                }
            else
                {
                a = new AliasAttribute(name, alias, new DateTimeAttribute(value));
                }
            this._attributes.Add(a);
            return this;
            }

        public OperationWithAttributesBase AddNullAttribute(string name)
            {
            var a = new NullAttribute(name);
            this._attributes.Add(a);
            return this;
            }

        /// <summary>
        /// Appends a new child object. Children must be added in the correct order. Check the 3e object schemas in TE_3e_Share\(instance)\Inetpub\XML\Object\Schema\Write
        /// </summary>
        /// <param name="name">The name of the related child object</param>
        /// <returns>The new DataObject</returns>
        public DataObject AddChild(string name)
            {
            var a = new DataObject(name);
            this._children.Add(a);
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
                foreach (NamedAttribute a in this.Attributes)
                    a.Render(writer);
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
                    a.Render(writer);
                writer.WriteEndElement();
                }
            }
        }
    }
