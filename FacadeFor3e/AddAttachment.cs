using System;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using JetBrains.Annotations;
using FacadeFor3e.TransactionService;

namespace FacadeFor3e
    {
    [PublicAPI]
    public class AddAttachment : IDisposable
        {
        public WindowsIdentity AccountToImpersonate { get; set; }
        public string EndpointName { get; set; }

        private int _chunkSize;
        private string _syncId;
        private TransactionServiceSoapClient _transactionServiceSoapClient;
        private static int _countOfUploads;

        public AddAttachment()
            {
            this.ChunkSize = 32 * 1024;         // 32kb sized chunks by default
            }

        public int ChunkSize
            {
            get { return this._chunkSize; }
            set
                {
                const int oneMegabyte = 1024 * 1024;
                if (value <= 0 || value > oneMegabyte)
                    throw new ArgumentOutOfRangeException("value");
                this._chunkSize = value;
                }
            }

        public string SyncId
            {
            get { return this._syncId; }
            set
                {
                if (value != null && (value.Length > 128 || value.Length != value.TrimEnd().Length))
                    throw new ArgumentOutOfRangeException("value");
                this.SyncId = value;
                }
            }

        /// <summary>
        /// Adds the content provided to a 3e record as an attachment
        /// </summary>
        /// <param name="syncId">A reasonably unique value that can identify the upload during repeated calls to the 3e framework</param>
        /// <param name="archetypeId">The name of the archetype that the attachment is associated with</param>
        /// <param name="itemId">The id of the record within the specified archetype that the attachment is associated with</param>
        /// <param name="originalFileName">The original name of the file being uploaded. Do not include path data.</param>
        /// <param name="fileContent">The content of the file</param>
        /// <param name="accountToImpersonate">Account to impersonate whilst performing the upload. Can be null.</param>
        /// <param name="endpointName">The name of the endpoint to use to connect to 3e. Can be null, in which case there must be only one endpoint defined.</param>
        public static void AddFile(string syncId, string archetypeId, Guid itemId, string originalFileName,
            byte[] fileContent, WindowsIdentity accountToImpersonate, string endpointName)
            {
            using (var addAttachment = new AddAttachment())
                {
                addAttachment.SyncId = syncId;
                addAttachment.AccountToImpersonate = accountToImpersonate;
                addAttachment.EndpointName = endpointName;

                addAttachment.AttachNewFile(archetypeId, itemId, originalFileName, fileContent);
                }
            }

        private string GetOrGenerateSyncId()
            {
            var result = this.SyncId ?? string.Format("Attachment upload {0} at {1:yyyy-MMM-dd hh:mm:ss}", _countOfUploads, DateTime.Now);
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

        public void AttachNewFile(string archetypeId, Guid itemId, string originalFileName, byte[] fileContent)
            {
            var ts = GetSoapClient();

            using (this.AccountToImpersonate != null ? this.AccountToImpersonate.Impersonate() : null)
                {
                OutputToConsoleDetailsOfTheJob(archetypeId, itemId, originalFileName, ts);    

                try
                    {
                    SendData(archetypeId, itemId, originalFileName, fileContent, ts);
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

        private void OutputToConsoleDetailsOfTheJob(string archetypeId, Guid itemId, string originalFileName, TransactionServiceSoapClient ts)
            {
            var sb = new StringBuilder();
            sb.AppendFormat("Adding attachment '{0}'", originalFileName);
            sb.AppendLine();
            sb.AppendFormat("For {0}.{0}ID = {1}", archetypeId, itemId);
            sb.AppendFormat("\tURL: {0}", ts.Endpoint.Address);
            sb.AppendLine();
            sb.AppendFormat("\tIdentity: {0}", this.AccountToImpersonate == null ? GetCurrentWindowsIdentity() : string.Format("Impersonating {0}", AccountToImpersonate.Name));
            sb.AppendLine();
            System.Diagnostics.Trace.WriteLine(sb.ToString());
            }

        private void SendData(string archetypeId, Guid itemId, string originalFileName, byte[] fileContent, TransactionServiceSoapClient ts)
            {
            int totalBytes = fileContent.GetLength(0);
            _countOfUploads += 1;
            string syncId = GetOrGenerateSyncId();
            var buffer = new byte[this._chunkSize];
            for (int offset = 0; offset < totalBytes; offset += this._chunkSize)
                {
                int chunkLength = Math.Min(totalBytes - offset, this._chunkSize);

                Buffer.BlockCopy(fileContent, offset, buffer, 0, chunkLength);
                if (offset == 0)
                    {
                    System.Diagnostics.Trace.WriteLine(string.Format("Sending initial content - {0:N} bytes", chunkLength));
                    ts.SendAttachment(itemId, archetypeId, syncId, originalFileName, buffer, offset, chunkLength, totalBytes);
                    }
                else
                    {
                    System.Diagnostics.Trace.WriteLine(string.Format("Sending additional content - {0:N} bytes at {1:N}", chunkLength, offset));
                    ts.SendAttachmentChunk(syncId, originalFileName, buffer, offset, chunkLength, totalBytes);
                    }
                }
            System.Diagnostics.Trace.WriteLine(string.Format("Completed - {0:N} total bytes", totalBytes));
            }

        [NotNullAttribute]
        private static string GetCurrentWindowsIdentity()
            {
            string result;
            using (var wi = WindowsIdentity.GetCurrent())
                result = wi.Name;
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
