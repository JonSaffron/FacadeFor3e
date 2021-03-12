using System;
using System.Linq;
using System.Xml;
using NUnit.Framework;

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

            Assert.True(processResult.HasDataError);
            Assert.AreEqual(1, processResult.DataErrors.Count());
            var dataError = processResult.DataErrors.Single();
            Assert.AreEqual("516557", dataError.PrimaryKey);
            Assert.AreEqual(3, dataError.RowIndex);
            Assert.AreEqual(0, dataError.AttributeErrors.Count);
            Assert.AreEqual(2, dataError.Children.Count);

            var profDetailTime = dataError.Children.First();
            Assert.AreEqual("ProfDetailTime", profDetailTime.ObjectId);
            Assert.AreEqual("61932954", profDetailTime.PrimaryKey);
            Assert.AreEqual(88, profDetailTime.RowIndex);
            Assert.AreEqual(1, profDetailTime.AttributeErrors.Count);
            Assert.IsNull(profDetailTime.ObjectException);

            var profDetailTimeAtt = profDetailTime.AttributeErrors.Single();
            Assert.AreEqual("WorkTimekeeper", profDetailTimeAtt.AttributeId);
            Assert.AreEqual("-2", profDetailTimeAtt.Value);
            Assert.True(profDetailTimeAtt.Error.StartsWith("Fee-Earner must exist"));

            var profDetailCost = dataError.Children.Skip(1).First();
            Assert.AreEqual("ProfDetailCost", profDetailCost.ObjectId);
            Assert.AreEqual("sample exception", profDetailCost.ObjectException);
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
            var lines = output.Split(new [] {'\r', '\n'}, StringSplitOptions.RemoveEmptyEntries);
            Assert.True(lines[0].Contains("Proforma"));
            Assert.True(lines[0].Contains("516557"));
            Assert.True(lines[1].Contains("ProfDetailTime"));
            Assert.True(lines[1].Contains("61932954"));
            Assert.True(lines[2].Contains("Fee-Earner must exist"));
            Assert.True(lines[3].Contains("ProfDetailCost"));
            Assert.True(lines[3].Contains("61933093"));
            Assert.True(lines[4].Contains("sample exception"));
            Assert.True(lines[5].Contains("allowed for Disbursement Entry"));
            }
        }
    }
