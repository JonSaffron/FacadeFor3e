using System;
using System.IO;
using System.Xml;
using FacadeFor3e.ProcessCommandBuilder;

namespace FacadeFor3e.Tests
    {
    public static class CommonLibrary
        {
        public static string GetRenderedOutput(Action<XmlWriter> action)
            {
            using (var sw = new StringWriter())
                {
                var settings = new XmlWriterSettings();
                settings.OmitXmlDeclaration = true;
                using (var xw = XmlWriter.Create(sw, settings))
                    {
                    action(xw);
                    }
                var result = sw.ToString();
                return result;
                }
            }

        public static string GetRenderedOutputWithNode(Action<XmlWriter> action)
            {
            using (var sw = new StringWriter())
                {
                var settings = new XmlWriterSettings();
                settings.OmitXmlDeclaration = true;
                using (var xw = XmlWriter.Create(sw, settings))
                    {
                    xw.WriteStartElement("Test");
                    action(xw);
                    xw.WriteEndElement();
                    }
                var result = sw.ToString();
                return result;
                }
            }

        }

    public class TestTransactionServiceRenderer : TransactionServiceRenderer
        {
        private readonly StringWriter _sw;
        private readonly bool _addTestNode;

        public TestTransactionServiceRenderer(bool addTestNode = false)
            {
            this._sw = new StringWriter();
            var settings = new XmlWriterSettings
                {
                OmitXmlDeclaration = true
                };
            this.Writer = XmlWriter.Create(this._sw, settings);
            if (addTestNode)
                {
                this._addTestNode = true;
                this.Writer.WriteStartElement("Test");
                }
            }

        public string Result
            {
            get
                {
                if (this._addTestNode)
                    {
                    this.Writer.WriteEndElement();
                    }
                this.Writer.Flush();
                this._sw.Flush();
                return this._sw.ToString();
                }
            }
        }
    }
