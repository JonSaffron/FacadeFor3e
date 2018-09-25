using System;
using System.Collections.Generic;
using System.Xml;
using JetBrains.Annotations;

namespace FacadeFor3e
    {
    /// <summary>
    /// Describes which object is to be updated and what operations to perform
    /// </summary>
    [PublicAPI]
    public class DataObject
        {
        private readonly List<OperationBase> _operations;

        /// <summary>
        /// Constructs a new data object
        /// </summary>
        /// <param name="objectName">Name of the object to be affected</param>
        public DataObject([NotNull] string objectName)
            {
            this.Name = objectName ?? throw new ArgumentNullException(nameof(objectName));

            this._operations =  new List<OperationBase>();
            }

        /// <summary>
        /// Gets the name of the object affected
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the operations to be performed
        /// </summary>
        public IList<OperationBase> Operations => this._operations;

        /// <summary>
        /// Output the object
        /// </summary>
        /// <param name="writer">An XMLWriter to output to</param>
        protected internal virtual void Render(XmlWriter writer)
            {
            writer.WriteStartElement(this.Name);
            foreach (OperationBase o in this._operations)
                {
                o.Render(writer, this.Name);
                }
            writer.WriteEndElement();
            }

///// Add operations

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
        /// Creates an Add operation from template data
        /// </summary>
        /// <param name="modelId">The ID of the template</param>
        /// <returns>An Add operation</returns>
        public OperationAdd AddOperationFromModel(Guid modelId)
            {
            var o = OperationAdd.AddFromModel(modelId);
            this._operations.Add(o);
            return o;
            }

        /// <summary>
        /// Creates an Add operation from an existing row
        /// </summary>
        /// <param name="rowId">The ID of the row to clone</param>
        /// <returns>An Add operation</returns>
        public OperationAdd AddOperationFromExistingRow(Guid rowId)
            {
            var o = OperationAdd.AddModelFromDb(rowId);
            this._operations.Add(o);
            return o;
            }

///// Delete operations

        /// <summary>
        /// Creates a Delete operation
        /// </summary>
        /// <param name="keyValue">Primary key value</param>
        /// <returns>A Delete operation</returns>
        public OperationDelete DeleteOperation(string keyValue)
            {
            var o = new DeleteByKey(keyValue);
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
            var o = new DeleteByKey(keyValue);
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
            var o = new DeleteByKey(keyValue);
            this._operations.Add(o);
            return o;
            }

        /// <summary>
        /// Creates a Delete operation
        /// </summary>
        /// <param name="keyValue">Primary key value</param>
        /// <returns>A Delete operation</returns>
        public OperationDelete DeleteOperation(DateTime keyValue)
            {
            var o = new DeleteByKey(keyValue);
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
            var o = new DeleteByAlias(aliasName, aliasValue);
            this._operations.Add(o);
            return o;
            }

        /// <summary>
        /// Creates a Delete operation
        /// </summary>
        /// <param name="aliasName">Name of the alias attribute</param>
        /// <param name="aliasValue">Alias value</param>
        /// <returns>A Delete operation</returns>
        public OperationDelete DeleteOperationByAlias(string aliasName, int aliasValue)
            {
            var o = new DeleteByAlias(aliasName, aliasValue);
            this._operations.Add(o);
            return o;
            }

        /// <summary>
        /// Creates a Delete operation
        /// </summary>
        /// <param name="aliasName">Name of the alias attribute</param>
        /// <param name="aliasValue">Alias value</param>
        /// <returns>A Delete operation</returns>
        public OperationDelete DeleteOperationByAlias(string aliasName, Guid aliasValue)
            {
            var o = new DeleteByAlias(aliasName, aliasValue);
            this._operations.Add(o);
            return o;
            }

        /// <summary>
        /// Creates a Delete operation
        /// </summary>
        /// <param name="aliasName">Name of the alias attribute</param>
        /// <param name="aliasValue">Alias value</param>
        /// <returns>A Delete operation</returns>
        public OperationDelete DeleteOperationByAlias(string aliasName, DateTime aliasValue)
            {
            var o = new DeleteByAlias(aliasName, aliasValue);
            this._operations.Add(o);
            return o;
            }

        /// <summary>
        /// Creates a Delete operation
        /// </summary>
        /// <param name="aliasName">Name of the alias attribute</param>
        /// <param name="aliasValue">Alias value</param>
        /// <returns>A Delete operation</returns>
        public OperationDelete DeleteOperationByAlias(string aliasName, decimal aliasValue)
            {
            var o = new DeleteByAlias(aliasName, aliasValue);
            this._operations.Add(o);
            return o;
            }

        /// <summary>
        /// Creates a Delete operation
        /// </summary>
        /// <param name="position">Zero based record number</param>
        /// <returns>A Delete operation</returns>
        /// <remarks>Can only be used in the context of a child collection</remarks>
        public OperationDelete DeleteOperationByPosition(int position)
            {
            var o = new DeleteByPosition(position);
            this._operations.Add(o);
            return o;
            }

        /// <summary>
        /// Creates a Delete operation
        /// </summary>
        /// <param name="keyField">Name of the attribute</param>
        /// <param name="fieldValue">Attribute value</param>
        /// <returns>A Delete operation</returns>
        /// <remarks>Can only be used in the context of a child collection</remarks>
        public OperationDelete DeleteOperationByKeyField(string keyField, string fieldValue)
            {
            var o = new DeleteByKeyField(keyField, fieldValue);
            this._operations.Add(o);
            return o;
            }

        /// <summary>
        /// Creates a Delete operation
        /// </summary>
        /// <param name="keyField">Name of the attribute</param>
        /// <param name="fieldValue">Attribute value</param>
        /// <returns>A Delete operation</returns>
        /// <remarks>Can only be used in the context of a child collection</remarks>
        public OperationDelete DeleteOperationByKeyField(string keyField, int fieldValue)
            {
            var o = new DeleteByKeyField(keyField, fieldValue);
            this._operations.Add(o);
            return o;
            }

        /// <summary>
        /// Creates a Delete operation
        /// </summary>
        /// <param name="keyField">Name of the attribute</param>
        /// <param name="fieldValue">Attribute value</param>
        /// <returns>A Delete operation</returns>
        /// <remarks>Can only be used in the context of a child collection</remarks>
        public OperationDelete DeleteOperationByKeyField(string keyField, Guid fieldValue)
            {
            var o = new DeleteByKeyField(keyField, fieldValue);
            this._operations.Add(o);
            return o;
            }

        /// <summary>
        /// Creates a Delete operation
        /// </summary>
        /// <param name="keyField">Name of the attribute</param>
        /// <param name="fieldValue">Attribute value</param>
        /// <returns>A Delete operation</returns>
        /// <remarks>Can only be used in the context of a child collection</remarks>
        public OperationDelete DeleteOperationByKeyField(string keyField, DateTime fieldValue)
            {
            var o = new DeleteByKeyField(keyField, fieldValue);
            this._operations.Add(o);
            return o;
            }

        /// <summary>
        /// Creates a Delete operation
        /// </summary>
        /// <param name="keyField">Name of the attribute</param>
        /// <param name="fieldValue">Attribute value</param>
        /// <returns>A Delete operation</returns>
        /// <remarks>Can only be used in the context of a child collection</remarks>
        public OperationDelete DeleteOperationByKeyField(string keyField, decimal fieldValue)
            {
            var o = new DeleteByKeyField(keyField, fieldValue);
            this._operations.Add(o);
            return o;
            }

        /// <summary>
        /// Creates a Delete operation
        /// </summary>
        /// <param name="keyField">Name of the attribute</param>
        /// <param name="fieldValue">Attribute value</param>
        /// <returns>A Delete operation</returns>
        /// <remarks>Can only be used in the context of a child collection</remarks>
        public OperationDelete DeleteOperationByKeyField(string keyField, bool fieldValue)
            {
            var o = new DeleteByKeyField(keyField, fieldValue);
            this._operations.Add(o);
            return o;
            }

///// Edit operations

        /// <summary>
        /// Creates a Edit operation
        /// </summary>
        /// <param name="keyValue">Primary key value</param>
        /// <returns>A Edit operation</returns>
        public OperationEdit EditOperation(string keyValue)
            {
            var o = new EditByKey(keyValue);
            this._operations.Add(o);
            return o;
            }

        /// <summary>
        /// Creates a Edit operation
        /// </summary>
        /// <param name="keyValue">Primary key value</param>
        /// <returns>A Edit operation</returns>
        public OperationEdit EditOperation(int keyValue)
            {
            var o = new EditByKey(keyValue);
            this._operations.Add(o);
            return o;
            }

        /// <summary>
        /// Creates a Edit operation
        /// </summary>
        /// <param name="keyValue">Primary key value</param>
        /// <returns>A Edit operation</returns>
        public OperationEdit EditOperation(Guid keyValue)
            {
            var o = new EditByKey(keyValue);
            this._operations.Add(o);
            return o;
            }

        /// <summary>
        /// Creates a Edit operation
        /// </summary>
        /// <param name="keyValue">Primary key value</param>
        /// <returns>A Edit operation</returns>
        public OperationEdit EditOperation(DateTime keyValue)
            {
            var o = new EditByKey(keyValue);
            this._operations.Add(o);
            return o;
            }

        /// <summary>
        /// Creates a Edit operation
        /// </summary>
        /// <param name="aliasName">Name of the alias attribute</param>
        /// <param name="aliasValue">Alias value</param>
        /// <returns>A Edit operation</returns>
        public OperationEdit EditOperationByAlias(string aliasName, string aliasValue)
            {
            var o = new EditByAlias(aliasName, aliasValue);
            this._operations.Add(o);
            return o;
            }

        /// <summary>
        /// Creates a Edit operation
        /// </summary>
        /// <param name="aliasName">Name of the alias attribute</param>
        /// <param name="aliasValue">Alias value</param>
        /// <returns>A Edit operation</returns>
        public OperationEdit EditOperationByAlias(string aliasName, int aliasValue)
            {
            var o = new EditByAlias(aliasName, aliasValue);
            this._operations.Add(o);
            return o;
            }

        /// <summary>
        /// Creates a Edit operation
        /// </summary>
        /// <param name="aliasName">Name of the alias attribute</param>
        /// <param name="aliasValue">Alias value</param>
        /// <returns>A Edit operation</returns>
        public OperationEdit EditOperationByAlias(string aliasName, Guid aliasValue)
            {
            var o = new EditByAlias(aliasName, aliasValue);
            this._operations.Add(o);
            return o;
            }

        /// <summary>
        /// Creates a Edit operation
        /// </summary>
        /// <param name="aliasName">Name of the alias attribute</param>
        /// <param name="aliasValue">Alias value</param>
        /// <returns>A Edit operation</returns>
        public OperationEdit EditOperationByAlias(string aliasName, DateTime aliasValue)
            {
            var o = new EditByAlias(aliasName, aliasValue);
            this._operations.Add(o);
            return o;
            }

        /// <summary>
        /// Creates a Edit operation
        /// </summary>
        /// <param name="aliasName">Name of the alias attribute</param>
        /// <param name="aliasValue">Alias value</param>
        /// <returns>A Edit operation</returns>
        public OperationEdit EditOperationByAlias(string aliasName, decimal aliasValue)
            {
            var o = new EditByAlias(aliasName, aliasValue);
            this._operations.Add(o);
            return o;
            }

        /// <summary>
        /// Creates a Edit operation
        /// </summary>
        /// <param name="position">Zero based record number</param>
        /// <returns>A Edit operation</returns>
        /// <remarks>Can only be used in the context of a child collection</remarks>
        public OperationEdit EditOperationByPosition(int position)
            {
            var o = new EditByPosition(position);
            this._operations.Add(o);
            return o;
            }

        /// <summary>
        /// Creates a Edit operation
        /// </summary>
        /// <param name="keyField">Name of the attribute</param>
        /// <param name="fieldValue">Attribute value</param>
        /// <returns>A Edit operation</returns>
        /// <remarks>Can only be used in the context of a child collection</remarks>
        public OperationEdit EditOperationByKeyField(string keyField, string fieldValue)
            {
            var o = new EditByKeyField(keyField, fieldValue);
            this._operations.Add(o);
            return o;
            }

        /// <summary>
        /// Creates a Edit operation
        /// </summary>
        /// <param name="keyField">Name of the attribute</param>
        /// <param name="fieldValue">Attribute value</param>
        /// <returns>A Edit operation</returns>
        /// <remarks>Can only be used in the context of a child collection</remarks>
        public OperationEdit EditOperationByKeyField(string keyField, int fieldValue)
            {
            var o = new EditByKeyField(keyField, fieldValue);
            this._operations.Add(o);
            return o;
            }

        /// <summary>
        /// Creates a Edit operation
        /// </summary>
        /// <param name="keyField">Name of the attribute</param>
        /// <param name="fieldValue">Attribute value</param>
        /// <returns>A Edit operation</returns>
        /// <remarks>Can only be used in the context of a child collection</remarks>
        public OperationEdit EditOperationByKeyField(string keyField, Guid fieldValue)
            {
            var o = new EditByKeyField(keyField, fieldValue);
            this._operations.Add(o);
            return o;
            }

        /// <summary>
        /// Creates a Edit operation
        /// </summary>
        /// <param name="keyField">Name of the attribute</param>
        /// <param name="fieldValue">Attribute value</param>
        /// <returns>A Edit operation</returns>
        /// <remarks>Can only be used in the context of a child collection</remarks>
        public OperationEdit EditOperationByKeyField(string keyField, DateTime fieldValue)
            {
            var o = new EditByKeyField(keyField, fieldValue);
            this._operations.Add(o);
            return o;
            }

        /// <summary>
        /// Creates a Edit operation
        /// </summary>
        /// <param name="keyField">Name of the attribute</param>
        /// <param name="fieldValue">Attribute value</param>
        /// <returns>A Edit operation</returns>
        /// <remarks>Can only be used in the context of a child collection</remarks>
        public OperationEdit EditOperationByKeyField(string keyField, decimal fieldValue)
            {
            var o = new EditByKeyField(keyField, fieldValue);
            this._operations.Add(o);
            return o;
            }

        /// <summary>
        /// Creates a Edit operation
        /// </summary>
        /// <param name="keyField">Name of the attribute</param>
        /// <param name="fieldValue">Attribute value</param>
        /// <returns>A Edit operation</returns>
        /// <remarks>Can only be used in the context of a child collection</remarks>
        public OperationEdit EditOperationByKeyField(string keyField, bool fieldValue)
            {
            var o = new EditByKeyField(keyField, fieldValue);
            this._operations.Add(o);
            return o;
            }
        }
    }
