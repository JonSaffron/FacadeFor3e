using System;
using System.Security.Principal;
using FacadeFor3e.TransactionService;
using JetBrains.Annotations;

namespace FacadeFor3e
    {
    /// <summary>
    /// A service to return the value of a system option/override for a user
    /// </summary>
    [PublicAPI]
    public class GetOption
        {
        private readonly TransactionServices _transactionServices;

        internal GetOption(TransactionServices transactionServices)
            {
            _transactionServices = transactionServices ?? throw new ArgumentNullException(nameof(transactionServices));
            }

        /// <summary>
        /// Gets the value of a boolean system option
        /// </summary>
        /// <param name="optionName">The name of the system option</param>
        /// <returns>The value of the specified option</returns>
        public bool GetBooleanOption(string optionName)
            {
            // response is bool.ToString()
            var response = GetOptionRaw(optionName, DataTypeEnum.BOOLEAN);
            return bool.Parse(response);
            }

        /// <summary>
        /// Gets the value of an integer system option
        /// </summary>
        /// <param name="optionName">The name of the system option</param>
        /// <returns>The value of the specified option</returns>
        public int GetIntegerOption(string optionName)
            {
            // response is int.ToString()
            var response = GetOptionRaw(optionName, DataTypeEnum.INTEGER);
            var culture = this._transactionServices.GetServiceCulture();
            return int.Parse(response, culture);
            }

        /// <summary>
        /// Gets the value of a date system option
        /// </summary>
        /// <param name="optionName">The name of the system option</param>
        /// <returns>The value of the specified option</returns>
        public DateTime GetDateOption(string optionName)
            {
            // response is a datetime.ToLongDateString
            var response = GetOptionRaw(optionName, DataTypeEnum.DATE);
            var culture = this._transactionServices.GetServiceCulture();
            return DateTime.Parse(response, culture);
            }

        /// <summary>
        /// Gets the value of a string system option
        /// </summary>
        /// <param name="optionName">The name of the system option</param>
        /// <returns>The value of the specified option</returns>
        public string GetStringOption(string optionName)
            {
            var response = GetOptionRaw(optionName, DataTypeEnum.STRING);
            return response;
            }

        /// <summary>
        /// Gets the value of a TimeSpan system option
        /// </summary>
        /// <param name="optionName">The name of the system option</param>
        /// <returns>The value of the specified option</returns>
        public TimeSpan GetTimeSpanOption(string optionName)
            {
            // response is a TimeSpan.TotalSeconds.ToString
            var response = GetOptionRaw(optionName, DataTypeEnum.DATETIME);
            var culture = this._transactionServices.GetServiceCulture();
            double totalSeconds = double.Parse(response, culture);
            return TimeSpan.FromSeconds(totalSeconds);
            }

        /// <summary>
        /// Gets the value of a list/enumerated system option
        /// </summary>
        /// <param name="optionName">The name of the system option</param>
        /// <returns>The value of the specified option</returns>
        /// <remarks>The return value is an entry from NxGenericListItem</remarks>
        public Guid GetListOption(string optionName)
            {
            // response is Guid.ToString()
            var response = GetOptionRaw(optionName, DataTypeEnum.RELATIONSHIP);
            return Guid.Parse(response);
            }

        /// <summary>
        /// Gets the value of a system option
        /// </summary>
        /// <param name="optionName">The name of the system option</param>
        /// <param name="optionType">The type of the system option</param>
        /// <returns>The value of the specified option</returns>
        /// <remarks>Only certain values for optionType are valid</remarks>
        public string GetOptionRaw(string optionName, DataTypeEnum optionType)
            {
            if (optionName == null)
                throw new ArgumentNullException(nameof(optionName));
            
            string Func() => CallTransactionService(optionName, optionType);
            var result = this._transactionServices.IsImpersonating
                // ReSharper disable AssignNullToNotNullAttribute
                ? WindowsIdentity.RunImpersonated(this._transactionServices.AccountToImpersonate!.AccessToken, Func)
                // ReSharper restore AssignNullToNotNullAttribute
                : Func();

            TransactionServices.LogForDebug($"{optionName} = {result}");
            return result;
            }

        private string CallTransactionService(string optionName, DataTypeEnum optionType)
            {
            OutputToConsoleDetailsOfTheJob(optionName, optionType);

            var result = this._transactionServices.SoapClient.GetOption(optionName, optionType)!;
            return result;
            }

        private void OutputToConsoleDetailsOfTheJob(string optionName, DataTypeEnum optionType)
            {
            var jobSpecifics = $"Getting option: {optionName} ({optionType})";
            this._transactionServices.LogDetailsOfTheJob(jobSpecifics);
            }
        }
    }
