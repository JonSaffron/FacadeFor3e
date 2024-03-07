using System;
using System.Collections.Generic;

#pragma warning disable OData

namespace FacadeFor3e
    {
    public class ODataCloudManager
        {
        private readonly Uri _baseEndpoint;
        private readonly Uri _tokenEndpoint;
        private readonly Dictionary<string, string> _credentials;
        private ODataServices? _odataServices;
        private ODataAuthentication? _authentication;

        public ODataCloudManager(Uri baseEndpoint, Uri tokenEndpoint, Dictionary<string, string> credentials)
            {
            this._baseEndpoint = baseEndpoint ?? throw new ArgumentNullException(nameof(baseEndpoint));
            this._tokenEndpoint = tokenEndpoint ?? throw new ArgumentNullException(nameof(tokenEndpoint));
            this._credentials = credentials ?? throw new ArgumentNullException(nameof(credentials));
            }

        public ODataServices GetODataServices()
            {
            if (IsNewTokenRequired())
                {
                this._authentication = ODataServices.Authenticate(this._tokenEndpoint, this._credentials);
                this._odataServices = new ODataServices(this._baseEndpoint, this._authentication);
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
