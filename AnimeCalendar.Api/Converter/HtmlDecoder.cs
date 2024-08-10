using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace AnimeCalendar.Api.Converter;

internal static partial class HtmlDecoder {
    public static string HtmlDecode(string htmlStr)
        => HtmlCode().Replace(htmlStr, match => {
            string codeStr = match.Groups["code"].Value;
            byte code1 = byte.Parse(codeStr[2..], NumberStyles.HexNumber);
            byte code2 = byte.Parse(codeStr[..2], NumberStyles.HexNumber);
            return Encoding.Unicode.GetString([code1, code2]);
        });

    [GeneratedRegex(@"&#x(?<code>[A-F\d]{4});", RegexOptions.IgnoreCase)]
    private static partial Regex HtmlCode();
}