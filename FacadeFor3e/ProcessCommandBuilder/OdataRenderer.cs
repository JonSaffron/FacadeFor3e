using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace FacadeFor3e.ProcessCommandBuilder
    {
    /// <summary>
    /// Describes all the various values needed to call the OData service
    /// </summary>
    public class ODataRequest
        {
        /// <summary>
        /// The HTML verb to use
        /// </summary>
        public readonly string Verb;

        /// <summary>
        /// The URL to use
        /// </summary>
        public readonly string EndPoint;

        /// <summary>
        /// The command to send
        /// </summary>
        public readonly string Json;

        /// <summary>
        /// Constructs a new OData request object
        /// </summary>
        /// <param name="verb">The HTML verb to use</param>
        /// <param name="endPoint">The URL to use</param>
        /// <param name="json">The command to send</param>
        public ODataRequest(string verb, string endPoint, string json)
            {
            this.Verb = verb;
            this.EndPoint = endPoint;
            this.Json = json;
            }
        }

    internal class ODataRenderer
        {
        private JSonWriter _writer = new JSonWriter();
        
        public ODataRequest Render(ProcessCommand processCommand)
            {
            if (processCommand == null) throw new ArgumentNullException(nameof(processCommand));
            var operation = processCommand.Operations.SingleOrDefault();
            if (operation == null)
                {
                throw new InvalidOperationException("The 3E OData interface expects one and only one operation.");
                }
            string verb = GetVerbForOperation(operation);
            string endPoint = GetEndPointForOperation(operation, processCommand.ObjectName);
            string json = GetJson(operation, processCommand.ObjectName);
            return new ODataRequest(verb, endPoint, json);
            }

        private static string GetVerbForOperation(OperationBase operation)
            {
            switch (operation)
                {
                case AddOperation _: return "POST";
                case EditOperation _: return "PATCH";
                case DeleteOperation _: return "DELETE";
                default: throw new InvalidOperationException();
                }
            }

        private static string GetEndPointForOperation(OperationBase operation, string superClass)
            {
            string entity = operation.SubClass ?? superClass;
            IdentifyBase key;
            switch (operation)
                {
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
            return $"{entity}({primaryKey.KeyValue.Value})";
            }

        private string GetJson(OperationBase operation, string superClass)
            {
            this._writer = new JSonWriter();
            this._writer.WriteObjectStart();
            WriteProcessCommand(operation, superClass);
            this._writer.WriteObjectEnd();
            return this._writer.ToString();
            }

        private void WriteProcessCommand(OperationBase operation, string superClass)
            {
            var entity = operation.SubClass ?? superClass;
            this._writer.WriteProperty("@odata.type", $"E3E.OData.AllTypes.{entity}");
            this._writer.WriteProperty("_release_process", true);
            this._writer.WriteProperty("_cleanup_process_on_failure", true);

            if (operation is OperationWithAttributesBase operationWithAttributes)
                {
                WriteOperationAttributesAndChildren(operationWithAttributes);
                }
            }

        private void WriteChildOperation(OperationBase operation)
            {
            this._writer.WriteObjectStart();
            this._writer.WriteProperty("_patch_operation", GetPatchType(operation));
            
            IdentifyBase? identifyBase = (operation as IHasKey)?.KeySpecification;
            if (identifyBase != null)
                {
                WriteKeySpecification(identifyBase);
                }

            if (operation is OperationWithAttributesBase operationWithAttributes)
                {
                WriteOperationAttributesAndChildren(operationWithAttributes);
                }

            this._writer.WriteObjectEnd();
            }

        private void WriteKeySpecification(IdentifyBase identifyBase)
            {
            switch (identifyBase)
                {
                case IdentifyByPrimaryKey identifyByPrimaryKey:
                    this._writer.WriteProperty("***primary key***", identifyByPrimaryKey.KeyValue);
                    break;
                case IdentifyByPosition identifyByPosition:
                    this._writer.WriteProperty("_index", identifyByPosition.Position);
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
                this._writer.WriteAttribute(attribute);
                }

            var childOps = BuildChildOps(operationWithAttributes.Children);
            foreach (var objectName in childOps.Keys)
                {
                this._writer.WriteArrayStart($"{objectName}s");
                foreach (var listItem in childOps[objectName])
                    {
                    WriteChildOperation(listItem);
                    }
                this._writer.WriteArrayEnd();
                }
            }






        private class JSonWriter
            {
            private readonly Stack<JsonElement> _stack = new Stack<JsonElement>();
            private bool _lastElementWasProperty;
            private readonly StringBuilder _stringBuilder = new StringBuilder();

            private enum JsonElement
                {
                Object,
                Array
                }

            public void WriteObjectStart()
                {
                if (this._lastElementWasProperty)
                    {
                    this._stringBuilder.AppendLine(",");
                    }
                this._stringBuilder.AppendLine($"{Indent}{{");
                this._stack.Push(JsonElement.Object);
                this._lastElementWasProperty = false;
                }

            public void WriteObjectEnd()
                {
                if (this._stack.Pop() != JsonElement.Object)
                    throw new InvalidOperationException("Cannot end object - an array needs to be finished first");
                if (this._lastElementWasProperty)
                    {
                    this._stringBuilder.AppendLine();
                    }
                this._stringBuilder.Append($"{Indent}}}");
                this._lastElementWasProperty = true;
                }

            public void WriteArrayStart(string element)
                {
                if (this._lastElementWasProperty)
                    {
                    this._stringBuilder.AppendLine(",");
                    }
                this._stringBuilder.AppendLine($"{Indent}\"{element}\": [");
                this._stack.Push(JsonElement.Array);
                this._lastElementWasProperty = false;
                }

            public void WriteArrayEnd()
                {
                if (this._stack.Pop() != JsonElement.Array)
                    throw new InvalidOperationException("Cannot end array - an object needs to be finished first");
                if (this._lastElementWasProperty)
                    {
                    this._stringBuilder.AppendLine();
                    }
                this._stringBuilder.Append($"{Indent}]");
                this._lastElementWasProperty = true;
                }

            public void WriteAttribute(NamedAttributeValue attribute)
                {
                if (this._lastElementWasProperty)
                    {
                    this._stringBuilder.AppendLine(",");
                    }
                this._stringBuilder.Append($"{Indent}\"{attribute.Name}\": {GetAttributeValue(attribute.Attribute)}");
                this._lastElementWasProperty = true;
                }

            public void WriteProperty(string name, string value)
                {
                if (this._lastElementWasProperty)
                    {
                    this._stringBuilder.AppendLine(",");
                    }
                this._stringBuilder.Append($"{Indent}\"{name}\": \"{value}\"");
                this._lastElementWasProperty = true;
                }

            public void WriteProperty(string name, int value)
                {
                if (this._lastElementWasProperty)
                    {
                    this._stringBuilder.AppendLine(",");
                    }
                this._stringBuilder.Append($"{Indent}\"{name}\": {value.ToString(CultureInfo.InvariantCulture)}");
                this._lastElementWasProperty = true;
                }

            public void WriteProperty(string name, bool value)
                {
                if (this._lastElementWasProperty)
                    {
                    this._stringBuilder.AppendLine(",");
                    }
                this._stringBuilder.Append($"{Indent}\"{name}\": {value.ToString().ToLowerInvariant()}");
                this._lastElementWasProperty = true;
                }

            public void WriteProperty(string name, IAttribute attribute)
                {
                if (this._lastElementWasProperty)
                    {
                    this._stringBuilder.AppendLine(",");
                    }
                this._stringBuilder.Append($"{Indent}\"{name}\": {GetAttributeValue(attribute)}");
                this._lastElementWasProperty = true;
                }

            private static string GetAttributeValue(IAttribute attribute)
                {
                if (!attribute.HasValue)
                    return "null";

                switch (attribute)
                    {
                    case DecimalAttribute decimalAttribute:
                        return decimalAttribute.Value!.Value.ToString(CultureInfo.InvariantCulture);
                    case IntAttribute intAttribute:
                        return intAttribute.Value!.Value.ToString(CultureInfo.InvariantCulture);
                    case StringAttribute stringAttribute:
                        return $"\"{stringAttribute.Value}\"";
                    case GuidAttribute guidAttribute:
                        return $"\"{guidAttribute.Value!.Value:D}\"";
                    case DateAttribute dateAttribute:
                        return $"\"{dateAttribute.Value!.Value:yyyy-MM-dd}\"";
                    case DateTimeAttribute dateTimeAttribute:
                        return $"\"{dateTimeAttribute.Value!.Value:yyyy-MM-dd HH:mm:ss}\"";
                    case BoolAttribute boolAttribute:
                        return boolAttribute.Value.ToString().ToLowerInvariant();
                    default:
                        throw new InvalidOperationException();
                    }
                }

            public override string ToString() => this._stringBuilder.ToString();

            private string Indent => new string(' ', this._stack.Count << 2);
            }
        }
    }
