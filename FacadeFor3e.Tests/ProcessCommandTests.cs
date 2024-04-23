using System;
using System.Xml;
using FacadeFor3e.ProcessCommandBuilder;
using NUnit.Framework;
using NUnit.Framework.Legacy;

// ReSharper disable ObjectCreationAsStatement
// ReSharper disable AssignNullToNotNullAttribute

namespace FacadeFor3e.Tests
    {
    [TestFixture]
    public class ProcessCommandTests
        {
        [Test]
        public void CanConstruct()
            {
            var p = new ProcessCommand("Matter_Srv", "Matter");
            p.AddRecord();
            ClassicAssert.AreEqual("Matter_Srv", p.ProcessCode);
            ClassicAssert.AreEqual("Matter", p.ObjectName);
            ClassicAssert.AreEqual("http://elite.com/schemas/transaction/process/write/Matter_Srv", p.ProcessNameSpace);
            ClassicAssert.AreEqual("http://elite.com/schemas/transaction/object/write/Matter", p.ObjectNameSpace);

            var renderer = new TransactionServiceRenderer();
            var xmlDoc = renderer.Render(p, ExecuteProcessOptions.Default);
            ClassicAssert.IsInstanceOf<XmlDocument>(xmlDoc);
            ClassicAssert.AreEqual("<Matter_Srv xmlns=\"http://elite.com/schemas/transaction/process/write/Matter_Srv\">" +
                            "<Initialize xmlns=\"http://elite.com/schemas/transaction/object/write/Matter\">" +
                            "<Add><Matter /></Add>" +
                            "</Initialize>" +
                            "</Matter_Srv>", xmlDoc.InnerXml);
            }

        [Test]
        public void InvalidConstructionParameters()
            {
            Assert.Throws<ArgumentNullException>(() => new ProcessCommand(null, "Matter"));
            Assert.Throws<ArgumentOutOfRangeException>(() => new ProcessCommand("25invalid", "Matter"));
            Assert.Throws<ArgumentNullException>(() => new ProcessCommand("Matter_Srv", null));
            Assert.Throws<ArgumentOutOfRangeException>(() => new ProcessCommand("Matter_Srv", "invalid object"));
            }

        [Test]
        public void OtherProcessProperties()
            {
            var p = new ProcessCommand("Matter_Srv", "Matter");
            p.ProcessName = "NBI";
            p.Description = "New Business Inception";
            p.Priority = ProcessPriority.Medium;
            p.OperatingUnit = "100";

            var renderer = new TestTransactionServiceRenderer(true);
            renderer.RenderProcessAttributes(p);
            var s = renderer.Result;
            ClassicAssert.AreEqual("<Test " +
                            "Name=\"NBI\" " +
                            "Description=\"New Business Inception\" " +
                            "Priority=\"MEDIUM\" " +
                            "OperatingUnit=\"100\" " +
                            "/>", s);
            }

        [Test]
        public void OtherProcessOptions()
            {
            var options = new ExecuteProcessOptions();
            options.CheckSum = 123.45m;
            options.ProxyUser = new Guid("E97FB230-2E09-43C1-B0C2-125E833AFAFE");
            options.ProcessExecutionRequestType = new ProcessExecutionRequestType(ProcessExecutionRequestTypeEnum.SaveFirstEndOnNoErrors, true);
            options.ProcessAutomationRoleAfterFirstStep = "NbiRole"; 
            options.ProcessRequestSignature = "xxxyyyyzzz";

            var renderer = new TestTransactionServiceRenderer(true);
            renderer.RenderProcessOptions(options);
            var s = renderer.Result;
            ClassicAssert.AreEqual("<Test " +
                                   "ProxyUser=\"e97fb230-2e09-43c1-b0c2-125e833afafe\" " +
                                   "CheckSum=\"123.45\" " +
                                   "ProcessRequestType=\"SaveFirstEndOnNoErrors_SuppressChildAutogeneration\" " +
                                   "ProcessAutomationRoleAfterFirstStep=\"NbiRole\" " +
                                   "ProcessRequestSignature=\"xxxyyyyzzz\" " +
                                   "/>", s);
            }
        }
    }
