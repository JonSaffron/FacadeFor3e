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
        /// <param name="options">Specifies options that control the request and how the response is handled</param>
        /// <returns>An <see cref="ExecuteProcessResult">ExecuteProcessResult</see> object</returns>
        public ExecuteProcessResult Execute(ProcessCommand process, ExecuteProcessOptions options)
            {
            if (process == null)
                throw new ArgumentNullException(nameof(process));
            ValidateProcess(process);

            var renderer = new TransactionServiceRenderer();
            XmlDocument request = renderer.Render(process, options);
            var result = Execute(request, options);
            return result;
            }

        /// <summary>
        /// Executes a process command defined by an XmlDocument and returns the result
        /// </summary>
        /// <param name="request">The process document to execute</param>
        /// <param name="executeProcessParams">Flags to control the request and how the response is handled</param>
        /// <returns>An <see cref="ExecuteProcessResult">ExecuteProcessResult</see> object</returns>
        public ExecuteProcessResult Execute(XmlDocument request, ExecuteProcessOptions executeProcessParams)
            {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            string Func() => CallTransactionService(request, executeProcessParams);
            string response = this.TransactionServices.IsImpersonating
                // ReSharper disable AssignNullToNotNullAttribute
                ? WindowsIdentity.RunImpersonated(this.TransactionServices.AccountToImpersonate!.AccessToken, Func)
                // ReSharper restore AssignNullToNotNullAttribute
                : Func();

            if (response.StartsWith("<ProcessExecutionResults ") && response.EndsWith("</ProcessExecutionResults>"))
                {
                return ProcessResponseFromTransactionService(request, response, executeProcessParams);
                }

            this.TransactionServices.LogForError(response);
            var processException = new ExecuteProcessException("An invalid response was returned from the web server:\r\n" + response);
            throw processException;
            }

        private ExecuteProcessResult ProcessResponseFromTransactionService(XmlDocument request, string response, ExecuteProcessOptions executeProcessParams)
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
                if (executeProcessParams.ThrowExceptionIfDataErrorsFound && result.HasDataError)
                    {
                    this.TransactionServices.LogForError(responseFormatted);
                    var processException = ExecuteProcessExceptionBuilder.BuildForDataError(result);
                    throw processException;
                    }

                if (result.ExecutionResult == "Interface" && executeProcessParams.ThrowExceptionIfProcessDoesNotComplete)
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

        private string CallTransactionService(XmlDocument request, ExecuteProcessOptions processParams)
            {
            OutputToConsoleDetailsOfTheJob(request);
            this.TransactionServices.LogForDebug(request.PrettyPrintXml());

            // ReSharper disable once BitwiseOperatorOnEnumWithoutFlags
            var returnInfoType = (int) ((processParams.GetKeys ? ReturnInfoType.Keys : ReturnInfoType.None) | ReturnInfoType.Timing);
            string result = this.TransactionServices.SoapClient.ExecuteProcess(request.OuterXml, returnInfoType)!;
            return result;
            }

        private void OutputToConsoleDetailsOfTheJob(XmlDocument request)
            {
            string jobSpecifics = $"Executing 3E process {request.DocumentElement?.LocalName}";
            this.TransactionServices.LogDetailsOfTheJob(jobSpecifics);
            }
        }
    }
