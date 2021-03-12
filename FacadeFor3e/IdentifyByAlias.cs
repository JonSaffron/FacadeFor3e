using System.Xml;

namespace FacadeFor3e
    {
    public class IdentifyByAlias<T> : IKeySpecification where T : IAttribute
        {
        public IdentifyByAlias(string alias, T aliasValue)
            {
            this.Alias = alias;
            this.AliasValue = aliasValue;
            }

        public string Alias;
        public T AliasValue;

        public void RenderKey(XmlWriter writer)
            {
            writer.WriteAttributeString("KeyValue", this.AliasValue.ToString());
            writer.WriteAttributeString("AliasField", this.Alias);
            }
        }
    }