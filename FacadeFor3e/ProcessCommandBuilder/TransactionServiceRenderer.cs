using System;
using System.Globalization;
using System.Linq;
using System.Xml;

namespace FacadeFor3e.ProcessCommandBuilder
    {
    internal class TransactionServiceRenderer
        {
        /// <summary>
        /// Generate the XML instructions to pass to 3E
        /// </summary>
        /// <returns>The transaction to be carried out</returns>
        public XmlDocument Render(ProcessCommand processCommand)
            {
            if (processCommand == null) throw new ArgumentNullException(nameof(processCommand));
            var xmlDoc = new XmlDocument();
            using XmlWriter w = xmlDoc.CreateNavigator()!.AppendChild();
            Render(processCommand, w);
            return xmlDoc;
            }

        private static void Render(ProcessCommand processCommand, XmlWriter writer)
            {
            writer.WriteStartDocument();
            writer.WriteStartElement(processCommand.ProcessCode, processCommand.ProcessNameSpace);
            RenderProcessAttributes(processCommand, writer);
            writer.WriteStartElement("Initialize", processCommand.ObjectNameSpace);
            foreach (OperationBase o in processCommand.Operations)
                {
                RenderOperation(o, writer, processCommand.ObjectName);
                }
            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            }

        internal static void RenderProcessAttributes(ProcessCommand processCommand, XmlWriter writer)
            {
            if (processCommand.ProcessName != null)
                writer.WriteAttributeString("ObjectName", processCommand.ProcessName);
            if (processCommand.Description != null)
                writer.WriteAttributeString("Description", processCommand.Description);
            if (processCommand.Priority.HasValue)
                writer.WriteAttributeString("Priority", processCommand.Priority.Value.ToString("G"));
            if (processCommand.OperatingUnit != null)
                writer.WriteAttributeString("OperatingUnit", processCommand.OperatingUnit);
            if (processCommand.CheckSum.HasValue)
                writer.WriteAttributeString("CheckSum", processCommand.CheckSum.Value.ToString("G"));
            if (processCommand.ProxyUser != null)
                writer.WriteAttributeString("ProxyUser", processCommand.ProxyUser);
            if (processCommand.ProxyUserId != null)
                writer.WriteAttributeString("ProxyUserID", processCommand.ProxyUserId);
            if (processCommand.ProcessRequestType.HasValue)
                writer.WriteAttributeString("ProcessRequestType", processCommand.ProcessRequestType.Value.ToString("G"));
            if (processCommand.ProcessAutomationRoleAfterFirstStep != null)
                writer.WriteAttributeString("ProcessAutomationRoleAfterFirstStep", processCommand.ProcessAutomationRoleAfterFirstStep);
            if (processCommand.ProcessRequestSignature != null)
                writer.WriteAttributeString("ProcessRequestSignature", processCommand.ProcessRequestSignature);
            }

        private static void RenderOperation(OperationBase operation, XmlWriter writer, string objectSuperclassName)
            {
            switch (operation)
                {
                case AddOperation add:
                    Render(add, writer, objectSuperclassName);
                    break;
                case EditOperation edit:
                    Render(edit, writer, objectSuperclassName);
                    break;
                case DeleteOperation delete:
                    Render(delete, writer, objectSuperclassName);
                    break;
                default:
                    throw new InvalidOperationException();
                }
            }

        internal static void Render(AddOperation operation, XmlWriter writer, string objectSuperclassName)
            {
            writer.WriteStartElement("Add");
            writer.WriteStartElement(operation.SubClass ?? objectSuperclassName);

            RenderAttributes(operation, writer);
            RenderChildren(operation, writer);

            writer.WriteEndElement();
            writer.WriteEndElement();
            }

        internal static void Render(EditOperation operation, XmlWriter writer, string objectSuperclassName)
            {
            writer.WriteStartElement("Edit");
            writer.WriteStartElement(operation.SubClass ?? objectSuperclassName);

            RenderKey(operation.KeySpecification, writer);

            RenderAttributes(operation, writer);
            RenderChildren(operation, writer);

            writer.WriteEndElement();
            writer.WriteEndElement();
            }

        internal static void Render(DeleteOperation operation, XmlWriter writer, string objectSuperclassName)
            {
            writer.WriteStartElement("Delete");
            writer.WriteStartElement(operation.SubClass ?? objectSuperclassName);

            RenderKey(operation.KeySpecification, writer);

            writer.WriteEndElement();
            writer.WriteEndElement();
            }

        private static void RenderAttributes(OperationWithAttributesBase operation, XmlWriter writer)
            {
            if (operation.Attributes.Any())
                {
                writer.WriteStartElement("Attributes");
                foreach (NamedAttributeValue a in operation.Attributes)
                    {
                    Render(a, writer);
                    }
                writer.WriteEndElement();
                }
            }

        internal static void Render(NamedAttributeValue attribute, XmlWriter writer)
            {
            writer.WriteStartElement(attribute.Name);
            if (attribute is AliasAttribute alias)
                writer.WriteAttributeString("AliasField", alias.Alias);
            RenderAttribute(attribute.Attribute, writer);
            writer.WriteEndElement();
            }

        private static void RenderAttribute(IAttribute attribute, XmlWriter writer)
            {
            if (!attribute.HasValue)
                return;

            switch (attribute)
                {
                case DecimalAttribute decimalAttribute:
                    writer.WriteValue(decimalAttribute.Value!.Value.ToString(CultureInfo.InvariantCulture));
                    break;
                case IntAttribute intAttribute:
                    writer.WriteValue(intAttribute.Value!.Value.ToString(CultureInfo.InvariantCulture));
                    break;
                case StringAttribute stringAttribute:
                    writer.WriteValue(stringAttribute.Value!);
                    break;
                case GuidAttribute guidAttribute:
                    writer.WriteValue(guidAttribute.Value!.Value.ToString("D"));
                    break;
                case DateAttribute dateAttribute:
                    writer.WriteValue(dateAttribute.Value!.Value.ToString("yyyy-MM-dd"));
                    break;
                case DateTimeAttribute dateTimeAttribute:
                    writer.WriteValue(dateTimeAttribute.Value!.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                    break;
                case BoolAttribute boolAttribute:
                    writer.WriteValue(boolAttribute.Value.ToString().ToLowerInvariant());
                    break;
                default:
                    throw new InvalidOperationException();
                }
            }

        private static void RenderChildren(OperationWithAttributesBase operation, XmlWriter writer)
            {
            if (!operation.Children.Any())
                return;

            writer.WriteStartElement("Children");
            foreach (DataObject a in operation.Children)
                {
                // ReSharper disable once PossibleNullReferenceException
                Render(a, writer);
                }
            writer.WriteEndElement();
            }

        internal static void Render(DataObject dataObject, XmlWriter writer)
            {
            writer.WriteStartElement(dataObject.ObjectName);
            foreach (OperationBase o in dataObject.Operations)
                {
                // ReSharper disable once PossibleNullReferenceException
                RenderOperation(o, writer, dataObject.ObjectName);
                }
            writer.WriteEndElement();
            }

        private static void RenderKey(IdentifyBase key, XmlWriter writer)
            {
            switch (key)
                {
                case IdentifyByPrimaryKey identifyByPrimaryKey:
                    RenderKey(identifyByPrimaryKey, writer);
                    break;
                case IdentifyByAlias identifyByAlias:
                    RenderKey(identifyByAlias, writer);
                    break;
                case IdentifyByPosition identifyByPosition:
                    RenderKey(identifyByPosition, writer);
                    break;
                case IdentifyByValue identifyByValue:
                    RenderKey(identifyByValue, writer);
                    break;
                default:
                    throw new InvalidOperationException();
                }
            }

        internal static void RenderKey(IdentifyByPrimaryKey key, XmlWriter writer)
            {
            writer.WriteAttributeString("KeyValue", key.KeyValue.ToString());
            }

        internal static void RenderKey(IdentifyByAlias key, XmlWriter writer)
            {
            writer.WriteAttributeString("KeyValue", key.KeyValue.ToString());
            writer.WriteAttributeString("AliasField", key.AliasField);
            }

        internal static void RenderKey(IdentifyByPosition key, XmlWriter writer)
            {
            writer.WriteAttributeString("Position", key.Position.ToString(CultureInfo.InvariantCulture));
            }

        internal static void RenderKey(IdentifyByValue key, XmlWriter writer)
            {
            writer.WriteAttributeString("KeyValue", key.KeyValue.ToString());
            writer.WriteAttributeString("KeyField", key.KeyField);
            }
        }
    }
