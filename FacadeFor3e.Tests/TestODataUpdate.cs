using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.Json;
using FacadeFor3e.ProcessCommandBuilder;
using NUnit.Framework;
using NUnit.Framework.Legacy;

#pragma warning disable OData

namespace FacadeFor3e.Tests
    {
    [TestFixture]
    public class TestODataUpdate
        {
        [OneTimeSetUp]
        public void Setup()
            {
            var cm = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None).FilePath;
            NLog.LogManager.Configuration = new NLog.Config.XmlLoggingConfiguration(cm);
            }

        [Test]
        public void Test1()
            {
            var command = new ProcessCommand("ProfMaster_ndt", "Proforma_ndt");
            var e = command.EditRecord(new IdentifyByPrimaryKey(225360));
            e.AddAttribute("InvNarrative", $"Updated narrative {DateTime.Now}");

            Uri baseUri = new Uri("https://rdfin91tewa01.dentons.global/TE_3E_DEV_EU_REPORT/odata/");
            var service = new ODataServices(baseUri);
            var response = service.Execute(command);
            Console.WriteLine(response);
            }


        [Test]
        public void Test2()
            {
            var command = new ProcessCommand("ProfMaster_ndt", "Proforma_ndt");
            var e = command.EditRecord(new IdentifyByPrimaryKey(225360));
            e.AddAttribute("InvNarrative", $"Updated narrative {DateTime.Now}");

            Uri tranSvc =
                new Uri("https://rdfin91tewa01.dentons.global/te_3e_dev_eu_report/web/transactionservice.asmx");
            var ts = new TransactionServices(tranSvc);
            ts.ExecuteProcess.Execute(command);
        }

        [Test]
        public void Test3()
            {
            var command = new ProcessCommand("Office", "Office");
            var edit = command.EditRecord(new IdentifyByPrimaryKey("0000"));
            edit.AddAttribute("Description", $"Default {DateTime.Now:s}");

            Uri baseUri = new Uri("https://rdfin91tewa01.dentons.global/TE_3E_DEV_EU_REPORT/odata/");
            var service = new ODataServices(baseUri);
            var response = service.Execute(command);
            Console.WriteLine(response.ResponseString);
            }

        [Test]
        public void TestGetProformaDetails()
            {
            var uri = new Uri("Proforma_ndt(225483)", UriKind.Relative);

            Uri baseUri = new Uri("https://rdfin91tewa01.dentons.global/TE_3E_DEV_EU_REPORT/odata/");
            var service = new ODataServices(baseUri);
            var result = service.Select(uri);
            Console.WriteLine(result.ResponseString);
            }

        [Test]
        public void TestGetProfStatus()
            {
            var uri = new Uri("Proforma_ndt(225483)?$select=ProfStatus", UriKind.Relative);

            Uri baseUri = new Uri("https://rdfin91tewa01.dentons.global/TE_3E_DEV_EU_REPORT/odata/");
            var service = new ODataServices(baseUri);
            var result = service.Select(uri);
            var profStatus = result.ResponseJSonDocument.RootElement.GetProperty("value")[0].GetProperty("ProfStatus").GetString();
            Console.WriteLine(profStatus);
            }

        [Test]
        public void TestProfDetResponse()
            {
            var jsonDocument = JsonDocument.Parse(ProfDetResponse);
            var result = jsonDocument.RootElement.GetProperty("value")[0].GetProperty("ProfDetailTimes").EnumerateArray().Select(item => item.GetProperty("ProfDetIndex").GetInt32()).ToList();
            ClassicAssert.AreEqual(2, result.Count);
            ClassicAssert.AreEqual(5117777, result[0]);
            ClassicAssert.AreEqual(5117778, result[1]);
            }

        private string ProfDetResponse
            {
            get
                {
                return
                    @"{
    ""@odata.context"": ""https://rdfin91tewa01.dentons.global/TE_3E_DEV_EU_REPORT/odata/$metadata#Proforma_ndt(ProfDetailTimes)"",
    ""value"": [
        {
            ""ProfDetailTimes"": [
                {
                    ""ProfDetailTimeID"": ""6f411a21-c281-4d1f-ba09-6c0d9c66a455"",
                    ""ArchetypeCode"": ""ProfDetailTime"",
                    ""LastProcItemID"": ""1558400f-88f6-46f4-bbe5-809b141b23ac"",
                    ""OrigProcItemID"": ""906210e9-d970-4a40-af0e-04a91b63dedd"",
                    ""HasAttachments"": false,
                    ""TimeStamp"": ""2023-08-23T21:58:44.477Z"",
                    ""ProfDetIndex"": 5117777,
                    ""ProfMaster"": 225370,
                    ""Currency"": ""EUR"",
                    ""CurrDate"": ""2023-08-23T00:00:00Z"",
                    ""TransactionType"": ""FEES"",
                    ""IsDisplay"": false,
                    ""WorkMatter"": 100445,
                    ""BillMatter"": 100445,
                    ""WorkTimekeeper"": 8117,
                    ""IsSplit"": false,
                    ""WorkDate"": ""2023-08-23T00:00:00Z"",
                    ""WorkAmt"": 852,
                    ""WorkLanguage"": 1031,
                    ""WorkNarrative"": ""Additional timecard 1 (5c4b62e9-c9f2-4493-854d-54159ce70a3b)"",
                    ""EditAmt"": 852,
                    ""PresMatter"": 100445,
                    ""PresTimekeeper"": 8117,
                    ""PresDate"": ""2023-08-23T00:00:00Z"",
                    ""PresAmt"": 0,
                    ""PresLanguage"": 1031,
                    ""PresNarrative"": ""Additional timecard 1 (5c4b62e9-c9f2-4493-854d-54159ce70a3b)"",
                    ""WorkRate"": 710,
                    ""EditRate"": 710,
                    ""IsExcluded"": false,
                    ""IsNB"": false,
                    ""IsCalculated"": false,
                    ""PresRate"": 0,
                    ""WorkWorkType"": ""DFLT"",
                    ""WorkQnt"": 1,
                    ""EditQnt"": 1,
                    ""PresQnt"": 1,
                    ""WorkMattEffDate"": ""45be993b-dd7b-4e41-bce9-2520720773de"",
                    ""BillMattEffDate"": ""45be993b-dd7b-4e41-bce9-2520720773de"",
                    ""WorkTkprEffDate"": ""9987f916-022b-4f83-83b1-194a825df141"",
                    ""ProfMatter"": ""9eeeaa7e-4c33-4343-bb7d-78602ed57047"",
                    ""WorkHrs"": 1.2,
                    ""EditHrs"": 1.2,
                    ""PresHrs"": 0,
                    ""PresWorkType"": ""DFLT"",
                    ""OrigCurrency"": ""EUR"",
                    ""ProfDetailArchetype"": ""Timecard"",
                    ""Timecard"": 32777779,
                    ""WorkTimeType"": ""FEES"",
                    ""PresTimeType"": ""FEES"",
                    ""IsChangedNarrative"": false,
                    ""Disposition"": ""Paragraph"",
                    ""Office"": ""8105"",
                    ""IsAnticipated"": false,
                    ""RateCalcList"": ""MR"",
                    ""SpvTimekeeper"": 5862,
                    ""IsDoNotSummarize"": false,
                    ""MatrixTaxCode"": ""DEOD10"",
                    ""WorkStdAmt"": 852,
                    ""PresMattEffDate"": ""45be993b-dd7b-4e41-bce9-2520720773de"",
                    ""IsEBillVal"": false,
                    ""PresIsNoCharge"": false,
                    ""EditStatus"": ""X"",
                    ""PresTkprEffDate"": ""9987f916-022b-4f83-83b1-194a825df141"",
                    ""RefCurrency"": ""EUR"",
                    ""RefRate"": 710,
                    ""StdCurrency"": ""EUR"",
                    ""StdRate"": 710,
                    ""StdAmt"": 852,
                    ""RefAmt"": 852
                },
                {
                    ""ProfDetailTimeID"": ""34ba03df-276c-4464-a624-7fc2d1ec7240"",
                    ""ArchetypeCode"": ""ProfDetailTime"",
                    ""LastProcItemID"": ""1558400f-88f6-46f4-bbe5-809b141b23ac"",
                    ""OrigProcItemID"": ""906210e9-d970-4a40-af0e-04a91b63dedd"",
                    ""HasAttachments"": false,
                    ""TimeStamp"": ""2023-08-23T21:58:44.48Z"",
                    ""ProfDetIndex"": 5117778,
                    ""ProfMaster"": 225370,
                    ""Currency"": ""EUR"",
                    ""CurrDate"": ""2023-08-23T00:00:00Z"",
                    ""TransactionType"": ""FEES"",
                    ""IsDisplay"": true,
                    ""WorkMatter"": 100445,
                    ""BillMatter"": 100445,
                    ""WorkTimekeeper"": 8117,
                    ""IsSplit"": false,
                    ""WorkDate"": ""2023-08-23T00:00:00Z"",
                    ""WorkAmt"": 1704,
                    ""WorkLanguage"": 1031,
                    ""WorkNarrative"": ""Additional timecard 2 (6eda14aa-2a83-4354-ae5e-352e6576712c)"",
                    ""EditAmt"": 1704,
                    ""PresMatter"": 100445,
                    ""PresTimekeeper"": 8117,
                    ""PresDate"": ""2023-08-23T00:00:00Z"",
                    ""PresAmt"": 2556,
                    ""PresLanguage"": 1031,
                    ""PresNarrative"": ""combined narrative"",
                    ""WorkRate"": 710,
                    ""EditRate"": 710,
                    ""IsExcluded"": false,
                    ""IsNB"": false,
                    ""IsCalculated"": false,
                    ""PresRate"": 710,
                    ""WorkWorkType"": ""DFLT"",
                    ""WorkQnt"": 1,
                    ""EditQnt"": 1,
                    ""PresQnt"": 1,
                    ""WorkMattEffDate"": ""45be993b-dd7b-4e41-bce9-2520720773de"",
                    ""BillMattEffDate"": ""45be993b-dd7b-4e41-bce9-2520720773de"",
                    ""WorkTkprEffDate"": ""9987f916-022b-4f83-83b1-194a825df141"",
                    ""ProfMatter"": ""9eeeaa7e-4c33-4343-bb7d-78602ed57047"",
                    ""WorkHrs"": 2.4,
                    ""EditHrs"": 2.4,
                    ""PresHrs"": 3.6,
                    ""PresWorkType"": ""DFLT"",
                    ""OrigCurrency"": ""EUR"",
                    ""ProfDetailArchetype"": ""Timecard"",
                    ""Timecard"": 32777780,
                    ""WorkTimeType"": ""FEES"",
                    ""PresTimeType"": ""FEES"",
                    ""IsChangedNarrative"": false,
                    ""Disposition"": ""Paragraph"",
                    ""Office"": ""8105"",
                    ""IsAnticipated"": false,
                    ""RateCalcList"": ""MR"",
                    ""SpvTimekeeper"": 5862,
                    ""IsDoNotSummarize"": false,
                    ""MatrixTaxCode"": ""DEOD10"",
                    ""WorkStdAmt"": 1704,
                    ""PresMattEffDate"": ""45be993b-dd7b-4e41-bce9-2520720773de"",
                    ""IsEBillVal"": false,
                    ""PresIsNoCharge"": false,
                    ""EditStatus"": ""X"",
                    ""PresTkprEffDate"": ""9987f916-022b-4f83-83b1-194a825df141"",
                    ""RefCurrency"": ""EUR"",
                    ""RefRate"": 710,
                    ""StdCurrency"": ""EUR"",
                    ""StdRate"": 710,
                    ""StdAmt"": 1704,
                    ""RefAmt"": 1704
                }
            ]
        }
    ]
}";
                }
            }


        [Test]
        public void TestAddAndUpdate()
            {
            var processCommandForAdd = new ProcessCommand("TimeCardUpdate", "TimeCard");
            var add = processCommandForAdd.AddRecord();
#if NET6_0_OR_GREATER
            add.AddAttribute("WorkDate", DateOnly.FromDateTime(DateTime.Today));
#else
            add.AddDateAttribute("WorkDate", DateTime.Today);
#endif
            add.AddAliasedAttribute("Timekeeper", "Number", "218669");
            add.AddAliasedAttribute("Matter", "Number", "0003778.0039");
            add.AddAttribute("WorkHrs", 2.5m);
            add.AddAttribute("Narrative", $"test at {DateTime.Now:T}");

            Uri baseUri = new Uri("https://rdfin91tewa01.dentons.global/TE_3E_DEV_EU_REPORT/odata/");
            var service = new ODataServices(baseUri);
            var response = service.Execute(processCommandForAdd);

            var origTimeIndex = response.ResponseJSonDocument.RootElement.GetProperty("TimeIndex").GetInt32();

            var processCommandForEdit = new ProcessCommand("TimeCardUpdate", "TimeCard");
            var edit = processCommandForEdit.EditRecord(new IdentifyByPrimaryKey(origTimeIndex));
            edit.AddAttribute("WorkRate", 700m);
            edit.AddAttribute("Currency", "GBP");

            service.Execute(processCommandForEdit);

            var uri = new Uri($"TimeCard?$filter=OrigTimeIndex eq {origTimeIndex} and IsActive eq true&$select=TimeIndex", UriKind.Relative);
            response = service.Select(uri);
            var newTimeIndex = response.ResponseJSonDocument.RootElement.GetProperty("value")[0].GetProperty("TimeIndex").GetInt32();

            var processCommandForUpdate = new ProcessCommand("TimeCardUpdate", "TimeCard");
            var update = processCommandForUpdate.EditRecord(new IdentifyByPrimaryKey(newTimeIndex));
            update.AddAttribute("Narrative", $"updated at {DateTime.Now:T}");

            service.Execute(processCommandForUpdate);

            response = service.Select(uri);
            newTimeIndex = response.ResponseJSonDocument.RootElement.GetProperty("value")[0].GetProperty("TimeIndex").GetInt32();

            uri = new Uri($"TimeCard/{newTimeIndex}?$select=OrigTimeIndex,TimeIndex,IsActive,Narrative,WorkRate,Currency,WorkHrs", UriKind.Relative);
            service.Select(uri);
            }
        }
    }

