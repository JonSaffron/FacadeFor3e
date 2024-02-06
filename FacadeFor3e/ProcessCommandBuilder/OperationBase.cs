using JetBrains.Annotations;

namespace FacadeFor3e.ProcessCommandBuilder
    {
    /// <summary>
    /// Defines the interface for an operation upon an object
    /// </summary>
    [PublicAPI]
    public abstract class OperationBase
        {
        private string? _subClass;

        /// <summary>
        /// The particular subclass of the parent DataObject's base class to carry out the operation on
        /// </summary>
        /// <remarks>If not set, the operation will be carried out using the parent DataObject's object type</remarks>
        public string? SubClass
            {
            get => this._subClass;
            set
                {
                if (value == null)
                    {
                    this._subClass = null;
                    return;
                    }
                CommonLibrary.EnsureValid(value);
                this._subClass = value;
                }
            }
        }
    }
