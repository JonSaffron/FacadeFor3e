using System;
using System.Globalization;
using System.Xml;

namespace FacadeFor3e
    {
    public class DeleteByPosition : OperationDelete
        {
        private readonly int _position;

        /// <summary>
        /// Constructs a Delete operation
        /// </summary>
        /// <param name="position">Zero based record number</param>
        public DeleteByPosition(int position)
            {
            if (position < 0)
                throw new ArgumentOutOfRangeException(nameof(position));
            this._position = position;
            }

        protected override void RenderKey(XmlWriter writer)
            {
            writer.WriteAttributeString("Position", this._position.ToString(CultureInfo.InvariantCulture));
            }
        }
    }
