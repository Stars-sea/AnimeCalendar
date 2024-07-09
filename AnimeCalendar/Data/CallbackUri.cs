using System;
using System.Text.RegularExpressions;

namespace AnimeCalendar.Data;

internal partial record CallbackUri(Uri Uri, string Code) {
    public static CallbackUri Create(Uri uri) {
        Match match = BangumiCallbackRegex().Match(uri.ToString());

        if (match.Success && match.Groups.TryGetValue("code", out Group codeGroup))
            return new CallbackUri(uri, codeGroup.Value);
        throw new ArgumentException($"Invalid callback uri. [{uri}]");
    }

    public static implicit operator CallbackUri(Uri uri) => Create(uri);

    [GeneratedRegex("^ac://callback/\\w+\\?code=(?<code>\\S+)$")]
    private static partial Regex BangumiCallbackRegex();
}
