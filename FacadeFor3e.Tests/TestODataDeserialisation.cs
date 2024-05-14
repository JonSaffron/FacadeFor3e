using System;
using System.Text.Json;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace FacadeFor3e.Tests
    {
    [TestFixture]
    public class TestODataDeserialisation
        {
        [Test]
        public void TestDeserialisation()
            {
            var json = JsonDocument.Parse(Resources.ExampleProfPresParagraphs);

            var element = json.RootElement.GetProperty("value")[0].GetProperty("ProfPresParagraphs")[0];
            var firstItem = element.JsonDeserialise<DbPresentationParagraph>();
            ClassicAssert.AreEqual("ProfPresentationParagraph test", firstItem.Narrative);
            ClassicAssert.AreEqual(2.5m, firstItem.PresHours);
            ClassicAssert.AreEqual("sorted", firstItem.SortString);

            var result = json.RootElement.GetProperty("value")[0].GetProperty("ProfPresParagraphs").JsonDeserialiseList<DbPresentationParagraph>();
            ClassicAssert.AreEqual(9, result.Count);
#if NET6_0_OR_GREATER
            ClassicAssert.AreEqual(new DateOnly(2023, 09, 19), result[0].PresDate);
#else
            ClassicAssert.AreEqual(new DateTime(2023, 09, 19), result[0].PresDate);
#endif
            ClassicAssert.AreEqual("EUR", result[1].Currency);
            ClassicAssert.AreEqual(543.12, result[5].PresAmount);
            ClassicAssert.AreEqual(new Guid("d07726c1-2a71-4283-b82b-d6bbe2d079f7"), result[6].ProfPresentationParagraphId);
            ClassicAssert.IsNull(result[2].PresAmount);
            }
        }

    // ReSharper disable once ClassNeverInstantiated.Global
    public class DbPresentationParagraph
        {
        // ReSharper disable UnassignedField.Global
        public Guid ProfPresentationParagraphId;
#if NET6_0_OR_GREATER
        public DateOnly PresDate;
#else
        public DateTime PresDate;
#endif
        public string Currency;
        public decimal? PresAmount;
        public decimal? PresHours;
        public string Narrative;
        public string SortString;
        // ReSharper restore UnassignedField.Global
        }
    }
