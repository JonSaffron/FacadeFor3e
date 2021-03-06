﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18444
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FacadeFor3e.TransactionService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://tempuri.org//ServiceExecuteProcess", ConfigurationName="TransactionService.TransactionServiceSoap")]
    internal interface TransactionServiceSoap {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org//ServiceExecuteProcess/ExposeEnum", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        FacadeFor3e.TransactionService.ReturnInfoType ExposeEnum(string processXML, int returnInfo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org//ServiceExecuteProcess/ExecuteProcess", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string ExecuteProcess(string processXML, int returnInfo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org//ServiceExecuteProcess/GetAppObjectSecurityRights", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string GetAppObjectSecurityRights(string projectXML);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org//ServiceExecuteProcess/CancelAllOpenProcesses", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string CancelAllOpenProcesses();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org//ServiceExecuteProcess/GetFormattedArchetypeData", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string GetFormattedArchetypeData(string selectXml, string delimiterChar, bool includeHeaders);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org//ServiceExecuteProcess/GetArchetypeData", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string GetArchetypeData(string selectXml);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org//ServiceExecuteProcess/GetFormattedMetricData", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string GetFormattedMetricData(string selectXml, int metricRunIndex, bool isRunIndexRelative, string metricGroupID, string delimiterChar, bool includeHeaders);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org//ServiceExecuteProcess/GetMetricData", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string GetMetricData(string selectXml, int metricRunIndex, bool isRunIndexRelative, string metricGroupID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org//ServiceExecuteProcess/GetFormattedNamedMetricData", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string GetFormattedNamedMetricData(string selectXml, string metricTableName, string delimiterChar, bool includeHeaders);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org//ServiceExecuteProcess/GetNamedMetricData", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string GetNamedMetricData(string selectXml, string metricTableName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org//ServiceExecuteProcess/GetArchetypeDataSQL", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string GetArchetypeDataSQL(string selectXml);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org//ServiceExecuteProcess/GetMetricDataSQL", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string GetMetricDataSQL(string selectXml, int metricRunIndex, bool isRunIndexRelative, string metricGroupID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org//ServiceExecuteProcess/GetNamedMetricDataSQL", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string GetNamedMetricDataSQL(string selectXml, string metricTableName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org//ServiceExecuteProcess/GetData", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string GetData(string reportQueryXML);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org//ServiceExecuteProcess/GetDataFromReportLayout", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string GetDataFromReportLayout(string reportLayoutId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org//ServiceExecuteProcess/GetDataFromReportLayoutCrystal", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string GetDataFromReportLayoutCrystal(string reportLayoutId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org//ServiceExecuteProcess/GetSyncIds", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string[] GetSyncIds();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org//ServiceExecuteProcess/CheckFileHash", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string CheckFileHash(string syncId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org//ServiceExecuteProcess/Ping", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        void Ping();
        
        // CODEGEN: Parameter 'buffer' requires additional schema information that cannot be captured using the parameter mode. The specific attribute is 'System.Xml.Serialization.XmlElementAttribute'.
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org//ServiceExecuteProcess/SendAttachmentChunk", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        FacadeFor3e.TransactionService.SendAttachmentChunkResponse SendAttachmentChunk(FacadeFor3e.TransactionService.SendAttachmentChunkRequest request);
        
        // CODEGEN: Parameter 'buffer' requires additional schema information that cannot be captured using the parameter mode. The specific attribute is 'System.Xml.Serialization.XmlElementAttribute'.
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org//ServiceExecuteProcess/SendAttachment", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        FacadeFor3e.TransactionService.SendAttachmentResponse SendAttachment(FacadeFor3e.TransactionService.SendAttachmentRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org//ServiceExecuteProcess/GetQueryResults", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string GetQueryResults(string archID, string metricID, string objectID, string sessionID, string processItemID, string XPath, string objectData, string quickFindValue, string XOQLStatement, int topRows);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org//ServiceExecuteProcess/GetActionsList", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string GetActionsList();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org//ServiceExecuteProcess/GetDataFromArchetypes", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string GetDataFromArchetypes(string paramsXML);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org//ServiceExecuteProcess/UpdateSyncItemTag", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string UpdateSyncItemTag(string itemIDToSync, string tag);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org//ServiceExecuteProcess/GetOutOfSyncItems", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string GetOutOfSyncItems(string LastSyncTime, string Tag, int FirstRows, string ExcludeItemIDList);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org//ServiceExecuteProcess/DoSpellCheck", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string DoSpellCheck(string ActionXML);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org//ServiceExecuteProcess/CreateModel", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string CreateModel(string Model, bool IsGlobal, string ProcessID, string PIID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org//ServiceExecuteProcess/EditModel", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string EditModel(string Model, string Action, bool IsGlobal, string NewModelName, string ProcessID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org//ServiceExecuteProcess/GetMappedItem", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string GetMappedItem(string ItemID, string SyncMapID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org//ServiceExecuteProcess/GetOneNoteMap", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string GetOneNoteMap(string MapName, string ItemID, string ArchetypeID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org//ServiceExecuteProcess/GetBusinessObjectMethods", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string GetBusinessObjectMethods(string businessObject);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org//ServiceExecuteProcess/StartProcessWithDefaults", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string StartProcessWithDefaults(string defaultsXML);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org//ServiceExecuteProcess/TEST_GetData", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string TEST_GetData(string reportQueryXML);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org//ServiceExecuteProcess/TEST_GetDataFromReportLayout", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string TEST_GetDataFromReportLayout(string reportLayoutId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org//ServiceExecuteProcess/BuildFromClientXML", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string BuildFromClientXML(string clientXML);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org//ServiceExecuteProcess/ImageConnectDelete", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string ImageConnectDelete(string attachmentID, string parentItemID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org//ServiceExecuteProcess/GetOption", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string GetOption(string optionName, FacadeFor3e.TransactionService.DataTypeEnum optionType);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org//ServiceExecuteProcess/GetExpansionCodes", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string GetExpansionCodes(string[] codes);
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34234")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org//ServiceExecuteProcess")]
    public enum ReturnInfoType {
        
        /// <remarks/>
        None,
        
        /// <remarks/>
        Keys,
        
        /// <remarks/>
        Timing,
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="SendAttachmentChunk", WrapperNamespace="http://tempuri.org//ServiceExecuteProcess", IsWrapped=true)]
    internal partial class SendAttachmentChunkRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org//ServiceExecuteProcess", Order=0)]
        public string syncId;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org//ServiceExecuteProcess", Order=1)]
        public string fileName;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org//ServiceExecuteProcess", Order=2)]
        [System.Xml.Serialization.XmlElementAttribute(DataType="base64Binary")]
        public byte[] buffer;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org//ServiceExecuteProcess", Order=3)]
        public long offset;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org//ServiceExecuteProcess", Order=4)]
        public int bytesRead;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org//ServiceExecuteProcess", Order=5)]
        public long totalBytes;
        
        public SendAttachmentChunkRequest() {
        }
        
        public SendAttachmentChunkRequest(string syncId, string fileName, byte[] buffer, long offset, int bytesRead, long totalBytes) {
            this.syncId = syncId;
            this.fileName = fileName;
            this.buffer = buffer;
            this.offset = offset;
            this.bytesRead = bytesRead;
            this.totalBytes = totalBytes;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="SendAttachmentChunkResponse", WrapperNamespace="http://tempuri.org//ServiceExecuteProcess", IsWrapped=true)]
    internal partial class SendAttachmentChunkResponse {
        
        public SendAttachmentChunkResponse() {
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="SendAttachment", WrapperNamespace="http://tempuri.org//ServiceExecuteProcess", IsWrapped=true)]
    internal partial class SendAttachmentRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org//ServiceExecuteProcess", Order=0)]
        public System.Guid itemID;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org//ServiceExecuteProcess", Order=1)]
        public string archetypeID;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org//ServiceExecuteProcess", Order=2)]
        public string syncId;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org//ServiceExecuteProcess", Order=3)]
        public string fileName;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org//ServiceExecuteProcess", Order=4)]
        [System.Xml.Serialization.XmlElementAttribute(DataType="base64Binary")]
        public byte[] buffer;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org//ServiceExecuteProcess", Order=5)]
        public long offset;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org//ServiceExecuteProcess", Order=6)]
        public int bytesRead;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org//ServiceExecuteProcess", Order=7)]
        public long totalBytes;
        
        public SendAttachmentRequest() {
        }
        
        public SendAttachmentRequest(System.Guid itemID, string archetypeID, string syncId, string fileName, byte[] buffer, long offset, int bytesRead, long totalBytes) {
            this.itemID = itemID;
            this.archetypeID = archetypeID;
            this.syncId = syncId;
            this.fileName = fileName;
            this.buffer = buffer;
            this.offset = offset;
            this.bytesRead = bytesRead;
            this.totalBytes = totalBytes;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="SendAttachmentResponse", WrapperNamespace="http://tempuri.org//ServiceExecuteProcess", IsWrapped=true)]
    internal partial class SendAttachmentResponse {
        
        public SendAttachmentResponse() {
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34234")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org//ServiceExecuteProcess")]
    public enum DataTypeEnum {
        
        /// <remarks/>
        AUTONUMBER,
        
        /// <remarks/>
        BOOLEAN,
        
        /// <remarks/>
        DATE,
        
        /// <remarks/>
        DATETIME,
        
        /// <remarks/>
        DECIMAL,
        
        /// <remarks/>
        EMAIL,
        
        /// <remarks/>
        GUID,
        
        /// <remarks/>
        INTEGER,
        
        /// <remarks/>
        IMAGE,
        
        /// <remarks/>
        MONEY,
        
        /// <remarks/>
        MULTILANGUAGESTRING,
        
        /// <remarks/>
        NARRATIVE,
        
        /// <remarks/>
        PREDICATE,
        
        /// <remarks/>
        RELATIONSHIP,
        
        /// <remarks/>
        STRING,
        
        /// <remarks/>
        TEXT,
        
        /// <remarks/>
        URL,
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    internal interface TransactionServiceSoapChannel : FacadeFor3e.TransactionService.TransactionServiceSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    internal partial class TransactionServiceSoapClient : System.ServiceModel.ClientBase<FacadeFor3e.TransactionService.TransactionServiceSoap>, FacadeFor3e.TransactionService.TransactionServiceSoap {
        
        public TransactionServiceSoapClient() {
        }
        
        public TransactionServiceSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public TransactionServiceSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public TransactionServiceSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public TransactionServiceSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public FacadeFor3e.TransactionService.ReturnInfoType ExposeEnum(string processXML, int returnInfo) {
            return base.Channel.ExposeEnum(processXML, returnInfo);
        }
        
        public string ExecuteProcess(string processXML, int returnInfo) {
            return base.Channel.ExecuteProcess(processXML, returnInfo);
        }
        
        public string GetAppObjectSecurityRights(string projectXML) {
            return base.Channel.GetAppObjectSecurityRights(projectXML);
        }
        
        public string CancelAllOpenProcesses() {
            return base.Channel.CancelAllOpenProcesses();
        }
        
        public string GetFormattedArchetypeData(string selectXml, string delimiterChar, bool includeHeaders) {
            return base.Channel.GetFormattedArchetypeData(selectXml, delimiterChar, includeHeaders);
        }
        
        public string GetArchetypeData(string selectXml) {
            return base.Channel.GetArchetypeData(selectXml);
        }
        
        public string GetFormattedMetricData(string selectXml, int metricRunIndex, bool isRunIndexRelative, string metricGroupID, string delimiterChar, bool includeHeaders) {
            return base.Channel.GetFormattedMetricData(selectXml, metricRunIndex, isRunIndexRelative, metricGroupID, delimiterChar, includeHeaders);
        }
        
        public string GetMetricData(string selectXml, int metricRunIndex, bool isRunIndexRelative, string metricGroupID) {
            return base.Channel.GetMetricData(selectXml, metricRunIndex, isRunIndexRelative, metricGroupID);
        }
        
        public string GetFormattedNamedMetricData(string selectXml, string metricTableName, string delimiterChar, bool includeHeaders) {
            return base.Channel.GetFormattedNamedMetricData(selectXml, metricTableName, delimiterChar, includeHeaders);
        }
        
        public string GetNamedMetricData(string selectXml, string metricTableName) {
            return base.Channel.GetNamedMetricData(selectXml, metricTableName);
        }
        
        public string GetArchetypeDataSQL(string selectXml) {
            return base.Channel.GetArchetypeDataSQL(selectXml);
        }
        
        public string GetMetricDataSQL(string selectXml, int metricRunIndex, bool isRunIndexRelative, string metricGroupID) {
            return base.Channel.GetMetricDataSQL(selectXml, metricRunIndex, isRunIndexRelative, metricGroupID);
        }
        
        public string GetNamedMetricDataSQL(string selectXml, string metricTableName) {
            return base.Channel.GetNamedMetricDataSQL(selectXml, metricTableName);
        }
        
        public string GetData(string reportQueryXML) {
            return base.Channel.GetData(reportQueryXML);
        }
        
        public string GetDataFromReportLayout(string reportLayoutId) {
            return base.Channel.GetDataFromReportLayout(reportLayoutId);
        }
        
        public string GetDataFromReportLayoutCrystal(string reportLayoutId) {
            return base.Channel.GetDataFromReportLayoutCrystal(reportLayoutId);
        }
        
        public string[] GetSyncIds() {
            return base.Channel.GetSyncIds();
        }
        
        public string CheckFileHash(string syncId) {
            return base.Channel.CheckFileHash(syncId);
        }
        
        public void Ping() {
            base.Channel.Ping();
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        FacadeFor3e.TransactionService.SendAttachmentChunkResponse FacadeFor3e.TransactionService.TransactionServiceSoap.SendAttachmentChunk(FacadeFor3e.TransactionService.SendAttachmentChunkRequest request) {
            return base.Channel.SendAttachmentChunk(request);
        }
        
        public void SendAttachmentChunk(string syncId, string fileName, byte[] buffer, long offset, int bytesRead, long totalBytes) {
            FacadeFor3e.TransactionService.SendAttachmentChunkRequest inValue = new FacadeFor3e.TransactionService.SendAttachmentChunkRequest();
            inValue.syncId = syncId;
            inValue.fileName = fileName;
            inValue.buffer = buffer;
            inValue.offset = offset;
            inValue.bytesRead = bytesRead;
            inValue.totalBytes = totalBytes;
            FacadeFor3e.TransactionService.SendAttachmentChunkResponse retVal = ((FacadeFor3e.TransactionService.TransactionServiceSoap)(this)).SendAttachmentChunk(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        FacadeFor3e.TransactionService.SendAttachmentResponse FacadeFor3e.TransactionService.TransactionServiceSoap.SendAttachment(FacadeFor3e.TransactionService.SendAttachmentRequest request) {
            return base.Channel.SendAttachment(request);
        }
        
        public void SendAttachment(System.Guid itemID, string archetypeID, string syncId, string fileName, byte[] buffer, long offset, int bytesRead, long totalBytes) {
            FacadeFor3e.TransactionService.SendAttachmentRequest inValue = new FacadeFor3e.TransactionService.SendAttachmentRequest();
            inValue.itemID = itemID;
            inValue.archetypeID = archetypeID;
            inValue.syncId = syncId;
            inValue.fileName = fileName;
            inValue.buffer = buffer;
            inValue.offset = offset;
            inValue.bytesRead = bytesRead;
            inValue.totalBytes = totalBytes;
            FacadeFor3e.TransactionService.SendAttachmentResponse retVal = ((FacadeFor3e.TransactionService.TransactionServiceSoap)(this)).SendAttachment(inValue);
        }
        
        public string GetQueryResults(string archID, string metricID, string objectID, string sessionID, string processItemID, string XPath, string objectData, string quickFindValue, string XOQLStatement, int topRows) {
            return base.Channel.GetQueryResults(archID, metricID, objectID, sessionID, processItemID, XPath, objectData, quickFindValue, XOQLStatement, topRows);
        }
        
        public string GetActionsList() {
            return base.Channel.GetActionsList();
        }
        
        public string GetDataFromArchetypes(string paramsXML) {
            return base.Channel.GetDataFromArchetypes(paramsXML);
        }
        
        public string UpdateSyncItemTag(string itemIDToSync, string tag) {
            return base.Channel.UpdateSyncItemTag(itemIDToSync, tag);
        }
        
        public string GetOutOfSyncItems(string LastSyncTime, string Tag, int FirstRows, string ExcludeItemIDList) {
            return base.Channel.GetOutOfSyncItems(LastSyncTime, Tag, FirstRows, ExcludeItemIDList);
        }
        
        public string DoSpellCheck(string ActionXML) {
            return base.Channel.DoSpellCheck(ActionXML);
        }
        
        public string CreateModel(string Model, bool IsGlobal, string ProcessID, string PIID) {
            return base.Channel.CreateModel(Model, IsGlobal, ProcessID, PIID);
        }
        
        public string EditModel(string Model, string Action, bool IsGlobal, string NewModelName, string ProcessID) {
            return base.Channel.EditModel(Model, Action, IsGlobal, NewModelName, ProcessID);
        }
        
        public string GetMappedItem(string ItemID, string SyncMapID) {
            return base.Channel.GetMappedItem(ItemID, SyncMapID);
        }
        
        public string GetOneNoteMap(string MapName, string ItemID, string ArchetypeID) {
            return base.Channel.GetOneNoteMap(MapName, ItemID, ArchetypeID);
        }
        
        public string GetBusinessObjectMethods(string businessObject) {
            return base.Channel.GetBusinessObjectMethods(businessObject);
        }
        
        public string StartProcessWithDefaults(string defaultsXML) {
            return base.Channel.StartProcessWithDefaults(defaultsXML);
        }
        
        public string TEST_GetData(string reportQueryXML) {
            return base.Channel.TEST_GetData(reportQueryXML);
        }
        
        public string TEST_GetDataFromReportLayout(string reportLayoutId) {
            return base.Channel.TEST_GetDataFromReportLayout(reportLayoutId);
        }
        
        public string BuildFromClientXML(string clientXML) {
            return base.Channel.BuildFromClientXML(clientXML);
        }
        
        public string ImageConnectDelete(string attachmentID, string parentItemID) {
            return base.Channel.ImageConnectDelete(attachmentID, parentItemID);
        }
        
        public string GetOption(string optionName, FacadeFor3e.TransactionService.DataTypeEnum optionType) {
            return base.Channel.GetOption(optionName, optionType);
        }
        
        public string GetExpansionCodes(string[] codes) {
            return base.Channel.GetExpansionCodes(codes);
        }
    }
}
