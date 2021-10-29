namespace ClientLibraries.Enums;

public static class ErrorEnumsConvertor
{
    public static string ToErrorText(this RegisterError error) => error switch
    {
        RegisterError.EmailExists => "این ایمیل از قبل موجود است",
        RegisterError.UserNameExists => "این نام کاربری از قبل موجود است",
        RegisterError.DtoValidationField => "اطلاعات وارد شده نادرست است",
        _ => throw new ArgumentOutOfRangeException(nameof(error), error, null)
    };

    public static string ToErrorText(this LoginError error) => error switch
    {
        LoginError.UserPasswordWrong => "نام کاربری یا رمز عبور غلط است",
        LoginError.EmailActivationRequired => "تایید ایمیل اجباری است",
        LoginError.PhoneActivationRequired => "تایید شماره تلفن اجباری است",
        _ => throw new ArgumentOutOfRangeException(nameof(error), error, null)
    };
}

public enum RegisterError
{
    UserNameExists,
    EmailExists,
    DtoValidationField,
}

public enum LoginError
{
    UserPasswordWrong,
    EmailActivationRequired,
    PhoneActivationRequired,
}