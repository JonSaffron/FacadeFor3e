using System;
using System.Collections.Generic;
using FacadeFor3e.ProcessCommandBuilder;
using NUnit.Framework;

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

            Assert.IsTrue(coll.Contains(b));
            Assert.IsTrue(coll.IndexOf(i) == 1);
            Assert.IsTrue(coll[2] == g);
            Assert.AreEqual(3, coll.Count);
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
            Assert.IsFalse(coll.Contains(b));
            coll.RemoveAt(0);
            Assert.IsFalse(coll.Contains(i));
            coll.Clear();
            Assert.IsFalse(coll.Contains(g));
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

            Assert.IsNotNull(coll["boolean"]);
            Assert.IsNotNull(coll["integer"]);
            Assert.IsNotNull(coll["guid"]);
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

            Assert.IsFalse(coll.TryGetValue("unknown boolean", out NamedAttributeValue outValue));
            Assert.IsNull(outValue);

            Assert.IsTrue(coll.TryGetValue("integer", out outValue));
            Assert.AreEqual(i, outValue);
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
            var n = new NamedAttributeValue("date", new DateAttribute(DateTime.Today));
            coll[0] = n;
            Assert.AreEqual(n, coll[0]);
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
        }
    }
