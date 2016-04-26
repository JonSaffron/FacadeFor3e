using System;
using System.IO;
using System.Xml;

namespace FacadeFor3e
    {
    public class AttributeRelationshipByAlias : AttributeValue
        {
        private readonly string _aliasField;

        public AttributeRelationshipByAlias(string name, string aliasField, string value) : base(name, value)
            {
            if (string.IsNullOrWhiteSpace("aliasField"))
                throw new ArgumentException("An aliasField must be a valid attribute name.", "aliasField");
            this._aliasField = aliasField;
            }

        public AttributeRelationshipByAlias(string name, string aliasField, DateTime value) : base(name, value)
            {
            if (string.IsNullOrWhiteSpace("aliasField"))
                throw new ArgumentException("An aliasField must be a valid attribute name.", "aliasField");
            this._aliasField = aliasField;
            }

        public AttributeRelationshipByAlias(string name, string aliasField, decimal value) : base(name, value)
            {
            if (string.IsNullOrWhiteSpace("aliasField"))
                throw new ArgumentException("An aliasField must be a valid attribute name.", "aliasField");
            this._aliasField = aliasField;
            }

        public AttributeRelationshipByAlias(string name, string aliasField, int value) : base(name, value)
            {
            if (string.IsNullOrWhiteSpace("aliasField"))
                throw new ArgumentException("An aliasField must be a valid attribute name.", "aliasField");
            this._aliasField = aliasField;
            }

        /// <summary>
        /// Outputs this attribute
        /// </summary>
        /// <param name="writer">An XMLWriter to output to</param>
        protected internal override void Render(XmlWriter writer)
            {
            writer.WriteStartElement(this.Name);
            writer.WriteAttributeString("AliasField", this._aliasField);
            writer.WriteValue(this.OutputValue);
            writer.WriteEndElement();
            }

        public override string ToString()
            {
            string result;
            using (var sw = new StringWriter())
                {
                using (var xw = XmlWriter.Create(sw))
                    {
                    Render(xw);
                    }
                result = sw.ToString();
                }
            return result;
            }
        }
    }
