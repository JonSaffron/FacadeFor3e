using System;
using System.Net.Http.Headers;

namespace FacadeFor3e
    {
    /// <summary>
    /// An object that contains the information returned from authenticating with an OData service in the cloud
    /// </summary>
    public class ODataAuthentication
        {
        internal ODataAuthentication(string scheme, string parameter, int secondsUntilExpiry)
            {
            this.Header = new AuthenticationHeaderValue(scheme, parameter);
            this.Expires = DateTime.UtcNow.AddSeconds(secondsUntilExpiry);
            }

        /// <summary>
        /// Returns the point in time when the authentication ticket expires
        /// </summary>
        /// <remarks>This is returned as a UTC date/time value</remarks>
        public DateTime Expires { get; }

        /// <summary>
        /// Returns the authentication header that must be specified in requests to the OData header
        /// </summary>
        public readonly AuthenticationHeaderValue Header;

        /// <summary>
        /// Returns whether the authentication token has expired or is close to expiring
        /// </summary>
        public bool IsExpiredOrAboutToExpire
            {
            get
                {
                var result = (this.Expires - DateTime.UtcNow).TotalSeconds <= 100;
                return result;
                }
            }
        }
    }
