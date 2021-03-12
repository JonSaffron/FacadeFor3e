using System;
using FacadeFor3e.ProcessCommandBuilder;
using NUnit.Framework;

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
            Assert.AreEqual(testNumber, d.Value);
            Assert.AreEqual("12345678", d.ToString());
            }

        [Test]
        public void TestDecimalWithNull()
            {
            var d = new DecimalAttribute(null);
            Assert.IsNull(d.Value);
            Assert.AreEqual(string.Empty, d.ToString());
            }

        [Test]
        public void TestDecimalChanges()
            {
            int n = -100;
            do
                {
                DecimalAttribute d = n;
                Assert.AreEqual(n, d.Value);
                n++;

                d.Value = null;
                Assert.IsNull(d.Value);
                n++;

                d.Value = n;
                Assert.AreEqual(n, d.Value);
                n++;
                } while (n < 100);
            }

        [Test]
        public void TestIntegerWithNumber()
            {
            const int testNumber = 111222333;
            var i = new IntAttribute(testNumber);
            Assert.AreEqual(testNumber, i.Value);
            Assert.AreEqual("111222333", i.ToString());
            }

        [Test]
        public void TestIntegerWithNull()
            {
            var i = new IntAttribute(null);
            Assert.IsNull(i.Value);
            Assert.AreEqual(string.Empty, i.ToString());
            }

        [Test]
        public void TestIntegerChanges()
            {
            int n = -100;
            do
                {
                IntAttribute i = n;
                Assert.AreEqual(n, i.Value);
                n++;

                i.Value = null;
                Assert.IsNull(i.Value);
                n++;

                i.Value = n;
                Assert.AreEqual(n, i.Value);
                n++;
                } while (n < 100);
            }

        [Test]
        public void TestStringWithValue()
            {
            string testString = DateTime.Now.ToLongDateString();
            var s = new StringAttribute(testString);
            Assert.AreEqual(testString, s.Value);
            Assert.AreEqual(testString, s.ToString());
            }

        [Test]
        public void TestStringWithNull()
            {
            var s = new StringAttribute(null);
            Assert.IsNull(s.Value);
            Assert.AreEqual(string.Empty, s.ToString());
            }

        [Test]
        public void TestStringChanges()
            {
            int n = -100;
            do
                {
                var testString = $"blah{n}blah";
                StringAttribute s = testString;
                Assert.AreEqual(testString, s.Value);
                n++;

                s.Value = null;
                Assert.IsNull(s.Value);
                n++;

                testString = $"blah{n}blah";
                s.Value = testString;
                Assert.AreEqual(testString, s.Value);
                n++;
                } while (n < 100);
            }

        [Test]
        public void TestGuidWithValue()
            {
            Guid testGuid = Guid.Parse("(4772822E-16F9-40B8-8265-47AF6648D60E)");
            var g = new GuidAttribute(testGuid);
            Assert.AreEqual(testGuid, g.Value);
            Assert.AreEqual("4772822e-16f9-40b8-8265-47af6648d60e", g.ToString());
            }

        [Test]
        public void TestGuidWithNull()
            {
            var g = new GuidAttribute(null);
            Assert.IsNull(g.Value);
            Assert.AreEqual(string.Empty, g.ToString());
            }

        [Test]
        public void TestGuidChanges()
            {
            int n = -100;
            do
                {
                var testGuid = Guid.NewGuid();
                GuidAttribute g = testGuid;
                Assert.AreEqual(testGuid, g.Value);
                n++;

                g.Value = null;
                Assert.IsNull(g.Value);
                n++;

                testGuid = Guid.NewGuid();
                g.Value = testGuid;
                Assert.AreEqual(testGuid, g.Value);
                n++;
                } while (n < 100);
            }

        [Test]
        public void TestDateWithValue()
            {
            DateTime testDate = new DateTime(1980, 2, 29);
            var d = new DateAttribute(testDate);
            Assert.AreEqual(testDate, d.Value);
            Assert.AreEqual("1980-02-29", d.ToString());
            }

        [Test]
        public void TestDateWithValue2()
            {
            DateTime testDate = new DateTime(1980, 2, 29);
            var d = new DateAttribute(testDate.Year, testDate.Month, testDate.Day);
            Assert.AreEqual(testDate, d.Value);
            Assert.AreEqual("1980-02-29", d.ToString());
            }

        [Test]
        public void TestDateWithNull()
            {
            var d = new DateAttribute(null);
            Assert.IsNull(d.Value);
            Assert.AreEqual(string.Empty, d.ToString());
            }

        [Test]
        public void TestDateChanges()
            {
            int n = -100;
            do
                {
                var testDate = DateTime.Today.AddDays(n);
                DateAttribute d = testDate;
                Assert.AreEqual(testDate, d.Value);
                n++;

                d.Value = null;
                Assert.IsNull(d.Value);
                n++;

                testDate = DateTime.Today.AddDays(n);
                d.Value = testDate;
                Assert.AreEqual(testDate, d.Value);
                n++;
                } while (n < 100);
            }

        [Test]
        public void TestDateWithTimeCausesException()
            {
            // ReSharper disable once ObjectCreationAsStatement
            Assert.Throws<ArgumentOutOfRangeException>(() => new DateAttribute(DateTime.Now));

            var d = new DateAttribute(DateTime.Today);
            Assert.Throws<ArgumentOutOfRangeException>(() => d.Value = DateTime.Now);
            }

        [Test]
        public void TestDateTimeWithValue()
            {
            DateTime testDateTime = new DateTime(1980, 2, 29, 12, 34, 56);
            var dt = new DateTimeAttribute(testDateTime);
            Assert.AreEqual(testDateTime, dt.Value);
            Assert.AreEqual("1980-02-29 12:34:56", dt.ToString());
            }

        [Test]
        public void TestDateTimeWithValue2()
            {
            DateTime testDateTime = new DateTime(1980, 2, 29, 12, 34, 56);
            var dt = new DateTimeAttribute(testDateTime.Year, testDateTime.Month, testDateTime.Day, testDateTime.Hour, testDateTime.Minute, testDateTime.Second);
            Assert.AreEqual(testDateTime, dt.Value);
            Assert.AreEqual("1980-02-29 12:34:56", dt.ToString());
            }

        [Test]
        public void TestDateTimeWithNull()
            {
            var dt = new DateTimeAttribute(null);
            Assert.IsNull(dt.Value);
            Assert.AreEqual(string.Empty, dt.ToString());
            }

        [Test]
        public void TestDateTimeChanges()
            {
            int n = -100;
            do
                {
                var testDateTime = DateTime.Now.AddDays(n);
                DateTimeAttribute dt = testDateTime;
                Assert.AreEqual(testDateTime, dt.Value);
                n++;

                dt.Value = null;
                Assert.IsNull(dt.Value);
                n++;

                testDateTime = DateTime.Now.AddDays(n);
                dt.Value = testDateTime;
                Assert.AreEqual(testDateTime, dt.Value);
                n++;
                } while (n < 100);
            }

        [Test]
        public void TestBoolWithTrue()
            {
            var bt = new BoolAttribute(true);
            Assert.IsTrue(bt.Value);
            Assert.AreEqual("true", bt.ToString());
            }

        [Test]
        public void TestDateTimeWithFalse()
            {
            var bf = new BoolAttribute(false);
            Assert.IsFalse(bf.Value);
            Assert.AreEqual("false", bf.ToString());
            }

        [Test]
        public void TestBoolChanges()
            {
            int n = -100;
            do
                {
                var testBool = n % 2 == 0;
                BoolAttribute b = testBool;
                Assert.AreEqual(testBool, b.Value);

                testBool = !testBool;
                b.Value = testBool;
                Assert.AreEqual(testBool, b.Value);

                n++;
                } while (n < 100);
            }
        }
    }
