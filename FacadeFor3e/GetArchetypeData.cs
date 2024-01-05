using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Principal;
using System.Xml;
using JetBrains.Annotations;

namespace FacadeFor3e
    {
    /// <summary>
    /// An object that runs an XOQL query to return data from the 3E database
    /// </summary>
    [PublicAPI]
    public class GetArchetypeData
        {
        private readonly TransactionServices _transactionServices;

        internal GetArchetypeData(TransactionServices transactionServices)
            {
            this._transactionServices = transactionServices ?? throw new ArgumentNullException(nameof(transactionServices));
            }

        /// <summary>
        /// Runs the specified query and returns the results as an XmlDocument
        /// </summary>
        /// <param name="xoql">Specifies the query to execute</param>
        /// <param name="endpoint">The url to use to connect to the 3E transaction service</param>
        /// <returns>An XML representation of the result set</returns>
        /// <remarks>The query is run in the context of the account running the process. Row level security will be observed.</remarks>
        public static XmlDocument GetData(XmlDocument xoql, Uri endpoint)
            {
            using var services = new TransactionServices(endpoint);
            var result = services.GetArchetypeData(xoql);
            return result;
            }

        /// <summary>
        /// Runs the specified query and returns the results as an XmlDocument
        /// </summary>
        /// <param name="xoql">Specifies the query to execute</param>
        /// <param name="endpoint">The url to use to connect to the 3E transaction service</param>
        /// <param name="accountToImpersonate">The account to impersonate</param>
        /// <returns>An XML representation of the result set</returns>
        /// <remarks>The query is run in the context of the account running the process. Row level security will be observed.</remarks>
        public static XmlDocument GetData(XmlDocument xoql, Uri endpoint, WindowsIdentity accountToImpersonate)
            {
            using var services = new TransactionServices(endpoint, accountToImpersonate);
            var result = services.GetArchetypeData(xoql);
            return result;
            }

        /// <summary>
        /// Runs the specified query and returns the results as an XmlDocument
        /// </summary>
        /// <param name="xoql">Specifies the query to execute</param>
        /// <param name="endpoint">The url to use to connect to the 3E transaction service</param>
        /// <param name="networkCredential">The credentials to use when calling the 3E transaction service</param>
        /// <returns>An XML representation of the result set</returns>
        /// <remarks>The query is run in the context of the account running the process. Row level security will be observed.</remarks>
        public static XmlDocument GetData(XmlDocument xoql, Uri endpoint, NetworkCredential networkCredential)
            {
            using var services = new TransactionServices(endpoint, networkCredential);
            var result = services.GetArchetypeData(xoql);
            return result;
            }

        /////////////////////////

        /// <summary>
        /// Runs the specified query and returns the results as an XmlDocument
        /// </summary>
        /// <param name="xoql">Specifies the query to execute</param>
        /// <param name="endpoint">The url to use to connect to the 3E transaction service</param>
        /// <returns>One of the following: Boolean, Integer, DateTime, DateOnly, Decimal, Guid, String</returns>
        /// <remarks>The query is run in the context of the account running the process. Row level security will be observed.</remarks>
        public static T GetScalarValue<T>(XmlDocument xoql, Uri endpoint)
            {
            using var services = new TransactionServices(endpoint);
            var result = services.GetArchetypeData(xoql);
            return TranslateScalarValue<T>(result, services.GetServiceCulture());
            }

        /// <summary>
        /// Runs the specified query and returns the results as an XmlDocument
        /// </summary>
        /// <param name="xoql">Specifies the query to execute</param>
        /// <param name="endpoint">The url to use to connect to the 3E transaction service</param>
        /// <param name="accountToImpersonate">The account to impersonate</param>
        /// <returns>One of the following: Boolean, Integer, DateTime, DateOnly, Decimal, Guid, String</returns>
        /// <remarks>The query is run in the context of the account running the process. Row level security will be observed.</remarks>
        public static T GetScalarValue<T>(XmlDocument xoql, Uri endpoint, WindowsIdentity accountToImpersonate)
            {
            using var services = new TransactionServices(endpoint, accountToImpersonate);
            var result = services.GetArchetypeData(xoql);
            return TranslateScalarValue<T>(result, services.GetServiceCulture());
            }

        /// <summary>
        /// Runs the specified query and returns the results as an XmlDocument
        /// </summary>
        /// <param name="xoql">Specifies the query to execute</param>
        /// <param name="endpoint">The url to use to connect to the 3E transaction service</param>
        /// <param name="networkCredential">The credentials to use when calling the 3E transaction service</param>
        /// <returns>One of the following: Boolean, Integer, DateTime, DateOnly, Decimal, Guid, String</returns>
        /// <remarks>The query is run in the context of the account running the process. Row level security will be observed.</remarks>
        public static T GetScalarValue<T>(XmlDocument xoql, Uri endpoint, NetworkCredential networkCredential)
            {
            using var services = new TransactionServices(endpoint, networkCredential);
            var result = services.GetArchetypeData(xoql);
            return TranslateScalarValue<T>(result, services.GetServiceCulture());
            }

        /////////////////////////

        /// <summary>
        /// Runs the specified query and returns the results as an XmlDocument
        /// </summary>
        /// <param name="xoql">Specifies the query to execute</param>
        /// <param name="endpoint">The url to use to connect to the 3E transaction service</param>
        /// <returns>A List of one of the following: Boolean, Integer, DateTime, DateOnly, Decimal, Guid, String</returns>
        /// <remarks>The query is run in the context of the account running the process. Row level security will be observed.</remarks>
        public static List<T> GetScalarList<T>(XmlDocument xoql, Uri endpoint)
            {
            using var services = new TransactionServices(endpoint);
            var result = services.GetArchetypeData(xoql);
            return TranslateScalarList<T>(result, services.GetServiceCulture());
            }

        /// <summary>
        /// Runs the specified query and returns the results as an XmlDocument
        /// </summary>
        /// <param name="xoql">Specifies the query to execute</param>
        /// <param name="endpoint">The url to use to connect to the 3E transaction service</param>
        /// <param name="accountToImpersonate">The account to impersonate</param>
        /// <returns>A List of one of the following: Boolean, Integer, DateTime, DateOnly, Decimal, Guid, String</returns>
        /// <remarks>The query is run in the context of the account running the process. Row level security will be observed.</remarks>
        public static List<T> GetScalarList<T>(XmlDocument xoql, Uri endpoint, WindowsIdentity accountToImpersonate)
            {
            using var services = new TransactionServices(endpoint, accountToImpersonate);
            var result = services.GetArchetypeData(xoql);
            return TranslateScalarList<T>(result, services.GetServiceCulture());
            }

        /// <summary>
        /// Runs the specified query and returns the results as an XmlDocument
        /// </summary>
        /// <param name="xoql">Specifies the query to execute</param>
        /// <param name="endpoint">The url to use to connect to the 3E transaction service</param>
        /// <param name="networkCredential">The credentials to use when calling the 3E transaction service</param>
        /// <returns>A List of one of the following: Boolean, Integer, DateTime, DateOnly, Decimal, Guid, String</returns>
        /// <remarks>The query is run in the context of the account running the process. Row level security will be observed.</remarks>
        public static List<T> GetScalarList<T>(XmlDocument xoql, Uri endpoint, NetworkCredential networkCredential)
            {
            using var services = new TransactionServices(endpoint, networkCredential);
            var result = services.GetArchetypeData(xoql);
            return TranslateScalarList<T>(result, services.GetServiceCulture());
            }

        /////////////////////////

        /// <summary>
        /// Runs the specified query and returns the results as an XmlDocument
        /// </summary>
        /// <param name="xoql">Specifies the query to execute</param>
        /// <param name="endpoint">The url to use to connect to the 3E transaction service</param>
        /// <returns>A POCO whose fields/properties match the column names returned and are of one of the following: Boolean, Integer, DateTime, DateOnly, Decimal, Guid, String</returns>
        /// <remarks>The query is run in the context of the account running the process. Row level security will be observed.</remarks>
        public static T GetCompoundValue<T>(XmlDocument xoql, Uri endpoint) where T : class, new()
            {
            using var services = new TransactionServices(endpoint);
            var result = services.GetArchetypeData(xoql);
            return TranslateCompoundValue<T>(result, services.GetServiceCulture());
            }

        /// <summary>
        /// Runs the specified query and returns the results as an XmlDocument
        /// </summary>
        /// <param name="xoql">Specifies the query to execute</param>
        /// <param name="endpoint">The url to use to connect to the 3E transaction service</param>
        /// <param name="accountToImpersonate">The account to impersonate</param>
        /// <returns>A POCO whose fields/properties match the column names returned and are of one of the following: Boolean, Integer, DateTime, DateOnly, Decimal, Guid, String</returns>
        /// <remarks>The query is run in the context of the account running the process. Row level security will be observed.</remarks>
        public static T GetCompoundValue<T>(XmlDocument xoql, Uri endpoint, WindowsIdentity accountToImpersonate) where T : class, new()
            {
            using var services = new TransactionServices(endpoint, accountToImpersonate);
            var result = services.GetArchetypeData(xoql);
            return TranslateCompoundValue<T>(result, services.GetServiceCulture());
            }

        /// <summary>
        /// Runs the specified query and returns the results as an XmlDocument
        /// </summary>
        /// <param name="xoql">Specifies the query to execute</param>
        /// <param name="endpoint">The url to use to connect to the 3E transaction service</param>
        /// <param name="networkCredential">The credentials to use when calling the 3E transaction service</param>
        /// <returns>A POCO whose fields/properties match the column names returned and are of one of the following: Boolean, Integer, DateTime, DateOnly, Decimal, Guid, String</returns>
        /// <remarks>The query is run in the context of the account running the process. Row level security will be observed.</remarks>
        public static T GetCompoundValue<T>(XmlDocument xoql, Uri endpoint, NetworkCredential networkCredential) where T : class, new()
            {
            using var services = new TransactionServices(endpoint, networkCredential);
            var result = services.GetArchetypeData(xoql);
            return TranslateCompoundValue<T>(result, services.GetServiceCulture());
            }

        /////////////////////////

        /// <summary>
        /// Runs the specified query and returns the results as an XmlDocument
        /// </summary>
        /// <param name="xoql">Specifies the query to execute</param>
        /// <param name="endpoint">The url to use to connect to the 3E transaction service</param>
        /// <returns>A List of POCOs whose fields/properties match the column names returned and are of one of the following: Boolean, Integer, DateTime, DateOnly, Decimal, Guid, String</returns>
        /// <remarks>The query is run in the context of the account running the process. Row level security will be observed.</remarks>
        public static List<T> GetCompoundList<T>(XmlDocument xoql, Uri endpoint) where T : class, new()
            {
            using var services = new TransactionServices(endpoint);
            var result = services.GetArchetypeData(xoql);
            return TranslateCompoundList<T>(result, services.GetServiceCulture());
            }

        /// <summary>
        /// Runs the specified query and returns the results as an XmlDocument
        /// </summary>
        /// <param name="xoql">Specifies the query to execute</param>
        /// <param name="endpoint">The url to use to connect to the 3E transaction service</param>
        /// <param name="accountToImpersonate">The account to impersonate</param>
        /// <returns>A List of POCOs whose fields/properties match the column names returned and are of one of the following: Boolean, Integer, DateTime, DateOnly, Decimal, Guid, String</returns>
        /// <remarks>The query is run in the context of the account running the process. Row level security will be observed.</remarks>
        public static List<T> GetCompoundList<T>(XmlDocument xoql, Uri endpoint, WindowsIdentity accountToImpersonate) where T : class, new()
            {
            using var services = new TransactionServices(endpoint, accountToImpersonate);
            var result = services.GetArchetypeData(xoql);
            return TranslateCompoundList<T>(result, services.GetServiceCulture());
            }

        /// <summary>
        /// Runs the specified query and returns the results as an XmlDocument
        /// </summary>
        /// <param name="xoql">Specifies the query to execute</param>
        /// <param name="endpoint">The url to use to connect to the 3E transaction service</param>
        /// <param name="networkCredential">The credentials to use when calling the 3E transaction service</param>
        /// <returns>A List of POCOs whose fields/properties match the column names returned and are of one of the following: Boolean, Integer, DateTime, DateOnly, Decimal, Guid, String</returns>
        /// <remarks>The query is run in the context of the account running the process. Row level security will be observed.</remarks>
        public static List<T> GetCompoundList<T>(XmlDocument xoql, Uri endpoint, NetworkCredential networkCredential) where T : class, new()
            {
            using var services = new TransactionServices(endpoint, networkCredential);
            var result = services.GetArchetypeData(xoql);
            return TranslateCompoundList<T>(result, services.GetServiceCulture());
            }

        /////////////////////////

        /// <summary>
        /// Runs the specified query and returns the results as an XmlDocument
        /// </summary>
        /// <param name="xoql">Specifies the query to execute</param>
        /// <returns>One of the following: Boolean, Integer, DateTime, DateOnly, Decimal, Guid, String</returns>
        /// <remarks>The query is run in the context of the account running the process. Row level security will be observed.</remarks>
        public T GetScalarValue<T>(XmlDocument xoql)
            {
            var data = this._transactionServices.GetArchetypeData(xoql);
            return TranslateScalarValue<T>(data, this._transactionServices.GetServiceCulture());
            }

        /// <summary>
        /// Runs the specified query and returns the results as an XmlDocument
        /// </summary>
        /// <param name="xoql">Specifies the query to execute</param>
        /// <returns>A List of one of the following: Boolean, Integer, DateTime, DateOnly, Decimal, Guid, String</returns>
        /// <remarks>The query is run in the context of the account running the process. Row level security will be observed.</remarks>
        public List<T> GetScalarList<T>(XmlDocument xoql)
            {
            var data = this._transactionServices.GetArchetypeData(xoql);
            return TranslateScalarList<T>(data, this._transactionServices.GetServiceCulture());
            }
        
        /// <summary>
        /// Runs the specified query and returns the results as an XmlDocument
        /// </summary>
        /// <param name="xoql">Specifies the query to execute</param>
        /// <returns>A POCO whose fields/properties match the column names returned and are of one of the following: Boolean, Integer, DateTime, DateOnly, Decimal, Guid, String</returns>
        /// <remarks>The query is run in the context of the account running the process. Row level security will be observed.</remarks>
        public T GetCompoundValue<T>(XmlDocument xoql) where T : class, new()
            {
            var data = this._transactionServices.GetArchetypeData(xoql);
            return TranslateCompoundValue<T>(data, this._transactionServices.GetServiceCulture());
            }
        
        /// <summary>
        /// Runs the specified query and returns the results as an XmlDocument
        /// </summary>
        /// <param name="xoql">Specifies the query to execute</param>
        /// <returns>A List of POCOs whose fields/properties match the column names returned and are of one of the following: Boolean, Integer, DateTime, DateOnly, Decimal, Guid, String</returns>
        /// <remarks>The query is run in the context of the account running the process. Row level security will be observed.</remarks>
        public List<T> GetCompoundList<T>(XmlDocument xoql) where T : class, new()
            {
            var data = this._transactionServices.GetArchetypeData(xoql);
            return TranslateCompoundList<T>(data, this._transactionServices.GetServiceCulture());
            }
        
        /// <summary>
        /// Runs the specified query and returns the results as an XmlDocument
        /// </summary>
        /// <param name="xoql">Specifies the query to execute</param>
        /// <returns>An XML representation of the result set</returns>
        /// <remarks>Row level security will be observed.</remarks>
        public XmlDocument GetData(XmlDocument xoql)
            {
            if (xoql == null)
                throw new ArgumentNullException(nameof(xoql));
            if (xoql.DocumentElement == null)
                throw new ArgumentOutOfRangeException(nameof(xoql));
            
            string? Func() => CallTransactionService(xoql);
            var response = this._transactionServices.IsImpersonating
                // ReSharper disable once AssignNullToNotNullAttribute
                ? WindowsIdentity.RunImpersonated(this._transactionServices.AccountToImpersonate!.AccessToken, Func)
                : Func();

            if (response == null)
                {
                // when no rows are returned, then return something useful
                var result = new XmlDocument();
                result.AppendChild(result.CreateElement("Data"));
                this._transactionServices.LogForDebug("no results returned from xoql query");
                return result;
                }

            if (response.StartsWith("<Data>") && response.EndsWith("</Data>"))
                {
                var result = new XmlDocument();
                result.LoadXml(response);
                string responseFormatted = result.PrettyPrintXml();
                this._transactionServices.LogForDebug(responseFormatted);
                return result;
                }

            this._transactionServices.LogForError(response);
            throw new InvalidOperationException("An invalid response was received from the web server:\r\n" + response);
            }

        private string? CallTransactionService(XmlDocument xoql)
            {
            OutputToConsoleDetailsOfTheJob(xoql);
            this._transactionServices.LogForDebug(xoql.PrettyPrintXml());

            // deliberately only passing through the document element, not the xml declaration on any leading or following comments
            // this is because the 3E transaction service is intolerant of anything but the simplest xml
            // ReSharper disable once PossibleNullReferenceException
            var request = xoql.DocumentElement!.OuterXml;
            var result = this._transactionServices.SoapClient.GetArchetypeData(request); // if no rows are returned, then result will be null
            return result;
            }

        private void OutputToConsoleDetailsOfTheJob(XmlDocument xoql)
            {
            // ReSharper disable once AssignNullToNotNullAttribute
            var xnm = new XmlNamespaceManager(xoql.NameTable);
            xnm.AddNamespace("ns", "http://elite.com/schemas/query");
            XmlElement? arch = (XmlElement?) xoql.SelectSingleNode("ns:SELECT/ns:OQL_CONTEXT/ns:NODEMAP[@ID='Node#1']", xnm);
            var archetype = arch?.GetAttribute("QueryID");
            var jobSpecifics = $"Getting data from archetype {archetype ?? "unknown"}";
            this._transactionServices.LogDetailsOfTheJob(jobSpecifics);
            }

        internal static T TranslateScalarValue<T>(XmlDocument xmlDoc, CultureInfo cultureInfo)
            {
            XmlElement data = xmlDoc.DocumentElement!;
            if (data.ChildNodes.Count == 0)
                throw new InvalidOperationException("No data returned from query.");
            if (data.ChildNodes.Count > 1)
                throw new InvalidOperationException("More than one row returned from query.");
            XmlElement row = (XmlElement)data.ChildNodes[0]!;
            if (row.ChildNodes.Count > 1)
                throw new InvalidOperationException("More than one value returned from query");
            string value = row.ChildNodes[0]!.InnerText;
            return TranslateValue<T>(value, cultureInfo);
            }

        internal static List<T> TranslateScalarList<T>(XmlDocument xmlDoc, CultureInfo cultureInfo)
            {
            XmlElement data = xmlDoc.DocumentElement!;
            var result = new List<T>();
            // ReSharper disable once RedundantSuppressNullableWarningExpression
            IEnumerator enumerator = data.ChildNodes.GetEnumerator()!;
            using var unknown = enumerator as IDisposable;
            if (!enumerator.MoveNext())
                {
                return result;
                }
            var row = (XmlElement)enumerator.Current!;
            if (row.ChildNodes.Count > 1)
                throw new InvalidOperationException("More than one value returned from query");
            do
                {
                T value = TranslateValue<T>(row.ChildNodes[0]!.InnerText, cultureInfo);
                result.Add(value);
                } while (enumerator.MoveNext());
            return result;
            }

        internal static T TranslateCompoundValue<T>(XmlDocument xmlDoc, CultureInfo cultureInfo) where T : class, new()
            {
            XmlElement data = xmlDoc.DocumentElement!;
            if (data.ChildNodes.Count == 0)
                throw new InvalidOperationException("No data returned from query.");
            if (data.ChildNodes.Count > 1)
                throw new InvalidOperationException("More than one row returned from query.");
            var map = MapColumns(xmlDoc, typeof(T));
            XmlElement row = (XmlElement)data.ChildNodes[0]!;
            var result = BuildCompoundValue<T>(row, map, cultureInfo);
            return result;
            }

        internal static List<T> TranslateCompoundList<T>(XmlDocument xmlDoc, CultureInfo cultureInfo) where T : class, new()
            {
            if (!xmlDoc.DocumentElement!.HasChildNodes)
                return new List<T>();
            var map = MapColumns(xmlDoc, typeof(T));
            XmlElement data = xmlDoc.DocumentElement!;
            var result = data.ChildNodes.Cast<XmlElement>().Select(row => BuildCompoundValue<T>(row, map, cultureInfo)).ToList();
            return result;
            }
            
        internal static T BuildCompoundValue<T>(XmlElement row, Dictionary<string, ICompoundMemberAccess> map, CultureInfo cultureInfo) where T : new()
            {
            var result = new T();
            foreach (XmlElement column in row.ChildNodes)
                {
                var name = column.LocalName;
                var compoundMemberAccess = map[name];
                compoundMemberAccess.CopyValue(column.InnerText, cultureInfo, result);
                }
            return result;
            }

        internal static Dictionary<string, ICompoundMemberAccess> MapColumns(XmlDocument xmlDoc, Type t)
            {
            XmlElement firstRow = (XmlElement) xmlDoc.DocumentElement!.ChildNodes[0]!;
            var result = new Dictionary<string, ICompoundMemberAccess>(firstRow.ChildNodes.Count);
            foreach (XmlElement column in firstRow)
                {
                string columnName = column.LocalName;
                if (result.ContainsKey(columnName))
                    throw new InvalidOperationException($"Returned data contains duplicate column {columnName}");
                var compoundMemberAccess = CompoundMemberAccess.GetMemberAccess(t, columnName);
                result.Add(columnName, compoundMemberAccess);
                }
            return result;
            }

        internal static T TranslateValue<T>(string value, CultureInfo cultureInfo)
            {
            if (typeof(T).IsAssignableFrom(typeof(bool)))
                {
                if (value == "")
                    {
                    if (typeof(T) == typeof(bool?))
                        {
                        return (T) (object) new bool?()!;
                        }
                    throw new InvalidOperationException("Null value returned.");
                    }
                if (value == "0")
                    return (T) (object) false;
                if (value == "1")
                    return (T) (object) true;
                throw new InvalidOperationException("Value does not contain Boolean data.");
                }
            if (typeof(T).IsAssignableFrom(typeof(int)))
                {
                if (value == "")
                    {
                    if (typeof(T) == typeof(int?))
                        {
                        return (T) (object) new int?()!;
                        }
                    throw new InvalidOperationException("Null value returned.");
                    }
                return (T)(object)int.Parse(value, cultureInfo);
                }
            if (typeof(T).IsAssignableFrom(typeof(Guid)))
                {
                if (value == "")
                    {
                    if (typeof(T) == typeof(Guid?))
                        {
                        return (T) (object) new Guid?()!;
                        }
                    throw new InvalidOperationException("Null value returned.");
                    }
                return (T)(object)Guid.Parse(value);
                }
            if (typeof(T) == typeof(string))
                {
                if (value == "")
                    {
                    return (T) (object) default(string)!;
                    }
                return (T)(object) value;
                }
            if (typeof(T).IsAssignableFrom(typeof(DateTime)))
                {
                if (value == "")
                    {
                    if (typeof(T) == typeof(DateTime?))
                        {
                        return (T) (object) new DateTime?()!;
                        }
                    throw new InvalidOperationException("Null value returned.");
                    }
                return (T)(object)DateTime.Parse(value, cultureInfo);
                }
#if NET6_0_OR_GREATER
            if (typeof(T).IsAssignableFrom(typeof(DateOnly)))
                {
                if (value == "")
                    {
                    if (typeof(T) == typeof(DateOnly?))
                        {
                        return (T) (object) new DateOnly?()!;
                        }
                    throw new InvalidOperationException("Null value returned.");
                    }
                return (T)(object)DateOnly.Parse(value, cultureInfo);
                }
#endif
            if (typeof(T).IsAssignableFrom(typeof(decimal)))
                {
                if (value == "")
                    {
                    if (typeof(T) == typeof(decimal?))
                        {
                        return (T) (object) new decimal?()!;
                        }
                    throw new InvalidOperationException("Null value returned.");
                    }
                return (T)(object)decimal.Parse(value, cultureInfo);
                }

            throw new InvalidOperationException("Type for return value is not supported.");
            }

        internal interface ICompoundMemberAccess
            {
            public void CopyValue(string text, CultureInfo cultureInfo, object compoundClass);
            }

        internal abstract class CompoundMemberAccess : ICompoundMemberAccess
            {
            protected static readonly MethodInfo TranslateValue = typeof(GetArchetypeData).GetMethod("TranslateValue", BindingFlags.NonPublic | BindingFlags.Static)!;

            public abstract void CopyValue(string text, CultureInfo cultureInfo, object compoundClass);

            public static ICompoundMemberAccess GetMemberAccess(Type t, string name)
                {
                FieldInfo? field = t.GetField(name);
                if (field != null)
                    return new FieldAccess(field);
                PropertyInfo? property = t.GetProperty(name);
                if (property != null)
                    return new PropertyAccess(property);
                throw new InvalidOperationException($"Could not find a field or property for the column {name}");
                }
            }

        internal class PropertyAccess : CompoundMemberAccess
            {
            private readonly PropertyInfo _propertyInfo;
            private readonly MethodInfo _translateValue;

            public PropertyAccess(PropertyInfo propertyInfo)
                {
                this._propertyInfo = propertyInfo;
                this._translateValue = TranslateValue.MakeGenericMethod(this._propertyInfo.PropertyType);
                }

            public override void CopyValue(string text, CultureInfo cultureInfo, object compoundClass)
                {
                var parameters = new object[] { text, cultureInfo };
                try
                    {
                    object? value = this._translateValue.Invoke(null, parameters);
                    this._propertyInfo.SetValue(compoundClass, value);
                    }
                catch (TargetInvocationException ex)
                    {
                    if (ex.InnerException != null)
                        throw ex.InnerException;
                    throw;
                    }
                }
            }

        internal class FieldAccess : CompoundMemberAccess
            {
            private readonly FieldInfo _fieldInfo;
            private readonly MethodInfo _translateValue;

            public FieldAccess(FieldInfo fieldInfo)
                {
                this._fieldInfo = fieldInfo;
                this._translateValue = TranslateValue.MakeGenericMethod(this._fieldInfo.FieldType);
                }

            public override void CopyValue(string text, CultureInfo cultureInfo, object compoundClass)
                {
                var parameters = new object[] { text, cultureInfo };
                try
                    {
                    object? value = this._translateValue.Invoke(null, parameters);
                    this._fieldInfo.SetValue(compoundClass, value);
                    }
                catch (TargetInvocationException ex)
                    {
                    if (ex.InnerException != null)
                        throw ex.InnerException;
                    throw;
                    }
                }
            }
        }
    }
