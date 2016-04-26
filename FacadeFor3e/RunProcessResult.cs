using System;
using System.Globalization;
using System.Xml;
using JetBrains.Annotations;

namespace FacadeFor3e
    {
    [PublicAPI]
    public class RunProcessResult
        {
        /// <summary>
        /// Gets the repsonse from 3e
        /// </summary>
        public XmlDocument Response {get; private set;}

        /// <summary>
        /// Gets the value of a key field created during the process
        /// </summary>
        public string NewKey { get; private set; }

        internal RunProcessResult(XmlDocument response)
            {
            if (response == null || response.DocumentElement == null)
                throw new ArgumentException("Invalid response.", "response");

            this.Response = response;
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

        internal void SetKey(string objectName)
            {
            string path = string.Format(CultureInfo.InvariantCulture, "Keys/{0}", objectName);
            // ReSharper disable once PossibleNullReferenceException
            var element = this.Response.DocumentElement.SelectSingleNode(path) as XmlElement;
            if (element == null)
                throw new InvalidOperationException("Failed to find new key value for " + objectName);
            this.NewKey = element.GetAttribute("KeyValue");
            }
        }
    }
