using System.Text.RegularExpressions;

namespace AnimeCalendar.Api.Converter;

internal static class RegexHelper {
    internal static string GetValueOrDefault(this GroupCollection groups, string key, string @default)
        => groups.TryGetValue(key, out var group) && !string.IsNullOrWhiteSpace(group.Value)
            ? group.Value
            : @default;
}
