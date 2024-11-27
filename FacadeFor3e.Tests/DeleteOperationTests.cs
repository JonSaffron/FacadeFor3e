using System;
using FacadeFor3e.ProcessCommandBuilder;
using NUnit.Framework;
using NUnit.Framework.Legacy;

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
            ClassicAssert.IsNull(d.SubClass);
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
            ClassicAssert.AreEqual("EntPerson", d.SubClass);
            }

        [Test]
        public void TestRenderWithoutSubclass()
            {
            var d = new DeleteOperation(new IdentifyByPrimaryKey(1));

            var renderer = new TestTransactionServiceRenderer();
            renderer.Render(d, "Entity");
            var s = renderer.Result;
            ClassicAssert.AreEqual("<Delete><Entity KeyValue=\"1\" /></Delete>", s);
            }

        [Test]
        public void TestRender()
            {
            var d = new DeleteOperation(new IdentifyByPrimaryKey(1), "EntOrg");

            var renderer = new TestTransactionServiceRenderer();
            renderer.Render(d, "Entity");
            var s = renderer.Result;
            ClassicAssert.AreEqual("<Delete><EntOrg KeyValue=\"1\" /></Delete>", s);
            }

        [Test]
        public void StaticBuilders()
            {
            var d1 = DeleteOperation.ByPrimaryKey(1);
            ClassicAssert.IsInstanceOf<DeleteOperation>(d1);
            ClassicAssert.IsInstanceOf<IdentifyByPrimaryKey>(d1.KeySpecification);
            ClassicAssert.AreEqual(1, ((IdentifyByPrimaryKey)d1.KeySpecification).KeyValue.Value);

            var g = Guid.NewGuid();
            var d2 = DeleteOperation.ByPrimaryKey(g);
            ClassicAssert.IsInstanceOf<DeleteOperation>(d2);
            ClassicAssert.IsInstanceOf<IdentifyByPrimaryKey>(d2.KeySpecification);
            ClassicAssert.AreEqual(g, ((IdentifyByPrimaryKey)d2.KeySpecification).KeyValue.Value);

            var d3 = DeleteOperation.ByPrimaryKey("CL");
            ClassicAssert.IsInstanceOf<DeleteOperation>(d3);
            ClassicAssert.IsInstanceOf<IdentifyByPrimaryKey>(d3.KeySpecification);
            ClassicAssert.AreEqual("CL", ((IdentifyByPrimaryKey)d3.KeySpecification).KeyValue.Value);

            var d4 = DeleteOperation.ByUniqueAlias("Number", "M1.123");
            ClassicAssert.IsInstanceOf<DeleteOperation>(d4);
            ClassicAssert.IsInstanceOf<IdentifyByAlias>(d4.KeySpecification);
            ClassicAssert.AreEqual("M1.123", ((IdentifyByAlias)d4.KeySpecification).KeyValue.Value);
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
