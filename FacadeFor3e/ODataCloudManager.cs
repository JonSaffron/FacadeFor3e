using System;
using System.Collections.Generic;
using JetBrains.Annotations;

#pragma warning disable OData

namespace FacadeFor3e
    {
    /// <summary>
    /// An object that assists in accessing an OData service in the cloud by handling the authentication aspects
    /// </summary>
    /// <remarks>This object will perform the initial authentication required to connect to the OData
    /// instance, and will re-authenticate when the authentication token has expired or is close to expiring</remarks>
    [PublicAPI]
    public class ODataCloudManager
        {
        private readonly Uri _baseEndpoint;
        private readonly Uri _tokenEndpoint;
        private readonly Dictionary<string, string> _credentials;
        private readonly string _instanceId;
        private ODataServices? _odataServices;
        private ODataAuthentication? _authentication;

        /// <summary>
        /// Constructs a new ODataCloudManager instance with the information required to connect and authenticate
        /// </summary>
        /// <param name="baseEndpoint">The <see cref="Uri"/> of the OData service itself</param>
        /// <param name="tokenEndpoint">The <see cref="Uri"/> of the web service used to provide authentication services</param>
        /// <param name="credentials">The set of credentials needed to authenticate</param>
        /// <param name="instanceId">The id of the OData instance</param>
        /// <exception cref="ArgumentNullException">If any of the parameters are null</exception>
        public ODataCloudManager(Uri baseEndpoint, Uri tokenEndpoint, Dictionary<string, string> credentials, string instanceId)
            {
            this._baseEndpoint = baseEndpoint ?? throw new ArgumentNullException(nameof(baseEndpoint));
            this._tokenEndpoint = tokenEndpoint ?? throw new ArgumentNullException(nameof(tokenEndpoint));
            this._credentials = credentials ?? throw new ArgumentNullException(nameof(credentials));
            this._instanceId = instanceId ?? throw new ArgumentNullException(nameof(instanceId));
            }

        /// <summary>
        /// Returns an ODataServices instance that can be used to make requests
        /// </summary>
        /// <returns>An instance of an <see cref="ODataServices"/> object</returns>
        public ODataServices GetODataServices()
            {
            if (IsNewTokenRequired())
                {
                this._authentication = ODataServices.Authenticate(this._tokenEndpoint, this._credentials);
                this._odataServices = new ODataServices(this._baseEndpoint, this._authentication, this._instanceId);
                }
            return this._odataServices!;
            }

        private bool IsNewTokenRequired()
            {
            if (this._odataServices == null || this._authentication == null)
                return true;
            return this._authentication.IsExpiredOrAboutToExpire;
            }
        }
    }
