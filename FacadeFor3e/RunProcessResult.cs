using System;
using System.Collections.Generic;
using System.Xml;
using JetBrains.Annotations;

namespace FacadeFor3e
    {
    [PublicAPI]
    public class RunProcessResult
        {
        internal RunProcessResult(XmlDocument request, XmlDocument response)
            {
            if (request?.DocumentElement == null)
                throw new ArgumentException("Invalid request.", nameof(request));
            if (response?.DocumentElement == null)
                throw new ArgumentException("Invalid response.", nameof(response));

            this.Request = request;
            this.Response = response;
            }

        /// <summary>
        /// Gets the request sent to 3e
        /// </summary>
        public XmlDocument Request { get; }

        /// <summary>
        /// Gets the response from 3e
        /// </summary>
        public XmlDocument Response { get; }

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
                var result = nextMessage?.InnerText;
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
                result.Add(node.GetAttribute("KeyValue"));
                }
            return result;
            }
        }
    }
