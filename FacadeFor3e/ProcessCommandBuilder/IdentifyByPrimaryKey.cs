using System;
using System.Xml;
using JetBrains.Annotations;

namespace FacadeFor3e.ProcessCommandBuilder
    {
    /// <summary>
    /// Identifies a record by its primary key
    /// </summary>
    [PublicAPI]
    public class IdentifyByPrimaryKey : IdentifyBase
        {
        private IAttribute _keyValue;

        /// <summary>
        /// Constructs an identifier for a specific row in a table by its primary key
        /// </summary>
        /// <param name="keyValue">Primary key value</param>
        // ReSharper disable once NotNullMemberIsNotInitialized
        public IdentifyByPrimaryKey(IAttribute keyValue)
            {
            if (keyValue == null) throw new ArgumentNullException(nameof(keyValue));
            Validate(keyValue);
            this._keyValue = keyValue;
            }

        /// <summary>
        /// Constructs an identifier for a specific row in a table by its primary key
        /// </summary>
        /// <param name="keyValue">Primary key value</param>
        // ReSharper disable once NotNullMemberIsNotInitialized
        public IdentifyByPrimaryKey(int keyValue)
            {
            this._keyValue = new IntAttribute(keyValue);
            }

        /// <summary>
        /// Constructs an identifier for a specific row in a table by its primary key
        /// </summary>
        /// <param name="keyValue">Primary key value</param>
        // ReSharper disable once NotNullMemberIsNotInitialized
        public IdentifyByPrimaryKey(Guid keyValue)
            {
            this._keyValue = new GuidAttribute(keyValue);
            }

        /// <summary>
        /// Constructs an identifier for a specific row in a table by its primary key
        /// </summary>
        /// <param name="keyValue">Primary key value</param>
        // ReSharper disable once NotNullMemberIsNotInitialized
        public IdentifyByPrimaryKey(string keyValue)
            {
            if (keyValue == null)
                throw new ArgumentNullException(nameof(keyValue));
            this._keyValue = new StringAttribute(keyValue);
            }

        /// <summary>
        /// Gets the value to search for
        /// </summary>
        public IAttribute KeyValue
            {
            get => this._keyValue;

            set
                {
                Validate(value);
                this._keyValue = value;
                }
            }

        private static void Validate(IAttribute value)
            {
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            if (value.Value == null)
                throw new ArgumentOutOfRangeException(nameof(value), "Primary key value cannot be null.");
            if (
                !(value is IntAttribute)
                && !(value is StringAttribute)
                && !(value is GuidAttribute)
                && !(value is DateAttribute)
                && !(value is DateTimeAttribute)
                )
                throw new ArgumentOutOfRangeException(nameof(value), "Primary key cannot be of the specified type.");
            }

        /// <inheritdoc />
        protected internal override void RenderKey(XmlWriter writer)
            {
            // ReSharper disable once AssignNullToNotNullAttribute
            writer.WriteAttributeString("KeyValue", this.KeyValue.ToString());
            }
        }
    }
