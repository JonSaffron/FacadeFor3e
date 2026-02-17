using System;
using System.Collections.Concurrent;
using System.Globalization;
using System.Security.Principal;
using JetBrains.Annotations;

namespace FacadeFor3e
    {
    /// <summary>
    /// A service to return the culture in use by the transaction service
    /// </summary>
    [PublicAPI]
    public class GetServiceCulture
        {
        private readonly TransactionServices _transactionServices;

        private static readonly ConcurrentDictionary<Uri, CultureInfo> Cache = new();

        internal GetServiceCulture(TransactionServices transactionServices)
            {
            this._transactionServices = transactionServices ?? throw new ArgumentNullException(nameof(transactionServices));
            }

        /// <summary>
        /// Gets the service culture in use by the transaction service
        /// </summary>
        /// <returns>A CultureInfo object</returns>
        public CultureInfo Get()
            {
            var result = Cache.GetOrAdd(this._transactionServices.Endpoint, _ => RetrieveServiceCulture())!;
            TransactionServices.LogForDebug($"ServiceCulture = {result}");
            return result;
            }

        private CultureInfo RetrieveServiceCulture()
            {
            var response = this._transactionServices.IsImpersonating
                ? WindowsIdentity.RunImpersonated(this._transactionServices.AccountToImpersonate!.AccessToken!, Func)
                : Func();
            var result = response == null ? CultureInfo.InvariantCulture : new CultureInfo(response);
            return result;

            string? Func() => CallTransactionService();
            }

        private string? CallTransactionService()
            {
            OutputToConsoleDetailsOfTheJob();

            var result = this._transactionServices.SoapClient.GetServiceCulture();
            return result;
            }

        private void OutputToConsoleDetailsOfTheJob()
            {
            var jobSpecifics = "Getting service culture";
            this._transactionServices.LogDetailsOfTheJob(jobSpecifics);
            }
        }
    }
