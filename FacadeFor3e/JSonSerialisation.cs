using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using JetBrains.Annotations;

namespace FacadeFor3e
    {
    /// <summary>
    /// Support for serialising data between 3E and JSon
    /// </summary>
    [PublicAPI]
    public static class JSonSerialisation
        {
        /// <summary>
        /// Returns the default <see cref="JsonSerializerOptions"/> object used to deserialise the JSON returned by 3E into an object
        /// </summary>
        public static readonly JsonSerializerOptions DefaultJSonSerializerOptions = BuildDefaultJSonSerializerOptions();

        /// <summary>
        /// Deserialises a JSON element to an object
        /// </summary>
        /// <typeparam name="T">The type of object to convert the json to</typeparam>
        /// <param name="element">Specifies the JSON to convert</param>
        /// <returns>An object of the specified type</returns>
        /// <exception cref="InvalidOperationException">If it was not possible to deserialise the JSON</exception>
        [Pure]
        public static T JsonDeserialise<T>(this JsonElement element)
            {
            var result = element.Deserialize<T>(DefaultJSonSerializerOptions);
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
        [Pure]
        public static List<T> JsonDeserialiseList<T>(this JsonElement arrayElement)
            {
            if (arrayElement.ValueKind != JsonValueKind.Array)
                throw new ArgumentOutOfRangeException(nameof(arrayElement), "Element is not an array");
            var result = arrayElement.EnumerateArray().Select(item => item.JsonDeserialise<T>()).ToList();
            return result;
            }

        private static JsonSerializerOptions BuildDefaultJSonSerializerOptions()
            {
            var options = new JsonSerializerOptions { IncludeFields = true, PropertyNameCaseInsensitive = true, };
#if NET6_0_OR_GREATER
            options.Converters.Add(new DateOnlyJsonConverter());
#endif
            options.Converters.Add(new DateTimeJsonConverter());
            options.MakeReadOnly(populateMissingResolver: true);
            return options;
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
            private const string Format = @"yyyy-MM-dd\THH:mm:ss\Z";

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
