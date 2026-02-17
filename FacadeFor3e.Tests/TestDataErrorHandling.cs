using System;
using System.Linq;
using System.Xml;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace FacadeFor3e.Tests
    {
    [TestFixture]
    public class TestDataErrorHandling
        {
        [Test]
        public void TestErrorData()
            {
            var request = new XmlDocument();
            request.LoadXml("<dummyrequest />");
            var response = new XmlDocument();
            response.LoadXml(Resources.ExampleResponseWithDataErrors);

            var processResult = new ExecuteProcessResult(request, response);

            ClassicAssert.True(processResult.HasDataError);
            ClassicAssert.AreEqual(1, processResult.DataErrors.Count());
            var dataError = processResult.DataErrors.Single();
            ClassicAssert.AreEqual("516557", dataError.PrimaryKey);
            ClassicAssert.AreEqual(3, dataError.RowIndex);
            ClassicAssert.AreEqual(0, dataError.AttributeErrors.Count);
            ClassicAssert.AreEqual(2, dataError.Children.Count);

            var profDetailTime = dataError.Children.First();
            ClassicAssert.AreEqual("ProfDetailTime", profDetailTime.ObjectId);
            ClassicAssert.AreEqual("61932954", profDetailTime.PrimaryKey);
            ClassicAssert.AreEqual(88, profDetailTime.RowIndex);
            ClassicAssert.AreEqual(1, profDetailTime.AttributeErrors.Count);
            ClassicAssert.IsNull(profDetailTime.ObjectException);

            var profDetailTimeAtt = profDetailTime.AttributeErrors.Single();
            ClassicAssert.AreEqual("WorkTimekeeper", profDetailTimeAtt.AttributeId);
            ClassicAssert.AreEqual("-2", profDetailTimeAtt.Value);
            ClassicAssert.True(profDetailTimeAtt.Error.StartsWith("Fee-Earner must exist"));

            var profDetailCost = dataError.Children.Skip(1).First();
            ClassicAssert.AreEqual("ProfDetailCost", profDetailCost.ObjectId);
            ClassicAssert.AreEqual("sample exception", profDetailCost.ObjectException);
            }

        [Test]
        public void TestDataErrorRendering()
            {
            var request = new XmlDocument();
            request.LoadXml("<dummyrequest />");
            var response = new XmlDocument();
            response.LoadXml(Resources.ExampleResponseWithDataErrors);

            var processResult = new ExecuteProcessResult(request, response);

            var output = ExecuteProcessResult.RenderDataErrors(processResult.DataErrors);
            var lines = output!.Split(new [] {'\r', '\n'}, StringSplitOptions.RemoveEmptyEntries);
            ClassicAssert.True(lines[0].Contains("Proforma"));
            ClassicAssert.True(lines[0].Contains("516557"));
            ClassicAssert.True(lines[1].Contains("ProfDetailTime"));
            ClassicAssert.True(lines[1].Contains("61932954"));
            ClassicAssert.True(lines[2].Contains("Fee-Earner must exist"));
            ClassicAssert.True(lines[3].Contains("ProfDetailCost"));
            ClassicAssert.True(lines[3].Contains("61933093"));
            ClassicAssert.True(lines[4].Contains("sample exception"));
            ClassicAssert.True(lines[5].Contains("allowed for Disbursement Entry"));
            }

        [Test]
        public void TestErrorHandlingWhenNothingUsefulReturned()
            {
            var request = new XmlDocument();
            request.LoadXml("<dummyrequest />");
            var response = new XmlDocument();
            response.LoadXml(Resources.ExampleResponseWithoutAnyUsefulErrors);

            var processResult = new ExecuteProcessResult(request, response);
            var dataErrors = processResult.DataErrors.ToList();
            ClassicAssert.AreEqual(1, dataErrors.Count());
            var output = ExecuteProcessResult.RenderDataErrors(dataErrors);
            ClassicAssert.IsNotNull(output);
            }
        }
    }
