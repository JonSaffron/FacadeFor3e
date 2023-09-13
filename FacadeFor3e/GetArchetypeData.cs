using System;
using System.Data;
using System.Globalization;
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
        /// Runs the specified query and returns the results as an XmlDocument
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
        /// Runs the specified query and returns the results as an XmlDocument
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
        /// Runs the specified query and returns the results as an XmlDocument
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
        /// Runs the specified query and returns the results as an DataTable
        /// </summary>
        /// <param name="xoql">Specifies the query to execute</param>
        /// <param name="endpoint">The url to use to connect to the 3E transaction service</param>
        /// <returns>An XML representation of the result set</returns>
        /// <remarks>The query is run in the context of the account running the process. Row level security will be observed.</remarks>
        public static DataTable GetDataTable(XmlDocument xoql, Uri endpoint)
            {
            using var services = new TransactionServices(endpoint);
            var result = services.GetArchetypeDataTable(xoql);
            return result;
            }

        /// <summary>
        /// Runs the specified query and returns the results as an DataTable
        /// </summary>
        /// <param name="xoql">Specifies the query to execute</param>
        /// <param name="endpoint">The url to use to connect to the 3E transaction service</param>
        /// <param name="accountToImpersonate">The account to impersonate</param>
        /// <returns>An XML representation of the result set</returns>
        /// <remarks>The query is run in the context of the account running the process. Row level security will be observed.</remarks>
        public static DataTable GetDataTable(XmlDocument xoql, Uri endpoint, WindowsIdentity accountToImpersonate)
            {
            using var services = new TransactionServices(endpoint, accountToImpersonate);
            var result = services.GetArchetypeDataTable(xoql);
            return result;
            }

        /// <summary>
        /// Runs the specified query and returns the results as an DataTable
        /// </summary>
        /// <param name="xoql">Specifies the query to execute</param>
        /// <param name="endpoint">The url to use to connect to the 3E transaction service</param>
        /// <param name="networkCredential">The credentials to use when calling the 3E transaction service</param>
        /// <returns>An XML representation of the result set</returns>
        /// <remarks>The query is run in the context of the account running the process. Row level security will be observed.</remarks>
        public static DataTable GetDataTable(XmlDocument xoql, Uri endpoint, NetworkCredential networkCredential)
            {
            using var services = new TransactionServices(endpoint, networkCredential);
            var result = services.GetArchetypeDataTable(xoql);
            return result;
            }

        /// <summary>
        /// Runs the specified query and returns the results as a DataTable
        /// </summary>
        /// <param name="xoql">Specifies the query to execute</param>
        /// <returns>An XML representation of the result set</returns>
        /// <remarks>Row level security will be observed.</remarks>
        public DataTable GetDataTable(XmlDocument xoql)
            {
            var xmlData = GetData(xoql);
            var result = BuildDataTableStructure(xmlData);
            FillDataTable(result, xmlData);
            return result;
            }

        internal static DataTable BuildDataTableStructure(XmlDocument xmlData)
            {
            var result = new DataTable();
            if (!xmlData.DocumentElement!.HasChildNodes)
                return result;

            foreach (XmlElement column in xmlData.DocumentElement.ChildNodes[0].ChildNodes)
                {
                var newColumnName = BuildUniqueColumnName(column.LocalName, result.Columns);
                var dataColumn = new DataColumn(newColumnName, typeof(string));
                result.Columns.Add(dataColumn);
                }
            return result;
            }

        private static string BuildUniqueColumnName(string rawName, DataColumnCollection columns)
            {
            if (!columns.Contains(rawName))
                return rawName;
            for (int i = 1; i < 999; i++)
                {
                var name = $"{rawName}{i.ToString(CultureInfo.InvariantCulture)}";
                if (!columns.Contains(name))
                    return name;
                }
            throw new InvalidOperationException("Could not generate a unique column name for " + rawName);
            }

        internal static void FillDataTable(DataTable dataTable, XmlDocument xmlData)
            {
            foreach (XmlElement inputRow in xmlData.DocumentElement!.ChildNodes)
                {
                int i = 0;
                var dr = dataTable.NewRow();
                foreach (XmlElement inputValue in inputRow.ChildNodes)
                    {
                    string value = inputValue.InnerText;
                    dr[i] = value.Length == 0 ? (object) DBNull.Value : value;
                    i++;
                    }
                dataTable.Rows.Add(dr);
                }
            }

        /// <summary>
        /// Runs the specified query and returns the results as an XmlDocument
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
            var result = this._transactionServices.SoapClient.GetArchetypeData(request); // if no rows are returned, then result will be null
            return result;
            }

        private void OutputToConsoleDetailsOfTheJob(XmlDocument xoql)
            {
            // ReSharper disable once AssignNullToNotNullAttribute
            var xnm = new XmlNamespaceManager(xoql.NameTable);
            xnm.AddNamespace("ns", "http://elite.com/schemas/query");
            XmlElement? arch = (XmlElement?) xoql.SelectSingleNode("ns:SELECT/ns:OQL_CONTEXT/ns:NODEMAP[@ID='Node#1']", xnm);
            var archetype = arch?.GetAttribute("QueryID");
            var jobSpecifics = $"Getting data from archetype {archetype ?? "unknown"}";
            this._transactionServices.LogDetailsOfTheJob(jobSpecifics);
            }
        }
    }
