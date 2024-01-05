using System;
using System.Xml;
using FacadeFor3e.ProcessCommandBuilder;
using NUnit.Framework;
using NUnit.Framework.Legacy;

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
            ClassicAssert.IsNull(e.SubClass);
            ClassicAssert.AreEqual(0, e.Attributes.Count);
            ClassicAssert.AreEqual(0, e.Children.Count);
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
            ClassicAssert.AreEqual("EntPerson", e.SubClass);
            }

        [Test]
        public void CanAddAttributes()
            {
            var e = new EditOperation(new IdentifyByPrimaryKey(1));

            e.AddAttribute("BasicString", "StringValue");
            ClassicAssert.IsInstanceOf<StringAttribute>(e.Attributes["BasicString"].Attribute);
            ClassicAssert.AreEqual("StringValue", e.Attributes["BasicString"].Attribute.Value);

            e.AddAttribute("BasicBool", true);
            ClassicAssert.IsInstanceOf<BoolAttribute>(e.Attributes["BasicBool"].Attribute);
            ClassicAssert.AreEqual(true, e.Attributes["BasicBool"].Attribute.Value);

            e.AddAttribute("BasicInt", 527);
            ClassicAssert.IsInstanceOf<IntAttribute>(e.Attributes["BasicInt"].Attribute);
            ClassicAssert.AreEqual(527, e.Attributes["BasicInt"].Attribute.Value);

            e.AddAttribute("BasicDecimal", 887766m);
            ClassicAssert.IsInstanceOf<DecimalAttribute>(e.Attributes["BasicDecimal"].Attribute);
            ClassicAssert.AreEqual(887766m, e.Attributes["BasicDecimal"].Attribute.Value);

            var now = DateTime.Now;
#if NET6_0_OR_GREATER
            var today = DateOnly.FromDateTime(now);
            e.AddDateAttribute("BasicDate", today);
            ClassicAssert.IsInstanceOf<DateAttribute>(e.Attributes["BasicDate"].Attribute);
            ClassicAssert.AreEqual(today, e.Attributes["BasicDate"].Attribute.Value);

            e.AddAttribute("BasicDate2", today);
            ClassicAssert.IsInstanceOf<DateAttribute>(e.Attributes["BasicDate2"].Attribute);
            ClassicAssert.AreEqual(today, e.Attributes["BasicDate2"].Attribute.Value);
#else
            e.AddDateAttribute("BasicDate", now.Date);
            ClassicAssert.IsInstanceOf<DateAttribute>(e.Attributes["BasicDate"].Attribute);
            ClassicAssert.AreEqual(now.Date, e.Attributes["BasicDate"].Attribute.Value);
#endif

            e.AddDateTimeAttribute("BasicDateTime", now);
            ClassicAssert.IsInstanceOf<DateTimeAttribute>(e.Attributes["BasicDateTime"].Attribute);
            ClassicAssert.AreEqual(now, e.Attributes["BasicDateTime"].Attribute.Value);

            var g = Guid.NewGuid();
            e.AddAttribute("BasicGuid", g);
            ClassicAssert.IsInstanceOf<GuidAttribute>(e.Attributes["BasicGuid"].Attribute);
            ClassicAssert.AreEqual(g, e.Attributes["BasicGuid"].Attribute.Value);

            e.AddAliasedAttribute("AliasedAttribute", "Number", "L1234.12345");
            ClassicAssert.IsInstanceOf<AliasAttribute>(e.Attributes["AliasedAttribute"]);
            ClassicAssert.IsInstanceOf<StringAttribute>(e.Attributes["AliasedAttribute"].Attribute);
            ClassicAssert.AreEqual("Number", ((AliasAttribute) e.Attributes["AliasedAttribute"]).Alias);
            ClassicAssert.AreEqual("L1234.12345", ((AliasAttribute) e.Attributes["AliasedAttribute"]).Attribute.Value);

#if NET6_0_OR_GREATER
            ClassicAssert.AreEqual(9, e.Attributes.Count);
#else
            ClassicAssert.AreEqual(8, e.Attributes.Count);
#endif
            }

        [Test]
        public void TestRenderWithoutSubclass()
            {
            var e = new EditOperation(new IdentifyByPrimaryKey(1));
            e.AddAttribute("OrgName", "acme corp");
            e.AddChild("Site");

            Action <XmlWriter> renderEditOperation = xw => e.Render(xw, "Entity");
            var s = CommonLibrary.GetRenderedOutput(renderEditOperation);
            ClassicAssert.AreEqual("<Edit><Entity KeyValue=\"1\"><Attributes><OrgName>acme corp</OrgName></Attributes><Children><Site /></Children></Entity></Edit>", s);
            }

        [Test]
        public void TestRender()
            {
            var e = new EditOperation(new IdentifyByPrimaryKey(1), "EntOrg");
            e.AddAttribute("OrgName", "acme corp");
            e.AddChild("Site");

            Action <XmlWriter> renderEditOperation = xw => e.Render(xw, "Entity");
            var s = CommonLibrary.GetRenderedOutput(renderEditOperation);
            ClassicAssert.AreEqual("<Edit><EntOrg KeyValue=\"1\"><Attributes><OrgName>acme corp</OrgName></Attributes><Children><Site /></Children></EntOrg></Edit>", s);
            }

        [Test]
        public void StaticBuilders()
            {
            var e1 = EditOperation.ByPrimaryKey(1);
            ClassicAssert.IsInstanceOf<EditOperation>(e1);
            ClassicAssert.IsInstanceOf<IdentifyByPrimaryKey>(e1.KeySpecification);
            ClassicAssert.AreEqual(1, ((IdentifyByPrimaryKey)e1.KeySpecification).KeyValue.Value);

            var g = Guid.NewGuid();
            var e2 = EditOperation.ByPrimaryKey(g);
            ClassicAssert.IsInstanceOf<EditOperation>(e2);
            ClassicAssert.IsInstanceOf<IdentifyByPrimaryKey>(e2.KeySpecification);
            ClassicAssert.AreEqual(g, ((IdentifyByPrimaryKey)e2.KeySpecification).KeyValue.Value);

            var e3 = EditOperation.ByPrimaryKey("CL");
            ClassicAssert.IsInstanceOf<EditOperation>(e3);
            ClassicAssert.IsInstanceOf<IdentifyByPrimaryKey>(e3.KeySpecification);
            ClassicAssert.AreEqual("CL", ((IdentifyByPrimaryKey)e3.KeySpecification).KeyValue.Value);

            var e4 = EditOperation.ByUniqueAlias("Number", "M1.123");
            ClassicAssert.IsInstanceOf<EditOperation>(e4);
            ClassicAssert.IsInstanceOf<IdentifyByAlias>(e4.KeySpecification);
            ClassicAssert.AreEqual("M1.123", ((IdentifyByAlias)e4.KeySpecification).KeyValue.Value);

            var e5 = EditOperation.ByPosition(5);
            ClassicAssert.IsInstanceOf<EditOperation>(e5);
            ClassicAssert.IsInstanceOf<IdentifyByPosition>(e5.KeySpecification);
            ClassicAssert.AreEqual(5, ((IdentifyByPosition)e5.KeySpecification).Position);

            }
        }
    }
