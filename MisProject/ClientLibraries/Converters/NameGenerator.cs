namespace ClientLibraries.Converters;

public static class NameGenerator
{
    public static string GenerateUniqueCode() => Guid.NewGuid().ToString().Replace("-", "");
}