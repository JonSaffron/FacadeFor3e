using System;
using JetBrains.Annotations;

namespace FacadeFor3e.ProcessCommandBuilder
    {
    /// <summary>
    /// Identifies a record within a child collection.
    /// The value should be unique within the child collection, although it doesn't have to be unique 
    /// within the whole table.
    /// For example, you can use this to identify:
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
        private string _attributeName = null!;
        private IAttribute _attribute = null!;

        /// <summary>
        /// Constructs an identifier for a row in a child collection
        /// </summary>
        /// <param name="attributeName">The name of the attribute that contains the value to search for</param>
        /// <param name="attributeValue">The value within the child collection to search for</param>
        public IdentifyByValue(string attributeName, IAttribute attributeValue)
            {
            this.KeyField = attributeName;
            this.KeyValue = attributeValue;
            }

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

            set
                {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));
                if (!value.HasValue)
                    throw new ArgumentOutOfRangeException(nameof(value), "KeyValue must have a value.");
                this._attribute = value;
                }
            }
        }
    }
