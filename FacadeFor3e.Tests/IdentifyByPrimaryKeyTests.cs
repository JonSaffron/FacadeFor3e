using System;
using FacadeFor3e.ProcessCommandBuilder;
using NUnit.Framework;

// ReSharper disable ObjectCreationAsStatement
// ReSharper disable AssignNullToNotNullAttribute

namespace FacadeFor3e.Tests
    {
    [TestFixture]
    public class IdentifyByPrimaryKeyTests
        {
        [Test]
        public void CanConstructForInt()
            {
            var pk = new IdentifyByPrimaryKey(10_000);
            Assert.IsInstanceOf<IntAttribute>(pk.KeyValue);
            Assert.AreEqual(10_000, pk.KeyValue.Value);
            var s = CommonLibrary.GetRenderedOutputWithNode(pk.RenderKey);
            Assert.AreEqual("<Test KeyValue=\"10000\" />", s);
            }

        [Test]
        public void CanConstructForString()
            {
            var pk = new IdentifyByPrimaryKey("TIMEONLY");
            Assert.IsInstanceOf<StringAttribute>(pk.KeyValue);
            Assert.AreEqual("TIMEONLY", pk.KeyValue.Value);
            var s = CommonLibrary.GetRenderedOutputWithNode(pk.RenderKey);
            Assert.AreEqual("<Test KeyValue=\"TIMEONLY\" />", s);
            }

        [Test]
        public void CanConstructForGuid()
            {
            var g = Guid.Parse("{da319fed-2491-492f-9a9d-56209acaef6f}");
            var pk = new IdentifyByPrimaryKey(g);
            Assert.IsInstanceOf<GuidAttribute>(pk.KeyValue);
            Assert.AreEqual(g, pk.KeyValue.Value);
            var s = CommonLibrary.GetRenderedOutputWithNode(pk.RenderKey);
            Assert.AreEqual("<Test KeyValue=\"da319fed-2491-492f-9a9d-56209acaef6f\" />", s);
            }

        [Test]
        public void CanConstructForAttribute()
            {
            var pk = new IdentifyByPrimaryKey(new IntAttribute(25_000));
            Assert.IsInstanceOf<IntAttribute>(pk.KeyValue);
            Assert.AreEqual(25_000, pk.KeyValue.Value);
            var s = CommonLibrary.GetRenderedOutputWithNode(pk.RenderKey);
            Assert.AreEqual("<Test KeyValue=\"25000\" />", s);
            }

        [Test]
        public void CannotConstructForNullValuesOrInvalidTypes()
            {
            IntAttribute att = null;
            Assert.Throws<ArgumentNullException>(() => new IdentifyByPrimaryKey(att));

            var i = new IntAttribute(null);
            Assert.Throws<ArgumentOutOfRangeException>(() => new IdentifyByPrimaryKey(i));

            var d = new DecimalAttribute(24000);
            Assert.Throws<ArgumentOutOfRangeException>(() => new IdentifyByPrimaryKey(d));

            string s = null;
            Assert.Throws<ArgumentNullException>(() => new IdentifyByPrimaryKey(s));
            }

        [Test]
        public void CannotConstructForOtherTypes()
            {
            Assert.Throws<ArgumentOutOfRangeException>(() => new IdentifyByPrimaryKey(new BoolAttribute(true)));
            Assert.Throws<ArgumentOutOfRangeException>(() => new IdentifyByPrimaryKey(new DecimalAttribute(1)));
            }

        [Test]
        public void CannotSetToNull()
            {
            var i = new IdentifyByPrimaryKey(1);
            StringAttribute s = null;
            Assert.Throws<ArgumentNullException>(() => i.KeyValue = s);
            }

        [Test]
        public void CanReset()
            {
            var i = new IdentifyByPrimaryKey(1);
            i.KeyValue = new IntAttribute(5);
            }
        }
    }
