namespace ClientLibraries.Enums;

public static class ErrorEnumsConvertor
{
    public static string ToErrorText(this RegisterError error) => error switch
    {
        RegisterError.EmailExists => "این ایمیل از قبل موجود است",
        RegisterError.UserNameExists => "این ایمیل از قبل موجود است",
        _ => throw new ArgumentOutOfRangeException(nameof(error), error, null)
    };
}

public enum RegisterError
{
    UserNameExists,
    EmailExists,
}