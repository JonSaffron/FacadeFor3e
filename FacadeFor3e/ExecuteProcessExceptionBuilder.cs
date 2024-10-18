using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace FacadeFor3e
    {
    internal static class ExecuteProcessExceptionBuilder
        {
        internal static ExecuteProcessException BuildForProcessError(ExecuteProcessResult executeProcessResult)
            {
            if (executeProcessResult == null) throw new ArgumentNullException(nameof(executeProcessResult));
            if (executeProcessResult.Response.DocumentElement == null)
                throw new InvalidOperationException("Response xml is invalid.");

            var errorMessages = GetErrorList(executeProcessResult.Response).ToList();
            bool isReadDataError = errorMessages[0] == "Error attempting to read data.";
            if (isReadDataError)
                {
                errorMessages.Insert(0, 
                    "An error occurred while the transaction service populated the data object(s). Amongst other things this can mean:\r\n"
                    + "- An invalid attribute was specified that doesn't exist on the object\r\n"
                    + "- An invalid child object was specified that doesn't exist on the object\r\n"
                    + "- If multiple child objects were used then the order they were specified in was wrong\r\n"
                    + "- An exception occurred in an AfterPopulate() handler.\r\n"
                    + "The actual error returned was:");
                }
            string msg = string.Join("\r\n", errorMessages);
            var result = new ExecuteProcessException(msg, executeProcessResult);
            return result;
            }

        private static IEnumerable<string> GetErrorList(XmlDocument xmlDocument)
            {
            var errors = new HashSet<string>();
            var topExceptionMessage = xmlDocument.DocumentElement!.GetAttribute("Message");
            errors.Add(topExceptionMessage);
            yield return topExceptionMessage;

            var errorElement = (XmlElement?) xmlDocument.DocumentElement.SelectSingleNode("MAIN/ERROR");
            while (errorElement != null)
                {
                var message = errorElement.SelectSingleNode("MESSAGE")!.InnerText.Trim();
                bool isNew = errors.Add(message);
                if (message.StartsWith("An error occurred in the "))
                    {
                    var parts = message.Split(new[] { ':' }, 2);
                    if (parts.Length == 2)
                        {
                        isNew = errors.Add(parts[1].Trim());
                        }
                    }

                if (isNew)
                    {
                    yield return message;
                    }

                errorElement = (XmlElement?) errorElement.SelectSingleNode("ERROR");
                }
            }
        }
    }
