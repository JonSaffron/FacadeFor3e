using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using JetBrains.Annotations;

namespace FacadeFor3e
    {
    /// <summary>
    /// An objects that provides information on the request sent to and the response received from the OData service
    /// </summary>
    [PublicAPI]
    public class ODataServiceResult
        {
        internal ODataServiceResult(HttpRequestMessage request, HttpResponseMessage response)
            {
            this.Request = request ?? throw new ArgumentNullException(nameof(request));
            this.Response = response ?? throw new ArgumentNullException(nameof(response));
            this.RawResponseBytes = Array.Empty<byte>();

#if !NET6_0_OR_GREATER
            if (this.Response.Content == null)
                return;
#endif
            var task = this.Response.Content.ReadAsByteArrayAsync();
            var content = task.Result;
#if !NET6_0_OR_GREATER
            if (content == null)
                return;
#endif
            this.RawResponseBytes = content;
            }

        /// <summary>
        /// Gets the request sent to 3E
        /// </summary>
        public HttpRequestMessage Request { get; }
        
        /// <summary>
        /// Gets the response from 3E
        /// </summary>
        public HttpResponseMessage Response { get; }

        /// <summary>
        /// The raw data of the response content received from the OData service
        /// </summary>
        public byte[] RawResponseBytes { get; }

        /// <summary>
        /// Gets whether the request succeeded or failed
        /// </summary>
        /// <remarks>Returns true if the HTTP status code received was in the 200-299 range.</remarks>
        public bool IsError => !this.Response.IsSuccessStatusCode;

        /// <summary>
        /// Returns whether the response was in JSON format
        /// </summary>
        public bool IsResponseJSon
            {
            get
                {
                if (this.RawResponseBytes.Length == 0)
                    return false;

                // ReSharper disable ConditionalAccessQualifierIsNonNullableAccordingToAPIContract
                var mediaType = this.Response.Content?.Headers?.ContentType?.MediaType ?? string.Empty;
                // ReSharper restore ConditionalAccessQualifierIsNonNullableAccordingToAPIContract
                return string.Equals(mediaType, "application/json");
                }
            }

        /// <summary>
        /// Returns the response body as a string
        /// </summary>
        public string ResponseString => Encoding.UTF8.GetString(this.RawResponseBytes);

        /// <summary>
        /// Returns the response body as a JsonDocument
        /// </summary>
        /// <exception cref="InvalidOperationException">If the response was not in JSON format</exception>
        public JsonDocument ResponseJSonDocument
            {
            get
                {
                if (!this.IsResponseJSon)
                    throw new InvalidOperationException("Response is not a json object");
                var result = JsonDocument.Parse(this.RawResponseBytes);
                return result;
                }
            }

        /// <summary>
        /// Returns any error messages returned from the OData service
        /// </summary>
        /// <remarks>Returns no items if <see cref="IsError"/> returns false</remarks>
        public IEnumerable<string> ErrorMessages
            {
            get
                {
                if (!this.IsError)
                    return Array.Empty<string>();

                if (!this.IsResponseJSon)
                    return new[] { this.ResponseString };

                var errors = this.ResponseJSonDocument.RootElement.GetProperty("error").GetProperty("message").GetString();
                if (errors == null || string.IsNullOrWhiteSpace(errors))
                    {
                    return new[] { $"Unknown error - HTTP status code {this.Response.StatusCode}" };
                    }

                var result = errors.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                return result;
                }
            }
        }
    }
