using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Xml;
using System.Globalization;
using System.Linq;
using JetBrains.Annotations;
using FacadeFor3e.TransactionService;

namespace FacadeFor3e
    {
    /// <summary>
    /// Executes a process
    /// </summary>
    [PublicAPI]
    public static class RunProcess
        {
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

            var rp = new RunProcessParameters(process)
                        {
                            GetKey = (process.Operations.Count == 1 && process.Operations[0] is OperationAdd),
                            AccountToImpersonate = wi,
                            EndpointName = endpointName,
                            ThrowExceptionIfProcessDoesNotComplete = true
                        };

            var r = ExecuteProcess(rp);

            string result = rp.GetKey ? r.NewKey : null;
            return result;
            }

        public static RunProcessResult ExecuteProcess(RunProcessParameters runProcessParameters)
            {
            if (runProcessParameters == null)
                throw new ArgumentNullException("runProcessParameters");
            if (runProcessParameters.Request == null)
                throw new InvalidOperationException("Request is not set.");

            string response = CallTransactionService(runProcessParameters);

            var resultsDoc = new XmlDocument();
            resultsDoc.LoadXml(response);
            var r = new RunProcessResult(resultsDoc);
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
                    processException = ProcessExceptionBuilder.BuildForDataError(runProcessParameters, r);
                    if (processException == null && runProcessParameters.ThrowExceptionIfProcessDoesNotComplete)
                        processException = new ProcessException("The process did not complete and will appear on action list.", runProcessParameters, r);
                    }
                else if (result == "Success")
                    {
                    // process completed - you can still get data errors and a next message though
                    processException = ProcessExceptionBuilder.BuildForDataError(runProcessParameters, r);
                    }
                else if (result == "Failure")
                    {
                    processException = ProcessExceptionBuilder.BuildForProcessError(runProcessParameters, r);
                    }
                else
                    {
                    throw new InvalidOperationException("Unexpected result value in ProcessExecutionResults.");
                    }
                }
            catch
                {
                Trace.WriteLine(response);
                processException = new ProcessException("An error occurred that was not dealt with appropriately.", runProcessParameters, r);
                }

            if (processException != null)
                throw processException;

            if (runProcessParameters.GetKey)
                r.SetKey(runProcessParameters.ObjectName);

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

        private static string CallTransactionService(RunProcessParameters p)
            {
            string result;
            WindowsImpersonationContext impersonationContext = null;

            try
                {
                if (p.AccountToImpersonate != null)
                    impersonationContext = p.AccountToImpersonate.Impersonate();

                var ts = string.IsNullOrWhiteSpace(p.EndpointName) ? new TransactionServiceSoapClient() : new TransactionServiceSoapClient(p.EndpointName);
                if (ts.ClientCredentials != null)
                    ts.ClientCredentials.Windows.AllowedImpersonationLevel = TokenImpersonationLevel.Identification;
                
                var sb = new StringBuilder();
                sb.AppendFormat("Executing 3e process {0}:-", p.ProcessName);
                sb.AppendLine();
                sb.AppendFormat("\tURL: {0}", ts.Endpoint.Address);
                sb.AppendLine();
                sb.AppendFormat("\tIdentity: {0}", p.AccountToImpersonate == null ? (GetCurrentWindowsIdentity() ?? "<none>") : string.Format("Impersonating {0}", p.AccountToImpersonate.Name));
                sb.AppendLine();
                sb.Append(p.Request.PrettyPrintXml());
                Trace.WriteLine(sb.ToString());

// ReSharper disable BitwiseOperatorOnEnumWithoutFlags
                var returnInfoType = (int) ((p.GetKey ? ReturnInfoType.Keys : ReturnInfoType.None) | ReturnInfoType.Timing);
// ReSharper restore BitwiseOperatorOnEnumWithoutFlags

                try
                    {
                    result = ts.ExecuteProcess(p.Request.OuterXml, returnInfoType);
                    }
                catch (EndpointNotFoundException ex)
                    {
                    throw new ProcessException("The 3e server did not respond - it could be unavailable or there could be a communication problem.", ex, p, null);
                    }
                finally
                    {
                    try
                        {
                        ts.Close();     // sadly, calling close can throw an exception
                        }
                    catch (CommunicationException)
                        {
                        ts.Abort();
                        }
                    catch (TimeoutException)
                        {
                        ts.Abort();
                        }
                    catch
                        {
                        ts.Abort();
                        throw;
                        }
                    }

                if (impersonationContext != null)
                    impersonationContext.Undo();
                }
            finally
                {
                if (impersonationContext != null)
                    impersonationContext.Dispose();
                }

            return result;
            }

        private static string PrettyPrintXml(this XmlDocument xmlDoc)
            {
            string result;
            using (var stringWriter = new StringWriter(CultureInfo.InvariantCulture))
                {
                var xmlNodeReader = new XmlNodeReader(xmlDoc);
                var xmlTextWriter = new XmlTextWriter(stringWriter)
                                        {   //set formatting options
                                            Formatting = Formatting.Indented,
                                            Indentation = 1,
                                            IndentChar = '\t'
                                        };

                // write the document formatted
                xmlTextWriter.WriteNode(xmlNodeReader, true);
                result = stringWriter.ToString();
                }
            return result;
            }
        }
    }
