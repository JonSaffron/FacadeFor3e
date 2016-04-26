using System;
using System.Security.Principal;
using System.Xml;
using JetBrains.Annotations;

namespace FacadeFor3e
    {
    /// <summary>
    /// Encapsulates the running of a process within 3e
    /// </summary>
    [PublicAPI]
    public class RunProcessParameters
        {
        /// <summary>
        /// Constructs a new RunProcessParameters object
        /// </summary>
        /// <param name="p">The process to execute</param>
        public RunProcessParameters(Process p = null)
            {
            if (p != null)
                this.Request = p.GenerateCommand();
            }

        /// <summary>
        /// Gets or sets the request to run
        /// </summary>
        public XmlDocument Request { get; set; }

        /// <summary>
        /// Gets or sets whether to return a new key created during the process
        /// </summary>
        public bool GetKey { get; set; }

        /// <summary>
        /// Gets or sets the account to impersonate during the process
        /// </summary>
        public WindowsIdentity AccountToImpersonate { get; set; }

        /// <summary>
        /// Gets or sets the endpoint to use to connect to the 3e server
        /// </summary>
        public string EndpointName { get; set; }

        /// <summary>
        /// Gets or sets whether to throw an exception if the process doesn't complete
        /// </summary>
        public bool ThrowExceptionIfProcessDoesNotComplete { get; set; }

        /// <summary>
        /// Gets the name of the process from the request
        /// </summary>
        public string ProcessName
            {
            get
                {
                if (this.Request == null || this.Request.DocumentElement == null)
                    throw new InvalidOperationException("Request has not been set.");
                string result = this.Request.DocumentElement.Name;
                return result;
                }
            }

        /// <summary>
        /// Gets the name of the object from the request
        /// </summary>
        public string ObjectName
            {
            get
                {
                if (this.Request == null || this.Request.DocumentElement == null)
                    throw new InvalidOperationException("Request has not been set.");
                var initialize = this.Request.DocumentElement.ChildNodes[0] as XmlElement;
                if (initialize != null)
                    {
                    var operation = initialize.ChildNodes[0] as XmlElement;
                    if (operation != null)
                        {
                        var objectName = operation.ChildNodes[0] as XmlElement;
                        if (objectName != null)
                            {
                            string result = objectName.Name;
                            return result;
                            }
                        }
                    }

                throw new InvalidOperationException("Could not determine the object name.");
                }
            }
        }
    }
