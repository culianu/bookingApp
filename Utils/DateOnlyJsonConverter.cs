using System;
using System.Text.Json;
using System.Text.Json.Serialization;

public class DateOnlyJsonConverter : JsonConverter<DateOnly>
{
	private const string DateFormat = "yyyyMMdd";

	public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		var dateString = reader.GetString();
		return string.IsNullOrEmpty(dateString) ? DateOnly.MinValue : DateOnly.ParseExact(dateString, DateFormat);
	}

	public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
	{
		writer.WriteStringValue(value.ToString(DateFormat));
	}
}