using System;
using System.Xml;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace FacadeFor3e.Tests
    {
    [TestFixture]
    public class TestProcessResult
        {
        [Test]
        public void TestResponseForError()
            {
            var request = new XmlDocument();
            request.LoadXml("<dummyrequest />");
            var response = new XmlDocument();
            response.LoadXml(Resources.ExampleResponseForError);

            var processResult = new ExecuteProcessResult(request, response);
            var ex = ExecuteProcessExceptionBuilder.BuildForProcessError(processResult);

            ClassicAssert.AreSame(processResult, ex.ExecuteProcessResult);
            ClassicAssert.IsTrue(ex.Message.Contains("Error attempting to read data."));
            ClassicAssert.IsTrue(ex.Message.Contains("The element 'Attributes' in namespace 'http://elite.com/schemas/transaction/object/write/Proforma' has invalid child element 'ProfStatusx' in namespace 'http://elite.com/schemas/transaction/object/write/Proforma'. List of possible elements expected: 'ProfIndex, ProfDate, "));
            ClassicAssert.IsTrue(ex.Message.Contains("An error occurred while the transaction service populated the data object(s)"));
            }

        [Test]
        public void TestResponseOnLockedRecord1()
            {
            var request = new XmlDocument();
            request.LoadXml("<dummyrequest />");
            var response = new XmlDocument();
            response.LoadXml(Resources.ExampleResponseInterfaceOnLockedRecord);

            var processResult = new ExecuteProcessResult(request, response);

            ClassicAssert.IsTrue(processResult.HasDataError);
            ClassicAssert.AreEqual(1, processResult.DataErrors.Count());
            var dataError = processResult.DataErrors.Single();
            ClassicAssert.AreEqual("Proforma", dataError.ObjectId);
            ClassicAssert.AreEqual("215960", dataError.PrimaryKey);
            ClassicAssert.AreEqual(0, dataError.RowIndex);
            ClassicAssert.AreEqual("The Proforma Edit record that you are trying to update is currently being edited and is locked by [Unknown]. Please try again later, or ask [Unknown] to complete or cancel their activity.", dataError.ObjectException);
            }

        [Test]
        public void TestResponseOnLockedRecord2()
            {
            var request = new XmlDocument();
            request.LoadXml("<dummyrequest />");
            var response = new XmlDocument();
            response.LoadXml(Resources.ExampleResponseSuccessOnLockedRecord);

            var processResult = new ExecuteProcessResult(request, response);

            ClassicAssert.IsTrue(processResult.HasDataError);
            ClassicAssert.AreEqual(1, processResult.DataErrors.Count());
            var dataError = processResult.DataErrors.Single();
            ClassicAssert.AreEqual("Proforma", dataError.ObjectId);
            ClassicAssert.AreEqual("215946", dataError.PrimaryKey);
            ClassicAssert.AreEqual(0, dataError.RowIndex);
            ClassicAssert.AreEqual("The Proforma Edit record that you are trying to update is currently being edited and is locked by Cobra Admin. Please try again later, or ask Cobra Admin to complete or cancel their activity.", dataError.ObjectException);
            }

        [Test]
        public void TestResponseWithKeyInfo()
            {
            var request = new XmlDocument();
            request.LoadXml("<dummyrequest />");
            var response = new XmlDocument();
            response.LoadXml(Resources.ExampleResponseSuccessWithKeyInfo);

            var processResult = new ExecuteProcessResult(request, response);

            ClassicAssert.AreEqual(Guid.Parse("fc2ce60a-5f75-4de2-a799-8a07c4997668"), processResult.ProcessId);
            ClassicAssert.AreEqual("Success", processResult.ExecutionResult);
            ClassicAssert.AreEqual("Success", processResult.OutputId);
            ClassicAssert.IsFalse(processResult.HasDataError);
            ClassicAssert.IsNull(processResult.NextMessage);
            ClassicAssert.IsTrue(processResult.GetKeys().SequenceEqual(new [] {"215966"}));
            ClassicAssert.IsFalse(processResult.DataErrors.Any());
            }

        [Test]
        public void TestResponseWithoutKeyInfo()
            {
            var request = new XmlDocument();
            request.LoadXml("<dummyrequest />");
            var response = new XmlDocument();
            response.LoadXml(Resources.ExampleResponseSuccessWithoutKeyInfo);

            var processResult = new ExecuteProcessResult(request, response);

            ClassicAssert.AreEqual(Guid.Parse("84647d6b-a173-4e10-a271-7e42394ef600"), processResult.ProcessId);
            ClassicAssert.AreEqual("Success", processResult.ExecutionResult);
            ClassicAssert.AreEqual("Success", processResult.OutputId);
            ClassicAssert.IsFalse(processResult.HasDataError);
            ClassicAssert.IsNull(processResult.NextMessage);
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Assert.Throws<InvalidOperationException>(() => processResult.GetKeys().ToList());
            ClassicAssert.IsFalse(processResult.DataErrors.Any());
            }

        [Test]
        public void TestResponseWithMessage()
            {
            var request = new XmlDocument();
            request.LoadXml("<dummyrequest />");
            var response = new XmlDocument();
            response.LoadXml(Resources.ExampleResponseWithMessage);

            var processResult = new ExecuteProcessResult(request, response);

            ClassicAssert.AreEqual("Proforma 215951 is in use by paperless proformas and cannot be edited here at this moment.", processResult.NextMessage);
            }
        }
    }
