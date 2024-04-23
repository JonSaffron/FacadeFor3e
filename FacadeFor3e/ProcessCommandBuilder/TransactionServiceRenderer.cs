using System;
using System.Globalization;
using System.Linq;
using System.Xml;

namespace FacadeFor3e.ProcessCommandBuilder
    {
    /// <summary>
    /// Renders a <see cref="ProcessCommand"/> into XML for the TransactionService
    /// </summary>
    public class TransactionServiceRenderer
        {
        /// <summary>
        /// The XMLWriter
        /// </summary>
        /// <remarks>Exposed for unit testing purposes</remarks>
        protected internal XmlWriter Writer = null!;

        /// <summary>
        /// Generate the XML instruction to pass to the ExecuteProcess command of the TransactionService
        /// </summary>
        /// <param name="processCommand">Specifies the command to be rendered</param>
        /// <param name="options">Specifies options that affect the output</param>
        /// <returns>The transaction to be carried out</returns>
        public XmlDocument Render(ProcessCommand processCommand, ExecuteProcessOptions options)
            {
            if (processCommand == null) throw new ArgumentNullException(nameof(processCommand));
            if (options == null) throw new ArgumentNullException(nameof(options));
            
            var xmlDoc = new XmlDocument();
            // ReSharper disable once RedundantSuppressNullableWarningExpression
            using (this.Writer = xmlDoc.CreateNavigator()!.AppendChild()!)
                {
                RenderProcess(processCommand, options);
                }

            return xmlDoc;
            }

        internal void RenderProcess(ProcessCommand processCommand, ExecuteProcessOptions options)
            {
            this.Writer.WriteStartDocument();
            this.Writer.WriteStartElement(processCommand.ProcessCode, processCommand.ProcessNameSpace);
            RenderProcessOptions(options);
            RenderProcessAttributes(processCommand);
            this.Writer.WriteStartElement("Initialize", processCommand.ObjectNameSpace);
            foreach (OperationBase o in processCommand.Operations)
                {
                RenderOperation(o, processCommand.ObjectName);
                }
            this.Writer.WriteEndElement();
            this.Writer.WriteEndElement();
            this.Writer.WriteEndDocument();
            this.Writer.Flush();
            }

        internal void RenderProcessOptions(ExecuteProcessOptions options)
            {
            if (options.ProxyUser != null)
                this.Writer.WriteAttributeString("ProxyUser", options.ProxyUser.Value.ToString("D"));

            if (options.CheckSum.HasValue)
                this.Writer.WriteAttributeString("CheckSum", options.CheckSum.Value.ToString(CultureInfo.InvariantCulture));
            
            if (options.ProcessExecutionRequestType != null)
                this.Writer.WriteAttributeString("ProcessRequestType", options.ProcessExecutionRequestType.Type);
            
            if (options.ProcessAutomationRoleAfterFirstStep != null)
                this.Writer.WriteAttributeString("ProcessAutomationRoleAfterFirstStep", options.ProcessAutomationRoleAfterFirstStep);
            
            if (options.ProcessRequestSignature != null)
                this.Writer.WriteAttributeString("ProcessRequestSignature", options.ProcessRequestSignature);
            }

        internal void RenderProcessAttributes(ProcessCommand processCommand)
            {
            if (processCommand.ProcessName != null)
                this.Writer.WriteAttributeString("Name", processCommand.ProcessName);

            if (processCommand.Description != null)
                this.Writer.WriteAttributeString("Description", processCommand.Description);

            if (processCommand.Priority != null)
                this.Writer.WriteAttributeString("Priority", processCommand.Priority.Priority);

            if (processCommand.OperatingUnit != null)
                this.Writer.WriteAttributeString("OperatingUnit", processCommand.OperatingUnit);
            }

        private void RenderOperation(OperationBase operation, string objectSuperclassName)
            {
            switch (operation)
                {
                case AddOperation add:
                    Render(add, objectSuperclassName);
                    break;
                case EditOperation edit:
                    Render(edit, objectSuperclassName);
                    break;
                case DeleteOperation delete:
                    Render(delete, objectSuperclassName);
                    break;
                default:
                    throw new InvalidOperationException();
                }
            }

        internal void Render(AddOperation operation, string objectSuperclassName)
            {
            this.Writer.WriteStartElement("Add");
            this.Writer.WriteStartElement(operation.SubClass ?? objectSuperclassName);

            RenderAttributes(operation);
            RenderChildren(operation);

            this.Writer.WriteEndElement();
            this.Writer.WriteEndElement();
            }

        internal void Render(EditOperation operation, string objectSuperclassName)
            {
            this.Writer.WriteStartElement("Edit");
            this.Writer.WriteStartElement(operation.SubClass ?? objectSuperclassName);

            RenderKey(operation.KeySpecification);

            RenderAttributes(operation);
            RenderChildren(operation);

            this.Writer.WriteEndElement();
            this.Writer.WriteEndElement();
            }

        internal void Render(DeleteOperation operation, string objectSuperclassName)
            {
            this.Writer.WriteStartElement("Delete");
            this.Writer.WriteStartElement(operation.SubClass ?? objectSuperclassName);

            RenderKey(operation.KeySpecification);

            this.Writer.WriteEndElement();
            this.Writer.WriteEndElement();
            }

        private void RenderAttributes(OperationWithAttributesBase operation)
            {
            if (operation.Attributes.Any())
                {
                this.Writer.WriteStartElement("Attributes");
                foreach (NamedAttributeValue a in operation.Attributes)
                    {
                    Render(a);
                    }
                this.Writer.WriteEndElement();
                }
            }

        internal void Render(NamedAttributeValue attribute)
            {
            this.Writer.WriteStartElement(attribute.Name);
            if (attribute is AliasAttribute alias)
                this.Writer.WriteAttributeString("AliasField", alias.Alias);
            RenderAttribute(attribute.Attribute);
            this.Writer.WriteEndElement();
            }

        private void RenderAttribute(IAttribute attribute)
            {
            if (!attribute.HasValue)
                return;

            switch (attribute)
                {
                case DecimalAttribute decimalAttribute:
                    this.Writer.WriteValue(decimalAttribute.Value!.Value.ToString(CultureInfo.InvariantCulture));
                    break;
                case IntAttribute intAttribute:
                    this.Writer.WriteValue(intAttribute.Value!.Value.ToString(CultureInfo.InvariantCulture));
                    break;
                case StringAttribute stringAttribute:
                    this.Writer.WriteValue(stringAttribute.Value!);
                    break;
                case GuidAttribute guidAttribute:
                    this.Writer.WriteValue(guidAttribute.Value!.Value.ToString("D"));
                    break;
                case DateAttribute dateAttribute:
                    this.Writer.WriteValue(dateAttribute.Value!.Value.ToString("yyyy-MM-dd"));
                    break;
                case DateTimeAttribute dateTimeAttribute:
                    this.Writer.WriteValue(dateTimeAttribute.Value!.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                    break;
                case BoolAttribute boolAttribute:
                    this.Writer.WriteValue(boolAttribute.Value.ToString().ToLowerInvariant());
                    break;
                default:
                    throw new InvalidOperationException();
                }
            }

        private void RenderChildren(OperationWithAttributesBase operation)
            {
            if (!operation.Children.Any())
                return;

            this.Writer.WriteStartElement("Children");
            foreach (DataObject a in operation.Children)
                {
                // ReSharper disable once PossibleNullReferenceException
                Render(a);
                }
            this.Writer.WriteEndElement();
            }

        internal void Render(DataObject dataObject)
            {
            this.Writer.WriteStartElement(dataObject.ObjectName);
            foreach (OperationBase o in dataObject.Operations)
                {
                // ReSharper disable once PossibleNullReferenceException
                RenderOperation(o, dataObject.ObjectName);
                }
            this.Writer.WriteEndElement();
            }

        private void RenderKey(IdentifyBase key)
            {
            switch (key)
                {
                case IdentifyByPrimaryKey identifyByPrimaryKey:
                    RenderKey(identifyByPrimaryKey);
                    break;
                case IdentifyByAlias identifyByAlias:
                    RenderKey(identifyByAlias);
                    break;
                case IdentifyByPosition identifyByPosition:
                    RenderKey(identifyByPosition);
                    break;
                case IdentifyByValue identifyByValue:
                    RenderKey(identifyByValue);
                    break;
                default:
                    throw new InvalidOperationException();
                }
            }

        internal void RenderKey(IdentifyByPrimaryKey key)
            {
            if (!key.KeyValue.HasValue)
                throw new InvalidOperationException("Primary Key value not set.");
            // ReSharper disable once AssignNullToNotNullAttribute
            this.Writer.WriteAttributeString("KeyValue", key.KeyValue.ToString());
            }

        internal void RenderKey(IdentifyByAlias key)
            {
            if (!key.KeyValue.HasValue)
                throw new InvalidOperationException("Alias value not set.");
            // ReSharper disable once AssignNullToNotNullAttribute
            this.Writer.WriteAttributeString("KeyValue", key.KeyValue.ToString());
            this.Writer.WriteAttributeString("AliasField", key.AliasField);
            }

        internal void RenderKey(IdentifyByPosition key)
            {
            this.Writer.WriteAttributeString("Position", key.Position.ToString(CultureInfo.InvariantCulture));
            }

        internal void RenderKey(IdentifyByValue key)
            {
            if (!key.KeyValue.HasValue)
                throw new InvalidOperationException("Value for key not set.");
            // ReSharper disable once AssignNullToNotNullAttribute
            this.Writer.WriteAttributeString("KeyValue", key.KeyValue.ToString());
            this.Writer.WriteAttributeString("KeyField", key.KeyField);
            }
        }
    }
