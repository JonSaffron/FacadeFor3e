using System;
using System.Xml;
using FacadeFor3e.ProcessCommandBuilder;
using NUnit.Framework;

// ReSharper disable AssignNullToNotNullAttribute
// ReSharper disable ObjectCreationAsStatement

namespace FacadeFor3e.Tests
    {
    [TestFixture]
    public class EditOperationTests
        {
        [Test]
        public void CanCreateEdit()
            {
            var e = new EditOperation(new IdentifyByPrimaryKey(1));
            Assert.IsNull(e.SubClass);
            Assert.AreEqual(0, e.Attributes.Count);
            Assert.AreEqual(0, e.Children.Count);
            }

        [Test]
        public void CannotCreateEditWithoutKeySpecification()
            {
            Assert.Throws<ArgumentNullException>(() => new EditOperation(null));
            }

        [Test]
        public void ResetKeySpecification()
            {
            var e = new EditOperation(new IdentifyByPrimaryKey(1));
            Assert.Throws<ArgumentNullException>(() => e.KeySpecification = null);
            e.KeySpecification = new IdentifyByPosition(0);
            }

        [Test]
        public void CanCreateEditWithSubClass()
            {
            var e = new EditOperation(new IdentifyByPrimaryKey(1), "EntPerson");
            Assert.AreEqual("EntPerson", e.SubClass);
            }

        [Test]
        public void CanAddAttributes()
            {
            var e = new EditOperation(new IdentifyByPrimaryKey(1));

            e.AddAttribute("BasicString", "StringValue");
            Assert.IsInstanceOf<StringAttribute>(e.Attributes["BasicString"].Attribute);
            Assert.AreEqual("StringValue", e.Attributes["BasicString"].Attribute.Value);

            e.AddAttribute("BasicBool", true);
            Assert.IsInstanceOf<BoolAttribute>(e.Attributes["BasicBool"].Attribute);
            Assert.AreEqual(true, e.Attributes["BasicBool"].Attribute.Value);

            e.AddAttribute("BasicInt", 527);
            Assert.IsInstanceOf<IntAttribute>(e.Attributes["BasicInt"].Attribute);
            Assert.AreEqual(527, e.Attributes["BasicInt"].Attribute.Value);

            e.AddAttribute("BasicDecimal", 887766m);
            Assert.IsInstanceOf<DecimalAttribute>(e.Attributes["BasicDecimal"].Attribute);
            Assert.AreEqual(887766m, e.Attributes["BasicDecimal"].Attribute.Value);

            var now = DateTime.Now;
#if NET6_0_OR_GREATER
            var today = DateOnly.FromDateTime(now);
            e.AddDateAttribute("BasicDate", today);
            Assert.IsInstanceOf<DateAttribute>(e.Attributes["BasicDate"].Attribute);
            Assert.AreEqual(today, e.Attributes["BasicDate"].Attribute.Value);

            e.AddAttribute("BasicDate2", today);
            Assert.IsInstanceOf<DateAttribute>(e.Attributes["BasicDate2"].Attribute);
            Assert.AreEqual(today, e.Attributes["BasicDate2"].Attribute.Value);
#else
            e.AddDateAttribute("BasicDate", now.Date);
            Assert.IsInstanceOf<DateAttribute>(e.Attributes["BasicDate"].Attribute);
            Assert.AreEqual(now.Date, e.Attributes["BasicDate"].Attribute.Value);
#endif

            e.AddDateTimeAttribute("BasicDateTime", now);
            Assert.IsInstanceOf<DateTimeAttribute>(e.Attributes["BasicDateTime"].Attribute);
            Assert.AreEqual(now, e.Attributes["BasicDateTime"].Attribute.Value);

            var g = Guid.NewGuid();
            e.AddAttribute("BasicGuid", g);
            Assert.IsInstanceOf<GuidAttribute>(e.Attributes["BasicGuid"].Attribute);
            Assert.AreEqual(g, e.Attributes["BasicGuid"].Attribute.Value);

            e.AddAliasedAttribute("AliasedAttribute", "Number", "L1234.12345");
            Assert.IsInstanceOf<AliasAttribute>(e.Attributes["AliasedAttribute"]);
            Assert.IsInstanceOf<StringAttribute>(e.Attributes["AliasedAttribute"].Attribute);
            Assert.AreEqual("Number", ((AliasAttribute) e.Attributes["AliasedAttribute"]).Alias);
            Assert.AreEqual("L1234.12345", ((AliasAttribute) e.Attributes["AliasedAttribute"]).Attribute.Value);

            Assert.AreEqual(8, e.Attributes.Count);
            }

        [Test]
        public void TestRenderWithoutSubclass()
            {
            var e = new EditOperation(new IdentifyByPrimaryKey(1));
            e.AddAttribute("OrgName", "acme corp");
            e.AddChild("Site");

            Action <XmlWriter> renderEditOperation = xw => e.Render(xw, "Entity");
            var s = CommonLibrary.GetRenderedOutput(renderEditOperation);
            Assert.AreEqual("<Edit><Entity KeyValue=\"1\"><Attributes><OrgName>acme corp</OrgName></Attributes><Children><Site /></Children></Entity></Edit>", s);
            }

        [Test]
        public void TestRender()
            {
            var e = new EditOperation(new IdentifyByPrimaryKey(1), "EntOrg");
            e.AddAttribute("OrgName", "acme corp");
            e.AddChild("Site");

            Action <XmlWriter> renderEditOperation = xw => e.Render(xw, "Entity");
            var s = CommonLibrary.GetRenderedOutput(renderEditOperation);
            Assert.AreEqual("<Edit><EntOrg KeyValue=\"1\"><Attributes><OrgName>acme corp</OrgName></Attributes><Children><Site /></Children></EntOrg></Edit>", s);
            }

        [Test]
        public void StaticBuilders()
            {
            var e1 = EditOperation.ByPrimaryKey(1);
            Assert.IsInstanceOf<EditOperation>(e1);
            Assert.IsInstanceOf<IdentifyByPrimaryKey>(e1.KeySpecification);
            Assert.AreEqual(1, ((IdentifyByPrimaryKey)e1.KeySpecification).KeyValue.Value);

            var g = Guid.NewGuid();
            var e2 = EditOperation.ByPrimaryKey(g);
            Assert.IsInstanceOf<EditOperation>(e2);
            Assert.IsInstanceOf<IdentifyByPrimaryKey>(e2.KeySpecification);
            Assert.AreEqual(g, ((IdentifyByPrimaryKey)e2.KeySpecification).KeyValue.Value);

            var e3 = EditOperation.ByPrimaryKey("CL");
            Assert.IsInstanceOf<EditOperation>(e3);
            Assert.IsInstanceOf<IdentifyByPrimaryKey>(e3.KeySpecification);
            Assert.AreEqual("CL", ((IdentifyByPrimaryKey)e3.KeySpecification).KeyValue.Value);

            var e4 = EditOperation.ByUniqueAlias("Number", "M1.123");
            Assert.IsInstanceOf<EditOperation>(e4);
            Assert.IsInstanceOf<IdentifyByAlias>(e4.KeySpecification);
            Assert.AreEqual("M1.123", ((IdentifyByAlias)e4.KeySpecification).KeyValue.Value);

            var e5 = EditOperation.ByPosition(5);
            Assert.IsInstanceOf<EditOperation>(e5);
            Assert.IsInstanceOf<IdentifyByPosition>(e5.KeySpecification);
            Assert.AreEqual(5, ((IdentifyByPosition)e5.KeySpecification).Position);

            }
        }
    }
