using System;
using System.Collections.Generic;
using NUnit.Framework;
using FacadeFor3e.ProcessCommandBuilder;

// ReSharper disable ReturnValueOfPureMethodIsNotUsed
// ReSharper disable AssignNullToNotNullAttribute
// ReSharper disable CollectionNeverQueried.Local

namespace FacadeFor3e.Tests
    {
    [TestFixture]
    public class ChildCollectionTests
        {
        [Test]
        public void CanAddAttributes()
            {
            var t = new DataObject("ProfDetailTime");
            var d = new DataObject("ProfDetailCost");
            var c = new DataObject("ProfDetailChrg");

            var coll = new ChildObjectCollection
                {
                t, d, c
                };

            Assert.IsTrue(coll.Contains(t));
            Assert.IsTrue(coll.IndexOf(d) == 1);
            Assert.IsTrue(coll[2] == c);
            Assert.AreEqual(3, coll.Count);
            }

        [Test]
        public void CannotAddDuplicateAttribute()
            {
            var t = new DataObject("ProfDetailTime");
            var d = new DataObject("ProfDetailCost");
            var c = new DataObject("ProfDetailChrg");

            var coll = new ChildObjectCollection
                {
                t, d, c
                };

            Assert.Throws<ArgumentOutOfRangeException>(() => coll.Add(t));
            var dup1 = new DataObject("ProfDetailCost");
            Assert.Throws<ArgumentOutOfRangeException>(() => coll.Add(dup1));
            var dup2 = new DataObject("ProfDetailChrg");
            Assert.Throws<ArgumentOutOfRangeException>(() => coll.Add(dup2));
            }

        [Test]
        public void CannotAddNullAttribute()
            {
            var t = new DataObject("ProfDetailTime");
            var d = new DataObject("ProfDetailCost");
            var c = new DataObject("ProfDetailChrg");

            var coll = new ChildObjectCollection
                {
                t, d, c
                };

            Assert.Throws<ArgumentNullException>(() => coll.Add(null));
            }

        [Test]
        public void CanRemoveAttributes()
            {
            var t = new DataObject("ProfDetailTime");
            var d = new DataObject("ProfDetailCost");
            var c = new DataObject("ProfDetailChrg");

            var coll = new ChildObjectCollection
                {
                t, d, c
                };

            coll.Remove(t);
            Assert.IsFalse(coll.Contains(t));
            coll.RemoveAt(0);
            Assert.IsFalse(coll.Contains(d));
            coll.Clear();
            Assert.IsFalse(coll.Contains(c));
            }

        [Test]
        public void CanRetrieveAttributeThatExists()
            {
            var t = new DataObject("ProfDetailTime");
            var d = new DataObject("ProfDetailCost");
            var c = new DataObject("ProfDetailChrg");

            var coll = new ChildObjectCollection
                {
                t, d, c
                };

            Assert.IsNotNull(coll["ProfDetailTime"]);
            Assert.IsNotNull(coll["ProfDetailCost"]);
            Assert.IsNotNull(coll["ProfDetailChrg"]);
            }

        [Test]
        public void RetrieveAttributeThatDoesNotExistsRaisesException()
            {
            var t = new DataObject("ProfDetailTime");
            var d = new DataObject("ProfDetailCost");
            var c = new DataObject("ProfDetailChrg");

            var coll = new ChildObjectCollection
                {
                t, d, c
                };

            Assert.Throws<KeyNotFoundException>(() => coll["unknown ProfDetailTime"].ToString());
            Assert.Throws<KeyNotFoundException>(() => coll["unknown ProfDetailCost"].ToString());
            Assert.Throws<KeyNotFoundException>(() => coll["unknown ProfDetailChrg"].ToString());

            Assert.Throws<ArgumentOutOfRangeException>(() => coll[10].ToString());
            }

        [Test]
        public void TryingToRetrieveAttributeReturnsCorrectInformation()
            {
            var t = new DataObject("ProfDetailTime");
            var d = new DataObject("ProfDetailCost");
            var c = new DataObject("ProfDetailChrg");

            var coll = new ChildObjectCollection
                {
                t, d, c
                };

            Assert.IsFalse(coll.TryGetValue("whatever", out DataObject outValue));
            Assert.IsNull(outValue);

            Assert.IsTrue(coll.TryGetValue("ProfDetailTime", out outValue));
            Assert.AreEqual(t, outValue);
            }

        [Test]
        public void SetAnItem()
            {
            var t = new DataObject("ProfDetailTime");
            var d = new DataObject("ProfDetailCost");
            var c = new DataObject("ProfDetailChrg");

            var coll = new ChildObjectCollection
                {
                t, d, c
                };

            Assert.Throws<ArgumentNullException>(() => coll[0] = null);
            Assert.Throws<ArgumentOutOfRangeException>(() => coll[0] = c);
            var n = new DataObject("ProfAdjust");
            coll[0] = n;
            Assert.AreEqual(n, coll[0]);
            }

        [Test]
        public void GetErrors()
            {
            var t = new DataObject("ProfDetailTime");
            var d = new DataObject("ProfDetailCost");
            var c = new DataObject("ProfDetailChrg");

            var coll = new ChildObjectCollection
                {
                t, d, c
                };

            Assert.Throws<ArgumentNullException>(() => coll[null].ToString());
            Assert.Throws<KeyNotFoundException>(() => coll["xxx"].ToString());
            Assert.Throws<ArgumentNullException>(() => coll.TryGetValue(null, out _));
            }
        }
    }
