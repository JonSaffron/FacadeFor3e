using System;
using System.IO;
using System.Xml;
using JetBrains.Annotations;

namespace FacadeFor3e
    {
    /// <summary>
    /// Defines a value for an object attribute
    /// </summary>
    [PublicAPI]
    public class AttributeValue
        {
        private AttributeValue(string name)
            {
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("The name parameter must specify a valid attribute name.");
            this.Name = name;
            }

        /// <summary>
        /// Constructs a new attribute
        /// </summary>
        /// <param name="name">The column name</param>
        /// <param name="value">The value to assign</param>
        public AttributeValue(string name, string value = null) : this(name)
            {
            this.Value = value;
            this.OutputValue = value.To3eString();
            }

        /// <summary>
        /// Constructs a new attribute
        /// </summary>
        /// <param name="name">The column name</param>
        /// <param name="value">The value to assign</param>
        public AttributeValue(string name, bool value) : this(name)
            {
            this.Value = value;
            this.OutputValue = value.To3eString();
            }

        /// <summary>
        /// Constructs a new attribute
        /// </summary>
        /// <param name="name">The column name</param>
        /// <param name="value">The value to assign</param>
        public AttributeValue(string name, int? value) : this(name)
            {
            this.Value = value;
            this.OutputValue = value.To3eString();
            }

        /// <summary>
        /// Constructs a new attribute
        /// </summary>
        /// <param name="name">The column name</param>
        /// <param name="value">The value to assign</param>
        public AttributeValue(string name, decimal? value) : this(name)
            {
            this.Value = value;
            this.OutputValue = value.To3eString();
            }

        /// <summary>
        /// Constructs a new attribute
        /// </summary>
        /// <param name="name">The column name</param>
        /// <param name="value">The value to assign</param>
        public AttributeValue(string name, DateTime? value) : this(name)
            {
            this.Value = value;
            this.OutputValue = value.To3eString();
            }

        /// <summary>
        /// Constructs a new attribute
        /// </summary>
        /// <param name="name">The column name</param>
        /// <param name="value">The value to assign</param>
        public AttributeValue(string name, Guid? value) : this(name)
            {
            this.Value = value;
            this.OutputValue = value.To3eString();
            }

        /// <summary>
        /// Gets the name of the attribute
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the value to assign to the attribute
        /// </summary>
        public object Value { get; }

        /// <summary>
        /// Gets the representation of the attribute value that will used in the call to the transaction service
        /// </summary>
        public string OutputValue { get; }

        /// <summary>
        /// Outputs this attribute
        /// </summary>
        /// <param name="writer">An XMLWriter to output to</param>
        protected internal virtual void Render(XmlWriter writer)
            {
            writer.WriteStartElement(this.Name);
            writer.WriteValue(this.OutputValue);
            writer.WriteEndElement();
            }

        public override string ToString()
            {
            string result;
            using (var sw = new StringWriter())
                {
                using (var xw = XmlWriter.Create(sw))
                    {
                    Render(xw);
                    }
                result = sw.ToString();
                }
            return result;
            }
        }
    }
