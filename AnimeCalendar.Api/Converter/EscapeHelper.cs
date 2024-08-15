using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace AnimeCalendar.Api.Converter;

internal static partial class EscapeHelper {
    public static string DecodeUnicode(this string htmlStr)
        => UnicodeEscape().Replace(htmlStr, match => {
            string code2Str = match.Groups["code2"].Value;
            byte code1 = byte.Parse(match.Groups["code1"].Value, NumberStyles.HexNumber);
            byte code2 = byte.Parse(string.IsNullOrEmpty(code2Str) ? "0" : code2Str, NumberStyles.HexNumber);
            return Encoding.Unicode.GetString([code1, code2]);
        });

    public static string DecodeHtml(this string htmlStr)
        => HtmlEscape().Replace(htmlStr, match => match.Groups["code"].Value switch {
            string value when value.EndsWith("sp") => " ",
            "lt"        => "<",
            "gt"        => ">",
            "amp"       => "&",
            "quot"      => "\"",
            "copy"      => "©",
            "reg"       => "®",
            "trade"     => "™",
            "times"     => "×",
            "divide"    => "÷",
            string v    => $"&{v};"
        });

    public static string Decode(this string htmlStr)
        => htmlStr.Trim().DecodeUnicode().DecodeHtml();

    [GeneratedRegex(@"&#x(?<code2>[A-F\d]{0,2})(?<code1>[A-F\d]{2});", RegexOptions.IgnoreCase)]
    private static partial Regex UnicodeEscape();

    [GeneratedRegex(@"&(?<code>[A-Z]+?);", RegexOptions.IgnoreCase)]
    private static partial Regex HtmlEscape();
}