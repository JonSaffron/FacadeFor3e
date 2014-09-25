using System;
using System.Globalization;
using System.Xml;
using JetBrains.Annotations;

namespace FacadeFor3e
    {
    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
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

        internal RunProcessResult(XmlDocument response, bool setKey, string objectName)
            {
            if (response == null || response.DocumentElement == null)
                throw new ArgumentException("Invalid response.", "response");

            if (setKey)
                {
                string path = string.Format(CultureInfo.InvariantCulture, "Keys/{0}", objectName);
                var element = response.DocumentElement.SelectSingleNode(path) as XmlElement;
                if (element == null)
                    throw new InvalidOperationException();
                this.NewKey = element.GetAttribute("KeyValue");
                }

            this.Response = response;
            }
        }
    }

