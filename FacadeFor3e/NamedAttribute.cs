using System;
using System.Xml;
using JetBrains.Annotations;

namespace FacadeFor3e
    {
    public abstract class NamedAttribute
        {
        [NotNull] private readonly string _name;

        public string Name => this._name;

        public NamedAttribute(string name)
            {
            this._name = name ?? throw new ArgumentNullException(nameof(name));
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentOutOfRangeException(nameof(name));
            }

        protected internal abstract void Render(XmlWriter writer);
        }

    public class NullAttribute : NamedAttribute
        {
        public NullAttribute(string name) : base(name)
            { }

        protected internal override void Render(XmlWriter writer)
            {
            writer.WriteStartElement(this.Name);
            writer.WriteEndElement();
            }
        }

    public class NamedAttributeValue : NamedAttribute
        {
        [NotNull] private readonly IAttribute _value;

        public NamedAttributeValue(string name, IAttribute value) : base(name)
            {
            this._value = value ?? throw new ArgumentNullException(nameof(value));
            }

        public IAttribute Value => this._value;

        protected internal override void Render(XmlWriter writer)
            {
            writer.WriteStartElement(this.Name);
            writer.WriteValue(this.Value.ToString());
            writer.WriteEndElement();
            }
        }

    public class AliasAttribute : NamedAttributeValue
        {
        private readonly string _alias;

        public AliasAttribute(string name, string alias, IAttribute value) : base(name, value)
            {
            this._alias = alias ?? throw new ArgumentNullException(nameof(alias));
            if (string.IsNullOrWhiteSpace(alias))
                throw new ArgumentOutOfRangeException(nameof(alias));
            }

        public string Alias => this._alias;

        protected internal override void Render(XmlWriter writer)
            {
            writer.WriteStartElement(this.Name);
            writer.WriteAttributeString("AliasField", this._alias);
            writer.WriteValue(this.Value.ToString());
            writer.WriteEndElement();
            }
        }
    }
