using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace FacadeFor3e
    {
    /// <summary>
    /// Defines the interface for an operation upon an object which requires a collection of attributes
    /// </summary>
    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public abstract class OperationWithAttributesBase : OperationBase
        {
        private readonly List<AttributeValue> _attributes;
        private readonly List<DataObject> _children;

        protected OperationWithAttributesBase()
            {
            this._attributes = new List<AttributeValue>();
            this._children = new List<DataObject>();
            }

        /// <summary>
        /// Gets the attributes to set with this operation
        /// </summary>
        public List<AttributeValue> Attributes
            {
            get { return this._attributes; }
            }

        /// <summary>
        /// Gets the collection of children
        /// </summary>
        public List<DataObject> Children
            {
            get { return this._children; }
            }

        /// <summary>
        /// Appends a new attribute to the operation
        /// </summary>
        /// <param name="name">The column name</param>
        /// <param name="value">The value to assign</param>
        /// <returns>The new attribute</returns>
        public AttributeValue AddAttribute(string name, string value)
            {
            var a = new AttributeValue(name, value);
            this._attributes.Add(a);
            return a;
            }

        /// <summary>
        /// Appends a new attribute to the operation
        /// </summary>
        /// <param name="name">The column name</param>
        /// <param name="value">The value to assign</param>
        /// <returns>The new attribute</returns>
        public AttributeValue AddAttribute(string name, bool? value)
            {
            var a = new AttributeValue(name, value);
            this._attributes.Add(a);
            return a;
            }

        /// <summary>
        /// Appends a new attribute to the operation
        /// </summary>
        /// <param name="name">The column name</param>
        /// <param name="value">The value to assign</param>
        /// <returns>The new attribute</returns>
        public AttributeValue AddAttribute(string name, int? value)
            {
            var a = new AttributeValue(name, value);
            this._attributes.Add(a);
            return a;
            }

        /// <summary>
        /// Appends a new attribute to the operation
        /// </summary>
        /// <param name="name">The column name</param>
        /// <param name="value">The value to assign</param>
        /// <returns>The new attribute</returns>
        public AttributeValue AddAttribute(string name, decimal? value)
            {
            var a = new AttributeValue(name, value);
            this._attributes.Add(a);
            return a;
            }

        /// <summary>
        /// Appends a new attribute to the operation
        /// </summary>
        /// <param name="name">The column name</param>
        /// <param name="value">The value to assign</param>
        /// <returns>The new attribute</returns>
        public AttributeValue AddAttribute(string name, DateTime? value)
            {
            var a = new AttributeValue(name, value);
            this._attributes.Add(a);
            return a;
            }

        /// <summary>
        /// Appends a new attribute to the operation
        /// </summary>
        /// <param name="name">The column name</param>
        /// <param name="value">The value to assign</param>
        /// <returns>The new attribute</returns>
        public AttributeValue AddAttribute(string name, Guid value)
            {
            var a = new AttributeValue(name, value);
            this._attributes.Add(a);
            return a;
            }

        /// <summary>
        /// Appends a new attribute to the operation
        /// </summary>
        /// <param name="name">The column name</param>
        /// <param name="alias">The name of the alias column in a foreign key relationship</param>
        /// <param name="value">The value to assign</param>
        /// <returns>The new attribute</returns>
        public AttributeValue AddAttribute(string name, string alias, string value)
            {
            var a = new AttributeValue(name, alias, value);
            this._attributes.Add(a);
            return a;
            }

        /// <summary>
        /// Appends a new attribute to the operation
        /// </summary>
        /// <param name="name">The column name</param>
        /// <param name="alias">The name of the alias column in a foreign key relationship</param>
        /// <param name="value">The value to assign</param>
        /// <returns>The new attribute</returns>
        public AttributeValue AddAttribute(string name, string alias, bool? value)
            {
            var a = new AttributeValue(name, alias, value);
            this._attributes.Add(a);
            return a;
            }

        /// <summary>
        /// Appends a new attribute to the operation
        /// </summary>
        /// <param name="name">The column name</param>
        /// <param name="alias">The name of the alias column in a foreign key relationship</param>
        /// <param name="value">The value to assign</param>
        /// <returns>The new attribute</returns>
        public AttributeValue AddAttribute(string name, string alias, int? value)
            {
            var a = new AttributeValue(name, alias, value);
            this._attributes.Add(a);
            return a;
            }

        /// <summary>
        /// Appends a new attribute to the operation
        /// </summary>
        /// <param name="name">The column name</param>
        /// <param name="alias">The name of the alias column in a foreign key relationship</param>
        /// <param name="value">The value to assign</param>
        /// <returns>The new attribute</returns>
        public AttributeValue AddAttribute(string name, string alias, decimal? value)
            {
            var a = new AttributeValue(name, alias, value);
            this._attributes.Add(a);
            return a;
            }

        /// <summary>
        /// Appends a new attribute to the operation
        /// </summary>
        /// <param name="name">The column name</param>
        /// <param name="alias">The name of the alias column in a foreign key relationship</param>
        /// <param name="value">The value to assign</param>
        /// <returns>The new attribute</returns>
        public AttributeValue AddAttribute(string name, string alias, DateTime? value)
            {
            var a = new AttributeValue(name, alias, value);
            this._attributes.Add(a);
            return a;
            }

        /// <summary>
        /// Appends a new attribute to the operation
        /// </summary>
        /// <param name="name">The column name</param>
        /// <param name="alias">The name of the alias column in a foreign key relationship</param>
        /// <param name="value">The value to assign</param>
        /// <returns>The new attribute</returns>
        public AttributeValue AddAttribute(string name, string alias, Guid value)
            {
            var a = new AttributeValue(name, alias, value);
            this._attributes.Add(a);
            return a;
            }

        /// <summary>
        /// Appends a new attribute to the operation with a null value
        /// </summary>
        /// <param name="name">The column name</param>
        /// <returns>The new attribute</returns>
        public AttributeValue ClearAttribute(string name)
            {
            var a = new AttributeValue(name);
            this._attributes.Add(a);
            return a;
            }

        /// <summary>
        /// Appends a new child object. Children must be added in the correct order. Check the 3e object schemas in TE_3e_Share\(instance)\Inetpub\XML\Object\Schema\Write
        /// </summary>
        /// <param name="name">The name of the related child object</param>
        /// <returns>The new DataObject</returns>
        public DataObject AddChild(string name)
            {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Invalid name.", "name");
            if (this._children.Exists(child => child.Name == name))
                throw new ArgumentOutOfRangeException("name", "A child with the name " + name + " has already been added.");

            var a = new DataObject(name);
            this._children.Add(a);
            return a;
            }
        }
    }
