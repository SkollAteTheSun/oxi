using System.Text;
using System.Text.RegularExpressions;

namespace Kp.Ms.Oxi.Common;

public static partial class CyrillicToLatin
{
    private static readonly Dictionary<string, string> ExclusionList = new()
    {
        { "Kp Reservation", "Kp Reservation" }
    };

    private static readonly Dictionary<char, string> CyrillicType = new()
    {
        { 'А', "A" },
        { 'Б', "B" },
        { 'В', "V" },
        { 'Г', "G" },
        { 'Д', "D" },
        { 'Е', "E" },
        { 'Ё', "E" },
        { 'Ж', "Zh" },
        { 'З', "Z" },
        { 'И', "I" },
        { 'Й', "I" },
        { 'К', "K" },
        { 'Л', "L" },
        { 'М', "M" },
        { 'Н', "N" },
        { 'О', "O" },
        { 'П', "P" },
        { 'Р', "R" },
        { 'С', "S" },
        { 'Т', "T" },
        { 'У', "U" },
        { 'Ф', "F" },
        { 'Х', "Kh" },
        { 'Ц', "Ts" },
        { 'Ч', "Ch" },
        { 'Ш', "Sh" },
        { 'Щ', "Shch" },
        { 'Ы', "Y" },
        { 'Ь', "" },
        { 'Ъ', "Ie" },
        { 'Э', "E" },
        { 'Ю', "Iu" },
        { 'Я', "Ia" },
        { 'а', "a" },
        { 'б', "b" },
        { 'в', "v" },
        { 'г', "g" },
        { 'д', "d" },
        { 'е', "e" },
        { 'ё', "e" },
        { 'ж', "zh" },
        { 'з', "z" },
        { 'и', "i" },
        { 'й', "i" },
        { 'к', "k" },
        { 'л', "l" },
        { 'м', "m" },
        { 'н', "n" },
        { 'о', "o" },
        { 'п', "p" },
        { 'р', "r" },
        { 'с', "s" },
        { 'т', "t" },
        { 'у', "u" },
        { 'ф', "f" },
        { 'х', "kh" },
        { 'ц', "ts" },
        { 'ч', "ch" },
        { 'ш', "sh" },
        { 'щ', "shch" },
        { 'ы', "y" },
        { 'ь', "" },
        { 'ъ', "ie" },
        { 'э', "e" },
        { 'ю', "iu" },
        { 'я', "ia" },
    };

    [GeneratedRegex(@"[^,;()\s]+")]
    private static partial Regex Pattern();

    public static string Convert(string text)
    {
        foreach (KeyValuePair<string, string> pair in ExclusionList)
        {
            text = text.Replace(pair.Key, pair.Value);
        }

        var sb = new StringBuilder();
        var index = 0;

        foreach (Match match in Pattern().Matches(text))
        {
            var word = ConvertWord(match.Value);
            if (index < match.Index)
            {
                sb.Append(text[index..match.Index]);
            }

            sb.Append(word);
            index = match.Index + match.Length;
        }

        return sb.ToString();
    }

    private static string ConvertWord(string word)
    {
        var result = new StringBuilder();
        foreach (char c in word)
        {
            string s = c.ToString();
            if (CyrillicType.TryGetValue(c, out string? val))
            {
                s = val;
            }

            result.Append(s);
        }

        return result.ToString();
    }
}