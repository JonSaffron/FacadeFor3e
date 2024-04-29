using System;
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
        public readonly Uri EndPoint;

        /// <summary>
        /// The command to send
        /// </summary>
        public readonly byte[] Json;

        /// <summary>
        /// Constructs a new OData request object
        /// </summary>
        /// <param name="verb">The HTML verb to use</param>
        /// <param name="relativeEndPoint">The URL to use, relative to the OData base endpoint</param>
        /// <param name="json">The command to send</param>
        public ODataRequest(HttpMethod verb, Uri relativeEndPoint, byte[] json)
            {
            this.Verb = verb ?? throw new ArgumentNullException(nameof(verb));
            if (relativeEndPoint == null)
                throw new ArgumentNullException(nameof(relativeEndPoint));
            if (relativeEndPoint.IsAbsoluteUri)
                throw new ArgumentOutOfRangeException(nameof(relativeEndPoint), "Specify a relative uri");
            this.EndPoint = relativeEndPoint; 
            this.Json = json ?? throw new ArgumentNullException(nameof(json));
            }
        }
    }
