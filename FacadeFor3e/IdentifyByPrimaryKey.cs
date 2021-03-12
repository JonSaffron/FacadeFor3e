using System.Xml;

namespace FacadeFor3e
    {
    public class IdentifyByPrimaryKey<T> : IKeySpecification where T: IAttribute
        {
        /// <summary>
        /// Identifies a row by its primary key
        /// </summary>
        /// <param name="keyValue">Primary key value</param>
        public IdentifyByPrimaryKey(T keyValue)
            {
            this.KeyValue = keyValue;
            }

        /// <summary>
        /// Gets the value to search for
        /// </summary>
        public T KeyValue;

        public void RenderKey(XmlWriter writer)
            {
            writer.WriteAttributeString("KeyValue", this.KeyValue.ToString());
            }
        }
    }
