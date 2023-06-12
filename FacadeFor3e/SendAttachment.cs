using System;
using System.Security.Principal;
using JetBrains.Annotations;

namespace FacadeFor3e
    {
    /// <summary>
    /// Allows a file to be added to a specified record, such as an engagement letter to a client record
    /// </summary>
    [PublicAPI]
    public class SendAttachment
        {
        private readonly TransactionServices _transactionServices;
        private int _chunkSize;
        private string? _syncId;
        private static int _countOfUploads;
        private const int OneMegabyte = 1024 * 1024;

        /// <summary>
        /// Constructs a new object to add attachments to a specified record
        /// </summary>
        /// <param name="transactionServices">The TransactionServices object to use to connect with 3E</param>
        /// <param name="chunkSize">The size in bytes of the chunks to transfer the file in</param>
        public SendAttachment(TransactionServices transactionServices, int chunkSize = 32 * 1024) // 32kb sized chunks by default
            {
            this._transactionServices = transactionServices ?? throw new ArgumentNullException(nameof(transactionServices));
            this.ChunkSize = chunkSize;
            }

        /// <summary>
        /// Get or sets the size in bytes of each chunk used to transfer files to 3E
        /// </summary>
        public int ChunkSize
            {
            get => this._chunkSize;
            private set
                {
                if (value <= 0 || value > OneMegabyte)
                    throw new ArgumentOutOfRangeException(nameof(value));
                this._chunkSize = value;
                }
            }

        /// <summary>
        /// Gets or sets a string used to identify a file transfer operation
        /// </summary>
        public string? SyncId
            {
            get => this._syncId;
            set
                {
                if (value != null && (value.Length > 128 || value.Length != value.TrimEnd().Length))
                    throw new ArgumentOutOfRangeException(nameof(value));
                this._syncId = value;
                }
            }

        /// <summary>
        /// Attaches a new file to the specified record
        /// </summary>
        /// <param name="archetypeId">The name of the archetype to attach the file to. The archetype must support attachments.</param>
        /// <param name="itemId">The ID of the record to attach the file to</param>
        /// <param name="originalFileName">The name to give the file attachment</param>
        /// <param name="fileContent">The file contents</param>
        public void AttachNewFile(string archetypeId, Guid itemId, string originalFileName, byte[] fileContent)
            {
            void Action() => CallTransactionService(archetypeId, itemId, originalFileName, fileContent);
            if (this._transactionServices.IsImpersonating)
                {
                // ReSharper disable AssignNullToNotNullAttribute
                WindowsIdentity.RunImpersonated(this._transactionServices.AccountToImpersonate!.AccessToken, Action);
                // ReSharper restore AssignNullToNotNullAttribute
                }
            else
                {
                Action();
                }
            }

        private void CallTransactionService(string archetypeId, Guid itemId, string originalFileName, byte[] fileContent)
            {
            var ts = this._transactionServices.GetSoapClient();
            OutputToConsoleDetailsOfTheJob(archetypeId, itemId, originalFileName);    

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
                    this._transactionServices.LogForDebug($"Sending initial content - {chunkLength:N} bytes");
                    ts.SendAttachment(itemId.ToString(), archetypeId, syncId, originalFileName, buffer, offset, chunkLength, totalBytes);
                    }
                else
                    {
                    this._transactionServices.LogForDebug($"Sending additional content - {chunkLength:N} bytes at {offset:N}");
                    ts.SendAttachmentChunk(syncId, originalFileName, buffer, offset, chunkLength, totalBytes);
                    }
                }
            this._transactionServices.LogForDebug($"Completed - {totalBytes:N} total bytes");
            }

        private void OutputToConsoleDetailsOfTheJob(string archetypeId, Guid itemId, string originalFileName)
            {
            var jobSpecifics = $"Adding attachment '{originalFileName}' to {archetypeId} with id {itemId}";
            this._transactionServices.OutputToConsoleDetailsOfTheJob(jobSpecifics);
            }

        private string GetOrGenerateSyncId()
            {
            var result = this.SyncId ?? $"Attachment upload {_countOfUploads} at {DateTime.Now:yyyy-MMM-dd hh:mm:ss}";
            return result;
            }
        }
    }
