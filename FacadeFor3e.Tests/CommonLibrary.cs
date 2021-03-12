using System;
using System.IO;
using System.Xml;

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
    }
