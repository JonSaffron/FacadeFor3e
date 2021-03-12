using System;
using FacadeFor3e.ProcessCommandBuilder;
using NUnit.Framework;

// ReSharper disable ObjectCreationAsStatement
// ReSharper disable AssignNullToNotNullAttribute

namespace FacadeFor3e.Tests
    {
    [TestFixture]
    public class IdentifyByValueTests
        {
        [Test]
        public void CanConstructWithDateAttribute()
            {
            var a = new IdentifyByValue("EffStart", new DateAttribute(1980, 2, 29));
            Assert.IsInstanceOf<DateAttribute>(a.KeyValue);
            Assert.AreEqual(new DateTime(1980, 2, 29), a.KeyValue.Value);
            var s = CommonLibrary.GetRenderedOutputWithNode(a.RenderKey);
            Assert.AreEqual("<Test KeyValue=\"1980-02-29\" KeyField=\"EffStart\" />", s);
            }

        [Test]
        public void CanConstructWithBooleanAttribute()
            {
            var a = new IdentifyByValue("IsDefault", new BoolAttribute(true));
            Assert.IsInstanceOf<BoolAttribute>(a.KeyValue);
            Assert.AreEqual(true, a.KeyValue.Value);
            var s = CommonLibrary.GetRenderedOutputWithNode(a.RenderKey);
            Assert.AreEqual("<Test KeyValue=\"true\" KeyField=\"IsDefault\" />", s);
            }

        [Test]
        public void InvalidKeyField()
            {
            Assert.Throws<ArgumentNullException>(() => new IdentifyByValue(null, new StringAttribute("L12-345")));
            Assert.Throws<ArgumentOutOfRangeException>(() => new IdentifyByValue(" ", new GuidAttribute(Guid.NewGuid())));
            }

        [Test]
        public void InvalidKeyValue()
            {
            StringAttribute a = null;
            Assert.Throws<ArgumentNullException>(() => new IdentifyByValue("Number", a));
            }

        [Test]
        public void ResetKeyField()
            {
            var a = new IdentifyByValue("Code", new StringAttribute("Mark"));
            Assert.Throws<ArgumentNullException>(() => a.KeyField = null);
            Assert.Throws<ArgumentOutOfRangeException>(() => a.KeyField = "(invalid)");
            a.KeyField = "ItemId";
            }

        [Test]
        public void ResetKeyValue()
            {
            var a = new IdentifyByValue("Code", new StringAttribute("One"));
            Assert.Throws<ArgumentNullException>(() => a.KeyValue = null);
            a.KeyValue = new StringAttribute("Two");
            }
        }
    }
