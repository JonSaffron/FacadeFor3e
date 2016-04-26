using System;
using System.Xml;

namespace FacadeFor3e
    {
    /// <summary>
    /// A Edit operation on a row that is identified by a value matched to the primary key
    /// </summary>
    public class EditByKey : OperationEdit
        {
        private readonly object _keyValue;
        private readonly string _stringValue;

        /// <summary>
        /// Constructs a Edit operation
        /// </summary>
        /// <param name="keyValue">Primary key value</param>
        public EditByKey(string keyValue)
            {
            if (keyValue == null)
                throw new ArgumentNullException("keyValue");

            this._keyValue = keyValue;
            this._stringValue = keyValue;
            }

        /// <summary>
        /// Constructs a Edit operation
        /// </summary>
        /// <param name="keyValue">Primary key value</param>
        public EditByKey(int keyValue)
            {
            this._keyValue = keyValue;
            this._stringValue = keyValue.To3eString();
            }

        /// <summary>
        /// Constructs a Edit operation
        /// </summary>
        /// <param name="keyValue">Primary key value</param>
        public EditByKey(Guid keyValue)
            {
            this._keyValue = keyValue;
            this._stringValue = keyValue.To3eString();
            }

        /// <summary>
        /// Constructs a Edit operation
        /// </summary>
        /// <param name="keyValue">Primary key value</param>
        public EditByKey(DateTime keyValue)
            {
            this._keyValue = keyValue;
            this._stringValue = keyValue.To3eString();
            }

        /// <summary>
        /// Constructs a Edit operation (decimal cannot be used as primary key)
        /// </summary>
        /// <param name="keyValue">Primary key value</param>
        protected EditByKey(decimal keyValue)
            {
            this._keyValue = keyValue;
            this._stringValue = keyValue.To3eString();
            }

        /// <summary>
        /// Constructs a Edit operation (bool cannot be used as primary key)
        /// </summary>
        /// <param name="keyValue">Primary key value</param>
        protected EditByKey(bool keyValue)
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
