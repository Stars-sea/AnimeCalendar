using Newtonsoft.Json;

using System.Diagnostics;
using System.Text.RegularExpressions;

namespace AnimeCalendar.Api.Converter;

internal partial class TimeSpanConverter : JsonConverter<TimeSpan?> {
    public override TimeSpan? ReadJson(JsonReader reader, Type objectType, TimeSpan? existingValue, bool hasExistingValue, JsonSerializer serializer) {
        string? value = (string?)reader.Value;
        if (string.IsNullOrWhiteSpace(value)) return null;

        int hours = 0, minutes = 0, seconds = 0;
        Match match;

        if ((match = NormalRegex().Match(value)).Success) {
            GroupCollection groups = match.Groups;
            hours   = int.Parse(groups.Values.FirstOrDefault(g => g.Name == "h")?.Value ?? "0");
            minutes = int.Parse(groups["m"].Value);
            seconds = int.Parse(groups["s"].Value);
        }
        else if ((match = ShortRegex().Match(value)).Success) {
            int    num  = int.Parse(match.Groups["num"].Value);
            string unit = match.Groups["unit"].Value;
            if (unit.StartsWith('h'))
                hours = num;
            else if (unit.StartsWith('m'))
                minutes = num;
            else if (unit.StartsWith('s'))
                seconds = num;
        }
        else return null;

        return new TimeSpan(hours, minutes, seconds);
    }

    public override void WriteJson(JsonWriter writer, TimeSpan? value, JsonSerializer serializer)
        => serializer.Serialize(writer, value);

    [GeneratedRegex(@"^((?<h>\d{1,2}):)?(?<m>\d{1,2}):(?<s>\d{1,2})$")]
    private static partial Regex NormalRegex();

    [GeneratedRegex(@"^(?<num>\d+)(?<unit>[A-Z]+)$")]
    private static partial Regex ShortRegex();
}
