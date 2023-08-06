using System;
using System.IO;
using System.Security.Principal;
using System.ServiceModel;
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
        /// <remarks>If you are attaching larger files, you should increase the chunkSize to make the transfer more reliable</remarks>
        public SendAttachment(TransactionServices transactionServices, int chunkSize = 64 * 1024) // 64kb sized chunks by default
            {
            this._transactionServices = transactionServices ?? throw new ArgumentNullException(nameof(transactionServices));
            this.ChunkSize = chunkSize;
            }

        /// <summary>
        /// Get or sets the size in bytes of each chunk used to transfer files to 3E
        /// </summary>
        /// <remarks>To attach larger files, use a larger ChunkSize</remarks>
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
        /// <param name="fileTitle">The name to give the file attachment</param>
        /// <param name="fileContent">The file contents</param>
        /// <param name="fileLength">The length of the file being attached. This parameter can be left unspecified if the stream supports seeking.</param>
        public void AttachNewFile(string archetypeId, Guid itemId, string fileTitle, Stream fileContent, long? fileLength = null)
            {
            if (archetypeId == null) throw new ArgumentNullException(nameof(archetypeId));
            if (fileTitle == null) throw new ArgumentNullException(nameof(fileTitle));
            if (fileContent == null) throw new ArgumentNullException(nameof(fileContent));

            if (fileLength == null)
                {
                if (!fileContent.CanSeek)
                    throw new InvalidOperationException("Cannot seek within the stream - please specify a value for the totalFileLength parameter.");
                fileLength = fileContent.Length;
                }

            if (fileContent.CanSeek && fileContent.Position != 0)
                {
                fileContent.Seek(0, SeekOrigin.Begin);
                }

            OutputToConsoleDetailsOfTheJob(archetypeId, itemId, fileTitle, fileLength.Value);

            string syncId = GetOrGenerateSyncId();
            int chunkSize = this.ChunkSize;
            long totalChunksNeeded = (fileLength.Value + chunkSize - 1) / chunkSize;
            for (long i = 0; i < totalChunksNeeded; i++)
                {
                long offset = i * chunkSize;
                int chunkLength = (int) Math.Min(fileLength.Value - offset, chunkSize);
                var buffer = new byte[chunkLength];

                int positionInBuffer = 0;
                do
                    {
                    int bytesToRead = chunkLength - positionInBuffer;
                    int bytesRead = fileContent.Read(buffer, positionInBuffer, bytesToRead);
                    if (bytesRead == 0)
                        throw new InvalidOperationException("Unexpected end of stream encountered.");
                    positionInBuffer += bytesRead;
                    } while (positionInBuffer < chunkLength);

                Action action;
                if (i == 0)
                    {
                    action = () => CallTransactionServiceForStartOfTransfer(syncId, archetypeId, itemId, fileTitle, buffer, fileLength.Value);
                    }
                else
                    {
                    System.Threading.Thread.Sleep(250); // Slight delay to allow 3E to finish writing the last chunk
                    action = () => CallTransactionServiceForContinuingTransfer(syncId, fileTitle, buffer, offset, fileLength.Value);
                    }
                
                this._transactionServices.LogForDebug($"- Chunk {i + 1:D0} of {totalChunksNeeded:D0}");
                if (this._transactionServices.IsImpersonating)
                    {
                    // ReSharper disable AssignNullToNotNullAttribute
                    WindowsIdentity.RunImpersonated(this._transactionServices.AccountToImpersonate!.AccessToken, action);
                    // ReSharper restore AssignNullToNotNullAttribute
                    }
                else
                    {
                    action();
                    }
                }
            this._transactionServices.LogForDebug($"{fileTitle} successfully attached");
            }

        private void CallTransactionServiceForStartOfTransfer(string syncId, string archetypeId, Guid itemId, string fileTitle, byte[] data, long totalFileLength)
            {
            var ts = this._transactionServices.SoapClient;
            int chunkLength = data.GetLength(0);
            this._transactionServices.LogForDebug($"  sending {(chunkLength == totalFileLength ? "all" : "initial")} content - {chunkLength:N0} bytes");

            int retry = 0;

        retryLoop:

            try
                {
                ts.SendAttachment(itemId.ToString(), archetypeId, syncId, fileTitle, data, 0, chunkLength, totalFileLength);
                }
            catch (FaultException ex) when (ex.Message != null && ex.Message.StartsWith("Failed to write file") && retry < 5)
                {
                this._transactionServices.LogForDebug($"Failed to write attachment chunk: {ex.Message}");
                this._transactionServices.LogForDebug($"Waiting and retrying");
                retry++;
                System.Threading.Thread.Sleep(retry * 1000);
                goto retryLoop;
                }
            }

        private void CallTransactionServiceForContinuingTransfer(string syncId, string fileTitle, byte[] data, long offset, long totalFileLength)
            {
            var ts = this._transactionServices.SoapClient;
            int chunkLength = data.GetLength(0);
            bool isFinalChunk = (chunkLength + offset) == totalFileLength;
            this._transactionServices.LogForDebug($"  sending {(isFinalChunk ? "final" : "additional")} content - {chunkLength:N0} bytes at {offset:N0}");
            
            int retry = 0;

        retryLoop:

            try
                {
                ts.SendAttachmentChunk(syncId, fileTitle, data, offset, chunkLength, totalFileLength);
                }
            catch (FaultException ex) when (ex.Message != null && ex.Message.StartsWith("Failed to write file") && retry < 5)
                {
                this._transactionServices.LogForDebug($"Failed to write attachment chunk: {ex.Message}");
                this._transactionServices.LogForDebug($"Waiting and retrying");
                retry++;
                System.Threading.Thread.Sleep(retry * 1000);
                goto retryLoop;
                }
            }

        private void OutputToConsoleDetailsOfTheJob(string archetypeId, Guid itemId, string fileTitle, long fileLength)
            {
            var jobSpecifics = $"Attaching '{fileTitle}' ({fileLength >> 10}KB) to {archetypeId} with id {itemId}";
            this._transactionServices.LogDetailsOfTheJob(jobSpecifics);
            }

        private string GetOrGenerateSyncId()
            {
            if (this.SyncId != null)
                return this.SyncId;
            _countOfUploads++;
            var result = $"Upload {_countOfUploads} at {DateTime.Now:yyyy-MMM-dd hh:mm:ss} by FacadeFor3E";
            return result;
            }
        }
    }
