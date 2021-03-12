using System;
using System.Collections.Generic;
using System.Xml;
using JetBrains.Annotations;

namespace FacadeFor3e.ProcessCommandBuilder
    {
    /// <summary>
    /// Describes which object is to be updated and what operations to perform
    /// </summary>
    [PublicAPI]
    public class DataObject
        {
        private readonly OperationCollection _operations;

        /// <summary>
        /// Constructs a new DataObject
        /// </summary>
        /// <param name="objectName">The name of the 3E object</param>
        public DataObject(string objectName)
            {
            this.ObjectName = objectName ?? throw new ArgumentNullException(nameof(objectName));
            CommonLibrary.EnsureValid(objectName);
            this._operations =  new OperationCollection();
            }

        /// <summary>
        /// Gets the name of the 3E object
        /// </summary>
        public string ObjectName { get; }

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
            writer.WriteStartElement(this.ObjectName);
            foreach (OperationBase o in this._operations)
                {
                // ReSharper disable once PossibleNullReferenceException
                o.Render(writer, this.ObjectName);
                }
            writer.WriteEndElement();
            }

        /// <summary>
        /// Creates and appends an operation to create a new record 
        /// </summary>
        /// <returns>An <see cref="AddOperation">AddOperation</see></returns>
        public AddOperation AddRecord()
            {
            var o = new AddOperation();
            this._operations.Add(o);
            return o;
            }

        /// <summary>
        /// Creates and appends an operation to delete a specific record
        /// </summary>
        /// <param name="keySpecification">Specifies the record to delete</param>
        /// <returns>A <see cref="DeleteOperation">DeleteOperation</see></returns>
        public DeleteOperation DeleteRecord(IdentifyBase keySpecification)
            {
            var o = new DeleteOperation(keySpecification);
            this._operations.Add(o);
            return o;
            }

        /// <summary>
        /// Creates and appends an operation to update a specific record
        /// </summary>
        /// <param name="keySpecification">Specifies the record to update</param>
        /// <returns>An <see cref="EditOperation">EditOperation</see></returns>
        public EditOperation EditRecord(IdentifyBase keySpecification)
            {
            var o = new EditOperation(keySpecification);
            this._operations.Add(o);
            return o;
            }
        }
    }
