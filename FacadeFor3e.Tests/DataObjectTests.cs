using System;
using FacadeFor3e.ProcessCommandBuilder;
using NUnit.Framework;

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
            Assert.AreEqual("Matter", d.ObjectName);
            Assert.AreEqual(0, d.Operations.Count);

            var s = CommonLibrary.GetRenderedOutput(d.Render);
            Assert.AreEqual("<Matter />", s);
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

            Assert.AreEqual(4, d.Operations.Count);
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

            var s = CommonLibrary.GetRenderedOutput(d.Render);
            Assert.AreEqual("<Client>" +
                                        "<Add><Client /></Add>" +
                                        "<Edit><Client KeyValue=\"1\" /></Edit>" +
                                        "<Delete><Client KeyValue=\"2\" /></Delete>" +
                                    "</Client>", s);
            }
        }
    }
