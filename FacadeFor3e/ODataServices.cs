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
        public Uri BaseEndpoint => this._httpClient.BaseAddress!;

        /// <summary>
        /// Returns whether this object has been disposed
        /// </summary>
        public bool IsDisposed { get; private set; }

        private readonly HttpClient _httpClient;

        private static readonly Lazy<Logger> LazyLogger = new Lazy<Logger>(() => LogManager.GetCurrentClassLogger()!);

        // ReSharper disable once AssignNullToNotNullAttribute
        private static Logger Logger => LazyLogger.Value;

        private readonly Func<string> _authenticationMethod;

        private static readonly MediaTypeHeaderValue JsonMediaType = MediaTypeHeaderValue.Parse("application/json; charset=utf-8");

        /// <summary>
        /// Constructs a new ODataServices object without impersonation or user credentials
        /// </summary>
        /// <param name="baseEndpoint">The url to use to connect to the 3E OData service</param>
        public ODataServices(Uri baseEndpoint)
            {
            this._httpClient = BuildHttpClient(baseEndpoint, CredentialCache.DefaultNetworkCredentials);
            this._authenticationMethod = () => $"current user {GetCurrentWindowsIdentity()}";
            }

        /// <summary>
        /// Constructs a new ODataServices object which will impersonate the specified account during calls to the on premises 3E OData service
        /// </summary>
        /// <param name="baseEndpoint">The url to use to connect to the 3E OData service</param>
        /// <param name="accountToImpersonate">The account to impersonate</param>
        public ODataServices(Uri baseEndpoint, WindowsIdentity accountToImpersonate)
            {
            this.AccountToImpersonate = accountToImpersonate ?? throw new ArgumentNullException(nameof(accountToImpersonate));
            this._httpClient = BuildHttpClient(baseEndpoint, CredentialCache.DefaultNetworkCredentials);
            this._authenticationMethod = () => $"impersonating {accountToImpersonate.Name}";
            }

        /// <summary>
        /// Constructs a new ODataServices object which will pass the specified credentials during calls to the on premises 3E OData service
        /// </summary>
        /// <param name="baseEndpoint">The url to use to connect to the 3E OData service</param>
        /// <param name="networkCredential">The credentials to use when calling the 3E OData service</param>
        public ODataServices(Uri baseEndpoint, NetworkCredential networkCredential)
            {
            this.NetworkCredential = networkCredential ?? throw new ArgumentNullException(nameof(networkCredential));
            this._httpClient = BuildHttpClient(baseEndpoint, networkCredential);
            this._authenticationMethod = () => $"windows credentials for {networkCredential.Domain}\\{networkCredential.UserName}";
            }

        /// <summary>
        /// Constructs a new ODataServices object which will pass the specified cloud credentials during calls to the 3E OData service
        /// </summary>
        /// <param name="baseEndpoint"></param>
        /// <param name="oDataAuthentication"></param>
        /// <param name="instanceId"></param>
        public ODataServices(Uri baseEndpoint, ODataAuthentication oDataAuthentication, string instanceId)
            {
            this._httpClient = BuildHttpClient(baseEndpoint, null);
            // ReSharper disable once PossibleNullReferenceException
            this._httpClient.DefaultRequestHeaders.Authorization = oDataAuthentication.Header;
            this._httpClient.DefaultRequestHeaders.Add("X-3E-InstanceId", instanceId);
            this._authenticationMethod = () => $"token for instance {instanceId} (expires {oDataAuthentication.Expires.ToLocalTime():HH:mm:ss})";
            }

        /// <summary>
        /// Authenticates against an OData service in the cloud
        /// </summary>
        /// <param name="tokenEndpoint">The uri of the web service that provides the authentication service</param>
        /// <param name="credentials">The set of credentials to present to the service</param>
        /// <returns>An ODataAuthentication object containing the token to use in subsequent requests and the point of expiry</returns>
        /// <exception cref="InvalidOperationException">If authentication fails</exception>
        public static ODataAuthentication Authenticate(Uri tokenEndpoint, Dictionary<string, string> credentials)
            {
            using var httpClient = new HttpClient();
            var content = new FormUrlEncodedContent(credentials);
            LogForDebug($"Acquiring token to access OData service from {tokenEndpoint}");
            var response = httpClient.PostAsync(tokenEndpoint, content).Result;
            LogForDebug($"{(response.IsSuccessStatusCode ? "Success" : "Failed")} {response.StatusCode:D}");

            var serviceResult = new ODataServiceResult(new HttpRequestMessage(), response);
            if (serviceResult.IsError)
                throw new InvalidOperationException($"The authentication request returned error code {response.StatusCode}");
            if (!serviceResult.IsResponseJSon)
                throw new InvalidOperationException("Received an invalid response during authentication.");

            var jsonDocument = serviceResult.ResponseJSonDocument;
            var tokenType = jsonDocument.RootElement.GetProperty("token_type").GetString();
            var token = jsonDocument.RootElement.GetProperty("access_token").GetString();
            if (tokenType == null || token == null)
                throw new InvalidOperationException("Invalid response.");
            int secondsToExpiry = jsonDocument.RootElement.GetProperty("expires_in").GetInt32();
            var result = new ODataAuthentication(tokenType, token, secondsToExpiry);
            return result;
            }

        /// <summary>
        /// Performs a GET request to the OData service to query the 3E database
        /// </summary>
        /// <param name="relativeUri">A <see cref="FormattableString"/> that defines the request</param>
        /// <returns>A <see cref="ODataServiceResult"/> that contains the response</returns>
        /// <exception cref="ArgumentNullException">If the Uri specified is null</exception>
        /// <remarks>The string supplied will be formatted using the InvariantCulture before being turned into a Relative URI</remarks>
        [Pure]
        public ODataServiceResult Select(FormattableString relativeUri)
            {
            if (relativeUri == null) throw new ArgumentNullException(nameof(relativeUri));
            var uri = new Uri(relativeUri.ToString(CultureInfo.InvariantCulture), UriKind.Relative);
            return Select(uri);
            }

        /// <summary>
        /// Performs a GET request to the OData service to query the 3E database
        /// </summary>
        /// <param name="relativeUri">The <see cref="Uri"/> that defines the request</param>
        /// <returns>A <see cref="ODataServiceResult"/> that contains the response</returns>
        /// <exception cref="ArgumentNullException">If the Uri specified is null</exception>
        [Pure]
        public ODataServiceResult Select(Uri relativeUri)
            {
            if (relativeUri == null)
                throw new ArgumentNullException(nameof(relativeUri));
            if (relativeUri.IsAbsoluteUri)
                throw new ArgumentOutOfRangeException(nameof(relativeUri), "Specify a relative uri");

            // ReSharper disable once AssignNullToNotNullAttribute - On HttpMethod.Get before .net 6
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, relativeUri);
            LogDetailsOfTheJob(request);

            var response = this._httpClient.SendAsync(request).Result;
            LogForDebug($"{(response.IsSuccessStatusCode ? "Success" : "Failed")} {response.StatusCode:D}");

            var result = new ODataServiceResult(request, response);
            LogForDebug(result.ResponseString);

            if (result.IsError)
                {
                var errorMessages = new List<string> { "An error occurred whilst trying to query 3E data through OData:" };
                errorMessages.AddRange(result.ErrorMessages);
                var msg = string.Join("\r\n", errorMessages);
                throw new ExecuteProcessException(msg, result);
                }

            return result;
            }

        /// <summary>
        /// Makes a request to the OData service to run a process to edit data
        /// </summary>
        /// <param name="command">Specifies a command to execute</param>
        /// <param name="options">Specifies any options required to execute the command</param>
        /// <returns>A <see cref="ODataServiceResult"/> that contains the response</returns>
        /// <exception cref="ArgumentNullException">If the parameters specified have null values</exception>
        /// <exception cref="ExecuteProcessException">If an invalid response is returned, or an error message is returned from 3E and <see cref="ODataExecuteOptions.ThrowExceptionIfProcessFails"/> is set</exception>
        public ODataServiceResult Execute(ProcessCommand command, ODataExecuteOptions options)
            {
            if (command == null) throw new ArgumentNullException(nameof(command));
            if (options == null) throw new ArgumentNullException(nameof(options));

            var renderer = new ODataRenderer();
            var requestDetails = renderer.Render(command, options);
            var result = Execute(requestDetails);

            if (result.IsError && options.ThrowExceptionIfProcessFails)
                {
                var errorMessages = new List<string> { "An error occurred whilst trying to execute a 3E process through OData:" };
                errorMessages.AddRange(result.ErrorMessages);
                var msg = string.Join("\r\n", errorMessages);
                throw new ExecuteProcessException(msg, result);
                }

            return result;
            }

        /// <summary>
        /// Makes a request to the OData service to run a process to edit data
        /// </summary>
        /// <param name="requestDetails">Specifies a request to execute</param>
        /// <returns>A <see cref="ODataServiceResult"/> that contains the response</returns>
        /// <exception cref="ArgumentNullException">If the parameters specified have null values</exception>
        public ODataServiceResult Execute(ODataRequest requestDetails)
            {
            if (requestDetails == null) throw new ArgumentNullException(nameof(requestDetails));
            var request = new HttpRequestMessage(requestDetails.Verb, requestDetails.EndPoint)
                {
                Content = new ByteArrayContent(requestDetails.Json)
                };
            request.Content.Headers.ContentType = JsonMediaType;
            LogDetailsOfTheJob(request);
            LogForDebug(Encoding.UTF8.GetString(requestDetails.Json));

            var response = this._httpClient.SendAsync(request).Result;
            LogForDebug($"{(response.IsSuccessStatusCode ? "Success" : "Failed")} {response.StatusCode:D}");

            var result = new ODataServiceResult(request, response);
            LogForDebug(result.ResponseString);

            return result;
            }

        private static HttpClient BuildHttpClient(Uri baseEndpoint, NetworkCredential? networkCredential)
            {
            if (baseEndpoint == null)
                throw new ArgumentNullException(nameof(baseEndpoint));
            if (!baseEndpoint.IsAbsoluteUri)
                throw new ArgumentOutOfRangeException(nameof(baseEndpoint), "Must be an absolute URI.");
            if (!baseEndpoint.AbsolutePath.EndsWith("/"))
                {
                baseEndpoint = new Uri(baseEndpoint, $"{baseEndpoint.AbsolutePath}/");
                }

            var handler = new HttpClientHandler();
            if (networkCredential != null)
                {
                var credentialCache = new CredentialCache
                    {
                        { baseEndpoint, "Negotiate", networkCredential }
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
            result.BaseAddress = baseEndpoint;
            return result;
            }

        internal static void LogForDebug(string message)
            {
            Logger.Debug(message);
            }

        internal void LogDetailsOfTheJob(HttpRequestMessage request)
            {
            if (request == null) 
                throw new ArgumentNullException(nameof(request));

            // ReSharper disable once ConditionalAccessQualifierIsNonNullableAccordingToAPIContract
            string method = request.Method?.Method ?? "unknown";
            // ReSharper disable once RedundantSuppressNullableWarningExpression
            string authenticationMethod = this._authenticationMethod()!;

            var sb = new StringBuilder();
            sb.AppendFormat("\t{0} {1}", method, new Uri(this._httpClient.BaseAddress!, request.RequestUri!));
            sb.AppendLine();
            sb.AppendFormat("\tauthentication: {0}", authenticationMethod);
            Logger.Info(sb.ToString());
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
    }
