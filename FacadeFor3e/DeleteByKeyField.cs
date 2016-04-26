using System;
using System.Xml;

namespace FacadeFor3e
    {
    /// <summary>
    /// A delete operation on a row that is identified by a value matched within the child collection
    /// to the specified field such as MattDate.EffStart
    /// </summary>
    /// <remarks>
    /// The KeyField indicates which attribute of the child collection object will be queried
    /// The KeyValue indicates the value to look for. The first row in the collection that matches will be returned.
    /// </remarks>
    public class DeleteByKeyField : DeleteByKey
        {
        private readonly string _keyField;

        /// <summary>
        /// Constructs a Delete operation
        /// </summary>
        /// <param name="keyField">Name of the alternative attribute</param>
        /// <param name="fieldValue">Value to look for</param>
        public DeleteByKeyField(string keyField, string fieldValue) : base(fieldValue)
            {
            if (string.IsNullOrWhiteSpace(keyField))
                throw new ArgumentException("The keyField parameter must specify a valid attribute name.");
            this._keyField = keyField;
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
            this._keyField = keyField;
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
            this._keyField = keyField;
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
            this._keyField = keyField;
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
            this._keyField = keyField;
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
            this._keyField = keyField;
            }

        /// <summary>
        /// Gets the value to assign to the attribute
        /// </summary>
        public string KeyField
            {
            get { return this._keyField; }
            }

        protected override void RenderKey(XmlWriter writer)
            {
            writer.WriteAttributeString("KeyValue", this.OutputValue);
            writer.WriteAttributeString("KeyField", this.KeyField);
            }
        }
    }
