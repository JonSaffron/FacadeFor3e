using System;
using System.Xml;
using FacadeFor3e.ProcessCommandBuilder;
using NUnit.Framework;

namespace FacadeFor3e.Tests
    {
    [TestFixture]
    public class AddOperationTests
        {
        [Test]
        public void CanCreateAdd()
            {
            var a = new AddOperation();
            Assert.IsNull(a.SubClass);
            Assert.AreEqual(0, a.Attributes.Count);
            Assert.AreEqual(0, a.Children.Count);
            }

        [Test]
        public void CanCreateAddWithSubClass()
            {
            var a = new AddOperation("EntPerson");
            Assert.AreEqual("EntPerson", a.SubClass);
            }

        [Test]
        public void ChangeSubClass()
            {
            var a = new AddOperation();
            Assert.IsNull(a.SubClass);
            Assert.Throws<ArgumentOutOfRangeException>(() => a.SubClass = " invalid ");
            a.SubClass = "EntOrg";
            Assert.AreEqual("EntOrg", a.SubClass);
            a.SubClass = null;
            Assert.IsNull(a.SubClass);
            }

        [Test]
        public void CanAddAttributes()
            {
            var a = new AddOperation();

            a.AddAttribute("BasicString", "StringValue");
            Assert.IsInstanceOf<StringAttribute>(a.Attributes["BasicString"].Attribute);
            Assert.AreEqual("StringValue", a.Attributes["BasicString"].Attribute.Value);

            a.AddAttribute("BasicBool", true);
            Assert.IsInstanceOf<BoolAttribute>(a.Attributes["BasicBool"].Attribute);
            Assert.AreEqual(true, a.Attributes["BasicBool"].Attribute.Value);

            a.AddAttribute("BasicInt", 527);
            Assert.IsInstanceOf<IntAttribute>(a.Attributes["BasicInt"].Attribute);
            Assert.AreEqual(527, a.Attributes["BasicInt"].Attribute.Value);

            a.AddAttribute("BasicDecimal", 887766m);
            Assert.IsInstanceOf<DecimalAttribute>(a.Attributes["BasicDecimal"].Attribute);
            Assert.AreEqual(887766m, a.Attributes["BasicDecimal"].Attribute.Value);

            var now = DateTime.Now;
            a.AddDateAttribute("BasicDate", now.Date);
            Assert.IsInstanceOf<DateAttribute>(a.Attributes["BasicDate"].Attribute);
            Assert.AreEqual(now.Date, a.Attributes["BasicDate"].Attribute.Value);

            a.AddDateTimeAttribute("BasicDateTime", now);
            Assert.IsInstanceOf<DateTimeAttribute>(a.Attributes["BasicDateTime"].Attribute);
            Assert.AreEqual(now, a.Attributes["BasicDateTime"].Attribute.Value);

            var g = Guid.NewGuid();
            a.AddAttribute("BasicGuid", g);
            Assert.IsInstanceOf<GuidAttribute>(a.Attributes["BasicGuid"].Attribute);
            Assert.AreEqual(g, a.Attributes["BasicGuid"].Attribute.Value);

            a.AddAliasedAttribute("AliasedAttribute", "Number", "L1234.12345");
            Assert.IsInstanceOf<AliasAttribute>(a.Attributes["AliasedAttribute"]);
            Assert.IsInstanceOf<StringAttribute>(a.Attributes["AliasedAttribute"].Attribute);
            Assert.AreEqual("Number", ((AliasAttribute) a.Attributes["AliasedAttribute"]).Alias);
            Assert.AreEqual("L1234.12345", ((AliasAttribute) a.Attributes["AliasedAttribute"]).Attribute.Value);

            Assert.AreEqual(8, a.Attributes.Count);
            }

        [Test]
        public void TestRenderWithoutSubclass()
            {
            var a = new AddOperation();
            a.AddAttribute("OrgName", "acme corp");
            a.AddChild("Site");

            Action <XmlWriter> renderAddOperation = xw => a.Render(xw, "Entity");
            var s = CommonLibrary.GetRenderedOutput(renderAddOperation);
            Assert.AreEqual("<Add><Entity><Attributes><OrgName>acme corp</OrgName></Attributes><Children><Site /></Children></Entity></Add>", s);
            }

        [Test]
        public void TestRenderWithSubclass()
            {
            var a = new AddOperation("EntOrg");
            a.AddAttribute("OrgName", "acme corp");
            a.AddChild("Site");

            Action <XmlWriter> renderAddOperation = xw => a.Render(xw, "Entity");
            var s = CommonLibrary.GetRenderedOutput(renderAddOperation);
            Assert.AreEqual("<Add><EntOrg><Attributes><OrgName>acme corp</OrgName></Attributes><Children><Site /></Children></EntOrg></Add>", s);
            }
        }
    }
