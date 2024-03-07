using System;
using JetBrains.Annotations;

namespace FacadeFor3e.ProcessCommandBuilder
    {
    /// <summary>
    /// Identifies a row by a unique value which may not be the primary key such as the Number for Matter, Client, or TimeKeeper.
    /// </summary>
    [PublicAPI]
    public class IdentifyByAlias : IdentifyBase
        {
        private string _attributeName = null!;
        private IAttribute _attribute = null!;

        /// <summary>
        /// Constructs an identifier for a row in a child collection
        /// </summary>
        /// <param name="aliasField">The name of the attribute that contains the value to search for</param>
        /// <param name="keyValue">The value within the collection to search for</param>
        public IdentifyByAlias(string aliasField, IAttribute keyValue)
            {
            this.AliasField = aliasField;
            this.KeyValue = keyValue;
            }

        /// <summary>
        /// Constructs an identifier for a row in a child collection
        /// </summary>
        /// <param name="aliasField">The name of the attribute that contains the value to search for</param>
        /// <param name="keyValue">The value within the collection to search for</param>
        public IdentifyByAlias(string aliasField, string keyValue)
            {
            this.AliasField = aliasField;
            this.KeyValue = new StringAttribute(keyValue);
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
