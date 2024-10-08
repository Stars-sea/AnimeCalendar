﻿using Newtonsoft.Json;

using System.Text.RegularExpressions;

namespace AnimeCalendar.Api.Converter;

internal partial class DateOnlyConverter : JsonConverter<DateOnly?> {
    public override DateOnly? ReadJson(JsonReader reader, Type objectType, DateOnly? existingValue, bool hasExistingValue, JsonSerializer serializer) {
        string? value = (string?)reader.Value;
        if (string.IsNullOrWhiteSpace(value)) return null;

        Match match = DateRegex().Match(value);
        if (!match.Success) return null;

        GroupCollection groups = match.Groups;
        string year  = groups["year"].Value;
        string month = groups.GetValueOrDefault("month", "1");
        string day   = groups.GetValueOrDefault("day", "1");

        return new DateOnly(int.Parse(year), int.Parse(month), int.Parse(day));
    }

    public override void WriteJson(JsonWriter writer, DateOnly? value, JsonSerializer serializer) {
        if (value == null) {
            if (serializer.NullValueHandling == NullValueHandling.Include)
                writer.WriteNull();
        }
        else writer.WriteValue(value.Value.ToString("yyyy-MM-dd"));
    }

    [GeneratedRegex(@"^(?<year>\d{4})-?(?<month>\d{1,2})?-?(?<day>\d{1,2})?$")]
    private static partial Regex DateRegex();
}
