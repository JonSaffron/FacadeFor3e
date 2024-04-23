using System;
using System.Xml;
using FacadeFor3e.ProcessCommandBuilder;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace FacadeFor3e.Tests
    {
    [TestFixture]
    public class AddOperationTests
        {
        [Test]
        public void CanCreateAdd()
            {
            var a = new AddOperation();
            ClassicAssert.IsNull(a.SubClass);
            ClassicAssert.AreEqual(0, a.Attributes.Count);
            ClassicAssert.AreEqual(0, a.Children.Count);
            }

        [Test]
        public void CanCreateAddWithSubClass()
            {
            var a = new AddOperation("EntPerson");
            ClassicAssert.AreEqual("EntPerson", a.SubClass);
            }

        [Test]
        public void ChangeSubClass()
            {
            var a = new AddOperation();
            ClassicAssert.IsNull(a.SubClass);
            Assert.Throws<ArgumentOutOfRangeException>(() => a.SubClass = " invalid ");
            a.SubClass = "EntOrg";
            ClassicAssert.AreEqual("EntOrg", a.SubClass);
            a.SubClass = null;
            ClassicAssert.IsNull(a.SubClass);
            }

        [Test]
        public void CanAddAttributes()
            {
            var a = new AddOperation();

            a.AddAttribute("BasicString", "StringValue");
            ClassicAssert.IsInstanceOf<StringAttribute>(a.Attributes["BasicString"].Attribute);
            ClassicAssert.AreEqual("StringValue", a.Attributes["BasicString"].Attribute.Value);

            a.AddAttribute("BasicBool", true);
            ClassicAssert.IsInstanceOf<BoolAttribute>(a.Attributes["BasicBool"].Attribute);
            ClassicAssert.AreEqual(true, a.Attributes["BasicBool"].Attribute.Value);

            a.AddAttribute("BasicInt", 527);
            ClassicAssert.IsInstanceOf<IntAttribute>(a.Attributes["BasicInt"].Attribute);
            ClassicAssert.AreEqual(527, a.Attributes["BasicInt"].Attribute.Value);

            a.AddAttribute("BasicDecimal", 887766m);
            ClassicAssert.IsInstanceOf<DecimalAttribute>(a.Attributes["BasicDecimal"].Attribute);
            ClassicAssert.AreEqual(887766m, a.Attributes["BasicDecimal"].Attribute.Value);

            var now = DateTime.Now;
#if NET6_0_OR_GREATER
            var today = DateOnly.FromDateTime(now);
            a.AddDateAttribute("BasicDate", today);
            ClassicAssert.IsInstanceOf<DateAttribute>(a.Attributes["BasicDate"].Attribute);
            ClassicAssert.AreEqual(today, a.Attributes["BasicDate"].Attribute.Value);

            a.AddAttribute("BasicDate2", today);
            ClassicAssert.IsInstanceOf<DateAttribute>(a.Attributes["BasicDate2"].Attribute);
            ClassicAssert.AreEqual(today, a.Attributes["BasicDate2"].Attribute.Value);
#else
            a.AddDateAttribute("BasicDate", now.Date);
            ClassicAssert.IsInstanceOf<DateAttribute>(a.Attributes["BasicDate"].Attribute);
            ClassicAssert.AreEqual(now.Date, a.Attributes["BasicDate"].Attribute.Value);
#endif

            a.AddDateTimeAttribute("BasicDateTime", now);
            ClassicAssert.IsInstanceOf<DateTimeAttribute>(a.Attributes["BasicDateTime"].Attribute);
            ClassicAssert.AreEqual(now, a.Attributes["BasicDateTime"].Attribute.Value);

            var g = Guid.NewGuid();
            a.AddAttribute("BasicGuid", g);
            ClassicAssert.IsInstanceOf<GuidAttribute>(a.Attributes["BasicGuid"].Attribute);
            ClassicAssert.AreEqual(g, a.Attributes["BasicGuid"].Attribute.Value);

            a.AddAliasedAttribute("AliasedAttribute", "Number", "L1234.12345");
            ClassicAssert.IsInstanceOf<AliasAttribute>(a.Attributes["AliasedAttribute"]);
            ClassicAssert.IsInstanceOf<StringAttribute>(a.Attributes["AliasedAttribute"].Attribute);
            ClassicAssert.AreEqual("Number", ((AliasAttribute) a.Attributes["AliasedAttribute"]).Alias);
            ClassicAssert.AreEqual("L1234.12345", ((AliasAttribute) a.Attributes["AliasedAttribute"]).Attribute.Value);

#if NET6_0_OR_GREATER
            ClassicAssert.AreEqual(9, a.Attributes.Count);
#else
            ClassicAssert.AreEqual(8, a.Attributes.Count);
#endif   
            }

        [Test]
        public void TestRenderWithoutSubclass()
            {
            var a = new AddOperation();
            a.AddAttribute("OrgName", "acme corp");
            a.AddChild("Site");

            var renderer = new TestTransactionServiceRenderer();
            renderer.Render(a, "Entity");
            var s = renderer.Result;
            ClassicAssert.AreEqual("<Add><Entity><Attributes><OrgName>acme corp</OrgName></Attributes><Children><Site /></Children></Entity></Add>", s);
            }

        [Test]
        public void TestRenderWithSubclass()
            {
            var a = new AddOperation("EntOrg");
            a.AddAttribute("OrgName", "acme corp");
            a.AddChild("Site");

            var renderer = new TestTransactionServiceRenderer();
            renderer.Render(a, "Entity");
            var s = renderer.Result;
            ClassicAssert.AreEqual("<Add><EntOrg><Attributes><OrgName>acme corp</OrgName></Attributes><Children><Site /></Children></EntOrg></Add>", s);
            }
        }
    }
