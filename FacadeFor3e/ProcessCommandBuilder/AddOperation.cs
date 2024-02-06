using System;
using JetBrains.Annotations;

namespace FacadeFor3e.ProcessCommandBuilder
    {
    /// <summary>
    /// Defines an operation to add a record
    /// </summary>
    [PublicAPI]
    public class AddOperation : OperationWithAttributesBase
        {
        /// <summary>
        /// Creates a new Add operation
        /// </summary>
        public AddOperation()
            {
            }

        /// <summary>
        /// Creates a new Add operation for the specified subclass
        /// </summary>
        /// <param name="subClass">The id of the subclass of object to create</param>
        public AddOperation(string subClass)
            {
            this.SubClass = subClass ?? throw new ArgumentNullException(nameof(subClass));
            CommonLibrary.IsValidId(subClass);
            }
        }
    }
