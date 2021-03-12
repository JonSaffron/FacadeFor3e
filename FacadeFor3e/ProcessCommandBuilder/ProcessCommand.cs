using System;
using System.Xml;
using JetBrains.Annotations;

namespace FacadeFor3e.ProcessCommandBuilder
    {
    /// <summary>
    /// Defines a process for performing one or more operations on an object
    /// </summary>
    [PublicAPI]
    public class ProcessCommand : DataObject
        {
        /// <summary>
        /// Constructs a new process
        /// </summary>
        /// <param name="processCode">Specifies the process to run</param>
        /// <param name="objectName">Name of the object to affect</param>
        public ProcessCommand(string processCode, string objectName)
            : base(objectName)
            {
            this.ProcessCode = processCode ?? throw new ArgumentNullException(nameof(processCode));
            CommonLibrary.EnsureValid(processCode);
            }

        /// <summary>
        /// Returns the process code.
        /// </summary>
        public string ProcessCode { get; }

        /// <summary>
        /// Sets or returns the process name displayed in a user's action list.
        /// </summary>
        /// <remarks>Value is stored in NxFwkProcessItem.Name</remarks>
        public string? ProcessName { get; set; }

        /// <summary>
        /// Sets or returns the process description displayed in a user's action list.
        /// </summary>
        /// <remarks>Value is stored in NxFwkProcessItem.Description</remarks>
        public string? Description { get; set; }

        /// <summary>
        /// Sets or returns the process priority.
        /// </summary>
        /// <remarks>Value is stored in NxFwkProcessItem.Priority</remarks>
        public PriorityEnum? Priority { get; set; }

        /// <summary>
        /// Gets or returns the Operating Unit to use whilst running the process.
        /// Setting this can be useful when adding zero vouchers. Its front-end equivalent is the Operating Unit setting in a process's Folder fields.
        /// </summary>
        /// <remarks>Value is stored in NxFwkProcessItem.Unit</remarks>
        public string? OperatingUnit { get; set; }

        /// <summary>
        /// Gets or returns the process CheckSum.
        /// </summary>
        /// <remarks>Value is stored in NxFwkProcessItem.CheckSum</remarks>
        public float? CheckSum { get; set; }

        /// <summary>
        /// Gets or sets the proxy user.
        /// </summary>
        public string? ProxyUser { get; set; }

        /// <summary>
        /// Gets or sets the proxy user id.
        /// </summary>
        public string? ProxyUserId { get; set; }

        /// <summary>
        /// Gets or sets how the execution of the process should be altered from the default.
        /// </summary>
        public ProcessExecutionRequestTypeEnum? ProcessRequestType { get; set; }

        /// <summary>
        /// Gets or sets the user role to use for the first step when ProcessRequestType is not set to Default.
        /// </summary>
        public string? ProcessAutomationRoleAfterFirstStep { get; set; }

        /// <summary>
        /// Validates request is coming from an authorized external application.
        /// </summary>
        public string? ProcessRequestSignature { get; set; }

        /// <summary>
        /// Generates the namespace required for the process
        /// </summary>
        /// <returns>The namespace that applies for the specified process</returns>
        protected internal string ProcessNameSpace => $@"http://elite.com/schemas/transaction/process/write/{this.ProcessCode}";

        /// <summary>
        /// Generates the namespace required for the object
        /// </summary>
        /// <returns>The namespace that applies for the specified object</returns>
        protected internal string ObjectNameSpace => $@"http://elite.com/schemas/transaction/object/write/{this.ObjectName}";

        /// <summary>
        /// Outputs this process
        /// </summary>
        /// <param name="writer">An XMLWriter to output to</param>
        protected internal override void Render(XmlWriter writer)
            {
            writer.WriteStartDocument();
            writer.WriteStartElement(this.ProcessCode, ProcessNameSpace);
            RenderProcessAttributes(writer);
            writer.WriteStartElement("Initialize", ObjectNameSpace);
            foreach (OperationBase o in this.Operations)
                {
                // ReSharper disable once PossibleNullReferenceException
                o.Render(writer, this.ObjectName);
                }
            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            }

        /// <summary>
        /// Outputs the optional attributes of the ProcessCommand
        /// </summary>
        /// <param name="writer">An XMLWriter to output to</param>
        protected internal void RenderProcessAttributes(XmlWriter writer)
            {
            if (this.ProcessName != null)
                writer.WriteAttributeString("ObjectName", this.ProcessName);
            if (this.Description != null)
                writer.WriteAttributeString("Description", this.Description);
            if (this.Priority.HasValue)
                // ReSharper disable once AssignNullToNotNullAttribute
                writer.WriteAttributeString("Priority", this.Priority.Value.ToString());
            if (this.OperatingUnit != null)
                writer.WriteAttributeString("OperatingUnit", this.OperatingUnit);
            if (this.CheckSum.HasValue)
                writer.WriteAttributeString("CheckSum", this.CheckSum.Value.ToString("G"));
            if (this.ProxyUser != null)
                writer.WriteAttributeString("ProxyUser", this.ProxyUser);
            if (this.ProxyUserId != null)
                writer.WriteAttributeString("ProxyUserID", this.ProxyUserId);
            if (this.ProcessRequestType.HasValue)
                // ReSharper disable once AssignNullToNotNullAttribute
                writer.WriteAttributeString("ProcessRequestType", this.ProcessRequestType.ToString());
            if (this.ProcessAutomationRoleAfterFirstStep != null)
                writer.WriteAttributeString("ProcessAutomationRoleAfterFirstStep", this.ProcessAutomationRoleAfterFirstStep);
            if (this.ProcessRequestSignature != null)
                writer.WriteAttributeString("ProcessRequestSignature", this.ProcessRequestSignature);
            }

        /// <summary>
        /// Generate the XML instructions to pass to 3E
        /// </summary>
        /// <returns>The transaction to be carried out</returns>
        public XmlDocument GenerateCommand()
            {
            var xmlDoc = new XmlDocument();
            // ReSharper disable AssignNullToNotNullAttribute
            using XmlWriter w = xmlDoc.CreateNavigator()!.AppendChild();
            Render(w);
            // ReSharper restore AssignNullToNotNullAttribute
            return xmlDoc;
            }
        }
    }
