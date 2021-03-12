using System;
using System.Globalization;
using System.IO;
using System.ServiceModel;
using System.Text;
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

        internal static BasicHttpBinding BuildBinding()
            {
            var result = new BasicHttpBinding
                {
                CloseTimeout = TimeSpan.FromMinutes(1),
                OpenTimeout = TimeSpan.FromMinutes(1),
                SendTimeout = TimeSpan.FromMinutes(1),
                ReceiveTimeout = TimeSpan.FromMinutes(10),
                AllowCookies = false,
                BypassProxyOnLocal = false,
                HostNameComparisonMode = HostNameComparisonMode.StrongWildcard,
                MessageEncoding = WSMessageEncoding.Text,
                TextEncoding = Encoding.UTF8,
                TransferMode = TransferMode.Buffered,
                UseDefaultWebProxy = true
                };

            result.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
            result.Security.Transport.ClientCredentialType = HttpClientCredentialType.Windows;

            return result;
            }

        }
    }
