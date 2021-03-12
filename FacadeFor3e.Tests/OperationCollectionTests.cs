using System;
using FacadeFor3e.ProcessCommandBuilder;
using NUnit.Framework;

// ReSharper disable CollectionNeverQueried.Local

namespace FacadeFor3e.Tests
    {
    [TestFixture]
    public class OperationCollectionTests
        {
        [Test]
        public void CanAddOperations()
            {
            var a = new AddOperation("EntPerson");
            var e = EditOperation.ByPosition(0);
            var d = DeleteOperation.ByPrimaryKey(12345);

            var coll = new OperationCollection
                {
                a, e, d
                };

            Assert.IsTrue(coll.Contains(a));
            Assert.IsTrue(coll.IndexOf(e) == 1);
            Assert.IsTrue(coll[2] == d);
            Assert.AreEqual(3, coll.Count);
            }

        [Test]
        public void CannotAddDuplicateOperation()
            {
            var a = new AddOperation("EntPerson");
            var e = EditOperation.ByPosition(0);
            var d = DeleteOperation.ByPrimaryKey(12345);

            var coll = new OperationCollection
                {
                a, e, d
                };

            Assert.Throws<ArgumentOutOfRangeException>(() => coll.Add(a));
            var dup1 = e;
            Assert.Throws<ArgumentOutOfRangeException>(() => coll.Add(dup1));
            var dup2 = d;
            Assert.Throws<ArgumentOutOfRangeException>(() => coll.Add(dup2));
            }

        [Test]
        public void CannotAddNullOperation()
            {
            var a = new AddOperation("EntPerson");
            var e = EditOperation.ByPosition(0);
            var d = DeleteOperation.ByPrimaryKey(12345);

            var coll = new OperationCollection
                {
                a, e, d
                };

            Assert.Throws<ArgumentNullException>(() => coll.Add(null));
            }

        [Test]
        public void CanRemoveOperations()
            {
            var a = new AddOperation("EntPerson");
            var e = EditOperation.ByPosition(0);
            var d = DeleteOperation.ByPrimaryKey(12345);

            var coll = new OperationCollection
                {
                a, e, d
                };

            coll.Remove(a);
            Assert.IsFalse(coll.Contains(a));
            coll.RemoveAt(0);
            Assert.IsFalse(coll.Contains(e));
            coll.Clear();
            Assert.IsFalse(coll.Contains(d));
            }

        [Test]
        public void SetAnItem()
            {
            var a = new AddOperation("EntPerson");
            var e = EditOperation.ByPosition(0);
            var d = DeleteOperation.ByPrimaryKey(12345);

            var coll = new OperationCollection
                {
                a, e, d
                };

            Assert.Throws<ArgumentNullException>(() => coll[0] = null);
            Assert.Throws<ArgumentOutOfRangeException>(() => coll[0] = d);
            var n = new AddOperation();
            coll[0] = n;
            Assert.AreEqual(n, coll[0]);

            coll[1] = e; // does not throw (is a nothing operation)
            }

        }
    }
