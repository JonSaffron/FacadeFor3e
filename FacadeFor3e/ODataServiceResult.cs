using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using JetBrains.Annotations;

namespace FacadeFor3e
    {
    [PublicAPI]
    public class ODataServiceResult
        {
        public ODataServiceResult(HttpRequestMessage request, HttpResponseMessage response)
            {
            this.Request = request ?? throw new ArgumentNullException(nameof(request));
            this.Response = response ?? throw new ArgumentNullException(nameof(response));
            this.RawResponseBytes = this.Response.Content.ReadAsByteArrayAsync().Result;
            }

        public HttpRequestMessage Request { get; }
        public HttpResponseMessage Response { get; }

        public bool IsError => !this.Response.IsSuccessStatusCode;

        public bool IsResponseJSon => string.Equals(this.Response.Content.Headers.ContentType?.MediaType, "application/json");

        public byte[] RawResponseBytes { get; }

        public string ResponseString => Encoding.UTF8.GetString(this.RawResponseBytes);

        public JsonDocument ResponseJSonDocument
            {
            get
                {
                if (this.IsError)
                    throw new InvalidOperationException("Response indicates an error occurred");
                if (!this.IsResponseJSon)
                    throw new InvalidOperationException("Response is not a json object");
                var result = JsonDocument.Parse(this.RawResponseBytes);
                return result;
                }
            }

        public string? ErrorMessage
            {
            get
                {
                if (!this.IsError)
                    return null;

                if (!this.IsResponseJSon)
                    return this.ResponseString;

                var result = this.ResponseJSonDocument.RootElement.GetProperty("error").GetProperty("message").GetString();
                return result;
                }
            }

        /// <summary>
        /// Returns the ProcessId used during the update. 
        /// </summary>
        /// <remarks>This requires customisation to the archetype and object to work, plus a change to informationobjectmapping.json</remarks>
        public Guid ProcessId
            {
            get
                {
                if (this.IsError)
                    throw new InvalidOperationException("Response indicates an error occurred");
                if (!this.IsResponseJSon)
                    throw new InvalidOperationException("Response is not a json object");
                var result = this.ResponseJSonDocument.RootElement.GetProperty("_ProcessId_ccc").GetGuid();
                return result;
                }
            }

        public T DeserialiseResult<T>()
            {
            if (this.IsError)
                throw new InvalidOperationException("Response indicates an error occurred");
            if (!this.IsResponseJSon)
                throw new InvalidOperationException("Response is not a json object");
            var result = JsonSerializer.Deserialize<T>(this.RawResponseBytes);
            System.Diagnostics.Debug.Assert(result != null);
            return result!;
            }
        }
    }
