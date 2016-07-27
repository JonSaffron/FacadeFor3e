using System;
using System.Collections.Generic;
using System.Xml;
using JetBrains.Annotations;

namespace FacadeFor3e
    {
    [PublicAPI]
    public class RunProcessResult
        {
        private readonly XmlDocument _request;
        private readonly XmlDocument _response;
        

        internal RunProcessResult(XmlDocument request, XmlDocument response)
            {
            if (request == null || request.DocumentElement == null)
                throw new ArgumentException("Invalid request.", "request");
            if (response == null || response.DocumentElement == null)
                throw new ArgumentException("Invalid response.", "response");

            this._request = request;
            this._response = response;
            }

        /// <summary>
        /// Gets the request sent to 3e
        /// </summary>
        public XmlDocument Request
            {
            get { return this._request; }
            }

        /// <summary>
        /// Gets the repsonse from 3e
        /// </summary>
        public XmlDocument Response
            {
            get { return this._response; }
            }

        public Guid ProcessId
            {
            get
                {
                // ReSharper disable once PossibleNullReferenceException
                var result = new Guid(this.Response.DocumentElement.GetAttribute("ProcessItemId"));
                return result;
                }
            }

        public string NextMessage
            {
            get
                {
                // ReSharper disable once PossibleNullReferenceException
                var nextMessage = this.Response.DocumentElement.SelectSingleNode("MESSAGE");
                var result = nextMessage == null ? null : nextMessage.InnerText;
                return result;
                }
            }

        public IEnumerable<string> GetKeys()
            {
            var keys = this.Response.SelectSingleNode("ProcessExecutionResults/Keys");
            if (keys == null)
                // The presence of the Keys node is dependant only upon whether ReturnInfoType.Keys was specified when calling ExecuteProcess 
                throw new InvalidOperationException("Key information was not requested.");
        
            var result = new List<string>();
            foreach (XmlElement node in keys.ChildNodes)
                {
                result.Add(node.InnerText);
                }
            return result;
            }
        }
    }
