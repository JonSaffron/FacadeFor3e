using System;
using System.Globalization;
using System.Text;
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
            XmlElement root = executeProcessResult.Response.DocumentElement;
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
            var result = new ExecuteProcessException(msg, executeProcessResult);
            return result;
            }

        internal static ExecuteProcessException BuildForDataError(ExecuteProcessResult executeProcessResult)
            {
            var msg = new StringBuilder();
            msg.AppendLine("There are problems with the data supplied to the process.");
            msg.AppendLine();
            msg.AppendLine("The specific errors are as follows:");
            msg.Append(ExecuteProcessResult.RenderDataErrors(executeProcessResult.DataErrors));
            var result = new ExecuteProcessException(msg.ToString(), executeProcessResult);
            return result;
            }
        }
    }
