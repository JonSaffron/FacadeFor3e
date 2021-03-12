using System;
using System.Xml;

namespace FacadeFor3e
    {
    /// <summary>
    /// A Delete operation on a row that is identified by a value matched within the child collection
    /// to the specified field such as MattDate.EffStart
    /// </summary>
    /// <remarks>
    /// This is similar to the DeleteByAlias operation, except it only looks at rows included
    /// in the child collection rather than at all the rows in the archetype.
    /// </remarks>
    public class DeleteByKeyField : DeleteByKey
        {
        /// <summary>
        /// Constructs a Delete operation
        /// </summary>
        /// <param name="keyField">Name of the alternative attribute</param>
        /// <param name="fieldValue">Value to look for</param>
        public DeleteByKeyField(string keyField, string fieldValue) : base(fieldValue)
            {
            if (string.IsNullOrWhiteSpace(keyField))
                throw new ArgumentException("The keyField parameter must specify a valid attribute name.");
            this.KeyField = keyField;
            }

        /// <summary>
        /// Constructs a Delete operation
        /// </summary>
        /// <param name="keyField">Name of the alternative attribute</param>
        /// <param name="fieldValue">Value to look for</param>
        public DeleteByKeyField(string keyField, int fieldValue) : base(fieldValue)
            {
            if (string.IsNullOrWhiteSpace(keyField))
                throw new ArgumentException("The keyField parameter must specify a valid attribute name.");
            this.KeyField = keyField;
            }

        /// <summary>
        /// Constructs a Delete operation
        /// </summary>
        /// <param name="keyField">Name of the alternative attribute</param>
        /// <param name="fieldValue">Value to look for</param>
        public DeleteByKeyField(string keyField, Guid fieldValue) : base(fieldValue)
            {
            if (string.IsNullOrWhiteSpace(keyField))
                throw new ArgumentException("The keyField parameter must specify a valid attribute name.");
            this.KeyField = keyField;
            }

        /// <summary>
        /// Constructs a Delete operation
        /// </summary>
        /// <param name="keyField">Name of the alternative attribute</param>
        /// <param name="fieldValue">Value to look for</param>
        public DeleteByKeyField(string keyField, DateTime fieldValue) : base(fieldValue)
            {
            if (string.IsNullOrWhiteSpace(keyField))
                throw new ArgumentException("The keyField parameter must specify a valid attribute name.");
            this.KeyField = keyField;
            }

        /// <summary>
        /// Constructs a Delete operation
        /// </summary>
        /// <param name="keyField">Name of the alternative attribute</param>
        /// <param name="fieldValue">Value to look for</param>
        public DeleteByKeyField(string keyField, decimal fieldValue) : base(fieldValue)
            {
            if (string.IsNullOrWhiteSpace(keyField))
                throw new ArgumentException("The keyField parameter must specify a valid attribute name.");
            this.KeyField = keyField;
            }

        /// <summary>
        /// Constructs a Delete operation
        /// </summary>
        /// <param name="keyField">Name of the alternative attribute</param>
        /// <param name="fieldValue">Value to look for</param>
        public DeleteByKeyField(string keyField, bool fieldValue) : base(fieldValue)
            {
            if (string.IsNullOrWhiteSpace(keyField))
                throw new ArgumentException("The keyField parameter must specify a valid attribute name.");
            this.KeyField = keyField;
            }

        /// <summary>
        /// Gets the id of the attribute to search
        /// </summary>
        public string KeyField { get; }

        protected override void RenderKey(XmlWriter writer)
            {
            writer.WriteAttributeString("KeyValue", this.OutputValue);
            writer.WriteAttributeString("KeyField", this.KeyField);
            }
        }
    }