/*
{
     "@odata.context": "https://rdfin91tewa01.dentons.global/TE_3E_DEV_EU_REPORT/odata/$metadata#Office/$entity",
     "OfficeID": "29a69ad1-970b-4636-8ede-80d90cc0f222",
     "Code": "0000",
     "Description": "Default 2024-02-23T16:11:06",
     "OfficeTaxCodes": [],
     "OfficeAddressLangs": [],
     "OfficeTemplateTexts": []
}
{
    "@odata.context":"https://rdfin91tewa01.dentons.global/TE_3E_DEV_EU_REPORT/odata/$metadata#Proforma_ndt/$entity",
    "ProfMasterID":"54421a94-322c-470f-8224-2bda20a2264c",
    "ProfIndex":225360,
    "NxAttachments":[],
    "ProfMatters":[],
    "TRNValidationResultss":[],
    "ProfDetailTimes":[],
    "ProfDetailChrgs":[],
    "ProfDetailCosts":[],
    "ProfAdjusts":[],
    "ProfPayors":[],
    "ProfTrusts":[],
    "ProfUnallocateds":[],
    "ProfBOAs":[],
    "ProfTemplateOptions":[],
    "ProfPresParagraphs":[],
    "ProfMasterDates":[],
    "ProfTaxArticles":[],
    "ProfPayorLayers":[],
    "ProformaContacts_cccs":[],
    "ProfUDF_cccs":[]
}



*/
