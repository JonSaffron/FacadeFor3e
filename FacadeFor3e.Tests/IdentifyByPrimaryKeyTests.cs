using System;
using FacadeFor3e.ProcessCommandBuilder;
using NUnit.Framework;
using NUnit.Framework.Legacy;

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
            ClassicAssert.IsInstanceOf<IntAttribute>(pk.KeyValue);
            ClassicAssert.AreEqual(10_000, pk.KeyValue.Value);
            var renderer = new TestTransactionServiceRenderer(true);
            renderer.RenderKey(pk);
            var s = renderer.Result;
            ClassicAssert.AreEqual("<Test KeyValue=\"10000\" />", s);
            }

        [Test]
        public void CanConstructForString()
            {
            var pk = new IdentifyByPrimaryKey("TIMEONLY");
            ClassicAssert.IsInstanceOf<StringAttribute>(pk.KeyValue);
            ClassicAssert.AreEqual("TIMEONLY", pk.KeyValue.Value);
            var renderer = new TestTransactionServiceRenderer(true);
            renderer.RenderKey(pk);
            var s = renderer.Result;
            ClassicAssert.AreEqual("<Test KeyValue=\"TIMEONLY\" />", s);
            }

        [Test]
        public void CanConstructForGuid()
            {
            var g = Guid.Parse("{da319fed-2491-492f-9a9d-56209acaef6f}");
            var pk = new IdentifyByPrimaryKey(g);
            ClassicAssert.IsInstanceOf<GuidAttribute>(pk.KeyValue);
            ClassicAssert.AreEqual(g, pk.KeyValue.Value);
            var renderer = new TestTransactionServiceRenderer(true);
            renderer.RenderKey(pk);
            var s = renderer.Result;
            ClassicAssert.AreEqual("<Test KeyValue=\"da319fed-2491-492f-9a9d-56209acaef6f\" />", s);
            }

        [Test]
        public void CanConstructForAttribute()
            {
            var pk = new IdentifyByPrimaryKey(new IntAttribute(25_000));
            ClassicAssert.IsInstanceOf<IntAttribute>(pk.KeyValue);
            ClassicAssert.AreEqual(25_000, pk.KeyValue.Value);
            var renderer = new TestTransactionServiceRenderer(true);
            renderer.RenderKey(pk);
            var s = renderer.Result;
            ClassicAssert.AreEqual("<Test KeyValue=\"25000\" />", s);
            }

        [Test]
        public void CannotConstructForNullValuesOrInvalidTypes()
            {
            Assert.Throws<ArgumentNullException>(() => new IdentifyByPrimaryKey((IAttribute)null));

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
