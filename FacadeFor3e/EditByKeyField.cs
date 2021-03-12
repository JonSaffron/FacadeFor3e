using System;
using System.Xml;

namespace FacadeFor3e
    {
    /// <summary>
    /// An Edit operation on a row that is identified by a value matched within the child collection
    /// to the specified field such as MattDate.EffStart
    /// </summary>
    /// <remarks>
    /// This is similar to the EditByAlias operation, except it only looks at rows included
    /// in the child collection rather than at all the rows in the archetype.
    /// </remarks>
    public class EditByKeyField : EditByKey
        {
        /// <summary>
        /// Constructs a Edit operation
        /// </summary>
        /// <param name="keyField">Name of the alternative attribute</param>
        /// <param name="fieldValue">Value to look for</param>
        public EditByKeyField(string keyField, string fieldValue) : base(fieldValue)
            {
            if (string.IsNullOrWhiteSpace(keyField))
                throw new ArgumentException("The keyField parameter must specify a valid attribute name.");
            this.KeyField = keyField;
            }

        /// <summary>
        /// Constructs a Edit operation
        /// </summary>
        /// <param name="keyField">Name of the alternative attribute</param>
        /// <param name="fieldValue">Value to look for</param>
        public EditByKeyField(string keyField, int fieldValue) : base(fieldValue)
            {
            if (string.IsNullOrWhiteSpace(keyField))
                throw new ArgumentException("The keyField parameter must specify a valid attribute name.");
            this.KeyField = keyField;
            }

        /// <summary>
        /// Constructs a Edit operation
        /// </summary>
        /// <param name="keyField">Name of the alternative attribute</param>
        /// <param name="fieldValue">Value to look for</param>
        public EditByKeyField(string keyField, Guid fieldValue) : base(fieldValue)
            {
            if (string.IsNullOrWhiteSpace(keyField))
                throw new ArgumentException("The keyField parameter must specify a valid attribute name.");
            this.KeyField = keyField;
            }

        /// <summary>
        /// Constructs a Edit operation
        /// </summary>
        /// <param name="keyField">Name of the alternative attribute</param>
        /// <param name="fieldValue">Value to look for</param>
        public EditByKeyField(string keyField, DateTime fieldValue) : base(fieldValue)
            {
            if (string.IsNullOrWhiteSpace(keyField))
                throw new ArgumentException("The keyField parameter must specify a valid attribute name.");
            this.KeyField = keyField;
            }

        /// <summary>
        /// Constructs a Edit operation
        /// </summary>
        /// <param name="keyField">Name of the alternative attribute</param>
        /// <param name="fieldValue">Value to look for</param>
        public EditByKeyField(string keyField, decimal fieldValue) : base(fieldValue)
            {
            if (string.IsNullOrWhiteSpace(keyField))
                throw new ArgumentException("The keyField parameter must specify a valid attribute name.");
            this.KeyField = keyField;
            }

        /// <summary>
        /// Constructs a Edit operation
        /// </summary>
        /// <param name="keyField">Name of the alternative attribute</param>
        /// <param name="fieldValue">Value to look for</param>
        public EditByKeyField(string keyField, bool fieldValue) : base(fieldValue)
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
