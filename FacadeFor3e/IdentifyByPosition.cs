using System;
using System.Globalization;
using System.Xml;

namespace FacadeFor3e
    {
    public class IdentifyByPosition : IKeySpecification
        {
        private readonly int _position;

        /// <summary>
        /// Identifies a row by its position in the collection
        /// </summary>
        /// <param name="position">Zero based record number</param>
        public IdentifyByPosition(int position)
            {
            if (position < 0)
                throw new ArgumentOutOfRangeException(nameof(position));
            this._position = position;
            }

        public void RenderKey(XmlWriter writer)
            {
            writer.WriteAttributeString("Position", this._position.ToString(CultureInfo.InvariantCulture));
            }
        }
    }
