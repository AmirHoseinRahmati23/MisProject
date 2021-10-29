namespace ClientLibraries.Security;

/// <summary>
/// Ignore property while **returning** in Json form
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class SecurityIgnoredAttribute : Attribute
{
}