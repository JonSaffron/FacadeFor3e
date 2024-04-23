using System;
using System.Linq;
using System.Net;
using System.Security.Principal;
using FacadeFor3e.ProcessCommandBuilder;
using JetBrains.Annotations;

namespace FacadeFor3e
    {
    /// <summary>
    /// Connects to the 3E transaction service and allows processes to be run
    /// </summary>
    [PublicAPI]
    public sealed class ExecuteProcess : ExecuteProcessService, IDisposable
        {
        /// <summary>
        /// Constructs a new ExecuteProcess object without impersonation or user credentials
        /// </summary>
        /// <param name="endpoint">The url to use to connect to the 3E transaction service</param>
        public ExecuteProcess(Uri endpoint)
            : base(new TransactionServices(endpoint))
            {
            }

        /// <summary>
        /// Constructs a new ExecuteProcess object which will impersonate the specified account during calls to the 3E transaction service
        /// </summary>
        /// <param name="endpoint">The url to use to connect to the 3E transaction service</param>
        /// <param name="accountToImpersonate">The account to impersonate</param>
        public ExecuteProcess(Uri endpoint, WindowsIdentity accountToImpersonate)
            : base(new TransactionServices(endpoint, accountToImpersonate))
            {
            }

        /// <summary>
        /// Constructs a new ExecuteProcess object which will pass the specified credentials during calls to the 3E transaction service
        /// </summary>
        /// <param name="endpoint">The url to use to connect to the 3E transaction service</param>
        /// <param name="networkCredential">The credentials to use when calling the 3E transaction service</param>
        public ExecuteProcess(Uri endpoint, NetworkCredential networkCredential)
            : base(new TransactionServices(endpoint, networkCredential))
            {
            }

        /// <summary>
        /// Executes the specified process and (where appropriate) returns the primary key of the record affected
        /// </summary>
        /// <param name="process">The process to run</param>
        /// <param name="endpoint">Specifies the url of the transaction service</param>
        /// <returns>For a single Add operation, the primary key of the first record affected, or null otherwise</returns>
        public static string? Execute(ProcessCommand process, Uri endpoint)
            {
            if (process == null) throw new ArgumentNullException(nameof(process));
            using (var services = new TransactionServices(endpoint))
                {
                return ExecuteInternal(services.ExecuteProcess, process);
                }
            }

        /// <summary>
        /// Executes the specified process and (where appropriate) returns the primary key of the record affected
        /// </summary>
        /// <param name="process">The process to run</param>
        /// <param name="endpoint">Specifies the url of the transaction service</param>
        /// <param name="accountToImpersonate">The account details to impersonate</param>
        /// <returns>For a single Add operation, the primary key of the first record affected, or null otherwise</returns>
        public static string? Execute(ProcessCommand process, Uri endpoint, WindowsIdentity accountToImpersonate)
            {
            if (process == null) throw new ArgumentNullException(nameof(process));
            using (var services = new TransactionServices(endpoint, accountToImpersonate))
                {
                return ExecuteInternal(services.ExecuteProcess, process);
                }
            }

        /// <summary>
        /// Executes the specified process and (where appropriate) returns the primary key of the record affected
        /// </summary>
        /// <param name="process">The process to run</param>
        /// <param name="endpoint">Specifies the url of the transaction service</param>
        /// <param name="networkCredential">The credentials to use when calling the 3E transaction service</param>
        /// <returns>For a single Add operation, the primary key of the first record affected, or null otherwise</returns>
        public static string? Execute(ProcessCommand process, Uri endpoint, NetworkCredential networkCredential)
            {
            if (process == null) throw new ArgumentNullException(nameof(process));
            using (var services = new TransactionServices(endpoint, networkCredential))
                {
                return ExecuteInternal(services.ExecuteProcess, process);
                }
            }

        private static string? ExecuteInternal(ExecuteProcessService executeProcessService, ProcessCommand process)
            {
            bool getKey = process.Operations.Any(item => item is AddOperation);
            var executeProcessParams = getKey ? ExecuteProcessOptions.DefaultWithKeys : ExecuteProcessOptions.Default;
            ExecuteProcessResult runProcessResult = executeProcessService.Execute(process, executeProcessParams);

            string? result = getKey ? runProcessResult.GetKeys().FirstOrDefault() : null;
            return result;
            }

        /// <summary>
        /// Disposes this object
        /// </summary>
        public void Dispose()
            {
            this.TransactionServices.Dispose();
            }
        }
    }
