using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;

namespace FacadeFor3e.ProcessCommandBuilder
    {
    /// <summary>
    /// Renders a <see cref="ProcessCommand"/> into a <see cref="ODataRequest"/> for the OData service
    /// </summary>
    [Experimental("OData")]
    public class ODataRenderer
        {
        private Utf8JsonWriter _writer = null!;
        private ODataExecuteOptions _options = null!;
        
        /// <summary>
        /// Generate the JSON instruction to pass to the OData service
        /// </summary>
        /// <param name="processCommand">Specifies the command to be rendered</param>
        /// <param name="options">Specifies options that affect the output</param>
        /// <returns>An <see cref="ODataRequest"/> that contains all the details necessary to call the OData service</returns>
        /// <exception cref="ArgumentNullException">If a null value is passed in for one of the parameters</exception>
        /// <exception cref="InvalidOperationException">If the <see cref="ProcessCommand"/> is not compatible with the OData service</exception>
        public ODataRequest Render(ProcessCommand processCommand, ODataExecuteOptions options)
            {
            if (processCommand == null) throw new ArgumentNullException(nameof(processCommand));
            if (options == null) throw new ArgumentNullException(nameof(options));
            var operation = processCommand.Operations.SingleOrDefault();
            if (operation == null)
                {
                throw new InvalidOperationException("The 3E OData interface expects one and only one operation.");
                }
            this._options = options;
            HttpMethod verb = GetVerbForOperation(operation);
            Uri endPoint = GetEndPointForOperation(operation, processCommand.ObjectName);
            var jsonDocument = GetJson(processCommand);
            return new ODataRequest(verb, endPoint, jsonDocument);
            }

        private static HttpMethod GetVerbForOperation(OperationBase operation)
            {
            return operation switch
                {
                AddOperation _ => HttpMethod.Post,
                EditOperation _ => new HttpMethod("PATCH"),
                DeleteOperation _ => HttpMethod.Delete,
                _ => throw new InvalidOperationException()
                };
            }

        private static Uri GetEndPointForOperation(OperationBase operation, string superClass)
            {
            string entity = operation.SubClass ?? superClass;
            IdentifyBase key;
            switch (operation)
                {
                case AddOperation _:
                    return new Uri(entity, UriKind.Relative);

                case EditOperation edit: 
                    key = edit.KeySpecification;
                    break;

                case DeleteOperation delete: 
                    key = delete.KeySpecification;
                    break;

                default: throw new InvalidOperationException();
                }

            if (!(key is IdentifyByPrimaryKey primaryKey))
                {
                throw new InvalidOperationException("Don't know how to specify something other than the primary key");
                }
            var result = new Uri($"{entity}({primaryKey.KeyValue.Value})", UriKind.Relative);
            return result;
            }

        private byte[] GetJson(ProcessCommand processCommand)
            {
            var i = new JsonWriterOptions { Indented = true, SkipValidation = false };
            using var stream = new MemoryStream();
            using (this._writer = new Utf8JsonWriter(stream, i))
                {
                this._writer.WriteStartObject();
                
                var operation = processCommand.Operations.Single();
                var entity = operation.SubClass ?? processCommand.ObjectName;
                this._writer.WriteString("@odata.type", $"E3E.OData.AllTypes.{entity}");
                WriteProcessOptions();
                WriteProcessCommand(processCommand);

                if (operation is OperationWithAttributesBase operationWithAttributes)
                    {
                    WriteOperationAttributesAndChildren(operationWithAttributes);
                    }
                
                this._writer.WriteEndObject();
                this._writer.Flush();
                }

            return stream.ToArray();
            }

        private void WriteProcessOptions()
            {
            ODataExecuteOptions options = this._options;

            this._writer.WriteBoolean("_release_process", options.ReleaseProcess);
            
            this._writer.WriteBoolean("_cleanup_process_on_failure", options.CleanupProcessOnFailure);
            
            if (options.OutputStepOverride != null)
                {
                this._writer.WriteString("_output_step_override", options.OutputStepOverride);
                }

            if (options.RoleId.HasValue)
                {
                this._writer.WriteString("_role_id", options.RoleId.Value);
                }
            }

        private void WriteProcessCommand(ProcessCommand processCommand)
            {
            this._writer.WriteString("_process_override", processCommand.ProcessCode);
            
            if (processCommand.ProcessName != null)
                {
                this._writer.WriteString("_name", processCommand.ProcessName);
                }
            
            if (processCommand.Description != null)
                {
                this._writer.WriteString("_description", processCommand.Description);
                }

            if (processCommand.Priority != null)
                {
                this._writer.WriteString("_priority", processCommand.Priority.Priority);
                }

            if (processCommand.OperatingUnit != null)
                {
                this._writer.WriteString("_operating_unit", processCommand.OperatingUnit);
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
                    throw new InvalidOperationException($"Row identification type {identifyBase.GetType().Name} is not supported by OData");
                }
            }

        private static string GetPatchType(OperationBase op)
            {
            return op switch
                {
                AddOperation _ => "Add",
                EditOperation _ => "Update",
                DeleteOperation _ => "Delete",
                _ => throw new InvalidOperationException()
                };
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
                    // ReSharper disable once PossibleNullReferenceException
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
            foreach (var keyAndList in childOps)
                {
                this._writer.WriteStartArray($"{keyAndList.Key}s");
                // ReSharper disable once PossibleNullReferenceException
                foreach (var listItem in keyAndList.Value)
                    {
                    WriteChildOperation(listItem);
                    }
                this._writer.WriteEndArray();
                }
            }

        private void WriteAttribute(NamedAttributeValue attribute)
            {
            if (attribute is AliasAttribute)
                {
                if (this._options.AliasAttributesSupportBeingNamed)
                    {
                    this._writer.WriteStartObject($"{attribute.Name}_Alias");
                    this._writer.WriteString("@odata.type", "E3E.OData.AllTypes.AttributeAlias");
                    this._writer.WriteString("Attribute", attribute.Name);
                    this._writer.WritePropertyName("Value");
                    WriteAttributeValue(attribute.Attribute);
                    this._writer.WriteEndObject();
                    }
                else
                    {
                    this._writer.WritePropertyName($"{attribute.Name}_Alias");
                    WriteAttributeValue(attribute.Attribute);
                    }
                }
            else
                {
                this._writer.WritePropertyName(attribute.Name);
                WriteAttributeValue(attribute.Attribute);
                }
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
                    this._writer.WriteStringValue($"{dateAttribute.Value!.Value:yyyy-MM-dd}T00:00:00Z");
                    break;

                case DateTimeAttribute dateTimeAttribute:
                    this._writer.WriteStringValue(dateTimeAttribute.Value!.Value.ToString("s"));
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
