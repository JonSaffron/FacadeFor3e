using System;
using JetBrains.Annotations;

namespace FacadeFor3e.ProcessCommandBuilder
    {
    /// <summary>
    /// Identifies a record by its primary key
    /// </summary>
    [PublicAPI]
    public class IdentifyByPrimaryKey : IdentifyBase
        {
        private string? _attributeName;
        private IAttribute _keyValue = null!;

        /// <summary>
        /// Constructs an identifier for a specific row in a table by its primary key
        /// </summary>
        /// <param name="keyValue">Primary key value</param>
        public IdentifyByPrimaryKey(IAttribute keyValue)
            {
            this.KeyValue = keyValue;
            }

        /// <summary>
        /// Constructs an identifier for a row in a child collection
        /// </summary>
        /// <param name="attributeName">The name of the attribute that corresponds to the primary key</param>
        /// <param name="keyValue">The value within the collection to search for</param>
        public IdentifyByPrimaryKey(string attributeName, IAttribute keyValue)
            {
            this.PrimaryKeyName = attributeName ?? throw new ArgumentNullException(nameof(attributeName));
            this.KeyValue = keyValue;
            }

        /// <summary>
        /// Constructs an identifier for a specific row in a table by its primary key
        /// </summary>
        /// <param name="keyValue">Primary key value</param>
        public IdentifyByPrimaryKey(int keyValue)
            {
            this.KeyValue = new IntAttribute(keyValue);
            }

        /// <summary>
        /// Constructs an identifier for a specific row in a table by its primary key
        /// </summary>
        /// <param name="attributeName">The name of the attribute that corresponds to the primary key</param>
        /// <param name="keyValue">Primary key value</param>
        public IdentifyByPrimaryKey(string attributeName, int keyValue)
            {
            this.PrimaryKeyName = attributeName ?? throw new ArgumentNullException(nameof(attributeName));
            this.KeyValue = new IntAttribute(keyValue);
            }

        /// <summary>
        /// Constructs an identifier for a specific row in a table by its primary key
        /// </summary>
        /// <param name="keyValue">Primary key value</param>
        public IdentifyByPrimaryKey(Guid keyValue)
            {
            this.KeyValue = new GuidAttribute(keyValue);
            }

        /// <summary>
        /// Constructs an identifier for a specific row in a table by its primary key
        /// </summary>
        /// <param name="attributeName">The name of the attribute that corresponds to the primary key</param>
        /// <param name="keyValue">Primary key value</param>
        public IdentifyByPrimaryKey(string attributeName, Guid keyValue)
            {
            this.PrimaryKeyName = attributeName ?? throw new ArgumentNullException(nameof(attributeName));
            this.KeyValue = new GuidAttribute(keyValue);
            }

        /// <summary>
        /// Constructs an identifier for a specific row in a table by its primary key
        /// </summary>
        /// <param name="keyValue">Primary key value</param>
        public IdentifyByPrimaryKey(string keyValue)
            {
            if (keyValue == null) throw new ArgumentNullException(nameof(keyValue));
            this.KeyValue = new StringAttribute(keyValue);
            }

        /// <summary>
        /// Constructs an identifier for a specific row in a table by its primary key
        /// </summary>
        /// <param name="attributeName">The name of the attribute that corresponds to the primary key</param>
        /// <param name="keyValue">Primary key value</param>
        public IdentifyByPrimaryKey(string attributeName, string keyValue)
            {
            this.PrimaryKeyName = attributeName ?? throw new ArgumentNullException(nameof(attributeName));
            this.KeyValue = new StringAttribute(keyValue);
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

        /// <summary>
        /// Gets or sets the id of the attribute that corresponds to the primary key
        /// </summary>
        /// <remarks>It is only necessary to set this when using odata and targeting a row in a child collection</remarks>
        public string? PrimaryKeyName
            {
            get => this._attributeName;

            set
                {
                if (value == null)
                    {
                    this._attributeName = null;
                    return;
                    }
                CommonLibrary.EnsureValid(value);
                this._attributeName = value;
                }
            }

        private static void Validate(IAttribute value)
            {
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            if (!value.HasValue)
                throw new ArgumentOutOfRangeException(nameof(value), "Primary key value cannot be null.");
            switch (value)
                {
                case IntAttribute _:
                case StringAttribute _:
                case GuidAttribute _:
                case DateAttribute _:
                case DateTimeAttribute _:
                    break;      // all good

                default:
                    throw new ArgumentOutOfRangeException(nameof(value), "Primary key cannot be of the specified type.");
                }
            }
        }
    }
