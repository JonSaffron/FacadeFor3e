﻿using System;
using System.Globalization;
using System.Xml;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace FacadeFor3e.Tests
    {
    [TestFixture]
    public class TestTranslateData
        {
        private readonly CultureInfo _cultureInfo = new ("en-US");

        [Test]
        public void TestNoData()
            {
            const string data = "<Data />";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(data);
            Assert.Throws<InvalidOperationException>(() => GetArchetypeData.TranslateScalarValue<bool>(xmlDoc, _cultureInfo));
            }

        [Test]
        public void TestTooManyRows()
            {
            const string data = "<Data><Row><Item /></Row><Row><Item /></Row></Data>";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(data);
            Assert.Throws<InvalidOperationException>(() => GetArchetypeData.TranslateScalarValue<bool>(xmlDoc, _cultureInfo));
            }

        [Test]
        public void TestTooManyValues()
            {
            const string data = "<Data><Row><Item1 /><Item2 /></Row></Data>";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(data);
            Assert.Throws<InvalidOperationException>(() => GetArchetypeData.TranslateScalarValue<bool>(xmlDoc, _cultureInfo));
            }

        /*------------------------*/

        [Test]
        public void TestNullBooleanValueExpected()
            {
            const string data = "<Data><Row><Item></Item></Row></Data>";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(data);
            ClassicAssert.IsNull(GetArchetypeData.TranslateScalarValue<bool?>(xmlDoc, _cultureInfo));
            }

        [Test]
        public void TestNullBooleanValueUnexpected()
            {
            const string data = "<Data><Row><Item /></Row></Data>";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(data);
            Assert.Throws<InvalidOperationException>(() => GetArchetypeData.TranslateScalarValue<bool>(xmlDoc, _cultureInfo));
            }

        [Test]
        public void TestTrueValue()
            {
            const string data = "<Data><Row><Item>1</Item></Row></Data>";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(data);
            ClassicAssert.IsTrue(GetArchetypeData.TranslateScalarValue<bool>(xmlDoc, _cultureInfo));
            ClassicAssert.IsTrue(GetArchetypeData.TranslateScalarValue<bool?>(xmlDoc, _cultureInfo));
            }

        [Test]
        public void TestFalseValue()
            {
            const string data = "<Data><Row><Item>0</Item></Row></Data>";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(data);
            ClassicAssert.IsFalse(GetArchetypeData.TranslateScalarValue<bool>(xmlDoc, _cultureInfo));
            ClassicAssert.IsFalse(GetArchetypeData.TranslateScalarValue<bool?>(xmlDoc, _cultureInfo));
            }

        [Test]
        public void TestNotBooleanValues()
            {
            const string data = "<Data><Row><Item>%%Value%%</Item></Row></Data>";
            for (int i = -1000; i <= 1000; i++)
                {
                if (i == 0 || i == 1)
                    continue;

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(data.Replace("%%Value%%", i.ToString(CultureInfo.InvariantCulture)));
                Assert.Throws<InvalidOperationException>(() => GetArchetypeData.TranslateScalarValue<bool>(xmlDoc, _cultureInfo));
                }
            }

        /*------------------------*/

        [Test]
        public void TestNullIntegerValueExpected()
            {
            const string data = "<Data><Row><Item></Item></Row></Data>";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(data);
            ClassicAssert.IsNull(GetArchetypeData.TranslateScalarValue<int?>(xmlDoc, _cultureInfo));
            }

        [Test]
        public void TestNullIntegerValueUnexpected()
            {
            const string data = "<Data><Row><Item /></Row></Data>";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(data);
            Assert.Throws<InvalidOperationException>(() => GetArchetypeData.TranslateScalarValue<int>(xmlDoc, _cultureInfo));
            }

        [Test]
        public void TestZeroIntegerValue()
            {
            const string data = "<Data><Row><Item>0</Item></Row></Data>";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(data);
            ClassicAssert.AreEqual(0, GetArchetypeData.TranslateScalarValue<int>(xmlDoc, _cultureInfo));
            ClassicAssert.AreEqual(0, GetArchetypeData.TranslateScalarValue<int?>(xmlDoc, _cultureInfo));
            }

        [Test]
        public void TestPositiveIntegerValue()
            {
            const string data = "<Data><Row><Item>12345</Item></Row></Data>";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(data);
            ClassicAssert.AreEqual(12345, GetArchetypeData.TranslateScalarValue<int>(xmlDoc, _cultureInfo));
            ClassicAssert.AreEqual(12345, GetArchetypeData.TranslateScalarValue<int?>(xmlDoc, _cultureInfo));
            }

        [Test]
        public void TestNegativeIntegerValue()
            {
            const string data = "<Data><Row><Item>-98765</Item></Row></Data>";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(data);
            ClassicAssert.AreEqual(-98765, GetArchetypeData.TranslateScalarValue<int>(xmlDoc, _cultureInfo));
            ClassicAssert.AreEqual(-98765, GetArchetypeData.TranslateScalarValue<int?>(xmlDoc, _cultureInfo));
            }

        [Test]
        public void TestNotIntegerValue()
            {
            const string data = "<Data><Row><Item>%%Value%%</Item></Row></Data>";
            foreach (string item in new[] { "rubbish", "prefix12345", "12345suffix", "5e2", "12.34", "12,35" })
                {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(data.Replace("%%Value%%", item));
                Assert.Throws<FormatException>(() => GetArchetypeData.TranslateScalarValue<int>(xmlDoc, _cultureInfo));
                }
            }

        /*------------------------*/

        [Test]
        public void TestNullGuidValueExpected()
            {
            const string data = "<Data><Row><Item></Item></Row></Data>";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(data);
            ClassicAssert.IsNull(GetArchetypeData.TranslateScalarValue<Guid?>(xmlDoc, _cultureInfo));
            }

        [Test]
        public void TestNullGuidValueUnexpected()
            {
            const string data = "<Data><Row><Item /></Row></Data>";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(data);
            Assert.Throws<InvalidOperationException>(() => GetArchetypeData.TranslateScalarValue<Guid>(xmlDoc, _cultureInfo));
            }

        [Test]
        public void TestGuidValue()
            {
            const string data = "<Data><Row><Item>%%Value%%</Item></Row></Data>";
            for (int i = 0; i <= 1000; i++)
                {
                XmlDocument xmlDoc = new XmlDocument();
                var g = new Guid();
                xmlDoc.LoadXml(data.Replace("%%Value%%", g.ToString()));
                ClassicAssert.AreEqual(g, GetArchetypeData.TranslateScalarValue<Guid>(xmlDoc, _cultureInfo));
                ClassicAssert.AreEqual(g, GetArchetypeData.TranslateScalarValue<Guid?>(xmlDoc, _cultureInfo));
                }
            }

        [Test]
        public void TestNotGuidValue()
            {
            const string data = "<Data><Row><Item>%%Value%%</Item></Row></Data>";
            foreach (string item in new[] { "rubbish", "781B5FF8-049F-4B42-B13B-C01573CFB86Fx", "x781B5FF8-049F-4B42-B13B-C01573CFB86F", "781B5FF8-049F-4B42B13B-C01573CFB86F" })
                {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(data.Replace("%%Value%%", item));
                Assert.Throws<FormatException>(() => GetArchetypeData.TranslateScalarValue<Guid>(xmlDoc, _cultureInfo));
                }
            }

        /*------------------------*/

        [Test]
        public void TestNullStringValue()
            {
            const string data = "<Data><Row><Item></Item></Row></Data>";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(data);
            ClassicAssert.IsNull(GetArchetypeData.TranslateScalarValue<string>(xmlDoc, _cultureInfo));
            }

        [Test]
        public void TestStringValue()
            {
            const string data = "<Data><Row><Item>%%Value%%</Item></Row></Data>";
            foreach (string item in new[] { "once", "upon", "a", "time", "p&o", "1<2" })
                {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(data);
                xmlDoc.DocumentElement!.ChildNodes[0]!.ChildNodes[0]!.InnerText = item;
                ClassicAssert.AreEqual(item, GetArchetypeData.TranslateScalarValue<string>(xmlDoc, _cultureInfo));
                }
            }

        /*------------------------*/

        [Test]
        public void TestNullDateTimeValueExpected()
            {
            const string data = "<Data><Row><Item></Item></Row></Data>";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(data);
            ClassicAssert.IsNull(GetArchetypeData.TranslateScalarValue<DateTime?>(xmlDoc, _cultureInfo));
            }

        [Test]
        public void TestNullDateTimeValueUnexpected()
            {
            const string data = "<Data><Row><Item /></Row></Data>";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(data);
            Assert.Throws<InvalidOperationException>(() => GetArchetypeData.TranslateScalarValue<DateTime>(xmlDoc, _cultureInfo));
            }

        [Test]
        public void TestDateTimeValue()
            {
            const string data = "<Data><Row><Item>%%Value%%</Item></Row></Data>";
            var cultureInfo = new CultureInfo("en-US");
            var now = DateTime.Now;
            now = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Month, now.Second);
            for (int i = -1000; i <= 1000; i++)
                {
                XmlDocument xmlDoc = new XmlDocument();
                var d = now.AddSeconds(i).AddDays(i);
                xmlDoc.LoadXml(data.Replace("%%Value%%", d.ToString(cultureInfo)));
                ClassicAssert.AreEqual(d, GetArchetypeData.TranslateScalarValue<DateTime>(xmlDoc, _cultureInfo));
                ClassicAssert.AreEqual(d, GetArchetypeData.TranslateScalarValue<DateTime?>(xmlDoc, _cultureInfo));
                }
            }

        [Test]
        public void TestNotDateTimeValue()
            {
            const string data = "<Data><Row><Item>%%Value%%</Item></Row></Data>";
            foreach (string item in new[] { "rubbish", "thursday", "20231512" })
                {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(data.Replace("%%Value%%", item));
                Assert.Throws<FormatException>(() => GetArchetypeData.TranslateScalarValue<DateTime>(xmlDoc, _cultureInfo));
                }
            }

        /*------------------------*/

#if NET6_0_OR_GREATER
        [Test]
        public void TestNullDateOnlyValueExpected()
            {
            const string data = "<Data><Row><Item></Item></Row></Data>";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(data);
            ClassicAssert.IsNull(GetArchetypeData.TranslateScalarValue<DateOnly?>(xmlDoc, _cultureInfo));
            }

        [Test]
        public void TestNullDateOnlyValueUnexpected()
            {
            const string data = "<Data><Row><Item /></Row></Data>";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(data);
            Assert.Throws<InvalidOperationException>(() => GetArchetypeData.TranslateScalarValue<DateOnly>(xmlDoc, _cultureInfo));
            }

        [Test]
        public void TestDateOnlyValue()
            {
            const string data = "<Data><Row><Item>%%Value%%</Item></Row></Data>";
            var cultureInfo = new CultureInfo("en-US");
            var now = DateTime.Today;
            now = new DateTime(now.Year, now.Month, now.Day);
            for (int i = -1000; i <= 1000; i++)
                {
                XmlDocument xmlDoc = new XmlDocument();
                var d = DateOnly.FromDateTime(now.AddDays(i));
                xmlDoc.LoadXml(data.Replace("%%Value%%", d.ToString("d", cultureInfo)));
                ClassicAssert.AreEqual(d, GetArchetypeData.TranslateScalarValue<DateOnly>(xmlDoc, _cultureInfo));
                ClassicAssert.AreEqual(d, GetArchetypeData.TranslateScalarValue<DateOnly?>(xmlDoc, _cultureInfo));
                }
            }

        [Test]
        public void TestNotDateOnlyValue()
            {
            const string data = "<Data><Row><Item>%%Value%%</Item></Row></Data>";
            foreach (string item in new[] { "rubbish", "thursday", "20231512", "1/27/2024 12:10:20 AM" })
                {
                var xmlData = data.Replace("%%Value%%", item);
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xmlData);
                Assert.Throws<FormatException>(() => GetArchetypeData.TranslateScalarValue<DateOnly>(xmlDoc, _cultureInfo));
                }
            }
