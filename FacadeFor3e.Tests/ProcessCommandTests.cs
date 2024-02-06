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
            var xmlDoc = renderer.Render(p);
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
        public void OtherProperties()
            {
            var p = new ProcessCommand("Matter_Srv", "Matter");
            p.ProcessName = "NBI";
            p.Description = "New Business Inception";
            p.Priority = PriorityEnum.MEDIUM;
            p.OperatingUnit = "100";
            p.CheckSum = 1;
            p.ProxyUser = "joe bloggs";
            p.ProxyUserId = "firm\\joe.bloggs";
            p.ProcessRequestType = ProcessExecutionRequestTypeEnum.SaveFirstEndOnNoErrors;
            p.ProcessAutomationRoleAfterFirstStep = "NbiRole";
            p.ProcessRequestSignature = "xxxyyyyzzz";

            var s = CommonLibrary.GetRenderedOutputWithNode(writer => TransactionServiceRenderer.RenderProcessAttributes(p, writer));
            ClassicAssert.AreEqual("<Test ObjectName=\"NBI\" " +
                            "Description=\"New Business Inception\" " +
                            "Priority=\"MEDIUM\" " +
                            "OperatingUnit=\"100\" " +
                            "CheckSum=\"1\" " +
                            "ProxyUser=\"joe bloggs\" " +
                            "ProxyUserID=\"firm\\joe.bloggs\" " +
                            "ProcessRequestType=\"SaveFirstEndOnNoErrors\" " +
                            "ProcessAutomationRoleAfterFirstStep=\"NbiRole\" " +
                            "ProcessRequestSignature=\"xxxyyyyzzz\" " +
                            "/>", s);
            }
        }
    }
