namespace ClientLibraries.Converters;

public static class Fixers
{
    public static string ToFixedText(this string text) => text.ToUpper().Trim();
}