using System;
using System.Xml;

namespace FacadeFor3e
    {
    /// <summary>
    /// A delete operation on a row that is identified by a unique value such as Matter.Number
    /// </summary>
    /// <remarks>
    /// The AliasField indicates which attribute of the underlying archetype will be queried
    /// The KeyValue indicates the unique value to look for. An exception will be raised if the value is found not to be unique.
    /// </remarks>
    public class DeleteByAlias : DeleteByKey
        {
        private readonly string _aliasField;

        /// <summary>
        /// Constructs a Delete operation
        /// </summary>
        /// <param name="aliasField">Name of the alias attribute</param>
        /// <param name="aliasValue">Alias value</param>
        public DeleteByAlias(string aliasField, string aliasValue) : base(aliasValue)
            {
            if (string.IsNullOrWhiteSpace(aliasField))
                throw new ArgumentException("The aliasField parameter must specify a valid attribute name.");
            this._aliasField = aliasField;
            }

        /// <summary>
        /// Constructs a Delete operation
        /// </summary>
        /// <param name="aliasField">Name of the alias attribute</param>
        /// <param name="aliasValue">Alias value</param>
        public DeleteByAlias(string aliasField, int aliasValue) : base(aliasValue)
            {
            if (string.IsNullOrWhiteSpace(aliasField))
                throw new ArgumentException("The aliasField parameter must specify a valid attribute name.");
            this._aliasField = aliasField;
            }

        /// <summary>
        /// Constructs a Delete operation
        /// </summary>
        /// <param name="aliasField">Name of the alias attribute</param>
        /// <param name="aliasValue">Alias value</param>
        public DeleteByAlias(string aliasField, Guid aliasValue) : base(aliasValue)
            {
            if (string.IsNullOrWhiteSpace(aliasField))
                throw new ArgumentException("The aliasField parameter must specify a valid attribute name.");
            this._aliasField = aliasField;
            }

        /// <summary>
        /// Constructs a Delete operation
        /// </summary>
        /// <param name="aliasField">Name of the alias attribute</param>
        /// <param name="aliasValue">Alias value</param>
        public DeleteByAlias(string aliasField, DateTime aliasValue) : base(aliasValue)
            {
            if (string.IsNullOrWhiteSpace(aliasField))
                throw new ArgumentException("The aliasField parameter must specify a valid attribute name.");
            this._aliasField = aliasField;
            }

        /// <summary>
        /// Constructs a Delete operation
        /// </summary>
        /// <param name="aliasField">Name of the alias attribute</param>
        /// <param name="aliasValue">Alias value</param>
        public DeleteByAlias(string aliasField, decimal aliasValue) : base(aliasValue)
            {
            if (string.IsNullOrWhiteSpace(aliasField))
                throw new ArgumentException("The aliasField parameter must specify a valid attribute name.");
            this._aliasField = aliasField;
            }

        public string AliasField
            {
            get { return this._aliasField; }
            }
        
        protected override void RenderKey(XmlWriter writer)
            {
            writer.WriteAttributeString("KeyValue", this.OutputValue);
            writer.WriteAttributeString("AliasField", this.AliasField);
            }
        }
    }
