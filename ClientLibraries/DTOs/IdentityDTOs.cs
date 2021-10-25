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
    [Compare(nameof(Password), ErrorMessage = "رمز عبور مطابقت ندارد")]
    [PasswordPropertyText]
    public string RepeatPassword { get; set; }
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