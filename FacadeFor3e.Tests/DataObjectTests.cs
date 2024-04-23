using System;
using FacadeFor3e.ProcessCommandBuilder;
using NUnit.Framework;
using NUnit.Framework.Legacy;

// ReSharper disable ObjectCreationAsStatement
// ReSharper disable AssignNullToNotNullAttribute

namespace FacadeFor3e.Tests
    {
    [TestFixture]
    public class DataObjectTests
        {
        [Test]
        public void CanConstructValidDataObject()
            {
            var d = new DataObject("Matter");
            ClassicAssert.AreEqual("Matter", d.ObjectName);
            ClassicAssert.AreEqual(0, d.Operations.Count);

            var renderer = new TestTransactionServiceRenderer();
            renderer.Render(d);
            var s = renderer.Result;
            ClassicAssert.AreEqual("<Matter />", s);
            }

        [Test]
        public void CannotCreateInvalidDataObject()
            {
            Assert.Throws<ArgumentNullException>(() => new DataObject(null));
            Assert.Throws<ArgumentOutOfRangeException>(() => new DataObject(" invalid - name "));
            }

        [Test]
        public void CanAddOperations()
            {
            var d = new DataObject("Client");
            d.EditRecord(new IdentifyByPrimaryKey(1));
            d.AddRecord();
            d.DeleteRecord(new IdentifyByPrimaryKey(2));

            var op = new AddOperation("hello");
            d.Operations.Add(op);

            ClassicAssert.AreEqual(4, d.Operations.Count);
            }

        [Test]
        public void CannotAddNullOperation()
            {
            var d = new DataObject("Client");
            Assert.Throws<ArgumentNullException>(() => d.Operations.Add(null));
            }

        [Test]
        public void TestRender()
            {
            var d = new DataObject("Client");
            d.AddRecord();
            d.EditRecord(new IdentifyByPrimaryKey(1));
            d.DeleteRecord(new IdentifyByPrimaryKey(2));

            var renderer = new TestTransactionServiceRenderer();
            renderer.Render(d);
            var s = renderer.Result;
            ClassicAssert.AreEqual("<Client>" +
                                   "<Add><Client /></Add>" +
                                   "<Edit><Client KeyValue=\"1\" /></Edit>" +
                                   "<Delete><Client KeyValue=\"2\" /></Delete>" +
                                   "</Client>", s);
            }
        }
    }
