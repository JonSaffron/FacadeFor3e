using System;
using System.Xml;

namespace FacadeFor3e
    {
    /// <summary>
    /// A Edit operation on a row that is identified by a unique value such as Matter.Number
    /// </summary>
    /// <remarks>
    /// The AliasField indicates which attribute of the underlying archetype will be queried
    /// The KeyValue indicates the unique value to look for. An exception will be raised if the value is found not to be unique.
    /// </remarks>
    public class EditByAlias : EditByKey
        {
        /// <summary>
        /// Constructs a Edit operation
        /// </summary>
        /// <param name="aliasField">Name of the alias attribute</param>
        /// <param name="aliasValue">Alias value</param>
        public EditByAlias(string aliasField, string aliasValue) : base(aliasValue)
            {
            if (string.IsNullOrWhiteSpace(aliasField))
                throw new ArgumentException("The aliasField parameter must specify a valid attribute name.");
            this.AliasField = aliasField;
            }

        /// <summary>
        /// Constructs a Edit operation
        /// </summary>
        /// <param name="aliasField">Name of the alias attribute</param>
        /// <param name="aliasValue">Alias value</param>
        public EditByAlias(string aliasField, int aliasValue) : base(aliasValue)
            {
            if (string.IsNullOrWhiteSpace(aliasField))
                throw new ArgumentException("The aliasField parameter must specify a valid attribute name.");
            this.AliasField = aliasField;
            }

        /// <summary>
        /// Constructs a Edit operation
        /// </summary>
        /// <param name="aliasField">Name of the alias attribute</param>
        /// <param name="aliasValue">Alias value</param>
        public EditByAlias(string aliasField, Guid aliasValue) : base(aliasValue)
            {
            if (string.IsNullOrWhiteSpace(aliasField))
                throw new ArgumentException("The aliasField parameter must specify a valid attribute name.");
            this.AliasField = aliasField;
            }

        /// <summary>
        /// Constructs a Edit operation
        /// </summary>
        /// <param name="aliasField">Name of the alias attribute</param>
        /// <param name="aliasValue">Alias value</param>
        public EditByAlias(string aliasField, DateTime aliasValue) : base(aliasValue)
            {
            if (string.IsNullOrWhiteSpace(aliasField))
                throw new ArgumentException("The aliasField parameter must specify a valid attribute name.");
            this.AliasField = aliasField;
            }

        /// <summary>
        /// Constructs a Edit operation
        /// </summary>
        /// <param name="aliasField">Name of the alias attribute</param>
        /// <param name="aliasValue">Alias value</param>
        public EditByAlias(string aliasField, decimal aliasValue) : base(aliasValue)
            {
            if (string.IsNullOrWhiteSpace(aliasField))
                throw new ArgumentException("The aliasField parameter must specify a valid attribute name.");
            this.AliasField = aliasField;
            }

        /// <summary>
        /// Gets the alias field name.
        /// </summary>
        public string AliasField { get; }

        protected override void RenderKey(XmlWriter writer)
            {
            writer.WriteAttributeString("KeyValue", this.OutputValue);
            writer.WriteAttributeString("AliasField", this.AliasField);
            }
        }
    }
