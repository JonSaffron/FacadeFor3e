using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace FacadeFor3e.Tests
    {
#if !NETFRAMEWORK

    [TestFixture]
    public class TestGetArchetypeDataWithTaxDates
        {
        [Test]
        public void Test()
            {
            var xoql = Resources.GetTaxDates;
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xoql);

            var url = new Uri("https://dev.eliteenv.freshfieldsbruckhaus.com/TE_3E_DEV/web/TransactionService.asmx");
            var ts = new TransactionServices(url);
            var result = ts.GetCompoundList<TaxDateInfo>(xmlDoc);
            ClassicAssert.IsNotNull(result);
            }
        }

    public class TaxDateInfo
        {
        public string TaxLkUp;
        public DateOnly EffStart;
        public DateOnly NxStartDate;
        public DateOnly NxEndDate;
        public decimal? Rate;
        public decimal? RatePercent;
        }
#endif
    }
