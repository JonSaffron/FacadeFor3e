using System.Xml;

namespace FacadeFor3e
    {
    public class IdentifyByValue<T> : IKeySpecification where T : IAttribute
        {
        public IdentifyByValue(string attribute, T attributeValue)
            {
            this.Attribute = attribute;
            this.AttributeValue = attributeValue;
            }

        public string Attribute;
        public T AttributeValue;

        public void RenderKey(XmlWriter writer)
            {
            writer.WriteAttributeString("KeyValue", this.AttributeValue.ToString());
            writer.WriteAttributeString("KeyField", this.Attribute);
            }
        }
    }