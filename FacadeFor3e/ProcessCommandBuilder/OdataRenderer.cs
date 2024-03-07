using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;

namespace FacadeFor3e.ProcessCommandBuilder
    {
    /// <summary>
    /// Describes all the various values needed to call the OData service
    /// </summary>
    [Experimental("OData")]
    public class ODataRequest
        {
        /// <summary>
        /// The HTML verb to use
        /// </summary>
        public readonly HttpMethod Verb;

        /// <summary>
        /// The URL to use
        /// </summary>
        public readonly string EndPoint;

        /// <summary>
        /// The command to send
        /// </summary>
        public readonly byte[] Json;

        /// <summary>
        /// Constructs a new OData request object
        /// </summary>
        /// <param name="verb">The HTML verb to use</param>
        /// <param name="endPoint">The URL to use</param>
        /// <param name="json">The command to send</param>
        public ODataRequest(HttpMethod verb, string endPoint, byte[] json)
            {
            this.Verb = verb;
            this.EndPoint = endPoint;
            this.Json = json;
            }
        }

    [Experimental("OData")]
    internal class ODataRenderer
        {
        private Utf8JsonWriter _writer = null!;
        
        public ODataRequest Render(ProcessCommand processCommand)
            {
            if (processCommand == null) throw new ArgumentNullException(nameof(processCommand));
            var operation = processCommand.Operations.SingleOrDefault();
            if (operation == null)
                {
                throw new InvalidOperationException("The 3E OData interface expects one and only one operation.");
                }
            HttpMethod verb = GetVerbForOperation(operation);
            string endPoint = GetEndPointForOperation(operation, processCommand.ObjectName);
            var jsonDocument = GetJson(processCommand);
            return new ODataRequest(verb, endPoint, jsonDocument);
            }

        private static HttpMethod GetVerbForOperation(OperationBase operation)
            {
            switch (operation)
                {
                case AddOperation _: return HttpMethod.Post;
                case EditOperation _: return new HttpMethod("PATCH");
                case DeleteOperation _: return HttpMethod.Delete;
                default: throw new InvalidOperationException();
                }
            }

        private static string GetEndPointForOperation(OperationBase operation, string superClass)
            {
            string entity = operation.SubClass ?? superClass;
            IdentifyBase key;
            switch (operation)
                {
                case AddOperation _:
                    return entity;

                case EditOperation edit: 
                    key = edit.KeySpecification;
                    break;

                case DeleteOperation delete: 
                    key = delete.KeySpecification;
                    break;

                default: throw new InvalidOperationException();
                }
            IdentifyByPrimaryKey? primaryKey = key as IdentifyByPrimaryKey;
            if (primaryKey == null)
                {
                throw new InvalidOperationException("Don't know how to specify something other than the primary key");
                }
            return $"{entity}/{primaryKey.KeyValue.Value}";
            }

        private byte[] GetJson(ProcessCommand processCommand)
            {
            var i = new JsonWriterOptions { Indented = true, SkipValidation = false };
            using var stream = new MemoryStream();
            using (this._writer = new Utf8JsonWriter(stream, i))
                {
                this._writer.WriteStartObject();
                WriteProcessCommand(processCommand);
                this._writer.WriteEndObject();
                this._writer.Flush();
                }

            return stream.ToArray();
            }

        private void WriteProcessCommand(ProcessCommand processCommand)
            {
            var operation = processCommand.Operations.Single();
            var entity = operation.SubClass ?? processCommand.ObjectName;
            this._writer.WriteString("@odata.type", $"E3E.OData.AllTypes.{entity}");
            this._writer.WriteString("_process_override", processCommand.ProcessCode);
            this._writer.WriteBoolean("_release_process", true);
            this._writer.WriteBoolean("_cleanup_process_on_failure", true);
            if (processCommand.OutputStepOverride != null)
                {
                this._writer.WriteString("_output_step_override", processCommand.OutputStepOverride);
                }
            if (processCommand.Description != null)
                {
                this._writer.WriteString("_description", processCommand.Description);
                }
            if (processCommand.ProcessName != null)
                {
                this._writer.WriteString("_name", processCommand.ProcessName);
                }
            if (processCommand.OperatingUnit != null)
                {
                this._writer.WriteString("_operating_unit", processCommand.OperatingUnit);
                }
            if (processCommand.ProxyUserId != null)
                {
                this._writer.WriteString("_role_id", processCommand.ProxyUserId);
                }

            if (operation is OperationWithAttributesBase operationWithAttributes)
                {
                WriteOperationAttributesAndChildren(operationWithAttributes);
                }
            }

        private void WriteChildOperation(OperationBase operation)
            {
            this._writer.WriteStartObject();
            this._writer.WriteString("_patch_operation", GetPatchType(operation));
            
            IdentifyBase? identifyBase = (operation as IHasKey)?.KeySpecification;
            if (identifyBase != null)
                {
                WriteKeySpecification(identifyBase);
                }

            if (operation is OperationWithAttributesBase operationWithAttributes)
                {
                WriteOperationAttributesAndChildren(operationWithAttributes);
                }

            this._writer.WriteEndObject();
            }

        private void WriteKeySpecification(IdentifyBase identifyBase)
            {
            switch (identifyBase)
                {
                case IdentifyByPrimaryKey identifyByPrimaryKey:
                    if (identifyByPrimaryKey.PrimaryKeyName == null)
                        throw new InvalidOperationException("OData requires the name of the primary key attribute when used to target a row in a child collection.");
                    this._writer.WritePropertyName(identifyByPrimaryKey.PrimaryKeyName);
                    WriteAttributeValue(identifyByPrimaryKey.KeyValue);
                    break;
                case IdentifyByPosition identifyByPosition:
                    this._writer.WriteNumber("_index", identifyByPosition.Position);
                    break;
                default:
                    throw new InvalidOperationException("Don't know how to deal with other row identification type " + identifyBase.GetType().Name);
                }
            }

        private static string GetPatchType(OperationBase op)
            {
            switch (op)
                {
                case AddOperation _: return "Add";
                case EditOperation _: return "Update";
                case DeleteOperation _: return "Delete";
                default: throw new InvalidOperationException();
                }
            }

        private static Dictionary<string, List<OperationBase>> BuildChildOps(IEnumerable<DataObject> childObjects)
            {
            var result = new Dictionary<string, List<OperationBase>>(StringComparer.OrdinalIgnoreCase);
            foreach (var dataObject in childObjects)
                {
                var baseClass = dataObject.ObjectName;
                foreach (var op in dataObject.Operations)
                    {
                    var entity = op.SubClass ?? baseClass;
                    if (!result.TryGetValue(entity, out List<OperationBase>? listOfOps))
                        {
                        listOfOps = new List<OperationBase>();
                        result.Add(entity, listOfOps);
                        }
                    listOfOps.Add(op);
                    }
                }
            return result;
            }

        private void WriteOperationAttributesAndChildren(OperationWithAttributesBase operationWithAttributes)
            {
            foreach (var attribute in operationWithAttributes.Attributes)
                {
                WriteAttribute(attribute);
                }

            var childOps = BuildChildOps(operationWithAttributes.Children);
            foreach (var objectName in childOps.Keys)
                {
                this._writer.WriteStartArray($"{objectName}s");
                foreach (var listItem in childOps[objectName])
                    {
                    WriteChildOperation(listItem);
                    }
                this._writer.WriteEndArray();
                }
            }

        private void WriteAttribute(NamedAttributeValue attribute)
            {
            if (attribute is AliasAttribute _)
                {
                this._writer.WritePropertyName($"{attribute.Name}_Alias");
                }
            else
                {
                this._writer.WritePropertyName(attribute.Name);
                }
            WriteAttributeValue(attribute.Attribute);
            }

        private void WriteAttributeValue(IAttribute attribute)
            {
            if (!attribute.HasValue)
                {
                this._writer.WriteNullValue();
                return;
                }

            switch (attribute)
                {
                case DecimalAttribute decimalAttribute:
                    this._writer.WriteNumberValue(decimalAttribute.Value!.Value);
                    break;

                case IntAttribute intAttribute:
                    this._writer.WriteNumberValue(intAttribute.Value!.Value);
                    break;

                case StringAttribute stringAttribute:
                    this._writer.WriteStringValue(stringAttribute.Value);
                    break;

                case GuidAttribute guidAttribute:
                    this._writer.WriteStringValue(guidAttribute.Value!.Value);
                    break;

                case DateAttribute dateAttribute:
                    this._writer.WriteStringValue(dateAttribute.Value!.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
                    break;

                case DateTimeAttribute dateTimeAttribute:
                    this._writer.WriteStringValue(dateTimeAttribute.Value!.Value);
                    break;

                case BoolAttribute boolAttribute:
                    this._writer.WriteBooleanValue(boolAttribute.Value);
                    break;

                default:
                    throw new InvalidOperationException();
                }
            }
        }
    }
