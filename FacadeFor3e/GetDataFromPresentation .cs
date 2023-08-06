using System;
using System.Net;
using System.Security.Principal;
using System.ServiceModel;
using System.Xml;
using JetBrains.Annotations;

namespace FacadeFor3e
    {
    /// <summary>
    /// An object that returns data from running a 3E presentation
    /// </summary>
    /// <remarks>This is a facade for the GetDataFromReportLayout service, a ReportLayout being the old and deprecated name for a Presentation. It only works for presentations that do NOT get data from a metric.</remarks>
    [PublicAPI]
    public class GetDataFromPresentation
        {
        private readonly TransactionServices _transactionServices;

        internal GetDataFromPresentation(TransactionServices transactionServices)
            {
            this._transactionServices = transactionServices ?? throw new ArgumentNullException(nameof(transactionServices));
            }

        /// <summary>
        /// Runs the specified query and returns the results
        /// </summary>
        /// <param name="presentationId">Specifies the presentation to run</param>
        /// <param name="endpoint">The url to use to connect to the 3E transaction service</param>
        /// <returns>An XML representation of the result set</returns>
        /// <remarks>The query is run in the context of the account running the process. Row level security will be observed.</remarks>
        public static XmlDocument GetData(string presentationId, Uri endpoint)
            {
            using var services = new TransactionServices(endpoint);
            var result = services.GetDataFromPresentation(presentationId);
            return result;
            }

        /// <summary>
        /// Runs the specified query and returns the results
        /// </summary>
        /// <param name="presentationId">Specifies the presentation to run</param>
        /// <param name="endpoint">The url to use to connect to the 3E transaction service</param>
        /// <param name="accountToImpersonate">The account to impersonate</param>
        /// <returns>An XML representation of the result set</returns>
        /// <remarks>The query is run in the context of the account running the process. Row level security will be observed.</remarks>
        public static XmlDocument GetData(string presentationId, Uri endpoint, WindowsIdentity accountToImpersonate)
            {
            using var services = new TransactionServices(endpoint, accountToImpersonate);
            var result = services.GetDataFromPresentation(presentationId);
            return result;
            }

        /// <summary>
        /// Runs the specified query and returns the results
        /// </summary>
        /// <param name="presentationId">Specifies the presentation to run</param>
        /// <param name="endpoint">The url to use to connect to the 3E transaction service</param>
        /// <param name="networkCredential">The credentials to use when calling the 3E transaction service</param>
        /// <returns>An XML representation of the result set</returns>
        /// <remarks>The query is run in the context of the account running the process. Row level security will be observed.</remarks>
        public static XmlDocument GetData(string presentationId, Uri endpoint, NetworkCredential networkCredential)
            {
            using var services = new TransactionServices(endpoint, networkCredential);
            var result = services.GetDataFromPresentation(presentationId);
            return result;
            }

        /// <summary>
        /// Runs the specified query and returns the results
        /// </summary>
        /// <param name="presentationId">Specifies the presentation to run</param>
        /// <returns>An XML representation of the result set</returns>
        /// <remarks>Row level security will be observed.</remarks>
        public XmlDocument GetData(string presentationId)
            {
            if (presentationId == null)
                throw new ArgumentNullException(nameof(presentationId));
            
            string? Func() => CallTransactionService(presentationId);
            var response = this._transactionServices.IsImpersonating
                // ReSharper disable once AssignNullToNotNullAttribute
                ? WindowsIdentity.RunImpersonated(this._transactionServices.AccountToImpersonate!.AccessToken, Func)
                : Func();

            if (IsResponseValid(response))
                {
                var result = new XmlDocument();
                result.LoadXml(response!);
                string responseFormatted = result.PrettyPrintXml();
                this._transactionServices.LogForDebug(responseFormatted);
                return result;
                }

            this._transactionServices.LogForError(response ?? "(null response)");
            throw new InvalidOperationException("An invalid response was received from the web server:\r\n" + response);
            }

        private static bool IsResponseValid(string? response)
            {
            if (response == null)
                return false;
            if (!response.StartsWith("<Data "))
                return false;
            if (response.EndsWith("</Data>"))
                // should be true if one or more records are returned
                return true;
            if (response.EndsWith("/>"))
                // should be true if no records are returned
                return true;
            return false;
            }

        private string? CallTransactionService(string presentationId)
            {
            OutputToConsoleDetailsOfTheJob(presentationId);

            try
                {
                var result = this._transactionServices.SoapClient.GetDataFromReportLayout(presentationId);
                return result;
                }
            catch (FaultException ex) when (ex.Message != null && ex.Message.StartsWith("Unable to find: "))
                {
                throw new InvalidOperationException("The specified presentation could not be found.");
                }
            catch (FaultException ex) when (ex.Message == "Error retrieving data" || ex.Message == "Cannot load an object with ObjectId = Nothing")
                {
                throw new InvalidOperationException("Could not retrieve data from the specified presentation (this is the expected behaviour if the report data is generated by a metric).");
                }
            }

        private void OutputToConsoleDetailsOfTheJob(string presentationId)
            {
            var jobSpecifics = $"Getting data from presentation {presentationId}";
            this._transactionServices.LogDetailsOfTheJob(jobSpecifics);
            }
        }
    }
