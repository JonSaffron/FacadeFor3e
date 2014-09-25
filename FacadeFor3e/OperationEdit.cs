using System;
using System.Globalization;
using System.Xml;

namespace FacadeFor3e
    {
    /// <summary>
    /// Defines an operation to update a record
    /// </summary>
    public class OperationEdit : OperationWithAttributesBase
        {
        private string _keyValue;
        private string _alias;
        private int _position;

        /// <summary>
        /// Private constructor
        /// </summary>
        private OperationEdit()
            {
            }

        /// <summary>
        /// Constructs an Edit operation
        /// </summary>
        /// <param name="keyValue">Primary key value</param>
        public static OperationEdit EditByKeyField(string keyValue)
            {
            if (keyValue == null)
                throw new ArgumentNullException("keyValue");

            var result = new OperationEdit {_keyValue = keyValue};
            return result;
            }

        /// <summary>
        /// Constructs an Edit operation
        /// </summary>
        /// <param name="keyValue">Primary key value</param>
        public static OperationEdit EditByKeyField(int keyValue)
            {
            var result = new OperationEdit {_keyValue = keyValue.ToString("F0")};
            return result;
            }
        
        /// <summary>
        /// Constructs an Edit operation
        /// </summary>
        /// <param name="keyValue">Primary key value</param>
        public static OperationEdit EditByKeyField(Guid keyValue)
            {
            var result = new OperationEdit {_keyValue = keyValue.ToString("B")};
            return result;
            }

        /// <summary>
        /// Constructs an Edit operation
        /// </summary>
        /// <param name="aliasName">Name of the alias attribute</param>
        /// <param name="aliasValue">Alias value</param>
        public static OperationEdit EditByAlias(string aliasName, string aliasValue)
            {
            if (aliasName == null)
                throw new ArgumentNullException("aliasName");

            var result = EditByKeyField(aliasValue);
            result._alias = aliasName;
            return result;
            }

        /// <summary>
        /// Constructs an Edit operation
        /// </summary>
        /// <param name="position">Zero based record number</param>
        public static OperationEdit EditByPosition(int position)
            {
            if (position < 0)
                throw new ArgumentOutOfRangeException("position");

            var result = new OperationEdit {_position = position};
            return result;
            }

        /// <summary>
        /// Outputs this operation
        /// </summary>
        /// <param name="writer">An XMLWriter to output to</param>
        /// <param name="objectName">The name of the parent data object</param>
        protected internal override void Render(XmlWriter writer, string objectName)
            {
            writer.WriteStartElement("Edit");
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

            if (this.Attributes.Count > 0)
                {
                writer.WriteStartElement("Attributes");
                foreach (AttributeValue a in this.Attributes)
                    a.Render(writer);
                writer.WriteEndElement();
                }

            if (this.Children.Count > 0)
                {
                writer.WriteStartElement("Children");
                foreach (DataObject a in this.Children)
                    a.Render(writer);
                writer.WriteEndElement();
                }

            writer.WriteEndElement();
            writer.WriteEndElement();
            }
        }
    }
