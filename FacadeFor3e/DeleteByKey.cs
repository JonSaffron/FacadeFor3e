using System;
using System.Xml;

namespace FacadeFor3e
    {
    /// <summary>
    /// A delete operation on a row that is identified by a value matched to the primary key
    /// </summary>
    public class DeleteByKey : OperationDelete
        {
        private readonly object _keyValue;
        private readonly string _stringValue;

        /// <summary>
        /// Constructs a Delete operation
        /// </summary>
        /// <param name="keyValue">Primary key value</param>
        public DeleteByKey(string keyValue)
            {
            if (keyValue == null)
                throw new ArgumentNullException("keyValue");

            this._keyValue = keyValue;
            this._stringValue = keyValue;
            }

        /// <summary>
        /// Constructs a Delete operation
        /// </summary>
        /// <param name="keyValue">Primary key value</param>
        public DeleteByKey(int keyValue)
            {
            this._keyValue = keyValue;
            this._stringValue = keyValue.To3eString();
            }

        /// <summary>
        /// Constructs a Delete operation
        /// </summary>
        /// <param name="keyValue">Primary key value</param>
        public DeleteByKey(Guid keyValue)
            {
            this._keyValue = keyValue;
            this._stringValue = keyValue.To3eString();
            }

        /// <summary>
        /// Constructs a Delete operation
        /// </summary>
        /// <param name="keyValue">Primary key value</param>
        public DeleteByKey(DateTime keyValue)
            {
            this._keyValue = keyValue;
            this._stringValue = keyValue.To3eString();
            }

        /// <summary>
        /// Constructs a Delete operation (decimal cannot be used as primary key)
        /// </summary>
        /// <param name="keyValue">Primary key value</param>
        protected DeleteByKey(decimal keyValue)
            {
            this._keyValue = keyValue;
            this._stringValue = keyValue.To3eString();
            }

        /// <summary>
        /// Constructs a Delete operation (bool cannot be used as primary key)
        /// </summary>
        /// <param name="keyValue">Primary key value</param>
        protected DeleteByKey(bool keyValue)
            {
            this._keyValue = keyValue;
            this._stringValue = keyValue.To3eString();
            }

        /// <summary>
        /// Gets the value to assign to the attribute
        /// </summary>
        public object KeyValue
            {
            get { return this._keyValue; }
            }

        /// <summary>
        /// Gets the representation of the attribute value that will used in the call to the transaction service
        /// </summary>
        public string OutputValue
            {
            get { return this._stringValue; }
            }

        protected override void RenderKey(XmlWriter writer)
            {
            writer.WriteAttributeString("KeyValue", this._stringValue);
            }
        }
    }
