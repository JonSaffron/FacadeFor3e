using System;
using System.Globalization;
using System.Xml;

namespace FacadeFor3e
    {
    /// <summary>
    /// Defines an operation to delete a record
    /// </summary>
    public class OperationDelete : OperationBase
        {
        private string _keyValue;
        private string _alias;
        private int _position;

        /// <summary>
        /// Private constructor
        /// </summary>
        private OperationDelete()
            {
            }

        /// <summary>
        /// Constructs a Delete operation
        /// </summary>
        /// <param name="keyValue">Primary key value</param>
        public static OperationDelete DeleteByKeyValue(string keyValue)
            {
            if (keyValue == null)
                throw new ArgumentNullException("keyValue");

            var result = new OperationDelete {_keyValue = keyValue};
            return result;
            }

        /// <summary>
        /// Constructs a Delete operation
        /// </summary>
        /// <param name="keyValue">Primary key value</param>
        public static OperationDelete DeleteByKeyValue(int keyValue)
            {
            var result = new OperationDelete {_keyValue = keyValue.ToString("F0")};
            return result;
            }

        /// <summary>
        /// Constructs a Delete operation
        /// </summary>
        /// <param name="keyValue">Primary key value</param>
        public static OperationDelete DeleteByKeyValue(Guid keyValue)
            {
            var result = new OperationDelete {_keyValue = keyValue.ToString("B")};
            return result;
            }

        /// <summary>
        /// Constructs a Delete operation
        /// </summary>
        /// <param name="aliasName">Name of the alias attribute</param>
        /// <param name="aliasValue">Alias value</param>
        public static OperationDelete DeleteByAlias(string aliasName, string aliasValue)
            {
            if (aliasName == null)
                throw new ArgumentNullException("aliasName");

            var result = DeleteByKeyValue(aliasValue);
            result._alias = aliasName;
            return result;
            }

        /// <summary>
        /// Constructs a Delete operation
        /// </summary>
        /// <param name="position">Zero based record number</param>
        public static OperationDelete DeleteByPosition(int position)
            {
            if (position < 0)
                throw new ArgumentOutOfRangeException("position");

            var result = new OperationDelete {_position = position};
            return result;
            }

        /// <summary>
        /// Outputs this operation
        /// </summary>
        /// <param name="writer">An XMLWriter to output to</param>
        /// <param name="objectName">The name of the parent data object</param>
        protected internal override void Render(XmlWriter writer, string objectName)
            {
            writer.WriteStartElement("Delete");
            writer.WriteStartElement(objectName);

            if (this._keyValue != null)
                {
                if (this._alias != null)
                    writer.WriteAttributeString("AliasField", this._alias);
                writer.WriteAttributeString("KeyValue", this._keyValue);
                }
            else
                {
                writer.WriteAttributeString("Position", this._position.ToString(CultureInfo.InvariantCulture));
                }

            writer.WriteEndElement();
            writer.WriteEndElement();
            }
        }
    }
