using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Xml;
using JetBrains.Annotations;

namespace FacadeFor3e
    {
    /// <summary>
    /// Utility methods
    /// </summary>
    [PublicAPI]
    public static class CommonLibrary
        {
        /// <summary>
        /// Outputs an XML document as a formatted string for easy reading
        /// </summary>
        /// <param name="xmlDoc">Specifies the XML document to process</param>
        /// <returns>A string containing the text in the XML document specified with appropriate indentation</returns>
        public static string PrettyPrintXml(this XmlDocument xmlDoc)
            {
            if (xmlDoc == null) throw new ArgumentNullException(nameof(xmlDoc));
            using var stringWriter = new StringWriter(CultureInfo.InvariantCulture);
            var xmlNodeReader = new XmlNodeReader(xmlDoc);
            var xmlTextWriter = new XmlTextWriter(stringWriter)
                {   //set formatting options
                Formatting = Formatting.Indented,
                Indentation = 1,
                IndentChar = '\t'
                };

            // write the document formatted
            xmlTextWriter.WriteNode(xmlNodeReader, true);
            var result = stringWriter.ToString();
            return result;
            }

        /// <summary>
        /// Determines whether a specified string is a valid ID in 3E
        /// </summary>
        /// <param name="id">The string to check</param>
        /// <returns>True if the string can be used as a valid ID in 3E, false otherwise</returns>
        public static bool IsValidId(string? id)
            {
            if (id == null)
                return false;
            const string pattern = "^[A-Za-z_]([A-Za-z0-9_])*$";
            var regex = new Regex(pattern);
            var result = regex.IsMatch(id);
            return result;
            }

        internal static void EnsureValid(string? id)
            {
            if (!IsValidId(id))
                throw new ArgumentOutOfRangeException(nameof(id), "Invalid identifier.");
            }

        /// <summary>
        /// Deserialises a JSON element to an object
        /// </summary>
        /// <typeparam name="T">The type of object to convert the json to</typeparam>
        /// <param name="element">Specifies the JSON to convert</param>
        /// <returns>An object of the specified type</returns>
        /// <exception cref="InvalidOperationException">If it was not possible to deserialise the JSON</exception>
        public static T JsonDeserialise<T>(this JsonElement element)
            {
            var options = new JsonSerializerOptions { IncludeFields = true };
#if NET6_0_OR_GREATER
            options.Converters.Add(new DateOnlyJsonConverter());
#endif
            options.Converters.Add(new DateTimeJsonConverter());
            var result = element.Deserialize<T>(options);
            if (result == null)
                throw new InvalidOperationException("Failed to deserialise json.");
            return result;
            }

        /// <summary>
        /// Deserialises a JSON array to a list of objects
        /// </summary>
        /// <typeparam name="T">The type of object to convert the json to</typeparam>
        /// <param name="arrayElement">Specifies the JSON to convert</param>
        /// <returns>A list of objects of the specified type</returns>
        /// <exception cref="InvalidOperationException">If it was not possible to deserialise the JSON</exception>
        public static List<T> JsonDeserialiseList<T>(this JsonElement arrayElement)
            {
            if (arrayElement.ValueKind != JsonValueKind.Array)
                throw new ArgumentOutOfRangeException(nameof(arrayElement), "Element is not an array");
            var result = arrayElement.EnumerateArray().Select(item => item.JsonDeserialise<T>()).ToList();
            return result;
            }

#if NET6_0_OR_GREATER
        /// <summary>
        /// Converter for the 3E representation of dates
        /// </summary>
        public class DateOnlyJsonConverter : JsonConverter<DateOnly>
            {
            private const string Format = @"yyyy-MM-dd\T\0\0\:\0\0\:\0\0\Z";

            /// <inheritdoc />
            public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
                {
                return DateOnly.ParseExact(reader.GetString() ?? string.Empty, Format, CultureInfo.InvariantCulture);
                }

            /// <inheritdoc />
            public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
                {
                writer.WriteStringValue(value.ToString(Format, CultureInfo.InvariantCulture));
                }
            }
#endif

        /// <summary>
        /// Converter for the 3E representation of dates
        /// </summary>
        public class DateTimeJsonConverter : JsonConverter<DateTime>
            {
            private const string Format = @"yyyy-MM-dd\Thh:mm:ss\Z";

            /// <inheritdoc />
            public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
                {
                return DateTime.ParseExact(reader.GetString() ?? string.Empty, Format, CultureInfo.InvariantCulture);
                }

            /// <inheritdoc />
            public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
                {
                writer.WriteStringValue(value.ToString(Format, CultureInfo.InvariantCulture));
                }
            }
        }
    }
