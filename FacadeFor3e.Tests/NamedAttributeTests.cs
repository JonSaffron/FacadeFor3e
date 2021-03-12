using System;
using FacadeFor3e.ProcessCommandBuilder;
using NUnit.Framework;

// ReSharper disable ObjectCreationAsStatement
// ReSharper disable AssignNullToNotNullAttribute

namespace FacadeFor3e.Tests
    {
    [TestFixture]
    public class NamedAttributeTests
        {
        [Test]
        public void CanCreateNamedAttribute()
            {
            var na = new NamedAttributeValue("matter", new IntAttribute(12345));
            Assert.AreEqual("matter", na.Name);
            Assert.AreEqual(12345, na.Attribute.Value);
            }

        [Test]
        public void CannotCreateInvalidNamedAttribute()
            {
            Assert.Throws<ArgumentNullException>(() => new NamedAttributeValue(null, new StringAttribute("matter")));
            Assert.Throws<ArgumentNullException>(() => new NamedAttributeValue("matter", null));
            Assert.Throws<ArgumentOutOfRangeException>(() => new NamedAttributeValue("", new IntAttribute(12345)));
            Assert.Throws<ArgumentOutOfRangeException>(() => new NamedAttributeValue(" ", new IntAttribute(12345)));
            Assert.Throws<ArgumentOutOfRangeException>(() => new NamedAttributeValue("café", new IntAttribute(12345)));
            Assert.Throws<ArgumentOutOfRangeException>(() => new NamedAttributeValue("25years", new IntAttribute(12345)));
            Assert.Throws<ArgumentOutOfRangeException>(() => new NamedAttributeValue("aging-bucket-1", new IntAttribute(12345)));
            Assert.Throws<ArgumentOutOfRangeException>(() => new NamedAttributeValue(" client", new IntAttribute(12345)));
            Assert.Throws<ArgumentOutOfRangeException>(() => new NamedAttributeValue("client ", new IntAttribute(12345)));
            Assert.Throws<ArgumentOutOfRangeException>(() => new NamedAttributeValue("client number", new IntAttribute(12345)));
            }

        [Test]
        public void CanChangeAttribute()
            {
            var na = new NamedAttributeValue("matter", new IntAttribute(1234));
            ((IntAttribute)na.Attribute).Value = 10;
            Assert.AreEqual(10, na.Attribute.Value);
            }

        [Test]
        public void RenderAttribute()
            {
            var na = new NamedAttributeValue("Matter", new IntAttribute(12345));
            var s = CommonLibrary.GetRenderedOutput(na.Render);
            Assert.AreEqual("<Matter>12345</Matter>", s);
            }

        [Test]
        public void RenderAliasAttribute()
            {
            var na = new AliasAttribute("Matter", "Number", new StringAttribute("L1.10"));
            var s = CommonLibrary.GetRenderedOutput(na.Render);
            Assert.AreEqual("<Matter AliasField=\"Number\">L1.10</Matter>", s);
            }
        }
    }