#endif
        /*------------------------*/

        [Test]
        public void TestNullDecimalValueExpected()
            {
            const string data = "<Data><Row><Item></Item></Row></Data>";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(data);
            ClassicAssert.IsNull(GetArchetypeData.TranslateScalarValue<decimal?>(xmlDoc, _cultureInfo));
            }

        [Test]
        public void TestNullDecimalValueUnexpected()
            {
            const string data = "<Data><Row><Item /></Row></Data>";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(data);
            Assert.Throws<InvalidOperationException>(() => GetArchetypeData.TranslateScalarValue<decimal>(xmlDoc, _cultureInfo));
            }

        [Test]
        public void TestZeroDecimalValue()
            {
            const string data = "<Data><Row><Item>0</Item></Row></Data>";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(data);
            ClassicAssert.AreEqual(0m, GetArchetypeData.TranslateScalarValue<decimal>(xmlDoc, _cultureInfo));
            ClassicAssert.AreEqual(0m, GetArchetypeData.TranslateScalarValue<decimal?>(xmlDoc, _cultureInfo));
            }

        [Test]
        public void TestPositiveDecimalValue()
            {
            const string data = "<Data><Row><Item>12345.6789</Item></Row></Data>";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(data);
            ClassicAssert.AreEqual(12345.6789m, GetArchetypeData.TranslateScalarValue<decimal>(xmlDoc, _cultureInfo));
            ClassicAssert.AreEqual(12345.6789m, GetArchetypeData.TranslateScalarValue<decimal?>(xmlDoc, _cultureInfo));
            }

        [Test]
        public void TestNegativeDecimalValue()
            {
            const string data = "<Data><Row><Item>-98765.4321</Item></Row></Data>";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(data);
            ClassicAssert.AreEqual(-98765.4321m, GetArchetypeData.TranslateScalarValue<decimal>(xmlDoc, _cultureInfo));
            ClassicAssert.AreEqual(-98765.4321m, GetArchetypeData.TranslateScalarValue<decimal?>(xmlDoc, _cultureInfo));
            }

        [Test]
        public void TestNotDecimalValue()
            {
            const string data = "<Data><Row><Item>%%Value%%</Item></Row></Data>";
            foreach (string item in new[] { "rubbish", "prefix12345", "12345suffix", "5e2", "12.34.56", "12-35" })
                {
                var xmlData = data.Replace("%%Value%%", item);
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xmlData);
                Assert.Throws<FormatException>(() => GetArchetypeData.TranslateScalarValue<decimal>(xmlDoc, _cultureInfo));
                }
            }

        /*------------------------*/

        public class TestResult
            {
            public int Item1;
            public string Item2 { get; set; }
            public TimeSpan NotUsed;

            public Guid ReadOnlyGuid => Guid.NewGuid();

            public Guid WriteOnlyGuid
                {
                set
                    {
                    var _ = value;
                    }
                }
            }

        [Test]
        public void TestCompoundValue()
            {
            const string data = "<Data><Row><Item1>123</Item1><Item2>hello</Item2></Row></Data>";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(data);
            var result = GetArchetypeData.TranslateCompoundValue<TestResult>(xmlDoc, new CultureInfo("en-US"));
            ClassicAssert.AreEqual(123, result.Item1);
            ClassicAssert.AreEqual("hello", result.Item2);
            }

        [Test]
        public void TestCompoundValueWithDuplicateColumns()
            {
            const string data = "<Data><Row><Item>123</Item><Item>456</Item></Row></Data>";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(data);
            Assert.Throws<InvalidOperationException>(() => GetArchetypeData.TranslateCompoundValue<TestResult>(xmlDoc, new CultureInfo("en-US")));
            }

        [Test]
        public void TestCompoundValueWithInvalidDataType()
            {
            const string data = "<Data><Row><Item1>123</Item1><NotUsed>hello</NotUsed></Row></Data>";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(data);
            Assert.Throws<InvalidOperationException>(() => GetArchetypeData.TranslateCompoundValue<TestResult>(xmlDoc, new CultureInfo("en-US")));
            }

        [Test]
        public void TestCompoundList()
            {
            const string data = "<Data><Row><Item1>123</Item1><Item2>hello</Item2></Row><Row><Item1>456</Item1><Item2>there</Item2></Row></Data>";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(data);
            var result = GetArchetypeData.TranslateCompoundList<TestResult>(xmlDoc, new CultureInfo("en-US"));
            ClassicAssert.AreEqual(123, result[0].Item1);
            ClassicAssert.AreEqual("hello", result[0].Item2);
            ClassicAssert.AreEqual(456, result[1].Item1);
            ClassicAssert.AreEqual("there", result[1].Item2);
            }

        [Test]
        public void TestCompoundListWithNoData()
            {
            const string data = "<Data/>";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(data);
            var result = GetArchetypeData.TranslateCompoundList<TestResult>(xmlDoc, new CultureInfo("en-US"));
            ClassicAssert.IsEmpty(result);
            }

        /*------------------------*/

        public record TkprCredit
            {
            public string Number;
            public decimal Percentage;
            public bool IsPrimary;
            }

        [Test]
        public void TestRecord()
            {
            const string data = @"
<Data>
  <Matter>
    <Number>020487</Number>
    <Percentage>40.0000</Percentage>
    <IsPrimary>1</IsPrimary>
  </Matter>
  <Matter>
    <Number>017023</Number>
    <Percentage>30.0000</Percentage>
    <IsPrimary>0</IsPrimary>
  </Matter>
  <Matter>
    <Number>017843</Number>
    <Percentage>30.0000</Percentage>
    <IsPrimary>0</IsPrimary>
  </Matter>
  <Matter>
    <Number>017843</Number>
    <Percentage>30.0000</Percentage>
    <IsPrimary>0</IsPrimary>
  </Matter>
</Data>
";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(data);
            var result = GetArchetypeData.TranslateCompoundList<TkprCredit>(xmlDoc, new CultureInfo("en-US"));
            ClassicAssert.AreEqual("020487", result[0].Number);
            ClassicAssert.AreNotEqual(result[0], result[1]);
            ClassicAssert.AreEqual(result[2], result[3]);
            }

        [Test]
        public void TestNarrative()
            {
            // If your XOQL asks for Narrative_UnformattedText then you get only Narrative_UnformattedText
            // If your XOQL asks for Narrative then you get both Narrative (plaintext) and Narrative_FormattedText (HTML)
            const string data = @"
<Data>
  <Timecard>
    <TimeIndex>1</TimeIndex>
    <Narrative>Drafting xxx</Narrative>
    <Narrative_FormattedText>&lt;p&gt;Drafting xxx&lt;/p&gt;</Narrative_FormattedText>
  </Timecard>
</Data>
";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(data);
            var resultBoth = GetArchetypeData.TranslateCompoundValue<NarrativeDataBoth>(xmlDoc, new CultureInfo("en-US"));
            ClassicAssert.AreEqual("Drafting xxx", resultBoth.Narrative);
            ClassicAssert.AreEqual("<p>Drafting xxx</p>", resultBoth.Narrative_FormattedText);

            var resultFormattedFullName = GetArchetypeData.TranslateCompoundValue<NarrativeDataFormattedFullName>(xmlDoc, new CultureInfo("en-US"));
            ClassicAssert.AreEqual("<p>Drafting xxx</p>", resultFormattedFullName.Narrative_FormattedText);

            var resultFormattedPartialName = GetArchetypeData.TranslateCompoundValue<NarrativeDataFormattedPartialName>(xmlDoc, new CultureInfo("en-US"));
            ClassicAssert.AreEqual("<p>Drafting xxx</p>", resultFormattedPartialName.Narrative);
            }

        internal class NarrativeDataBoth
            {
            public int TimeIndex;
            public string Narrative;
            // ReSharper disable once InconsistentNaming
            public string Narrative_FormattedText;
            }

        internal class NarrativeDataFormattedFullName
            {
            public int TimeIndex;
            // ReSharper disable once InconsistentNaming
            public string Narrative_FormattedText;
            }

        internal class NarrativeDataFormattedPartialName
            {
            public int TimeIndex;
            public string Narrative;
            }

        [Test]
        public void TestNarrativeUnformatted()
            {
            // If your XOQL asks for Narrative_UnformattedText then you get only Narrative_UnformattedText
            const string data = @"
<Data>
  <Timecard>
    <TimeIndex>1</TimeIndex>
    <Narrative_UnformattedText>Drafting xxx</Narrative_UnformattedText>
  </Timecard>
</Data>
";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(data);

            var resultUnformattedFullName = GetArchetypeData.TranslateCompoundValue<NarrativeDataUnformattedFullName>(xmlDoc, new CultureInfo("en-US"));
            ClassicAssert.AreEqual("Drafting xxx", resultUnformattedFullName.Narrative_UnformattedText);

            var resultUnformattedPartialName = GetArchetypeData.TranslateCompoundValue<NarrativeDataUnformattedPartialName>(xmlDoc, new CultureInfo("en-US"));
            ClassicAssert.AreEqual("Drafting xxx", resultUnformattedPartialName.Narrative);

            var resultUnformattedPartialNameAttribute = GetArchetypeData.TranslateCompoundValue<NarrativeDataUnformattedPartialNameAttribute>(xmlDoc, new CultureInfo("en-US"));
            ClassicAssert.AreEqual("Drafting xxx", resultUnformattedPartialNameAttribute.TimecardNarrative);
            }

        internal class NarrativeDataUnformattedFullName
            {
            public int TimeIndex;
            [ColumnMapping("Narrative")]
            // ReSharper disable once InconsistentNaming
            public string Narrative_UnformattedText;
            }

        internal class NarrativeDataUnformattedPartialName
            {
            public int TimeIndex;
            [ColumnMapping("Narrative")]
            public string Narrative;
            }

        internal class NarrativeDataUnformattedPartialNameAttribute
            {
            public int TimeIndex;

            [ColumnMapping("Narrative")] 
            public string TimecardNarrative;
            }

        [Test]
        public void TestScalarList()
            {
            const string data = "<Data><ProfDetail><ProfDetIndex>5117860</ProfDetIndex></ProfDetail><ProfDetail><ProfDetIndex>5117861</ProfDetIndex></ProfDetail><ProfDetail><ProfDetIndex>5117862</ProfDetIndex></ProfDetail></Data>";
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(data);
            var result = GetArchetypeData.TranslateScalarList<int>(xmlDoc, this._cultureInfo);
            Assert.That(result[0], Is.EqualTo(5117860));
            Assert.That(result[1], Is.EqualTo(5117861));
            Assert.That(result[2], Is.EqualTo(5117862));
            }

        [Test]
        public void TestNarrativeAgain()
            {
            const string data = @"
<Data>
	<ProfDetailTime>
		<WorkHrs>1.00000</WorkHrs>
		<WorkNarrative>First split</WorkNarrative>
		<WorkNarrative_FormattedText>First split</WorkNarrative_FormattedText>
		<WorkPhase />
		<WorkTask />
		<WorkActivity />
		<IsDisplay>1</IsDisplay>
		<PresHrs>1.00000</PresHrs>
		<PresAmt>260.00</PresAmt>
		<PresNarrative>First split</PresNarrative>
		<PresNarrative_FormattedText>First split</PresNarrative_FormattedText>
	</ProfDetailTime>
</Data>";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(data);

            var result = GetArchetypeData.TranslateCompoundValue<ProfDetailTimeByProfDetIndex>(xmlDoc, new CultureInfo("en-US"));
            ClassicAssert.AreEqual("First split", result.WorkNarrative);
            ClassicAssert.AreEqual("First split", result.PresNarrative);
            }

        [Test]
        public void TestErrorMessages()
            {
            const string data = @"
<Data>
	<ProfDetailTime>
		<WorkHrs></WorkHrs>
		<WorkNarrative>First split</WorkNarrative>
		<WorkNarrative_FormattedText>First split</WorkNarrative_FormattedText>
		<WorkPhase />
		<WorkTask />
		<WorkActivity />
		<IsDisplay>1</IsDisplay>
		<PresHrs>1.00000</PresHrs>
		<PresAmt>260.00</PresAmt>
		<PresNarrative>First split</PresNarrative>
		<PresNarrative_FormattedText>First split</PresNarrative_FormattedText>
	</ProfDetailTime>
</Data>";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(data);

            Assert.Throws<InvalidOperationException>(() => GetArchetypeData.TranslateCompoundValue<ProfDetailTimeByProfDetIndex>(xmlDoc, new CultureInfo("en-US")));
            }

        internal class ProfDetailTimeByProfDetIndex
            {
            public decimal WorkHrs;
            public string WorkNarrative;
            public Guid? WorkPhase;
            public Guid? WorkTask;
            public Guid? WorkActivity;
            public bool IsDisplay;
            public decimal PresHrs;
            public decimal PresAmt;
            public string PresNarrative;
            }
        }
    }
