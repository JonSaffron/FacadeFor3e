using System;
using System.Xml;
using JetBrains.Annotations;

namespace FacadeFor3e.ProcessCommandBuilder
    {
    /// <summary>
    /// Identifies a record within a child collection.
    /// The value should be unique within the child collection, but doesn't have to be unique in the table,
    /// for example you can identify a particular MattDate record within a matter by its EffStart value,
    /// or identify the default Site in a Relate record by the IsDefault value.
    /// </summary>
    [PublicAPI]
    public class IdentifyByValue : IdentifyBase
        {
        private string _attributeName;
        private IAttribute _attribute;

        /// <summary>
        /// Gets or sets the id of the attribute that is checked for a matching value
        /// </summary>
        public string KeyField
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

        /// <summary>
        /// Constructs an identifier for a row in a child collection
        /// </summary>
        /// <param name="attributeName">The name of the attribute that contains the value to search for</param>
        /// <param name="attributeValue">The value within the child collection to search for</param>
        // ReSharper disable once NotNullMemberIsNotInitialized
        public IdentifyByValue(string attributeName, IAttribute attributeValue)
            {
            if (attributeName == null)
                throw new ArgumentNullException(nameof(attributeName));
            if (attributeValue == null) 
                throw new ArgumentNullException(nameof(attributeValue));
            CommonLibrary.EnsureValid(attributeName);
            this._attributeName = attributeName;
            this._attribute = attributeValue;
            }

        /// <inheritdoc />
        protected internal override void RenderKey(XmlWriter writer)
            {
            // ReSharper disable once AssignNullToNotNullAttribute
            writer.WriteAttributeString("KeyValue", this.KeyValue.ToString());
            writer.WriteAttributeString("KeyField", this.KeyField);
            }
        }
    }
