using System;
using System.Xml;
using JetBrains.Annotations;

namespace FacadeFor3e.ProcessCommandBuilder
    {
    /// <summary>
    /// Identifies a record within a child collection.
    /// The value should be unique within the child collection, although it doesn't have to be unique 
    /// within the whole table.
    /// For example you can use this to identify:
    /// - one of the MattDate children of a Matter record by its EffStart value,
    /// - one of the MattPrlfTkpr children of a MattDate record by specifying a timekeeper
    /// - the default Site in the child collection of a Relate record by getting the record where IsDefault is True,
    /// - a particular proforma template option value on a proforma by the template option name
    /// Unfortunately, you cannot specify the record using an alias value, so if you want to identify
    /// a MattPrlfTkpr record by timekeeper you must do it by the timekeeper Index rather than 
    /// the timekeeper Number.
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
