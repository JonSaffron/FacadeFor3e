using System;
using System.Xml;
using FacadeFor3e.ProcessCommandBuilder;
using NUnit.Framework;

// ReSharper disable AssignNullToNotNullAttribute
// ReSharper disable ObjectCreationAsStatement

namespace FacadeFor3e.Tests
    {
    [TestFixture]
    public class DeleteOperationTests
        {
        [Test]
        public void CanCreateDelete()
            {
            var d = new DeleteOperation(new IdentifyByPrimaryKey(1));
            Assert.IsNull(d.SubClass);
            }

        [Test]
        public void CannotCreateDeleteWithoutKeySpecification()
            {
            Assert.Throws<ArgumentNullException>(() => new DeleteOperation(null));
            }

        [Test]
        public void CanCreateDeleteWithSubClass()
            {
            var d = new DeleteOperation(new IdentifyByPrimaryKey(1), "EntPerson");
            Assert.AreEqual("EntPerson", d.SubClass);
            }

        [Test]
        public void TestRenderWithoutSubclass()
            {
            var d = new DeleteOperation(new IdentifyByPrimaryKey(1));

            Action <XmlWriter> renderDeleteOperation = xw => d.Render(xw, "Entity");
            var s = CommonLibrary.GetRenderedOutput(renderDeleteOperation);
            Assert.AreEqual("<Delete><Entity KeyValue=\"1\" /></Delete>", s);
            }

        [Test]
        public void TestRender()
            {
            var d = new DeleteOperation(new IdentifyByPrimaryKey(1), "EntOrg");

            Action <XmlWriter> renderDeleteOperation = xw => d.Render(xw, "Entity");
            var s = CommonLibrary.GetRenderedOutput(renderDeleteOperation);
            Assert.AreEqual("<Delete><EntOrg KeyValue=\"1\" /></Delete>", s);
            }

        [Test]
        public void StaticBuilders()
            {
            var d1 = DeleteOperation.ByPrimaryKey(1);
            Assert.IsInstanceOf<DeleteOperation>(d1);
            Assert.IsInstanceOf<IdentifyByPrimaryKey>(d1.KeySpecification);
            Assert.AreEqual(1, ((IdentifyByPrimaryKey)d1.KeySpecification).KeyValue.Value);

            var g = Guid.NewGuid();
            var d2 = DeleteOperation.ByPrimaryKey(g);
            Assert.IsInstanceOf<DeleteOperation>(d2);
            Assert.IsInstanceOf<IdentifyByPrimaryKey>(d2.KeySpecification);
            Assert.AreEqual(g, ((IdentifyByPrimaryKey)d2.KeySpecification).KeyValue.Value);

            var d3 = DeleteOperation.ByPrimaryKey("CL");
            Assert.IsInstanceOf<DeleteOperation>(d3);
            Assert.IsInstanceOf<IdentifyByPrimaryKey>(d3.KeySpecification);
            Assert.AreEqual("CL", ((IdentifyByPrimaryKey)d3.KeySpecification).KeyValue.Value);

            var d4 = DeleteOperation.ByUniqueAlias("Number", "M1.123");
            Assert.IsInstanceOf<DeleteOperation>(d4);
            Assert.IsInstanceOf<IdentifyByAlias>(d4.KeySpecification);
            Assert.AreEqual("M1.123", ((IdentifyByAlias)d4.KeySpecification).KeyValue.Value);
            }

        [Test]
        public void CanReset()
            {
            var d1 = DeleteOperation.ByPrimaryKey("CODE");
            Assert.Throws<ArgumentNullException>(() => d1.KeySpecification = null);
            d1.KeySpecification = new IdentifyByPrimaryKey(1);
            }
        }
    }
