using System.Text.RegularExpressions;

namespace Lar.TesteDotNet.Domain.Extensions;

public static class StringExtensions
{
    public static string OnlyDigits(this string str)
    {
        return Regex.Replace(str, @"[^\p{L}\p{N}]", "");
    }

    public static bool IsValidEmail(this string? str)
    {
        if (string.IsNullOrEmpty(str)) return false;
        Regex allowed = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);
        return allowed.IsMatch(str);
    }
}