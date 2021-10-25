using System.ComponentModel.DataAnnotations;

namespace DbLayer.Entities.Users;

public partial class User
{
    #region Personal Information

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

    [Display(Name = "نام پدر")]
    [MinLength(3, ErrorMessage = "فیلد {0} باید حداقل {1} کاراکتر باشد.")]
    [MaxLength(30, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر باشد")]
    //TODO: Persian Check RegEx here
    // [RegularExpression("MyAwesomeRegEx")]
    public string? FatherName { get; set; }

    [Display(Name = "کد ملی", Prompt = "1090121200")]
    [MinLength(3, ErrorMessage = "{0} نباید کمتر از {1} کاراکتر باشد")]
    [MaxLength(10, ErrorMessage = "{0} نباید بیشتر از {1} کاراکتر باشد")]
    [RegularExpression(@"[0-9]{10}", ErrorMessage = "{0} باید متشکل از اعداد باشد")]
    public string? NationalCode { get; set; }

    [Display(Name = "تاریخ تولد")]
    [DataType(DataType.Date)]
    public DateTime? BirthDay { get; set; }

    #endregion

    #region Contact info

    [Display(Name = "شماره منزل", Prompt = "021********")]
    [MinLength(11, ErrorMessage = "{0} نباید کمتر از {1} کاراکتر باشد")]
    [MaxLength(11, ErrorMessage = "{0} نباید بیشتر از {1} کاراکتر باشد")]
    [RegularExpression(@"[0-9]{10}", ErrorMessage = "{0} باید متشکل از اعداد باشد")]
    public string? HomeNumber { get; set; }

    [Display(Name = "شماره همراه", Prompt = "0913*******")]
    [MinLength(11, ErrorMessage = "{0} نباید کمتر از {1} کاراکتر باشد")]
    [MaxLength(11, ErrorMessage = "{0} نباید بیشتر از {1} کاراکتر باشد")]
    [RegularExpression(@"[0-9]{10}", ErrorMessage = "{0} باید متشکل از اعداد باشد")]
    public string? PhoneNumber { get; set; }

    [Display(Name = "ایدی تلگرام", Prompt = "@Your_Id")]
    [MaxLength(30, ErrorMessage = "{0} نباید بیشتر از {1} کاراکتر باشد")]
    public string? TelegramId { get; set; }


    [Display(Name = "ادرس خانه")]
    [MaxLength(500)]
    public string? Address { get; set; }

    #endregion

    #region Interests

    [Display(Name = "علاقه مندی ها")]
    [MaxLength(2000)]
    public string? Interests { get; set; }

    #endregion
}