using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Xml;
using System.Linq;
using FacadeFor3e.ProcessCommandBuilder;
using FacadeFor3e.TransactionService;
using JetBrains.Annotations;

namespace FacadeFor3e
    {
    /// <summary>
    /// Executes a <see cref="ProcessCommand">ProcessCommand</see>
    /// </summary>
    [PublicAPI]
    public class ExecuteProcessService
        {
        /// <summary>
        /// The TransactionServices object that will interface to 3E
        /// </summary>
        protected TransactionServices TransactionServices;

        /// <summary>
        /// Gets or sets whether to throw an exception if the response contains details of data errors
        /// </summary>
        public bool ThrowExceptionIfDataErrorsFound { get; set; } = true;

        /// <summary>
        /// Gets or sets whether to throw an exception if the process doesn't complete
        /// </summary>
        public bool ThrowExceptionIfProcessDoesNotComplete { get; set; } = true;

        /// <summary>
        /// Gets or sets whether to return the primary keys of the top-level records that remain in the process worklist when the process finishes
        /// </summary>
        public bool GetKeys { get; set; } = true;

        /// <summary>
        /// Constructs a new object for executing a <see cref="ProcessCommand">ProcessCommand</see>
        /// </summary>
        internal ExecuteProcessService(TransactionServices transactionServices)
            {
            this.TransactionServices = transactionServices ?? throw new ArgumentNullException(nameof(transactionServices));
            }

        /// <summary>
        /// Executes the specified ProcessCommand and returns the result
        /// </summary>
        /// <param name="process">The ProcessCommand object to execute</param>
        /// <returns>An <see cref="ExecuteProcessResult">ExecuteProcessResult</see> object</returns>
        public ExecuteProcessResult Execute(ProcessCommand process)
            {
            if (process == null)
                throw new ArgumentNullException(nameof(process));
            ValidateProcess(process);

            var request = process.GenerateCommand();
            var result = Execute(request);
            return result;
            }

        /// <summary>
        /// Executes a process command defined by an XmlDocument and returns the result
        /// </summary>
        /// <param name="request">The process document to execute</param>
        /// <returns>An <see cref="ExecuteProcessResult">ExecuteProcessResult</see> object</returns>
        public ExecuteProcessResult Execute(XmlDocument request)
            {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            string Func() => CallTransactionService(request);
            string response = this.TransactionServices.IsImpersonating
                // ReSharper disable AssignNullToNotNullAttribute
                ? WindowsIdentity.RunImpersonated(this.TransactionServices.AccountToImpersonate!.AccessToken, Func)
                // ReSharper restore AssignNullToNotNullAttribute
                : Func();

            if (response.StartsWith("<ProcessExecutionResults ") && response.EndsWith("</ProcessExecutionResults>"))
                {
                return ProcessResponseFromTransactionService(request, response);
                }

            this.TransactionServices.LogForError(response);
            var processException = new ExecuteProcessException("An invalid response was returned from the web server:\r\n" + response);
            throw processException;
            }

        private ExecuteProcessResult ProcessResponseFromTransactionService(XmlDocument request, string response)
            {
            var resultsDoc = new XmlDocument();
            resultsDoc.LoadXml(response);
            string responseFormatted = resultsDoc.PrettyPrintXml();

            var result = new ExecuteProcessResult(request, resultsDoc);
            if (result.ExecutionResult == "Failure")
                {
                this.TransactionServices.LogForError(responseFormatted);
                var processException = ExecuteProcessExceptionBuilder.BuildForProcessError(result);
                throw processException;
                }

            if (result.ExecutionResult == "Interface" || result.ExecutionResult == "Success")
                {
                if (this.ThrowExceptionIfDataErrorsFound && result.HasDataError)
                    {
                    this.TransactionServices.LogForError(responseFormatted);
                    var processException = ExecuteProcessExceptionBuilder.BuildForDataError(result);
                    throw processException;
                    }

                if (result.ExecutionResult == "Interface" && this.ThrowExceptionIfProcessDoesNotComplete)
                    {
                    this.TransactionServices.LogForError(responseFormatted);
                    var processException = new ExecuteProcessException("The process did not complete and will appear on action list.", result);
                    throw processException;
                    }

                this.TransactionServices.LogForDebug(responseFormatted);
                return result;
                }

            throw new InvalidOperationException("Unexpected result value in ProcessExecutionResults: " + result.ExecutionResult);
            }

        private static void ValidateProcess(ProcessCommand process)
            {
            if (!process.Operations.Any())
                {
                throw new InvalidOperationException("There are no operations to carry out for process " + process.ProcessCode);
                }

            // this is a rule imposed by the transaction service
            if (process.Operations.OfType<DeleteOperation>().Any(op => op.KeySpecification is IdentifyByPosition || op.KeySpecification is IdentifyByValue)
                || process.Operations.OfType<EditOperation>().Any(op => op.KeySpecification is IdentifyByPosition || op.KeySpecification is IdentifyByValue))
                throw new InvalidOperationException("Cannot use KeyField or by Position operations at the top level.");

            CheckForCircularReferences(process);
            }

        private static void CheckForCircularReferences(DataObject da)
            {
            CheckForCircularReferences(da, new HashSet<DataObject>());
            }

        private static void CheckForCircularReferences(DataObject da, HashSet<DataObject> hierarchy)
            {
            if (!hierarchy.Add(da))
                throw new InvalidOperationException("There is a circular reference within the Process definition.");

            foreach (var childOp in da.Operations.OfType<OperationWithAttributesBase>())
                {
                foreach (var childDa in childOp.Children)
                    {
                    CheckForCircularReferences(childDa, hierarchy);
                    }
                }
            }

        private string CallTransactionService(XmlDocument request)
            {
            var ts = this.TransactionServices.GetSoapClient();
            OutputToConsoleDetailsOfTheJob(request);
            this.TransactionServices.LogForDebug(request.PrettyPrintXml());

            // ReSharper disable once BitwiseOperatorOnEnumWithoutFlags
            var returnInfoType = (int) ((this.GetKeys ? ReturnInfoType.Keys : ReturnInfoType.None) | ReturnInfoType.Timing);
            string result = ts.ExecuteProcess(request.OuterXml, returnInfoType)!;
            return result;
            }

        private void OutputToConsoleDetailsOfTheJob(XmlDocument request)
            {
            string jobSpecifics = $"Executing 3E process {request.DocumentElement?.LocalName}";
            this.TransactionServices.LogDetailsOfTheJob(jobSpecifics);
            }
        }
    }
