using System;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using JetBrains.Annotations;

namespace FacadeFor3e
    {
    /// <summary>
    /// Utility methods
    /// </summary>
    [PublicAPI]
    public static class CommonLibrary
        {
        /// <summary>
        /// Outputs an XML document as a formatted string for easy reading
        /// </summary>
        /// <param name="xmlDoc">Specifies the XML document to process</param>
        /// <returns>A string containing the text in the XML document specified with appropriate indentation</returns>
        public static string PrettyPrintXml(this XmlDocument xmlDoc)
            {
            if (xmlDoc == null) throw new ArgumentNullException(nameof(xmlDoc));
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

        /// <summary>
        /// Determines whether a specified string is a valid ID in 3E
        /// </summary>
        /// <param name="id">The string to check</param>
        /// <returns>True if the string can be used as a valid ID in 3E, false otherwise</returns>
        public static bool IsValidId(string? id)
            {
            if (id == null)
                return false;
            const string pattern = "^[A-Za-z_]([A-Za-z0-9_])*$";
            var regex = new Regex(pattern);
            var result = regex.IsMatch(id);
            return result;
            }

        internal static void EnsureValid(string? id)
            {
            if (!IsValidId(id))
                throw new ArgumentOutOfRangeException(nameof(id), "Invalid identifier.");
            }
        }
    }
