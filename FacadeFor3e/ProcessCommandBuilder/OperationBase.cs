using System.Xml;
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
        /// The subclass of object to create
        /// </summary>
        /// <remarks>If null, this defaults to the parent data object</remarks>
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

        /// <summary>
        /// Output the operation
        /// </summary>
        /// <param name="writer">An XMLWriter to output to</param>
        /// <param name="objectSuperclassName">The name of the parent data object for when SubClass is not specified</param>
        protected internal abstract void Render(XmlWriter writer, string objectSuperclassName);
        }
    }
