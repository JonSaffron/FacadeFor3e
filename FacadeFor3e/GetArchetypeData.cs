using System;
using System.Net;
using System.Security.Principal;
using System.Xml;
using JetBrains.Annotations;

namespace FacadeFor3e
    {
    /// <summary>
    /// An object that runs an XOQL query to return data from the 3E database
    /// </summary>
    [PublicAPI]
    public class GetArchetypeData
        {
        private readonly TransactionServices _transactionServices;

        internal GetArchetypeData(TransactionServices transactionServices)
            {
            this._transactionServices = transactionServices ?? throw new ArgumentNullException(nameof(transactionServices));
            }

        /// <summary>
        /// Runs the specified query and returns the results
        /// </summary>
        /// <param name="xoql">Specifies the query to execute</param>
        /// <param name="endpoint">The url to use to connect to the 3E transaction service</param>
        /// <returns>An XML representation of the result set</returns>
        /// <remarks>The query is run in the context of the account running the process. Row level security will be observed.</remarks>
        public static XmlDocument GetData(XmlDocument xoql, Uri endpoint)
            {
            using var services = new TransactionServices(endpoint);
            var result = services.GetArchetypeData(xoql);
            return result;
            }

        /// <summary>
        /// Runs the specified query and returns the results
        /// </summary>
        /// <param name="xoql">Specifies the query to execute</param>
        /// <param name="endpoint">The url to use to connect to the 3E transaction service</param>
        /// <param name="accountToImpersonate">The account to impersonate</param>
        /// <returns>An XML representation of the result set</returns>
        /// <remarks>The query is run in the context of the account running the process. Row level security will be observed.</remarks>
        public static XmlDocument GetData(XmlDocument xoql, Uri endpoint, WindowsIdentity accountToImpersonate)
            {
            using var services = new TransactionServices(endpoint, accountToImpersonate);
            var result = services.GetArchetypeData(xoql);
            return result;
            }

        /// <summary>
        /// Runs the specified query and returns the results
        /// </summary>
        /// <param name="xoql">Specifies the query to execute</param>
        /// <param name="endpoint">The url to use to connect to the 3E transaction service</param>
        /// <param name="networkCredential">The credentials to use when calling the 3E transaction service</param>
        /// <returns>An XML representation of the result set</returns>
        /// <remarks>The query is run in the context of the account running the process. Row level security will be observed.</remarks>
        public static XmlDocument GetData(XmlDocument xoql, Uri endpoint, NetworkCredential networkCredential)
            {
            using var services = new TransactionServices(endpoint, networkCredential);
            var result = services.GetArchetypeData(xoql);
            return result;
            }

        /// <summary>
        /// Runs the specified query and returns the results
        /// </summary>
        /// <param name="xoql">Specifies the query to execute</param>
        /// <returns>An XML representation of the result set</returns>
        /// <remarks>Row level security will be observed.</remarks>
        public XmlDocument GetData(XmlDocument xoql)
            {
            if (xoql == null)
                throw new ArgumentNullException(nameof(xoql));
            if (xoql.DocumentElement == null)
                throw new ArgumentOutOfRangeException(nameof(xoql));
            // todo check xoql starts with SELECT
            
            string? Func() => CallTransactionService(xoql);
            var response = this._transactionServices.IsImpersonating
                // ReSharper disable once AssignNullToNotNullAttribute
                ? WindowsIdentity.RunImpersonated(this._transactionServices.AccountToImpersonate!.AccessToken, Func)
                : Func();

            if (response == null)
                {
                // when no rows are returned, then return something useful
                var result = new XmlDocument();
                result.AppendChild(result.CreateElement("Data"));
                this._transactionServices.LogForDebug("no results returned from xoql query");
                return result;
                }

            if (response.StartsWith("<Data>") && response.EndsWith("</Data>"))
                {
                var result = new XmlDocument();
                result.LoadXml(response);
                string responseFormatted = result.PrettyPrintXml();
                this._transactionServices.LogForDebug(responseFormatted);
                return result;
                }

            this._transactionServices.LogForError(response);
            throw new InvalidOperationException("An invalid response was received from the web server:\r\n" + response);
            }

        private string? CallTransactionService(XmlDocument xoql)
            {
            OutputToConsoleDetailsOfTheJob(xoql);
            this._transactionServices.LogForDebug(xoql.PrettyPrintXml());

            // deliberately only passing through the document element, not the xml declaration on any leading or following comments
            // this is because the 3E transaction service is intolerant of anything but the simplest xml
            // ReSharper disable once PossibleNullReferenceException
            var request = xoql.DocumentElement!.OuterXml;
            var ts = this._transactionServices.GetSoapClient();
            var result = ts.GetArchetypeData(request); // if no rows are returned, then result will be null
            return result;
            }

        private void OutputToConsoleDetailsOfTheJob(XmlDocument xoql)
            {
            // ReSharper disable once AssignNullToNotNullAttribute
            var xnm = new XmlNamespaceManager(xoql.NameTable);
            xnm.AddNamespace("ns", "http://elite.com/schemas/query");
            XmlElement? arch = (XmlElement?) xoql.SelectSingleNode("ns:SELECT/ns:OQL_CONTEXT/ns:NODEMAP[@ID='Node#1']", xnm);
            var archetype = arch?.GetAttribute("QueryID");
            var jobSpecifics = $"Getting data from {archetype ?? "unknown"}";
            this._transactionServices.LogDetailsOfTheJob(jobSpecifics);
            }
        }
    }
