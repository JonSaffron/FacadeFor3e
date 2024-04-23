using System.Diagnostics.CodeAnalysis;
using System.Net.Http;

namespace FacadeFor3e.ProcessCommandBuilder
    {
    /// <summary>
    /// Describes all the various values needed to call the OData service
    /// </summary>
    [Experimental("OData")]
    public class ODataRequest
        {
        /// <summary>
        /// The HTML verb to use
        /// </summary>
        public readonly HttpMethod Verb;

        /// <summary>
        /// The URL to use
        /// </summary>
        public readonly string EndPoint;

        /// <summary>
        /// The command to send
        /// </summary>
        public readonly byte[] Json;

        /// <summary>
        /// Constructs a new OData request object
        /// </summary>
        /// <param name="verb">The HTML verb to use</param>
        /// <param name="endPoint">The URL to use</param>
        /// <param name="json">The command to send</param>
        internal ODataRequest(HttpMethod verb, string endPoint, byte[] json)
            {
            this.Verb = verb;
            this.EndPoint = endPoint;
            this.Json = json;
            }
        }
    }