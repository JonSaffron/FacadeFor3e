using System;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using JetBrains.Annotations;
using FacadeFor3e.TransactionService;

namespace FacadeFor3e
    {
    [PublicAPI]
    public static class AddAttachment
        {
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
        public static void AddFile(string syncId, string archetypeId, Guid itemId, string originalFileName, byte[] fileContent, WindowsIdentity accountToImpersonate, string endpointName)
            {
            WindowsImpersonationContext impersonationContext = null;
            int totalBytes = fileContent.GetLength(0);
            const int chunkSize = 32 * 1024;      // 32Kb sized chunks

            try
                {
                if (accountToImpersonate != null)
                    impersonationContext = accountToImpersonate.Impersonate();

                var ts = string.IsNullOrWhiteSpace(endpointName) ? new TransactionServiceSoapClient() : new TransactionServiceSoapClient(endpointName);
                if (ts.ClientCredentials != null)
                    ts.ClientCredentials.Windows.AllowedImpersonationLevel = TokenImpersonationLevel.Identification;
                
                var sb = new StringBuilder();
                sb.AppendFormat("Adding attachment to 3e '{0}' at {1} bytes long", originalFileName, totalBytes);
                sb.AppendLine();
                sb.AppendFormat("For {0}.{0}ID = {1}", archetypeId, itemId);
                sb.AppendFormat("\tURL: {0}", ts.Endpoint.Address);
                sb.AppendLine();
                sb.AppendFormat("\tIdentity: {0}", accountToImpersonate == null ? (GetCurrentWindowsIdentity() ?? "<none>") : string.Format("Impersonating {0}", accountToImpersonate.Name));
                sb.AppendLine();
                System.Diagnostics.Trace.WriteLine(sb.ToString());

                var buffer = new byte[chunkSize];

                try
                    {
                    for (int offset = 0; offset < totalBytes; offset += chunkSize)
                        {
                        int length = Math.Min(totalBytes - offset, chunkSize);

                        Buffer.BlockCopy(fileContent, offset, buffer, 0, length);
                        if (offset == 0)
                            {
                            System.Diagnostics.Trace.WriteLine(string.Format("Sending initial content - {0} bytes", length));
                            ts.SendAttachment(itemId, archetypeId, syncId, originalFileName, buffer, offset, length, totalBytes);
                            }
                        else
                            {
                            System.Diagnostics.Trace.WriteLine(string.Format("Sending additional content - {0} bytes at {1}kb", length, offset / 1024));
                            ts.SendAttachmentChunk(syncId, originalFileName, buffer, offset, length, totalBytes);
                            }
                        }
                    }
                catch (EndpointNotFoundException ex)
                    {
                    throw new ProcessException("The 3e server did not respond - it could be unavailable or there could be a communication problem.", ex);
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
            }

        private static string GetCurrentWindowsIdentity()
            {
            string result;
            using (var wi = WindowsIdentity.GetCurrent())
                result = wi == null ? null : wi.Name;
            return result;
            }
        }
    }
