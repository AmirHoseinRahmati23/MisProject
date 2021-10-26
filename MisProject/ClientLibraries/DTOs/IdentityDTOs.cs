using FluentValidation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ClientLibraries.DTOs;

public class RegisterDTO
{
    [Display(Name = "نام")]
    [Required(ErrorMessage = "{0} نمیتواند خالی باشد")]
    [MinLength(3, ErrorMessage = "فیلد {0} باید حداقل {1} کاراکتر باشد.")]
    [MaxLength(30, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر باشد")]
    //TODO: Persian Check RegEx here
    // [RegularExpression("MyAwesomeRegEx")]
    public string FirstName { get; set; }

    [Display(Name = "نام خانوادگی")]
    [Required(ErrorMessage = "{0} نمیتواند خالی باشد")]
    [MinLength(3, ErrorMessage = "فیلد {0} باید حداقل {1} کاراکتر باشد.")]
    [MaxLength(30, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر باشد")]
    //TODO: Persian Check RegEx here
    // [RegularExpression("MyAwesomeRegEx")]
    public string LastName { get; set; }

    [Display(Name = "نام کاربری", Prompt = "نام کاربری")]
    [Required(ErrorMessage = "{0} نمیتواند خالی باشد")]
    [MinLength(5, ErrorMessage = "فیلد {0} باید حداقل {1} کاراکتر باشد.")]
    [MaxLength(20, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر باشد")]
    [RegularExpression("^[a-zA-Z][a-zA-Z0-9_]{4,19}$", ErrorMessage = "لطفا فقط از حروف انگلیسی و اعداد و اندرلاین استفاده کنید.")]
    public string UserName { get; set; }

    [EmailAddress(ErrorMessage = "ایمیل شما معتبر نمیباشد")]
    [Display(Name = "ایمیل", Prompt = "Name@Example.com")]
    [Required(ErrorMessage = "{0} نمیتواند خالی باشد")]
    [MinLength(3, ErrorMessage = "فیلد {0} باید حداقل {1} کاراکتر باشد.")]
    [MaxLength(100, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر باشد")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }

    [Display(Name = "رمز عبور", Prompt = "رمز عبور")]
    [Required(ErrorMessage = "{0} نمیتواند خالی باشد")]
    [MinLength(8, ErrorMessage = "فیلد {0} باید حداقل {1} کاراکتر باشد.")]
    [MaxLength(200, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر باشد")]
    [PasswordPropertyText]
    public string Password { get; set; }

    [Display(Name = "تکرار کلمه عبور")]
    [Required(ErrorMessage = "{0} نمیتواند خالی باشد")]
    [PasswordPropertyText]
    public string RepeatPassword { get; set; }
}

public class RegisterDTOFluentValidator : AbstractValidator<RegisterDTO>
{
    public RegisterDTOFluentValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("نام نمیتواند خالی باشد")
            .MinimumLength(3).WithMessage("فیلد نام باید حداقل 3 کاراکتر باشد.")
            .MaximumLength(30).WithMessage("فیلد نام نمیتواند بیش از 30 کاراکتر باشد.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("نام خانوادگی نمیتواند خالی باشد")
            .MinimumLength(3).WithMessage("فیلد نام خانوادگی باید حداقل 3 کاراکتر باشد.")
            .MaximumLength(30).WithMessage("فیلد نام خانوادگی نمیتواند بیش از 30 کاراکتر باشد.");

        RuleFor(p => p.UserName)
            .NotEmpty().WithMessage("نام کاربری نمیتواند خالی باشد")
            .MinimumLength(5).WithMessage("نام کاربری باید حداقل 5 کاراکتر باشد")
            .MaximumLength(20).WithMessage("نام کاربری باید حداکثر 20 کاراکتر باشد")
            .Matches("^[a-zA-Z][a-zA-Z0-9_]{4,19}$").WithMessage("لطفا فقط از حروف انگلیسی و اعداد و اندرلاین استفاده کنید.");

        RuleFor(p => p.Email)
            .NotEmpty().WithMessage("ایمیل نمیتواند خالی باشد")
            .MinimumLength(3).WithMessage("ایمیل باید حداقل 3 کاراکتر باشد")
            .MaximumLength(100).WithMessage("ایمیل باید حداکثر 100 کاراکتر باشد")
            .EmailAddress().WithMessage("ایمیل شما معتبر نمیباشد")
            .NotEqual(p => p.UserName).WithMessage("ایمیل و نام کاربری نمیتواند یکسان باشد");

        RuleFor(p => p.Password)
            .NotEmpty().WithMessage("رمز عبور نمیتواند خالی باشد")
            .MinimumLength(8).WithMessage("رمز عبور باید حداقل 8 کاراکتر باشد")
            .MaximumLength(200).WithMessage("رمز عبور باید حداکثر 200 کاراکتر باشد");

        RuleFor(p => p.RepeatPassword)
            .NotEmpty().WithMessage("تکرار رمز عبور نمیتواند خالی باشد")
            .Equal(p => p.Password).WithMessage("رمز عبور و تکرار آن باید یکسان باشد");
    }

    public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
    {
        var result = await ValidateAsync(ValidationContext<RegisterDTO>.CreateWithOptions((RegisterDTO)model, x => x.IncludeProperties(propertyName)));
        return result.IsValid
            ? Array.Empty<string>()
            : result.Errors.Select(e => e.ErrorMessage);
    };
}

public class LoginDTO
{
    [Display(Name = "نام کاربری", Prompt = "نام کاربری")]
    [Required(ErrorMessage = "{0} نمیتواند خالی باشد")]
    [MinLength(5, ErrorMessage = "فیلد {0} باید حداقل {1} کاراکتر باشد.")]
    [MaxLength(20, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر باشد")]
    [RegularExpression("^[a-zA-Z][a-zA-Z0-9_]{4,19}$", ErrorMessage = "لطفا فقط از حروف انگلیسی و اعداد و اندرلاین استفاده کنید.")]
    public string UserName { get; set; }


    [Display(Name = "رمز عبور", Prompt = "رمز عبور")]
    [Required(ErrorMessage = "{0} نمیتواند خالی باشد")]
    [MinLength(8, ErrorMessage = "فیلد {0} باید حداقل {1} کاراکتر باشد.")]
    [MaxLength(200, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر باشد")]
    [PasswordPropertyText]
    public string Password { get; set; }
}