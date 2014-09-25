using System;
using System.Collections.Generic;
using System.Xml;
using JetBrains.Annotations;

namespace FacadeFor3e
    {
    /// <summary>
    /// Describes which object is to be updated and what operations to perform
    /// </summary>
    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public class DataObject
        {
        private readonly string _objectName;
        private readonly List<OperationBase> _operations;

        /// <summary>
        /// Constructs a new data object
        /// </summary>
        /// <param name="objectName">Name of the object to be affected</param>
        public DataObject(string objectName)
            {
            if (objectName == null)
                throw new ArgumentNullException("objectName");
            this._objectName = objectName;

            this._operations =  new List<OperationBase>();
            }

        /// <summary>
        /// Gets the name of the object affected
        /// </summary>
        public string Name
            {
            get { return this._objectName; }
            }

        /// <summary>
        /// Gets the operations to be performed
        /// </summary>
        public IList<OperationBase> Operations
            {
            get { return this._operations; }
            }

        /// <summary>
        /// Output the object
        /// </summary>
        /// <param name="writer">An XMLWriter to output to</param>
        protected internal virtual void Render(XmlWriter writer)
            {
            writer.WriteStartElement(this._objectName);
            foreach (OperationBase o in this._operations)
                {
                o.Render(writer, this._objectName);
                }
            writer.WriteEndElement();
            }

        /// <summary>
        /// Creates an Add operation
        /// </summary>
        /// <returns>An Add operation</returns>
        public OperationAdd AddOperation()
            {
            var o = new OperationAdd();
            this._operations.Add(o);
            return o;
            }

        /// <summary>
        /// Creates a Delete operation
        /// </summary>
        /// <param name="keyValue">Primary key value</param>
        /// <returns>A Delete operation</returns>
        public OperationDelete DeleteOperation(string keyValue)
            {
            var o = OperationDelete.DeleteByKeyValue(keyValue);
            this._operations.Add(o);
            return o;
            }

        /// <summary>
        /// Creates a Delete operation
        /// </summary>
        /// <param name="keyValue">Primary key value</param>
        /// <returns>A Delete operation</returns>
        public OperationDelete DeleteOperation(int keyValue)
            {
            var o = OperationDelete.DeleteByKeyValue(keyValue);
            this._operations.Add(o);
            return o;
            }

        /// <summary>
        /// Creates a Delete operation
        /// </summary>
        /// <param name="keyValue">Primary key value</param>
        /// <returns>A Delete operation</returns>
        public OperationDelete DeleteOperation(Guid keyValue)
            {
            var o = OperationDelete.DeleteByKeyValue(keyValue);
            this._operations.Add(o);
            return o;
            }

        /// <summary>
        /// Creates a Delete operation
        /// </summary>
        /// <param name="aliasName">Name of the alias attribute</param>
        /// <param name="aliasValue">Alias value</param>
        /// <returns>A Delete operation</returns>
        public OperationDelete DeleteOperationByAlias(string aliasName, string aliasValue)
            {
            var o = OperationDelete.DeleteByAlias(aliasName, aliasValue);
            this._operations.Add(o);
            return o;
            }

        /// <summary>
        /// Creates a Delete operation
        /// </summary>
        /// <param name="position">Zero based record number</param>
        /// <returns>A Delete operation</returns>
        public OperationDelete DeleteOperationByPosition(int position)
            {
            var o = OperationDelete.DeleteByPosition(position);
            this._operations.Add(o);
            return o;
            }

        /// <summary>
        /// Creates an Edit operation
        /// </summary>
        /// <param name="keyValue">Primary key value</param>
        /// <returns>An Edit operation</returns>
        public OperationEdit EditOperation(string keyValue)
            {
            var o = OperationEdit.EditByKeyField(keyValue);
            this._operations.Add(o);
            return o;
            }

        /// <summary>
        /// Creates an Edit operation
        /// </summary>
        /// <param name="keyValue">Primary key value</param>
        /// <returns>An Edit operation</returns>
        public OperationEdit EditOperation(int keyValue)
            {
            var o = OperationEdit.EditByKeyField(keyValue);
            this._operations.Add(o);
            return o;
            }

        /// <summary>
        /// Creates an Edit operation
        /// </summary>
        /// <param name="keyValue">Primary key value</param>
        /// <returns>An Edit operation</returns>
        public OperationEdit EditOperation(Guid keyValue)
            {
            var o = OperationEdit.EditByKeyField(keyValue);
            this._operations.Add(o);
            return o;
            }

        /// <summary>
        /// Creates an Edit operation
        /// </summary>
        /// <param name="aliasName">Name of the alias attribute</param>
        /// <param name="aliasValue">Alias value</param>
        /// <returns>An Edit operation</returns>
        public OperationEdit EditOperationByAlias(string aliasName, string aliasValue)
            {
            var o = OperationEdit.EditByAlias(aliasName, aliasValue);
            this._operations.Add(o);
            return o;
            }

        /// <summary>
        /// Creates an Edit operation
        /// </summary>
        /// <param name="position">Zero based record number</param>
        /// <returns>An Edit operation</returns>
        public OperationEdit EditOperationByPosition(int position)
            {
            var o = OperationEdit.EditByPosition(position);
            this._operations.Add(o);
            return o;
            }
        }
    }
