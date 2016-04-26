using System;
using System.Globalization;
using System.Xml;

namespace FacadeFor3e
    {
    public class EditByPosition : OperationEdit
        {
        private readonly int _position;

        /// <summary>
        /// Constructs a Edit operation
        /// </summary>
        /// <param name="position">Zero based record number</param>
        public EditByPosition(int position)
            {
            if (position < 0)
                throw new ArgumentOutOfRangeException("position");
            this._position = position;
            }

        protected override void RenderKey(XmlWriter writer)
            {
            writer.WriteAttributeString("Position", this._position.ToString(CultureInfo.InvariantCulture));
            }
        }
    }
