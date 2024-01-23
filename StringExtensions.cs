namespace MonkeyLanguage;

public static class StringExtensions
{
    public static string Simplify(this string s)
        => s.Replace(" ", "").ToLower();
}