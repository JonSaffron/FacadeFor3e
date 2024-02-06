using System;
using JetBrains.Annotations;

namespace FacadeFor3e.ProcessCommandBuilder
    {
    /// <summary>
    /// Defines an operation to delete a record
    /// </summary>
    [PublicAPI]
    public class DeleteOperation : OperationBase, IHasKey
        {
        private IdentifyBase _keySpecification;

        /// <summary>
        /// Creates a new Delete operation
        /// </summary>
        /// <param name="keySpecification">Defines which row should be deleted</param>
        // ReSharper disable once NotNullMemberIsNotInitialized
        public DeleteOperation(IdentifyBase keySpecification)
            {
            this._keySpecification = keySpecification ?? throw new ArgumentNullException(nameof(keySpecification));
            }

        /// <summary>
        /// Creates a new Delete operation
        /// </summary>
        /// <param name="keySpecification">Defines which row should be edited</param>
        /// <param name="subClass">The id of the subclass of object to delete</param>
        // ReSharper disable once NotNullMemberIsNotInitialized
        public DeleteOperation(IdentifyBase keySpecification, string subClass) : this(keySpecification)
            {
            this.SubClass = subClass ?? throw new ArgumentNullException(nameof(subClass));
            }

        /// <summary>
        /// Gets or sets the specification of which row to edit
        /// </summary>
        public IdentifyBase KeySpecification
            {
            get => this._keySpecification;

            set => this._keySpecification = value ?? throw new ArgumentNullException(nameof(value));
            }

        /// <summary>
        /// Returns a DeleteOperation object upon a row identified by primary key
        /// </summary>
        /// <param name="primaryKey">Specifies the primary key</param>
        /// <returns>A DeleteOperation object that targets a row by primary key</returns>
        public static DeleteOperation ByPrimaryKey(int primaryKey)
            {
            return new DeleteOperation(new IdentifyByPrimaryKey(primaryKey));
            }

        /// <summary>
        /// Returns a DeleteOperation object upon a row identified by primary key
        /// </summary>
        /// <param name="primaryKey">Specifies the primary key</param>
        /// <returns>A DeleteOperation object that targets a row by primary key</returns>
        public static DeleteOperation ByPrimaryKey(Guid primaryKey)
            {
            return new DeleteOperation(new IdentifyByPrimaryKey(primaryKey));
            }

        /// <summary>
        /// Returns a DeleteOperation object upon a row identified by primary key
        /// </summary>
        /// <param name="primaryKey">Specifies the primary key</param>
        /// <returns>A DeleteOperation object that targets a row by primary key</returns>
        public static DeleteOperation ByPrimaryKey(string primaryKey)
            {
            return new DeleteOperation(new IdentifyByPrimaryKey(primaryKey));
            }

        /// <summary>
        /// Returns a DeleteOperation object upon a row identified by a unique value found on the specified attribute
        /// </summary>
        /// <param name="alias">Specifies the attribute that contains the value to search for</param>
        /// <param name="value">Specifies the value to search for</param>
        /// <returns>A DeleteOperation object that targets a row by primary key</returns>
        public static DeleteOperation ByUniqueAlias(string alias, string value)
            {
            return new DeleteOperation(new IdentifyByAlias(alias, new StringAttribute(value)));
            }
        }
    }
