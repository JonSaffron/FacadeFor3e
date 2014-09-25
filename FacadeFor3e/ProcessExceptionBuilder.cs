﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace FacadeFor3e
    {
    internal static class ProcessExceptionBuilder
        {
        private const string ErrorMessagePrefix = "Error in the application.-";

        public static ProcessException BuildForProcessError(RunProcessParameters p, RunProcessResult r)
            {
            XmlElement root = r.Response.DocumentElement;
            if (root == null)
                throw new InvalidOperationException();
            var errorMessageNode = root.SelectSingleNode("MAIN/ERROR") as XmlElement;
            if (errorMessageNode == null)
                throw new InvalidOperationException("Cannot identify the process errors node in the output.");
            do
                {
                var e = errorMessageNode.SelectSingleNode("ERROR") as XmlElement;
                if (e == null)
                    break;
                errorMessageNode = e;
                } while (true);

            errorMessageNode = errorMessageNode.SelectSingleNode("MESSAGE") as XmlElement;
            string errorMessage = errorMessageNode != null ? errorMessageNode.InnerText : "no further details are available";
            string message = root.GetAttribute("Message");
            if (string.IsNullOrEmpty(message))
                message = "Unknown error";
            string msg = string.Format(CultureInfo.InvariantCulture, "{0} {1}", message, errorMessage);
            var result = new ProcessException(msg, p, r);
            return result;
            }

        public static ProcessException BuildForDataError(RunProcessParameters p, RunProcessResult r)
            {
            XmlElement root = r.Response.DocumentElement;
            if (root == null)
                throw new InvalidOperationException();
            var element = root.SelectSingleNode("DATA_ERRORS") as XmlElement;
            if (element == null)
                {
                element = root.SelectSingleNode("MESSAGE") as XmlElement;
                if (element != null && element.ChildNodes.Count == 1 && element.ChildNodes[0].NodeType == XmlNodeType.CDATA)
                    {
                    var result2 = new ProcessException(element.ChildNodes[0].InnerText, p, r);
                    return result2;
                    }

                throw new InvalidOperationException("Cannot identify the data errors node in the output.");
                }

            var errors = new List<string>();
            GatherDataErrors(element, string.Empty, errors);
            
            var msg = new StringBuilder();
            msg.AppendFormat("There are problems with the data supplied to the {0} process.", p.ProcessName);
            if (errors.Count == 0)
                msg.Append(" No further details are available.");
            else
                {
                msg.AppendLine();
                msg.AppendLine();
                msg.AppendLine("The specific errors are as follows:");
                foreach (var item in errors)
                    {
                    msg.AppendFormat("    - {0}", item);
                    msg.AppendLine();
                    }
                }
            var result = new ProcessException(msg.ToString(), p, r, errors);
            return result;
            }

        private static void AddDataErrors(XmlElement atts, string objectName, ICollection<string> errors)
            {
            foreach (XmlElement c in atts.ChildNodes)
                {
                string attributeName = c.GetAttribute("ID");
                string value = c.GetAttribute("V");
                var e = c.SelectSingleNode("E") as XmlElement;
                string error = null;
                if (e != null)
                    {
                    error = e.InnerText;
                    if (error.StartsWith(ErrorMessagePrefix))
                        error = error.Substring(ErrorMessagePrefix.Length);
                    }
                if (string.IsNullOrWhiteSpace(error))
                    error = "(unknown error)";
                string msg = string.Format("{0} (error caused when setting {1}.{2} to '{3}')", error, objectName, attributeName, value);
                errors.Add(msg);
                }
            }

        private static void GatherDataErrors(XmlElement row, string topObjectName, ICollection<string> errors)
            {
            foreach (XmlElement objectNode in row.ChildNodes)
                {
                string objectName = objectNode.Name;
                if (objectName == "ATTS")
                    {
                    AddDataErrors(objectNode, topObjectName, errors);
                    }
                else if (objectName == "E")
                    {
                    string msg = objectNode.InnerText.Trim();
                    if (msg.StartsWith(ErrorMessagePrefix))
                        msg = msg.Substring(ErrorMessagePrefix.Length);
                    var r = new Regex(@"^([A-Za-z0-9]{1,}) record was modified by ([A-Za-z ]{1,}) since it was opened.  Reopen the record to save changes.$");
                    var m = r.Match(msg);
                    if (m.Success && m.Groups.Count == 3)
                        {
                        string objectName2 = m.Groups[1].Value;
                        string lockingAccount = m.Groups[2].Value;
                        msg = string.Format("The {0} record that you are trying to update is currently being edited and is locked by {1}. Please try again later, or ask {1} to complete or cancel their activity.", objectName2, lockingAccount);
                        }
                    errors.Add(msg);
                    }
                else
                    {
                    foreach (XmlElement item in objectNode)
                        {
                        if (item.Name != "ROW")
                            throw new InvalidOperationException();
                        GatherDataErrors(item, objectName, errors);
                        }
                    }
                }
            }
        }
    }