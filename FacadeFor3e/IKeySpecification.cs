using System.Xml;

namespace FacadeFor3e
    {
    public interface IKeySpecification
        {
        void RenderKey(XmlWriter writer);
        }
    }