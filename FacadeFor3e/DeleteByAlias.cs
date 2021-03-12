using System;
using System.Xml;

namespace FacadeFor3e
    {
    /// <summary>
    /// A Delete operation on a row that is identified by a unique value such as Matter.Number
    /// </summary>
    /// <remarks>
    /// This operation can be used to target a record by an attribute that isn't the primary key,
    /// as long as the attribute value is unique within the archetype. This would often be the
    /// the attribute marked as the alias in the archetype design.
    /// If the value matches multiple rows then the operation will fail.
    /// </remarks>
    public class DeleteByAlias : DeleteByKey
        {
        /// <summary>
        /// Constructs a Delete operation
        /// </summary>
        /// <param name="aliasField">Name of the alias attribute</param>
        /// <param name="aliasValue">Alias value</param>
        public DeleteByAlias(string aliasField, string aliasValue) : base(aliasValue)
            {
            if (string.IsNullOrWhiteSpace(aliasField))
                throw new ArgumentException("The aliasField parameter must specify a valid attribute name.");
            this.AliasField = aliasField;
            }

        /// <summary>
        /// Constructs a Delete operation
        /// </summary>
        /// <param name="aliasField">Name of the alias attribute</param>
        /// <param name="aliasValue">Alias value</param>
        public DeleteByAlias(string aliasField, int aliasValue) : base(aliasValue)
            {
            if (string.IsNullOrWhiteSpace(aliasField))
                throw new ArgumentException("The aliasField parameter must specify a valid attribute name.");
            this.AliasField = aliasField;
            }

        /// <summary>
        /// Constructs a Delete operation
        /// </summary>
        /// <param name="aliasField">Name of the alias attribute</param>
        /// <param name="aliasValue">Alias value</param>
        public DeleteByAlias(string aliasField, Guid aliasValue) : base(aliasValue)
            {
            if (string.IsNullOrWhiteSpace(aliasField))
                throw new ArgumentException("The aliasField parameter must specify a valid attribute name.");
            this.AliasField = aliasField;
            }

        /// <summary>
        /// Constructs a Delete operation
        /// </summary>
        /// <param name="aliasField">Name of the alias attribute</param>
        /// <param name="aliasValue">Alias value</param>
        public DeleteByAlias(string aliasField, DateTime aliasValue) : base(aliasValue)
            {
            if (string.IsNullOrWhiteSpace(aliasField))
                throw new ArgumentException("The aliasField parameter must specify a valid attribute name.");
            this.AliasField = aliasField;
            }

        /// <summary>
        /// Constructs a Delete operation
        /// </summary>
        /// <param name="aliasField">Name of the alias attribute</param>
        /// <param name="aliasValue">Alias value</param>
        public DeleteByAlias(string aliasField, decimal aliasValue) : base(aliasValue)
            {
            if (string.IsNullOrWhiteSpace(aliasField))
                throw new ArgumentException("The aliasField parameter must specify a valid attribute name.");
            this.AliasField = aliasField;
            }

        /// <summary>
        /// Gets the id of the attribute to query
        /// </summary>
        public string AliasField { get; }

        protected override void RenderKey(XmlWriter writer)
            {
            writer.WriteAttributeString("KeyValue", this.OutputValue);
            writer.WriteAttributeString("AliasField", this.AliasField);
            }
        }
    }
