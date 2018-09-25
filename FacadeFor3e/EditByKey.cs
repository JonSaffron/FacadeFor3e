using System;
using System.Xml;
using JetBrains.Annotations;

namespace FacadeFor3e
    {
    /// <summary>
    /// A Edit operation on a row that is identified by a value matched to the primary key
    /// </summary>
    public class EditByKey : OperationEdit
        {
        /// <summary>
        /// Constructs a Edit operation
        /// </summary>
        /// <param name="keyValue">Primary key value</param>
        public EditByKey([NotNull] string keyValue)
            {
            this.KeyValue = keyValue ?? throw new ArgumentNullException(nameof(keyValue));
            this.OutputValue = keyValue;
            }

        /// <summary>
        /// Constructs a Edit operation
        /// </summary>
        /// <param name="keyValue">Primary key value</param>
        public EditByKey(int keyValue)
            {
            this.KeyValue = keyValue;
            this.OutputValue = keyValue.To3eString();
            }

        /// <summary>
        /// Constructs a Edit operation
        /// </summary>
        /// <param name="keyValue">Primary key value</param>
        public EditByKey(Guid keyValue)
            {
            this.KeyValue = keyValue;
            this.OutputValue = keyValue.To3eString();
            }

        /// <summary>
        /// Constructs a Edit operation
        /// </summary>
        /// <param name="keyValue">Primary key value</param>
        public EditByKey(DateTime keyValue)
            {
            this.KeyValue = keyValue;
            this.OutputValue = keyValue.To3eString();
            }

        /// <summary>
        /// Constructs a Edit operation (decimal cannot be used as primary key)
        /// </summary>
        /// <param name="keyValue">Primary key value</param>
        protected EditByKey(decimal keyValue)
            {
            this.KeyValue = keyValue;
            this.OutputValue = keyValue.To3eString();
            }

        /// <summary>
        /// Constructs a Edit operation (bool cannot be used as primary key)
        /// </summary>
        /// <param name="keyValue">Primary key value</param>
        protected EditByKey(bool keyValue)
            {
            this.KeyValue = keyValue;
            this.OutputValue = keyValue.To3eString();
            }

        /// <summary>
        /// Gets the value to assign to the attribute
        /// </summary>
        public object KeyValue { get; }

        /// <summary>
        /// Gets the representation of the attribute value that will used in the call to the transaction service
        /// </summary>
        public string OutputValue { get; }

        protected override void RenderKey(XmlWriter writer)
            {
            writer.WriteAttributeString("KeyValue", this.OutputValue);
            }
        }
    }
