using System;
using JetBrains.Annotations;

namespace FacadeFor3e.ProcessCommandBuilder
    {
    /// <summary>
    /// Identifies a row by its position in the collection
    /// </summary>
    [PublicAPI]
    public class IdentifyByPosition : IdentifyBase
        {
        private int _position;

        /// <summary>
        /// Constructs an identifier for a specific row in a child collection
        /// </summary>
        /// <param name="position">Zero based record number</param>
        public IdentifyByPosition(int position)
            {
            this.Position = position;
            }

        /// <summary>
        /// Gets or sets the position in the child collection
        /// </summary>
        public int Position
            {
            get => this._position;
            set
                {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(value));
                this._position = value;
                }
            }
        }
    }
