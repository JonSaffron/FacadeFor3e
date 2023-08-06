using System;
using System.Globalization;
using System.Net;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Xml;
using FacadeFor3e.TransactionService;
using JetBrains.Annotations;
using NLog;

namespace FacadeFor3e
    {
    /// <summary>
    /// Connects to the 3E transaction service and provides access to selected services
    /// </summary>
    [PublicAPI]
    public class TransactionServices : IDisposable
        {
        /// <summary>
        /// The credentials being passed when calling the 3E transaction service
        /// </summary>
        public NetworkCredential? NetworkCredential { get; }

        /// <summary>
        /// The WindowsIdentity being impersonated during calls to the 3E transaction service
        /// </summary>
        public WindowsIdentity? AccountToImpersonate { get; }

        /// <summary>
        /// The url to use to connect to the 3E transaction service 
        /// </summary>
        public Uri Endpoint { get; }

        /// <summary>
        /// Returns whether this object has ben disposed
        /// </summary>
        public bool IsDisposed { get; private set; }

        private readonly Lazy<TransactionServiceClient> _transactionServiceSoapClient;
        private ExecuteProcessService? _executeProcess;
        private SendAttachment? _sendAttachment;

        private readonly Lazy<Logger> _logger = new Lazy<Logger>(() => LogManager.GetCurrentClassLogger()!);

        /// <summary>
        /// Constructs a new TransactionServices object without impersonation or user credentials
        /// </summary>
        /// <param name="endpoint">The url to use to connect to the 3E transaction service</param>
        public TransactionServices(Uri endpoint)
            {
            this.Endpoint = endpoint ?? throw new ArgumentNullException(nameof(endpoint));
            this._transactionServiceSoapClient = new Lazy<TransactionServiceClient>(() => BuildSoapClient(endpoint, networkCredentials: null));
            }

        /// <summary>
        /// Constructs a new TransactionServices object which will impersonate the specified account during calls to the 3E transaction service
        /// </summary>
        /// <param name="endpoint">The url to use to connect to the 3E transaction service</param>
        /// <param name="accountToImpersonate">The account to impersonate</param>
        public TransactionServices(Uri endpoint, WindowsIdentity accountToImpersonate) : this(endpoint)
            {
            this.AccountToImpersonate = accountToImpersonate ?? throw new ArgumentNullException(nameof(accountToImpersonate));
            this._transactionServiceSoapClient = new Lazy<TransactionServiceClient>(() => BuildSoapClient(endpoint, networkCredentials: null));
            }

        /// <summary>
        /// Constructs a new TransactionServices object which will pass the specified credentials during calls to the transaction service
        /// </summary>
        /// <param name="endpoint">The url to use to connect to the 3E transaction service</param>
        /// <param name="networkCredential">The credentials to use when calling the 3E transaction service</param>
        public TransactionServices(Uri endpoint, NetworkCredential networkCredential) : this(endpoint)
            {
            this.NetworkCredential = networkCredential ?? throw new ArgumentNullException(nameof(networkCredential));
            this._transactionServiceSoapClient = new Lazy<TransactionServiceClient>(() => BuildSoapClient(endpoint, networkCredential));
            }

        /// <summary>
        /// Ping service
        /// </summary>
        public void Ping()
            {
            EnsureObjectIsNotDisposed();
            LogDetailsOfTheJob("Pinging the transaction service");
            this.SoapClient.Ping();
            }
        
        /// <summary>
        /// Runs the specified query against the 3E database
        /// </summary>
        /// <param name="xoql">Specifies the query to run</param>
        /// <returns>An XML representation of the resulting data</returns>
        /// <remarks>The query is run in the context of the user so row level security will be applied</remarks>
        public XmlDocument GetArchetypeData(XmlDocument xoql)
            {
            EnsureObjectIsNotDisposed();
            var getArchetypeData = new GetArchetypeData(this);
            var result = getArchetypeData.GetData(xoql);
            return result;
            }

        /// <summary>
        /// Returns the data from the specified presentation. 
        /// </summary>
        /// <param name="presentationId">Specifies the presentation to run. The report data must come from archetypes and not a metric.</param>
        /// <returns>An XML representation of the resulting data</returns>
        /// <remarks>The query is run in the context of the user so row level security will be applied</remarks>
        public XmlDocument GetDataFromPresentation(string presentationId)
            {
            EnsureObjectIsNotDisposed();
            var getDataFromPresentation = new GetDataFromPresentation(this);
            var result = getDataFromPresentation.GetData(presentationId);
            return result;
            }

        /// <summary>
        /// Gets an object that allows system options/overrides to be interrogated
        /// </summary>
        public GetOption GetOption
            {
            get
                {
                EnsureObjectIsNotDisposed();
                return new GetOption(this);
                }
            }

        /// <summary>
        /// Gets the culture in use by the 3E Transaction Service
        /// </summary>
        /// <returns>A CultureInfo object</returns>
        public CultureInfo GetServiceCulture()
            {
            EnsureObjectIsNotDisposed();
            var getServiceCulture = new GetServiceCulture(this);
            var result = getServiceCulture.Get();
            return result;
            }

        /// <summary>
        /// Gets an object that allows a specified process to be run to add, edit and delete records
        /// </summary>
        public ExecuteProcessService ExecuteProcess
            {
            get
                {
                EnsureObjectIsNotDisposed();
                return this._executeProcess ??= new ExecuteProcessService(this);
                }
            }

        /// <summary>
        /// Gets an objects that allows attachments to be added to records
        /// </summary>
        public SendAttachment SendAttachment
            {
            get
                {
                EnsureObjectIsNotDisposed();
                return this._sendAttachment ??= new SendAttachment(this);
                }
            }

        /// <summary>
        /// Gets whether or not impersonation is being used during calls to the 3E transaction service
        /// </summary>
        public bool IsImpersonating => this.AccountToImpersonate != null && this.AccountToImpersonate != WindowsIdentity.GetCurrent();

        // ReSharper disable once RedundantSuppressNullableWarningExpression (applies to .net 6 only)
        internal TransactionServiceClient SoapClient => this._transactionServiceSoapClient.Value!;

        // ReSharper disable once RedundantSuppressNullableWarningExpression (applies to .net 6 only)
        private Logger Logger => this._logger.Value!;

        private static TransactionServiceClient BuildSoapClient(Uri endPoint, NetworkCredential? networkCredentials)
            {
            bool isSecure = endPoint.Scheme.Equals(Uri.UriSchemeHttps);
            var binding = BuildBinding(useHttps: isSecure);
            var endpointAddress = new EndpointAddress(endPoint);
            var result = new TransactionServiceClient(binding, endpointAddress);    // ClientCredentials should be available once endpoint has been provided

            // Delegation is required for 3E version 3, earlier versions just needed Identification
            // ReSharper disable PossibleNullReferenceException
            result.ClientCredentials!.Windows.AllowedImpersonationLevel = TokenImpersonationLevel.Delegation;
            // ReSharper restore PossibleNullReferenceException

            if (networkCredentials != null)
                {
                // ReSharper disable PossibleNullReferenceException
                result.ClientCredentials!.Windows.ClientCredential = networkCredentials;
                // ReSharper restore PossibleNullReferenceException
                }

            return result;
            }

        internal void LogDetailsOfTheJob(string jobSpecifics)
            {
            if (jobSpecifics == null) throw new ArgumentNullException(nameof(jobSpecifics));
            var sb = new StringBuilder();
            sb.AppendFormat(jobSpecifics);
            sb.AppendLine();
            sb.AppendFormat("\tURL: {0}", this.SoapClient.Endpoint!.Address!);
            sb.AppendLine();
            string userName;
            string authenticationMethod;
            if (this.IsImpersonating)
                {
                userName = this.AccountToImpersonate!.Name;
                authenticationMethod = "using impersonation";
                }
            else if (this.NetworkCredential != null)
                {
                userName = $"{this.NetworkCredential.Domain}\\{this.NetworkCredential.UserName}";
                authenticationMethod = "using credentials";
                }
            else
                {
                userName = GetCurrentWindowsIdentity();
                authenticationMethod = "currently logged in user";
                }
            sb.AppendFormat("\tUser: {0} ({1})", userName, authenticationMethod);
            sb.AppendLine();
            this.Logger.Info(sb.ToString());
            }

        internal void LogForDebug(string message)
            {
            this.Logger.Debug(message);
            }

        internal void LogForError(string message)
            {
            this.Logger.Error(message);
            }

        private static BasicHttpBinding BuildBinding(bool useHttps)
            {
            var result = new BasicHttpBinding
                {
                CloseTimeout = TimeSpan.FromMinutes(1),
                OpenTimeout = TimeSpan.FromMinutes(1),
                SendTimeout = TimeSpan.FromMinutes(10),
                ReceiveTimeout = TimeSpan.FromMinutes(10),
                AllowCookies = false,
                BypassProxyOnLocal = false,
                MessageEncoding = WSMessageEncoding.Text,
                TextEncoding = Encoding.UTF8,
                TransferMode = TransferMode.Buffered,
                UseDefaultWebProxy = true,

                // specify larger value than default for when large amounts of data needed
                MaxReceivedMessageSize = 1024 * 1024 * 10,
                Security =
                    {
                    Mode = useHttps ? BasicHttpSecurityMode.Transport : BasicHttpSecurityMode.TransportCredentialOnly,
                    Transport =
                        {
                        ClientCredentialType = HttpClientCredentialType.Windows
                        }
                    }
                };

            return result;
            }

        internal static string GetCurrentWindowsIdentity()
            {
            using var wi = WindowsIdentity.GetCurrent();
            var result = wi.Name;
            return result;
            }

        /// <summary>
        /// Disposes this object
        /// </summary>
        public void Dispose()
            {
            if (this._transactionServiceSoapClient.IsValueCreated)
                {
                ForceClose(this.SoapClient);
                }
            this.IsDisposed = true;
            GC.SuppressFinalize(this);
            }

        // Based on code from https://msdn.microsoft.com/en-us/library/aa355056.aspx "Window Communication Foundation Samples" "Avoiding Problems with the Using Statement"
        private static void ForceClose(TransactionServiceClient ts)
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

        private void EnsureObjectIsNotDisposed()
            {
            if (this.IsDisposed)
                throw new ObjectDisposedException("Object has been disposed.");
            }
        }
    }
