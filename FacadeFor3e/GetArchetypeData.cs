using System;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Xml;
using FacadeFor3e.TransactionService;
using JetBrains.Annotations;

namespace FacadeFor3e
    {
    [PublicAPI]
    public class GetArchetypeData: IDisposable
        {
        public WindowsIdentity AccountToImpersonate { get; set; }
        public string EndpointName { get; set; }

        private TransactionServiceSoapClient _transactionServiceSoapClient;

        public static XmlDocument GetData(string xoql, WindowsIdentity accountToImpersonate, string endpointName)
            {
            using (var getArchetypeData = new GetArchetypeData())
                {
                getArchetypeData.AccountToImpersonate = accountToImpersonate;
                getArchetypeData.EndpointName = endpointName;

                var result = getArchetypeData.GetData(xoql);
                return result;
                }
            }

        public XmlDocument GetData(string xoql)
            {
            var ts = GetSoapClient();

            using (this.AccountToImpersonate != null ? this.AccountToImpersonate.Impersonate() : null)
                {
                OutputToConsoleDetailsOfTheJob(xoql, ts);

                try
                    {
                    var response = ts.GetArchetypeData(xoql);
                    var xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(response);
                    return xmlDoc;
                    }
                catch (EndpointNotFoundException ex)
                    {
                    throw new ProcessException("The 3e server did not respond - it could be unavailable or there could be a communication problem.", ex);
                    }
                finally
                    {
                    ForceClose(ts);
                    }
                }
            }

        private void OutputToConsoleDetailsOfTheJob(string xoql, TransactionServiceSoapClient ts)
            {
            var sb = new StringBuilder();
            sb.AppendFormat("Getting data: {0}", xoql);
            sb.AppendLine();
            sb.AppendFormat("\tURL: {0}", ts.Endpoint.Address);
            sb.AppendLine();
            sb.AppendFormat("\tIdentity: {0}", this.AccountToImpersonate == null ? GetCurrentWindowsIdentity() : string.Format("Impersonating {0}", AccountToImpersonate.Name));
            sb.AppendLine();
            System.Diagnostics.Trace.WriteLine(sb.ToString());
            }

        [NotNull]
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
