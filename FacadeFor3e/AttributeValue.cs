using System;
using System.Xml;

namespace FacadeFor3e
    {
    /// <summary>
    /// Defines a value for an object attribute
    /// </summary>
    public class AttributeValue
        {
        private readonly string _name;
        private readonly string _alias;
        private readonly string _value;

        /// <summary>
        /// Constructs a new attribute
        /// </summary>
        /// <param name="name">The column name</param>
        /// <param name="value">The value to assign</param>
        public AttributeValue(string name, string value)
            {
            if (name == null)
                throw new ArgumentNullException("name");
            this._name = name;
            this._value = value ?? string.Empty;
            }

        /// <summary>
        /// Constructs a new attribute
        /// </summary>
        /// <param name="name">The column name</param>
        /// <param name="value">The value to assign</param>
        public AttributeValue(string name, bool? value)
            {
            if (name == null)
                throw new ArgumentNullException("name");
            this._name = name;
            this._value = value.HasValue ? (value.Value ? "1" : "0") : string.Empty;
            }

        /// <summary>
        /// Constructs a new attribute
        /// </summary>
        /// <param name="name">The column name</param>
        /// <param name="value">The value to assign</param>
        public AttributeValue(string name, int? value)
            {
            if (name == null)
                throw new ArgumentNullException("name");
            this._name = name;
            this._value = value.HasValue ? value.Value.ToString("F0") : string.Empty;
            }

        /// <summary>
        /// Constructs a new attribute
        /// </summary>
        /// <param name="name">The column name</param>
        /// <param name="value">The value to assign</param>
        public AttributeValue(string name, decimal? value)
            {
            if (name == null)
                throw new ArgumentNullException("name");
            this._name = name;
            this._value = value.HasValue ? value.Value.ToString("G") : string.Empty;
            }

        /// <summary>
        /// Constructs a new attribute
        /// </summary>
        /// <param name="name">The column name</param>
        /// <param name="value">The value to assign</param>
        public AttributeValue(string name, DateTime? value)
            {
            if (name == null)
                throw new ArgumentNullException("name");
            this._name = name;
            if (!value.HasValue)
                this._value = string.Empty;
            else if (value.Value.TimeOfDay == TimeSpan.Zero)
                this._value = value.Value.ToString("d-MMM-yyyy");
            else
                this._value = value.Value.ToString("d-MMM-yyyy HH:mm:ss");
            }

        /// <summary>
        /// Constructs a new attribute
        /// </summary>
        /// <param name="name">The column name</param>
        /// <param name="value">The value to assign</param>
        public AttributeValue(string name, Guid value)
            {
            if (name == null)
                throw new ArgumentNullException("name");
            this._name = name;
            this._value = value.ToString("B");
            }

        /// <summary>
        /// Constructs a new attribute
        /// </summary>
        /// <param name="name">The column name</param>
        /// <param name="alias">The name of the alias column in a foreign key relationship</param>
        /// <param name="value">The value to assign</param>
        public AttributeValue(string name, string alias, string value) : this(name, value)
            {
            if (alias == null)
                throw new ArgumentNullException("alias", "alias cannot be null (name is " + name + ")");
            this._alias = alias;
            }

        /// <summary>
        /// Constructs a new attribute
        /// </summary>
        /// <param name="name">The column name</param>
        /// <param name="alias">The name of the alias column in a foreign key relationship</param>
        /// <param name="value">The value to assign</param>
        public AttributeValue(string name, string alias, bool? value) : this(name, value)
            {
            if (alias == null)
                throw new ArgumentNullException("alias", "alias cannot be null (name is " + name + ")");
            this._alias = alias;
            }

        /// <summary>
        /// Constructs a new attribute
        /// </summary>
        /// <param name="name">The column name</param>
        /// <param name="alias">The name of the alias column in a foreign key relationship</param>
        /// <param name="value">The value to assign</param>
        public AttributeValue(string name, string alias, int? value) : this(name, value)
            {
            if (alias == null)
                throw new ArgumentNullException("alias", "alias cannot be null (name is " + name + ")");
            this._alias = alias;
            }

        /// <summary>
        /// Constructs a new attribute
        /// </summary>
        /// <param name="name">The column name</param>
        /// <param name="alias">The name of the alias column in a foreign key relationship</param>
        /// <param name="value">The value to assign</param>
        public AttributeValue(string name, string alias, decimal? value) : this(name, value)
            {
            if (alias == null)
                throw new ArgumentNullException("alias", "alias cannot be null (name is " + name + ")");
            this._alias = alias;
            }

        /// <summary>
        /// Constructs a new attribute
        /// </summary>
        /// <param name="name">The column name</param>
        /// <param name="alias">The name of the alias column in a foreign key relationship</param>
        /// <param name="value">The value to assign</param>
        public AttributeValue(string name, string alias, DateTime? value) : this(name, value)
            {
            if (alias == null)
                throw new ArgumentNullException("alias", "alias cannot be null (name is " + name + ")");
            this._alias = alias;
            }

        /// <summary>
        /// Constructs a new attribute
        /// </summary>
        /// <param name="name">The column name</param>
        /// <param name="alias">The name of the alias column in a foreign key relationship</param>
        /// <param name="value">The value to assign</param>
        public AttributeValue(string name, string alias, Guid value) : this(name, value)
            {
            if (alias == null)
                throw new ArgumentNullException("alias", "alias cannot be null (name is " + name + ")");
            this._alias = alias;
            }

        /// <summary>
        /// Constructs a new attribute which will have a null value
        /// </summary>
        /// <param name="name">The column name</param>
        public AttributeValue(string name) : this(name, string.Empty)
            {
            }

        /// <summary>
        /// Gets the name of the column
        /// </summary>
        public string Name
            {
            get { return this._name; }
            }

        /// <summary>
        ///  Gets the name of the alias
        /// </summary>
        public string Alias
            {
            get { return this._alias; }
            }

        /// <summary>
        /// Gets the value to assign
        /// </summary>
        public string Value
            {
            get { return this._value; }
            }

        /// <summary>
        /// Outputs this attribute
        /// </summary>
        /// <param name="writer">An XMLWriter to output to</param>
        internal void Render(XmlWriter writer)
            {
            writer.WriteStartElement(this._name);
            if (this._alias != null)
                writer.WriteAttributeString("AliasField", this._alias);
            writer.WriteValue(this._value);
            writer.WriteEndElement();
            }
        }
    }
