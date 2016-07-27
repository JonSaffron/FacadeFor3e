using System.Globalization;
using System.IO;
using System.Xml;

namespace FacadeFor3e
    {
    public static class CommonLibrary
        {
        public static string PrettyPrintXml(this XmlDocument xmlDoc)
            {
            string result;
            using (var stringWriter = new StringWriter(CultureInfo.InvariantCulture))
                {
                var xmlNodeReader = new XmlNodeReader(xmlDoc);
                var xmlTextWriter = new XmlTextWriter(stringWriter)
                                        {   //set formatting options
                                            Formatting = Formatting.Indented,
                                            Indentation = 1,
                                            IndentChar = '\t'
                                        };

                // write the document formatted
                xmlTextWriter.WriteNode(xmlNodeReader, true);
                result = stringWriter.ToString();
                }
            return result;
            }
        }
    }
