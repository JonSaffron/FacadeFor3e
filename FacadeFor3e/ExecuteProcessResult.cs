﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using JetBrains.Annotations;

namespace FacadeFor3e
    {
    /// <summary>
    /// An object that provides information on the request and response received from the Transaction Service
    /// </summary>
    [PublicAPI]
    public class ExecuteProcessResult
        {
        private const string ErrorMessagePrefix = "Error in the application.-";

        /// <summary>
        /// Constructs a new <see cref="ExecuteProcessResult"/> object with the specified request and response
        /// </summary>
        /// <param name="request">Specifies the request sent to the Transaction Service</param>
        /// <param name="response">Specifies the response received from the Transaction Service</param>
        /// <exception cref="ArgumentNullException">If the value for the request or response parameters is null</exception>
        /// <exception cref="ArgumentException">If the request or response parameters appear to contain invalid values</exception>
        public ExecuteProcessResult(XmlDocument request, XmlDocument response)
            {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (response == null) throw new ArgumentNullException(nameof(response));
            if (request.DocumentElement == null)
                throw new ArgumentException("Invalid request.", nameof(request));
            if (response.DocumentElement == null || response.DocumentElement.LocalName != "ProcessExecutionResults")
                throw new ArgumentException("Invalid response.", nameof(response));

            this.Request = request;
            this.Response = response;
            }

        /// <summary>
        /// Gets the request sent to 3E
        /// </summary>
        public XmlDocument Request { get; }

        /// <summary>
        /// Gets the response from 3E
        /// </summary>
        public XmlDocument Response { get; }

        /// <summary>
        /// Gets the ID of the process that was executed
        /// </summary>
        public string ProcessCode
            {
            get
                {
                var result = this.Response.DocumentElement!.GetAttribute("Process");
                return result;
                }
            }

        /// <summary>
        /// Gets the name of the process that was executed
        /// </summary>
        /// <remarks>The process name can be changed programatically during execution</remarks>
        public string ProcessName
            {
            get
                {
                var result = this.Response.DocumentElement!.GetAttribute("Process");
                return result;
                }
            }

        /// <summary>
        /// Gets the process ID that was used to run the request
        /// </summary>
        /// <remarks>This corresponds to a row in the NxFwkProcessItem table identified by the ProcItemID column</remarks>
        public Guid ProcessId
            {
            get
                {
                var result = new Guid(this.Response.DocumentElement!.GetAttribute("ProcessItemId"));
                return result;
                }
            }

        /// <summary>
        /// Returns the final state of the process 
        /// </summary>
        /// <remarks>Can be one of
        ///     <c>Success</c> (the process completed),
        ///     <c>Failure</c> (an error occurred), or
        ///     <c>Interface</c> (the process has not finished running and is stopped at a UI step)</remarks>
        public string ExecutionResult
            {
            get
                {
                // ReSharper disable once PossibleNullReferenceException
                var result = this.Response.DocumentElement!.GetAttribute("Result");
                return result;
                }
            }

        /// <summary>
        /// Gets the final output ID the process used before it completed. Only available when the process completed successfully.
        /// </summary>
        /// <remarks>Sometimes this can be useful to determine what path the process took, and even whether it actually failed when it appeared to succeed.</remarks>
        public string OutputId
            {
            get
                {
                var result = this.Response.DocumentElement!.GetAttribute("OutputId");
                return result;
                }
            }

        /// <summary>
        /// Gets the step id the process is currently paused on. Only available when the process stopped on UI step.
        /// </summary>
        public string StepId
            {
            get
                {
                var result = this.Response.DocumentElement!.GetAttribute("StepId");
                return result;
                }
            }

        /// <summary>
        /// Gets the name of the user used to run the process
        /// </summary>
        /// <remarks>If the process completed, this identifies the final user context.
        /// If the process pauses on a UI step, this identifies the current owner of the process.</remarks>
        public string User
            {
            get
                {
                var result = this.Response.DocumentElement!.GetAttribute("User");
                return result;
                }
            }

        /// <summary>
        /// Gets whether the response contains any data error details
        /// </summary>
        public bool HasDataError
            {
            get
                {
                var result = this.Response.SelectSingleNode("ProcessExecutionResults/DATA_ERRORS") != null;
                return result;
                }
            }

        /// <summary>
        /// Returns any notification messages generated by the process
        /// </summary>
        public string? NextMessage
            {
            get
                {
                // ReSharper disable once PossibleNullReferenceException
                var nextMessage = this.Response.DocumentElement!.SelectSingleNode("MESSAGE");
                var result = nextMessage?.InnerText;
                return result;
                }
            }

        /// <summary>
        /// Returns the primary keys of records created by the process.
        /// </summary>
        /// <returns>An enumerable list of primary keys in string format</returns>
        public IEnumerable<string> GetKeys()
            {
            var keys = this.Response.SelectSingleNode("ProcessExecutionResults/Keys");
            if (keys == null)
                // The presence of the Keys node is dependent only upon whether ReturnInfoType.Keys was specified when calling ExecuteProcess 
                throw new InvalidOperationException("Key information was not requested when making the request to the Transaction Service.");

            foreach (XmlElement node in keys.ChildNodes)
                {
                // ReSharper disable once PossibleNullReferenceException
                string result = node.GetAttribute("KeyValue");
                yield return result;
                }
            }

        /// <summary>
        /// Returns a collection of data errors contained in the response from the Transaction Service
        /// </summary>
        public IEnumerable<DataError> DataErrors => this.Response.SelectNodes("ProcessExecutionResults/DATA_ERRORS/*/ROW")!.Cast<XmlElement>().Select(item => new DataError(item));

        /// <summary>
        /// An object representing one or more errors on the specified object or its children
        /// </summary>
        [PublicAPI]
        public class DataError
            {
            /// <summary>
            /// The id of the object
            /// </summary>
            public readonly string ObjectId;

            /// <summary>
            /// The primary key of the object (in string format)
            /// </summary>
            public readonly string PrimaryKey;

            /// <summary>
            /// The index of the row in the worklist
            /// </summary>
            public readonly int RowIndex;

            /// <summary>
            /// The exception on the object (null if not present)
            /// </summary>
            public readonly string? ObjectException;

            /// <summary>
            /// Returns errors on child objects
            /// </summary>
            public List<DataError> Children { get; private set; }

            /// <summary>
            /// Returns a collection of errors on the object's attributes
            /// </summary>
            public List<AttributeError> AttributeErrors { get; private set; }

            internal DataError(XmlElement rowElement)
                {
                Debug.Assert(rowElement.Name == "ROW");

                ObjectId = rowElement.ParentNode!.Name;
                PrimaryKey = rowElement.GetAttribute("ID");
                RowIndex = int.Parse(rowElement.GetAttribute("Idx"));

                var exceptionElement = (XmlElement?) rowElement.SelectSingleNode("E");
                if (exceptionElement != null)
                    {
                    string msg = exceptionElement.InnerText.Trim();
                    if (msg.StartsWith(ErrorMessagePrefix))
                        msg = msg.Substring(ErrorMessagePrefix.Length);
                    var r = new Regex("^(.+) record was modified by (.+) since it was opened.  Reopen the record to save changes.$");
                    var m = r.Match(msg);
                    if (m.Success && m.Groups.Count == 3)
                        {
                        string objectName2 = m.Groups[1].Value;
                        string lockingAccount = m.Groups[2].Value;
                        msg = string.Format("The {0} record that you are trying to update is currently being edited and is locked by {1}. Please try again later, or ask {1} to complete or cancel their activity.", objectName2, lockingAccount);
                        }

                    ObjectException = msg;
                    }

                Children = rowElement.SelectNodes("*/ROW")!.Cast<XmlElement>().Select(item => new DataError(item)).ToList();

                AttributeErrors = rowElement.SelectNodes("ATTS/ATT")!.Cast<XmlElement>().Select(item => new AttributeError(item)).ToList();
                }
            }

        /// <summary>
        /// An object representing an error encountered while setting an attribute's value
        /// </summary>
        public class AttributeError
            {
            /// <summary>
            /// The id of the attribute
            /// </summary>
            public readonly string AttributeId;

            /// <summary>
            /// The value that the attribute was given
            /// </summary>
            public readonly string Value;

            /// <summary>
            /// The error message
            /// </summary>
            public readonly string Error;

            internal AttributeError(XmlElement attributeElement)
                {
                if (attributeElement == null)
                    throw new ArgumentNullException(nameof(attributeElement));
                if (attributeElement.Name != "ATT")
                    throw new ArgumentOutOfRangeException(nameof(attributeElement), "Expecting ATT element.");

                // ReSharper disable PossibleNullReferenceException
                AttributeId = attributeElement.GetAttribute("ID");
                Value = attributeElement.GetAttribute("V");
                // ReSharper restore PossibleNullReferenceException

                string? error = attributeElement.SelectSingleNode("E")?.InnerText;
                if (error != null && error.StartsWith(ErrorMessagePrefix))
                    {
                    error = error.Substring(ErrorMessagePrefix.Length);
                    }

                // ReSharper disable once AssignNullToNotNullAttribute
                if (error == null || string.IsNullOrWhiteSpace(error))
                    {
                    error = "an unknown error occurred";
                    }

                Error = error;
                }
            }

        /// <summary>
        /// Generates a string description from a list of <see cref="DataErrors">DataError</see>
        /// </summary>
        /// <param name="dataErrors">A collection of data errors returned from running a process</param>
        /// <returns>A string containing a description of each of the data errors specified, or null if the data errors collection is empty</returns>
        public static string? RenderDataErrors(IEnumerable<DataError> dataErrors)
            {
            if (dataErrors == null)
                throw new ArgumentNullException(nameof(dataErrors));
            var sb = new StringBuilder();
            using (var enumerator = dataErrors.GetEnumerator())
                {
                if (!enumerator.MoveNext())
                    return null;
                // ReSharper disable once AssignNullToNotNullAttribute
                AppendDataErrorInfo(enumerator.Current, sb, 0);
                while (enumerator.MoveNext())
                    {
                    sb.AppendLine();
                    // ReSharper disable once AssignNullToNotNullAttribute
                    AppendDataErrorInfo(enumerator.Current, sb, 0);
                    }
                }
            return sb.ToString();
            }

        private static void AppendDataErrorInfo(DataError dataError, StringBuilder sb, int indentLevel)
            {
            if (dataError == null) throw new ArgumentNullException(nameof(dataError));
            if (sb == null) throw new ArgumentNullException(nameof(sb));

            string indent = new string(' ', indentLevel * 2);
            bool isNumber = dataError.PrimaryKey.All(Char.IsDigit);
            sb.AppendFormat("{0}{1} with {2} {3}:", indent, dataError.ObjectId, isNumber ? "number" : "id", dataError.PrimaryKey);
            sb.AppendLine();

            indent = new string(' ', (indentLevel + 1) * 2);
            if (dataError.ObjectException != null)
                {
                sb.AppendFormat("{0}- {1}", indent, dataError.ObjectException);
                sb.AppendLine();
                }

            foreach (var attributeError in dataError.AttributeErrors)
                {
                sb.AppendFormat("{0}- {1} (error caused when setting {2} to '{3}')", indent, attributeError.Error, attributeError.AttributeId, attributeError.Value);
                sb.AppendLine();
                }

            foreach (var childDataError in dataError.Children)
                {
                AppendDataErrorInfo(childDataError, sb, indentLevel + 1);
                }
            }
        }
    }
