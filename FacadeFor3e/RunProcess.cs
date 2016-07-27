using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Xml;
using System.Linq;
using JetBrains.Annotations;
using FacadeFor3e.TransactionService;

namespace FacadeFor3e
    {
    /// <summary>
    /// Executes a process
    /// </summary>
    [PublicAPI]
    public class RunProcess : IDisposable
        {
        /// <summary>
        /// Gets or sets the account to impersonate during the process
        /// </summary>
        public WindowsIdentity AccountToImpersonate { get; set; }

        /// <summary>
        /// Gets or sets the endpoint to use to connect to the 3e server
        /// </summary>
        public string EndpointName { get; set; }

        /// <summary>
        /// Gets or sets whether to throw an exception if the process doesn't complete
        /// </summary>
        public bool ThrowExceptionIfProcessDoesNotComplete { get; set; }

        /// <summary>
        /// Gets or sets whether to return the primary keys of the top-level records remaining in the worklist when the process finishes
        /// </summary>
        public bool GetKeys { get; set; }

        private TransactionServiceSoapClient _transactionServiceSoapClient;

        public RunProcess()
            {
            this.ThrowExceptionIfProcessDoesNotComplete = true;
            this.GetKeys = true;
            }

        /// <summary>
        /// Executes the specified process and (where appropriate) returns the primary key of the record affected
        /// </summary>
        /// <param name="process">The process to run</param>
        /// <param name="wi">The account details to impersonate</param>
        /// <param name="endpointName">Specifies which endpoint </param>
        /// <returns>The primary key of the record affected</returns>
        public static string ExecuteProcess(Process process, WindowsIdentity wi = null, string endpointName = null)
            {
            if (process == null)
                throw new ArgumentNullException("process");
            ValidateProcess(process);
            bool getKey = process.Operations.Count == 1 && process.Operations[0] is OperationAdd;

            RunProcessResult runProcessResult;
            using (var rp = new RunProcess())
                {
                rp.AccountToImpersonate = wi;
                rp.EndpointName = endpointName;
                rp.ThrowExceptionIfProcessDoesNotComplete = true;

                runProcessResult = rp.Execute(process);
                }

            string result = getKey ? runProcessResult.GetKeys().FirstOrDefault() : null;
            return result;
            }

        public RunProcessResult Execute(Process process)
            {
            if (process == null)
                throw new ArgumentNullException("process");
            ValidateProcess(process);

            var request = process.GenerateCommand();
            var result = Execute(request);
            return result;
            }

        public RunProcessResult Execute(XmlDocument request)
            {
            if (request == null)
                throw new ArgumentNullException("request");

            string response = CallTransactionService(request);

            var resultsDoc = new XmlDocument();
            resultsDoc.LoadXml(response);
            var r = new RunProcessResult(request, resultsDoc);
            string responseFormatted = resultsDoc.PrettyPrintXml();
            Trace.WriteLine(responseFormatted);

            ProcessException processException;
            try
                {
                XmlElement root = resultsDoc.DocumentElement;
                if (root == null)
                    throw new InvalidOperationException();
                string result = root.GetAttribute("Result");
                if (result == "Interface")
                    {
                    // process did not complete
                    processException = ProcessExceptionBuilder.BuildForDataError(r);
                    if (processException == null && this.ThrowExceptionIfProcessDoesNotComplete)
                        processException = new ProcessException("The process did not complete and will appear on action list.", r);
                    }
                else if (result == "Success")
                    {
                    // process completed - you can still get data errors and a next message though
                    processException = ProcessExceptionBuilder.BuildForDataError(r);
                    }
                else if (result == "Failure")
                    {
                    processException = ProcessExceptionBuilder.BuildForProcessError(r);
                    }
                else
                    {
                    throw new InvalidOperationException("Unexpected result value in ProcessExecutionResults.");
                    }
                }
            catch
                {
                Trace.WriteLine(response);
                processException = new ProcessException("An error occurred that was not dealt with appropriately.", r);
                }

            if (processException != null)
                throw processException;

            return r;
            }

        private static void ValidateProcess(Process process)
            {
            if (!process.Operations.Any())
                {
                throw new InvalidOperationException("There are no operations to carry out for process " + process.ProcessName);
                }
            if (process.Operations.OfType<DeleteByPosition>().Any() || process.Operations.OfType<DeleteByKeyField>().Any()
                || process.Operations.OfType<EditByPosition>().Any() || process.Operations.OfType<EditByKeyField>().Any())
                throw new InvalidOperationException("Cannot use KeyField or by Position operations at the top level.");

            CheckForCircularReferences(process);
            }

        private static void CheckForCircularReferences(DataObject da, HashSet<DataObject> hierarchy = null)
            {
            if (hierarchy == null)
                hierarchy = new HashSet<DataObject>();

            if (!hierarchy.Add(da))
                throw new InvalidOperationException("There is a circular reference within the Process definition.");

            foreach (var childOp in da.Operations.OfType<OperationWithAttributesBase>())
                foreach (var childDa in childOp.Children)
                    CheckForCircularReferences(childDa, hierarchy);
            }

        private static string GetCurrentWindowsIdentity()
            {
            string result;
            using (var wi = WindowsIdentity.GetCurrent())
                result = wi.Name;
            return result;
            }

        private TransactionServiceSoapClient GetSoapClient()
            {
            var result = this._transactionServiceSoapClient;
            if (result == null)
                {
                result = string.IsNullOrWhiteSpace(this.EndpointName) ? new TransactionServiceSoapClient() : new TransactionServiceSoapClient(this.EndpointName);
                if (result.ClientCredentials != null)
                    result.ClientCredentials.Windows.AllowedImpersonationLevel = TokenImpersonationLevel.Identification;
                this._transactionServiceSoapClient = result;
                }
            return result;
            }

        private string CallTransactionService(XmlDocument request)
            {
            string result;

            var ts = GetSoapClient();

            using (this.AccountToImpersonate != null ? this.AccountToImpersonate.Impersonate() : null)
                {
                OutputToConsoleDetailsOfTheJob(request, ts);    

                try
                    {
                    // ReSharper disable once BitwiseOperatorOnEnumWithoutFlags
                    var returnInfoType = (int) ((this.GetKeys ? ReturnInfoType.Keys : ReturnInfoType.None) | ReturnInfoType.Timing);
                    result = ts.ExecuteProcess(request.OuterXml, returnInfoType);
                    }
                catch (EndpointNotFoundException ex)
                    {
                    throw new ProcessException("The 3e server did not respond - it could be unavailable or there could be a communication problem.", ex, null);
                    }
                finally
                    {
                    ForceClose(ts);
                    }
                }

            return result;
            }

        private void OutputToConsoleDetailsOfTheJob(XmlDocument request, TransactionServiceSoapClient ts)
            {
            var sb = new StringBuilder();
            // ReSharper disable once PossibleNullReferenceException
            sb.AppendFormat("Executing 3e process {0}:-", request.DocumentElement.LocalName);
            sb.AppendLine();
            sb.AppendFormat("\tURL: {0}", ts.Endpoint.Address);
            sb.AppendLine();
            sb.AppendFormat("\tIdentity: {0}", this.AccountToImpersonate == null ? GetCurrentWindowsIdentity() : string.Format("Impersonating {0}", this.AccountToImpersonate.Name));
            sb.AppendLine();
            sb.Append(request.PrettyPrintXml());
            Trace.WriteLine(sb.ToString());
            }

        public void Dispose()
            {
            if (this._transactionServiceSoapClient != null)
                {
                ForceClose(this._transactionServiceSoapClient);
                }
            this._transactionServiceSoapClient = null;
            }

        // Based on code from https://msdn.microsoft.com/en-us/library/aa355056.aspx "Window Communication Foundation Samples" "Avoiding Problems with the Using Statement"
        private static void ForceClose(TransactionServiceSoapClient ts)
            {
            try
                {
                ts.Close();     // sadly, calling close can throw an exception
                }
            catch
                {
                ts.Abort();
                }
            }
        }
    }
