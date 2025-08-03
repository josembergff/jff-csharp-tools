
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace JffCsharpTools6.Apresentation.Configs
{
    /// <summary>
    /// Generic JSON converter for enums that handles string-to-enum and enum-to-string conversion
    /// during JSON serialization and deserialization.
    /// </summary>
    /// <typeparam name="T">The enum type to convert. Must be a struct and implement Enum interface.</typeparam>
    public class EnumConverter<T> : JsonConverter<T> where T : struct, Enum
    {
        /// <summary>
        /// Reads and converts JSON string values to enum values during deserialization.
        /// </summary>
        /// <param name="reader">The JSON reader that contains the value to convert</param>
        /// <param name="typeToConvert">The target enum type to convert to</param>
        /// <param name="options">JSON serializer options (not used in this implementation)</param>
        /// <returns>The parsed enum value</returns>
        /// <exception cref="JsonException">Thrown when the JSON value cannot be converted to the target enum type</exception>
        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Check if the JSON token is a string value
            if (reader.TokenType == JsonTokenType.String)
            {
                // Get the string value from the JSON reader
                var enumString = reader.GetString();

                // Try to parse the string as an enum value (case-sensitive by default)
                if (Enum.TryParse<T>(enumString, out var enumValue))
                {
                    return enumValue;
                }
            }

            // Throw an exception if conversion fails
            throw new JsonException($"Unable to convert \"{reader.GetString()}\" to {nameof(T)}.");
        }

        /// <summary>
        /// Writes enum values as JSON string values during serialization.
        /// </summary>
        /// <param name="writer">The JSON writer to write the enum value to</param>
        /// <param name="value">The enum value to serialize</param>
        /// <param name="options">JSON serializer options (not used in this implementation)</param>
        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            // Convert the enum value to its string representation and write it as a JSON string
            writer.WriteStringValue(value.ToString());
        }
    }
}