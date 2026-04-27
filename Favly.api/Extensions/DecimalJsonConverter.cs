using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Favly.api.Extensions
{
    /// <summary>
    /// Aceita decimais em formato PT-BR (vírgula) ou padrão JSON (ponto).
    /// Ex: 1,70 → 1.70 | 1.70 → 1.70 | "1,70" → 1.70
    /// </summary>
    public class DecimalJsonConverter : JsonConverter<decimal>
    {
        public override decimal Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Number)
                return reader.GetDecimal();

            if (reader.TokenType == JsonTokenType.String)
            {
                var raw = reader.GetString()!.Replace(',', '.');
                if (decimal.TryParse(raw, NumberStyles.Any, CultureInfo.InvariantCulture, out var result))
                    return result;
            }

            throw new JsonException($"Não foi possível converter o valor para decimal.");
        }

        public override void Write(Utf8JsonWriter writer, decimal value, JsonSerializerOptions options)
            => writer.WriteNumberValue(value);
    }

    public class NullableDecimalJsonConverter : JsonConverter<decimal?>
    {
        public override decimal? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
                return null;

            if (reader.TokenType == JsonTokenType.Number)
                return reader.GetDecimal();

            if (reader.TokenType == JsonTokenType.String)
            {
                var raw = reader.GetString();
                if (string.IsNullOrWhiteSpace(raw)) return null;

                var normalized = raw.Replace(',', '.');
                if (decimal.TryParse(normalized, NumberStyles.Any, CultureInfo.InvariantCulture, out var result))
                    return result;
            }

            throw new JsonException($"Não foi possível converter o valor para decimal.");
        }

        public override void Write(Utf8JsonWriter writer, decimal? value, JsonSerializerOptions options)
        {
            if (value is null) writer.WriteNullValue();
            else writer.WriteNumberValue(value.Value);
        }
    }
}
