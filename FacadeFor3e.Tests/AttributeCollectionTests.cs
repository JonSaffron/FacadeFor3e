using System;
using System.Collections.Generic;
using FacadeFor3e.ProcessCommandBuilder;
using NUnit.Framework;
using NUnit.Framework.Legacy;

// ReSharper disable CollectionNeverQueried.Local
// ReSharper disable AssignNullToNotNullAttribute
// ReSharper disable ReturnValueOfPureMethodIsNotUsed

namespace FacadeFor3e.Tests
    {
    [TestFixture]
    public class AttributeCollectionTests
        {
        [Test]
        public void CanAddAttributes()
            {
            var b = new NamedAttributeValue("boolean", new BoolAttribute(true));
            var i = new NamedAttributeValue("integer", new IntAttribute(25));
            var g = new NamedAttributeValue("guid", new GuidAttribute(Guid.NewGuid()));

            var coll = new AttributeCollection
                {
                b, i, g
                };

            ClassicAssert.IsTrue(coll.Contains(b));
            ClassicAssert.IsTrue(coll.IndexOf(i) == 1);
            ClassicAssert.IsTrue(coll[2] == g);
            ClassicAssert.AreEqual(3, coll.Count);
            }

        [Test]
        public void CannotAddDuplicateAttribute()
            {
            var b = new NamedAttributeValue("boolean", new BoolAttribute(true));
            var i = new NamedAttributeValue("integer", new IntAttribute(25));
            var g = new NamedAttributeValue("guid", new GuidAttribute(Guid.NewGuid()));

            var coll = new AttributeCollection
                {
                b, i, g
                };

            Assert.Throws<ArgumentOutOfRangeException>(() => coll.Add(b));
            var dup1 = new NamedAttributeValue("boolean", new BoolAttribute(false));
            Assert.Throws<ArgumentOutOfRangeException>(() => coll.Add(dup1));
            var dup2 = new NamedAttributeValue("guid", new StringAttribute("duplicate"));
            Assert.Throws<ArgumentOutOfRangeException>(() => coll.Add(dup2));
            }

        [Test]
        public void CannotAddNullAttribute()
            {
            var b = new NamedAttributeValue("boolean", new BoolAttribute(true));
            var i = new NamedAttributeValue("integer", new IntAttribute(25));
            var g = new NamedAttributeValue("guid", new GuidAttribute(Guid.NewGuid()));

            var coll = new AttributeCollection
                {
                b, i, g
                };

            Assert.Throws<ArgumentNullException>(() => coll.Add(null));
            }

        [Test]
        public void CanRemoveAttributes()
            {
            var b = new NamedAttributeValue("boolean", new BoolAttribute(true));
            var i = new NamedAttributeValue("integer", new IntAttribute(25));
            var g = new NamedAttributeValue("guid", new GuidAttribute(Guid.NewGuid()));

            var coll = new AttributeCollection
                {
                b, i, g
                };

            coll.Remove(b);
            ClassicAssert.IsFalse(coll.Contains(b));
            coll.RemoveAt(0);
            ClassicAssert.IsFalse(coll.Contains(i));
            coll.Clear();
            ClassicAssert.IsFalse(coll.Contains(g));
            }

        [Test]
        public void CanRetrieveAttributeThatExists()
            {
            var b = new NamedAttributeValue("boolean", new BoolAttribute(true));
            var i = new NamedAttributeValue("integer", new IntAttribute(25));
            var g = new NamedAttributeValue("guid", new GuidAttribute(Guid.NewGuid()));

            var coll = new AttributeCollection
                {
                b, i, g
                };

            ClassicAssert.IsNotNull(coll["boolean"]);
            ClassicAssert.IsNotNull(coll["integer"]);
            ClassicAssert.IsNotNull(coll["guid"]);
            }

        [Test]
        public void RetrieveAttributeThatDoesNotExistsRaisesException()
            {
            var b = new NamedAttributeValue("boolean", new BoolAttribute(true));
            var i = new NamedAttributeValue("integer", new IntAttribute(25));
            var g = new NamedAttributeValue("guid", new GuidAttribute(Guid.NewGuid()));

            var coll = new AttributeCollection
                {
                b, i, g
                };

            Assert.Throws<KeyNotFoundException>(() => coll["unknown boolean"].ToString());
            Assert.Throws<KeyNotFoundException>(() => coll["unknown integer"].ToString());
            Assert.Throws<KeyNotFoundException>(() => coll["unknown guid"].ToString());

            Assert.Throws<ArgumentOutOfRangeException>(() => coll[10].ToString());
            }

        [Test]
        public void TryingToRetrieveAttributeReturnsCorrectInformation()
            {
            var b = new NamedAttributeValue("boolean", new BoolAttribute(true));
            var i = new NamedAttributeValue("integer", new IntAttribute(25));
            var g = new NamedAttributeValue("guid", new GuidAttribute(Guid.NewGuid()));

            var coll = new AttributeCollection
                {
                b, i, g
                };

            ClassicAssert.IsFalse(coll.TryGetValue("unknown boolean", out NamedAttributeValue outValue));
            ClassicAssert.IsNull(outValue);

            ClassicAssert.IsTrue(coll.TryGetValue("integer", out outValue));
            ClassicAssert.AreEqual(i, outValue);
            }

        [Test]
        public void SetAnItem()
            {
            var b = new NamedAttributeValue("boolean", new BoolAttribute(true));
            var i = new NamedAttributeValue("integer", new IntAttribute(25));
            var g = new NamedAttributeValue("guid", new GuidAttribute(Guid.NewGuid()));

            var coll = new AttributeCollection
                {
                b, i, g
                };

            Assert.Throws<ArgumentNullException>(() => coll[0] = null);
            Assert.Throws<ArgumentOutOfRangeException>(() => coll[0] = g);
#if NET6_0_OR_GREATER
            var today = DateOnly.FromDateTime(DateTime.Today);
#else
            var today = DateTime.Today;
#endif
            var n = new NamedAttributeValue("date", new DateAttribute(today));
            coll[0] = n;
            ClassicAssert.AreEqual(n, coll[0]);
            }

        [Test]
        public void GetErrors()
            {
            var b = new NamedAttributeValue("boolean", new BoolAttribute(true));
            var i = new NamedAttributeValue("integer", new IntAttribute(25));
            var g = new NamedAttributeValue("guid", new GuidAttribute(Guid.NewGuid()));

            var coll = new AttributeCollection
                {
                b, i, g
                };

            Assert.Throws<ArgumentNullException>(() => coll[null].ToString());
            Assert.Throws<KeyNotFoundException>(() => coll["xxx"].ToString());
            Assert.Throws<ArgumentNullException>(() => coll.TryGetValue(null, out _));
            }

        [Test]
        public void CanChangeAttributeValue()
            {
            var b = new NamedAttributeValue("boolean", new BoolAttribute(true));
            var i = new NamedAttributeValue("integer", new IntAttribute(25));
            var g = new NamedAttributeValue("guid", new GuidAttribute(Guid.NewGuid()));

            var coll = new AttributeCollection
                {
                b, i, g
                };

            var n = new NamedAttributeValue("integer", new IntAttribute(42));
            coll[1] = n;
            Assert.Throws<ArgumentOutOfRangeException>(() => coll[0] = n);

            coll.Remove("integer");
            coll.Add(new NamedAttributeValue("integer", new IntAttribute(-1)));

            ClassicAssert.AreEqual(3, coll.Count);
            }
        }
    }
