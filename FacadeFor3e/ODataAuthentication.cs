using System;
using System.Net.Http.Headers;

namespace FacadeFor3e
    {
    public class ODataAuthentication
        {
        public ODataAuthentication(string scheme, string parameter, int secondsUntilExpiry)
            {
            this.Header = new AuthenticationHeaderValue(scheme, parameter);
            this.Expires = DateTime.UtcNow.AddSeconds(secondsUntilExpiry);
            }

        public DateTime Expires { get; }

        public readonly AuthenticationHeaderValue Header;

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
