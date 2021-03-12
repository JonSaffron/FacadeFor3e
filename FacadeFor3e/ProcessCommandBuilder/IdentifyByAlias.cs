using System;
using System.Xml;
using JetBrains.Annotations;

namespace FacadeFor3e.ProcessCommandBuilder
    {
    /// <summary>
    /// Identifies a row by a unique value which may not be the primary key such as Matter.Number
    /// </summary>
    [PublicAPI]
    public class IdentifyByAlias : IdentifyBase
        {
        private string _attributeName;
        private IAttribute _attribute;

        /// <summary>
        /// Constructs an identifier for a row in a child collection
        /// </summary>
        /// <param name="aliasField">The name of the attribute that contains the value to search for</param>
        /// <param name="keyValue">The value within the collection to search for</param>
        // ReSharper disable once NotNullMemberIsNotInitialized
        public IdentifyByAlias(string aliasField, IAttribute keyValue)
            {
            this._attributeName = aliasField ?? throw new ArgumentNullException(nameof(aliasField));
            this._attribute = keyValue ?? throw new ArgumentNullException(nameof(keyValue));
            }

        /// <summary>
        /// Constructs an identifier for a row in a child collection
        /// </summary>
        /// <param name="aliasField">The name of the attribute that contains the value to search for</param>
        /// <param name="keyValue">The value within the collection to search for</param>
        // ReSharper disable once NotNullMemberIsNotInitialized
        public IdentifyByAlias(string aliasField, string keyValue)
            {
            if (aliasField == null) throw new ArgumentNullException(nameof(aliasField));
            CommonLibrary.EnsureValid(aliasField);
            this._attributeName = aliasField;
            if (keyValue == null) throw new ArgumentNullException(nameof(keyValue));
            this._attribute = new StringAttribute(keyValue);
            }

        /// <summary>
        /// Gets or sets the id of the attribute that is checked for a matching value
        /// </summary>
        public string AliasField
            {
            get => this._attributeName;

            set
                {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));
                CommonLibrary.EnsureValid(value);
                this._attributeName = value;
                }
            }

        /// <summary>
        /// Gets or sets the value that is being looked for
        /// </summary>
        public IAttribute KeyValue
            {
            get => this._attribute;

            set => this._attribute = value ?? throw new ArgumentNullException(nameof(value));
            }

        /// <inheritdoc />
        protected internal override void RenderKey(XmlWriter writer)
            {
            // ReSharper disable once AssignNullToNotNullAttribute
            writer.WriteAttributeString("KeyValue", this.KeyValue.ToString());
            writer.WriteAttributeString("AliasField", this.AliasField);
            }
        }
    }
