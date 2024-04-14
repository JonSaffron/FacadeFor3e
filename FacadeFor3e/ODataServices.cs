using JetBrains.Annotations;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Text;
using System.Text.Json;
using FacadeFor3e.ProcessCommandBuilder;

namespace FacadeFor3e
    {
    /// <summary>
    /// Connects to the 3E ODara service and provides access to retrieve and update data
    /// </summary>
    [PublicAPI]
    [Experimental("OData")]
    public class ODataServices
        {
        /// <summary>
        /// The credentials being passed when calling the 3E OData service
        /// </summary>
        public NetworkCredential? NetworkCredential { get; }

        /// <summary>
        /// The WindowsIdentity being impersonated during calls to the 3E OData service
        /// </summary>
        public WindowsIdentity? AccountToImpersonate { get; }

        /// <summary>
        /// The base url to use to connect to the 3E OData service 
        /// </summary>
        public Uri BaseEndpoint { get; }

        /// <summary>
        /// Returns whether this object has been disposed
        /// </summary>
        public bool IsDisposed { get; private set; }

        private readonly HttpClient _httpClient;

        private readonly Lazy<Logger> _logger = new Lazy<Logger>(() => LogManager.GetCurrentClassLogger()!);

        // ReSharper disable once AssignNullToNotNullAttribute
        private Logger Logger => this._logger.Value;

        /// <summary>
        /// Constructs a new ODataServices object without impersonation or user credentials
        /// </summary>
        /// <param name="baseEndpoint">The url to use to connect to the 3E OData service</param>
        public ODataServices(Uri baseEndpoint)
            {
            this.BaseEndpoint = baseEndpoint ?? throw new ArgumentNullException(nameof(baseEndpoint));
            this._httpClient = BuildHttpClient(CredentialCache.DefaultNetworkCredentials);
            }

        /// <summary>
        /// Constructs a new ODataServices object which will impersonate the specified account during calls to the 3E OData service
        /// </summary>
        /// <param name="baseEndpoint">The url to use to connect to the 3E OData service</param>
        /// <param name="accountToImpersonate">The account to impersonate</param>
        public ODataServices(Uri baseEndpoint, WindowsIdentity accountToImpersonate) : this(baseEndpoint)
            {
            this.AccountToImpersonate = accountToImpersonate ?? throw new ArgumentNullException(nameof(accountToImpersonate));
            this._httpClient = BuildHttpClient(CredentialCache.DefaultNetworkCredentials);
            }

        /// <summary>
        /// Constructs a new ODataServices object which will pass the specified credentials during calls to the OData service
        /// </summary>
        /// <param name="baseEndpoint">The url to use to connect to the 3E OData service</param>
        /// <param name="networkCredential">The credentials to use when calling the 3E OData service</param>
        public ODataServices(Uri baseEndpoint, NetworkCredential networkCredential) : this(baseEndpoint)
            {
            this.NetworkCredential = networkCredential ?? throw new ArgumentNullException(nameof(networkCredential));
            this._httpClient = BuildHttpClient(networkCredential);
            }

        public ODataServices(Uri baseEndpoint, ODataAuthentication oDataAuthentication) : this(baseEndpoint)
            {
            this._httpClient = BuildHttpClient(null);
            // ReSharper disable once PossibleNullReferenceException
            this._httpClient.DefaultRequestHeaders.Authorization = oDataAuthentication.Header;
            }

        public static ODataAuthentication Authenticate(Uri tokenEndpoint, Dictionary<string, string> credentials)
            {
            using HttpClient httpClient = new HttpClient();
            var content = new FormUrlEncodedContent(credentials);
            // ReSharper disable once PossibleNullReferenceException
            // ReSharper disable once AssignNullToNotNullAttribute
            HttpResponseMessage response = httpClient.PostAsync(tokenEndpoint, content).Result;
            // ReSharper disable once PossibleNullReferenceException
            response.EnsureSuccessStatusCode();
            // ReSharper disable once PossibleNullReferenceException
            var jsonDocument = JsonDocument.Parse(response.Content.ReadAsByteArrayAsync().Result);
            var tokenType = jsonDocument.RootElement.GetProperty("token_type").GetString();
            var token = jsonDocument.RootElement.GetProperty("access_token").GetString();
            if (tokenType == null || token == null)
                throw new InvalidOperationException("Invalid response.");
            int secondsToExpiry = jsonDocument.RootElement.GetProperty("expires_in").GetInt32();
            var result = new ODataAuthentication(tokenType, token, secondsToExpiry);
            return result;
            }

        public ODataServiceResult Select(FormattableString relativeUri)
            {
            if (relativeUri == null) throw new ArgumentNullException(nameof(relativeUri));
            var uri = new Uri(relativeUri.ToString(CultureInfo.InvariantCulture), UriKind.Relative);
            return Select(uri);
            }

        [Obsolete]
        public ODataServiceResult Select(Uri relativeUri)
            {
            if (relativeUri == null)
                throw new ArgumentNullException(nameof(relativeUri));
            if (relativeUri.IsAbsoluteUri)
                throw new ArgumentOutOfRangeException(nameof(relativeUri), "Specify a relative uri");
            // ReSharper disable once AssignNullToNotNullAttribute
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, relativeUri);
            LogDetailsOfTheJob(request);

            // ReSharper disable once PossibleNullReferenceException
            // ReSharper disable once AssignNullToNotNullAttribute
            HttpResponseMessage response = this._httpClient.SendAsync(request).Result;
            // ReSharper disable once AssignNullToNotNullAttribute
            // ReSharper disable PossibleNullReferenceException
            string responseText = response.Content.ReadAsStringAsync().Result;
            // ReSharper restore PossibleNullReferenceException
            LogForDebug($"{response.StatusCode:D} {responseText}");
            response.EnsureSuccessStatusCode();
            var result = new ODataServiceResult(request, response);
            return result;
            }

        public ODataServiceResult Execute(ProcessCommand command, ExecuteParams executeParams)
            {
            var renderer = new ODataRenderer();
            var requestParameters = renderer.Render(command);
            HttpRequestMessage request = new HttpRequestMessage(requestParameters.Verb, requestParameters.EndPoint);
            request.Content = new ByteArrayContent(requestParameters.Json);
            // ReSharper disable once PossibleNullReferenceException
            // ReSharper disable once AssignNullToNotNullAttribute
            request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json; charset=utf-8");
            LogDetailsOfTheJob(request);
            LogForDebug(Encoding.UTF8.GetString(requestParameters.Json));

            // ReSharper disable once PossibleNullReferenceException
            // ReSharper disable once AssignNullToNotNullAttribute
            HttpResponseMessage response = this._httpClient.SendAsync(request).Result;
            // ReSharper disable once AssignNullToNotNullAttribute
            // ReSharper disable PossibleNullReferenceException
            string responseText = response.Content.ReadAsStringAsync().Result;
            // ReSharper restore PossibleNullReferenceException
            LogForDebug($"{response.StatusCode:D} {responseText}");
            if (executeParams.ThrowExceptionIfProcessFails)
                {
                response.EnsureSuccessStatusCode();
                }

            var result = new ODataServiceResult(request, response);
            return result;
            }

        private HttpClient BuildHttpClient(NetworkCredential? networkCredential)
            {
            var handler = new HttpClientHandler();
            if (networkCredential != null)
                {
                var credentialCache = new CredentialCache
                    {
                        { this.BaseEndpoint, "Negotiate", networkCredential }
                    };
                handler.Credentials = credentialCache;
                handler.PreAuthenticate = true;
                }

            //handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            //handler.ServerCertificateCustomValidationCallback = 
            //    (httpRequestMessage, cert, cetChain, policyErrors) =>
            //        {
            //        return true;
            //        };
            
            var result = new HttpClient(handler);
            result.BaseAddress = this.BaseEndpoint;
            return result;
            }

        internal void LogForDebug(string message)
            {
            this.Logger.Debug(message);
            }

        internal void LogDetailsOfTheJob(HttpRequestMessage request)
            {
            if (request == null) throw new ArgumentNullException(nameof(request));
            var sb = new StringBuilder();
            // ReSharper disable once PossibleNullReferenceException
            // ReSharper disable once AssignNullToNotNullAttribute
            sb.AppendFormat(request.Method.ToString());
            sb.AppendLine();
            sb.AppendFormat("\tURL: {0}", new Uri(this._httpClient.BaseAddress!, request.RequestUri!));
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

        /// <summary>
        /// Gets whether impersonation is being used during calls to the 3E transaction service
        /// </summary>
        public bool IsImpersonating => this.AccountToImpersonate != null && this.AccountToImpersonate != WindowsIdentity.GetCurrent();

        internal static string GetCurrentWindowsIdentity()
            {
            using var wi = WindowsIdentity.GetCurrent();
            var result = wi.Name;
            return result;
            }
        }

    public class ExecuteParams
        {
        private static readonly ExecuteParams DefaultExecuteParams = new ExecuteParams
            {
            ThrowExceptionIfProcessFails = true
            };

        public bool ThrowExceptionIfProcessFails;

        public static ExecuteParams Default => DefaultExecuteParams;
        }
    }
