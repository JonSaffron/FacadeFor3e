using System;
using FacadeFor3e.ProcessCommandBuilder;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace FacadeFor3e.Tests
    {
    [TestFixture]
    public class AttributeTests
        {
        [Test]
        public void TestDecimalWithNumber()
            {
            const decimal testNumber = 12345678m;
            var d = new DecimalAttribute(testNumber);
            ClassicAssert.AreEqual(testNumber, d.Value);
            ClassicAssert.AreEqual("12345678", d.ToString());
            }

        [Test]
        public void TestDecimalWithNull()
            {
            var d = new DecimalAttribute(null);
            ClassicAssert.IsNull(d.Value);
            ClassicAssert.AreEqual(string.Empty, d.ToString());
            }

        [Test]
        public void TestIntegerWithNumber()
            {
            const int testNumber = 111222333;
            var i = new IntAttribute(testNumber);
            ClassicAssert.AreEqual(testNumber, i.Value);
            ClassicAssert.AreEqual("111222333", i.ToString());
            }

        [Test]
        public void TestIntegerWithNull()
            {
            var i = new IntAttribute(null);
            ClassicAssert.IsNull(i.Value);
            ClassicAssert.AreEqual(string.Empty, i.ToString());
            }

        [Test]
        public void TestStringWithValue()
            {
            string testString = DateTime.Now.ToLongDateString();
            var s = new StringAttribute(testString);
            ClassicAssert.AreEqual(testString, s.Value);
            ClassicAssert.AreEqual(testString, s.ToString());
            }

        [Test]
        public void TestStringWithNull()
            {
            var s = new StringAttribute(null);
            ClassicAssert.IsNull(s.Value);
            ClassicAssert.AreEqual(string.Empty, s.ToString());
            }

        [Test]
        public void TestGuidWithValue()
            {
            Guid testGuid = Guid.Parse("(4772822E-16F9-40B8-8265-47AF6648D60E)");
            var g = new GuidAttribute(testGuid);
            ClassicAssert.AreEqual(testGuid, g.Value);
            ClassicAssert.AreEqual("4772822e-16f9-40b8-8265-47af6648d60e", g.ToString());
            }

        [Test]
        public void TestGuidWithNull()
            {
            var g = new GuidAttribute(null);
            ClassicAssert.IsNull(g.Value);
            ClassicAssert.AreEqual(string.Empty, g.ToString());
            }

        [Test]
        public void TestDateWithValue()
            {
#if NET6_0_OR_GREATER
            var testDate = new DateOnly(1980, 2, 29);
#else
            var testDate = new DateTime(1980, 2, 29);
#endif
            var d = new DateAttribute(testDate);
            ClassicAssert.AreEqual(testDate, d.Value);
            ClassicAssert.AreEqual("1980-02-29", d.ToString());
            }

        [Test]
        public void TestDateWithValue2()
            {
#if NET6_0_OR_GREATER
            DateOnly testDate = new DateOnly(1980, 2, 29);
#else
            DateTime testDate = new DateTime(1980, 2, 29);
#endif
            var d = new DateAttribute(testDate.Year, testDate.Month, testDate.Day);
            ClassicAssert.AreEqual(testDate, d.Value);
            ClassicAssert.AreEqual("1980-02-29", d.ToString());
            }

        [Test]
        public void TestDateWithNull()
            {
            var d = new DateAttribute(null);
            ClassicAssert.IsNull(d.Value);
            ClassicAssert.AreEqual(string.Empty, d.ToString());
            }

#if !NET6_0_OR_GREATER
        [Test]
        public void TestDateWithTimeCausesException()
            {
            // ReSharper disable once ObjectCreationAsStatement
            Assert.Throws<ArgumentOutOfRangeException>(() => new DateAttribute(DateTime.Now));
            }
#endif

        [Test]
        public void TestDateTimeWithValue()
            {
            DateTime testDateTime = new DateTime(1980, 2, 29, 12, 34, 56);
            var dt = new DateTimeAttribute(testDateTime);
            ClassicAssert.AreEqual(testDateTime, dt.Value);
            ClassicAssert.AreEqual("1980-02-29 12:34:56", dt.ToString());
            }

        [Test]
        public void TestDateTimeWithValue2()
            {
            DateTime testDateTime = new DateTime(1980, 2, 29, 12, 34, 56);
            var dt = new DateTimeAttribute(testDateTime.Year, testDateTime.Month, testDateTime.Day, testDateTime.Hour, testDateTime.Minute, testDateTime.Second);
            ClassicAssert.AreEqual(testDateTime, dt.Value);
            ClassicAssert.AreEqual("1980-02-29 12:34:56", dt.ToString());
            }

        [Test]
        public void TestDateTimeWithNull()
            {
            var dt = new DateTimeAttribute(null);
            ClassicAssert.IsNull(dt.Value);
            ClassicAssert.AreEqual(string.Empty, dt.ToString());
            }

        [Test]
        public void TestBoolWithTrue()
            {
            var bt = new BoolAttribute(true);
            ClassicAssert.IsTrue(bt.Value);
            ClassicAssert.AreEqual("true", bt.ToString());
            ClassicAssert.AreEqual(bt, BoolAttribute.True);
            }

        [Test]
        public void TestDateTimeWithFalse()
            {
            var bf = new BoolAttribute(false);
            ClassicAssert.IsFalse(bf.Value);
            ClassicAssert.AreEqual("false", bf.ToString());
            }

        [Test]
        public void TestBoolChanges()
            {
            int n = -100;
            do
                {
                var testBool = n % 2 == 0;
                BoolAttribute b = testBool;
                ClassicAssert.AreEqual(testBool, b.Value);
                ClassicAssert.IsTrue(testBool == b.Value);

                testBool = !testBool;
                ClassicAssert.AreNotEqual(testBool, b.Value);
                ClassicAssert.IsFalse(testBool == b.Value);



                n++;
                } while (n < 100);
            }
        }
    }
