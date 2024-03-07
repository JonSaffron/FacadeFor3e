using System;
using System.Net.Http.Headers;

namespace FacadeFor3e
    {
    public class ODataAuthentication
        {
        private ODataAuthentication()
            {
            this.Header = new AuthenticationHeaderValue("Bearer", "OnPremises");
            this.Expires = DateTime.MaxValue;
            }

        public ODataAuthentication(string scheme, string parameter, int secondsUntilExpiry)
            {
            this.Header = new AuthenticationHeaderValue(scheme, parameter);
            this.Expires = DateTime.UtcNow.AddSeconds(secondsUntilExpiry);
            }

        public static ODataAuthentication OnPremises => new ODataAuthentication();
        public DateTime Expires { get; }

        public readonly AuthenticationHeaderValue Header;

        public bool IsExpiredOrAboutToExpire => (this.Expires - DateTime.UtcNow).Seconds <= 100;
        }
    }
