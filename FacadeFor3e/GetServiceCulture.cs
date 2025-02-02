using System;
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
        private CultureInfo? _cultureInfo;

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
            if (this._cultureInfo != null)
                {
                return this._cultureInfo;
                }

            string? Func() => CallTransactionService();
            var response = this._transactionServices.IsImpersonating
                // ReSharper disable AssignNullToNotNullAttribute
                ? WindowsIdentity.RunImpersonated(this._transactionServices.AccountToImpersonate!.AccessToken, Func)
                // ReSharper restore AssignNullToNotNullAttribute
                : Func();
            this._cultureInfo = response == null ? CultureInfo.InvariantCulture : new CultureInfo(response);
            TransactionServices.LogForDebug($"ServiceCulture = {this._cultureInfo}");
            return this._cultureInfo;
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
