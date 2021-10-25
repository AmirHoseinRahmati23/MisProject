using System.ComponentModel.DataAnnotations;

namespace DbLayer.Entities.Users;

public partial class User
{
    public User()
    {
        RegisterTime = DateTime.Now;
    }

    public User(string userName) : this()
    {
        UserName = userName;
    }

    [Key] public int UserId { get; set; }

    #region Statics

    #region Username

    private string _userName;

    [Display(Name = "نام کاربری", Prompt = "نام کاربری")]
    [Required(ErrorMessage = "{0} نمیتواند خالی باشد")]
    [MinLength(5, ErrorMessage = "فیلد {0} باید حداقل {1} کاراکتر باشد.")]
    [MaxLength(20, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر باشد")]
    [RegularExpression("^[a-zA-Z][a-zA-Z0-9_]{4,19}$", ErrorMessage = "لطفا فقط از حروف انگلیسی و اعداد و اندرلاین استفاده کنید.")]
    public string UserName
    {
        get => _userName;
        set => _userName = FixedUserName = value;
    }

    //----------------------------------------------------------------------------------------------

    private string _fixedUserName;

    [Required, MaxLength(20)]
    public string FixedUserName
    {
        get => _fixedUserName;
        private set => _fixedUserName = value.ToUpper().Trim();
    }

    #endregion

    #region Email

    private string _email;

    [EmailAddress(ErrorMessage = "ایمیل شما معتبر نمیباشد")]
    [Display(Name = "ایمیل", Prompt = "Name@Example.com")]
    [Required(ErrorMessage = "{0} نمیتواند خالی باشد")]
    [MinLength(3, ErrorMessage = "فیلد {0} باید حداقل {1} کاراکتر باشد.")]
    [MaxLength(100, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر باشد")]
    [DataType(DataType.EmailAddress)]
    public string Email
    {
        get => _email;
        set => _email = FixedEmail = value;
    }

    //-----------------------------------------------------------------------------------------------------

    private string _fixedEmail;

    [Required, MaxLength(100), DataType(DataType.EmailAddress)]
    public string FixedEmail
    {
        get => _fixedEmail;
        private set => _fixedEmail = value.ToUpper().Trim();
    }

    #endregion

    public bool IsDeleted { get; set; }

    [Display(Name = "تایید ایمیل")]
    public bool IsEmailConfirmed { get; set; }

    [Display(Name = "تایید شماره تلفن")]
    public bool IsPhoneNumberConfirmed { get; set; }

    [Display(Name = "تاریخ ثبت نام")]
    public DateTime RegisterTime { get; set; }

    #endregion

    #region Security

    [Display(Name = "رمز عبور", Prompt = "رمز عبور")]
    [Required(ErrorMessage = "{0} نمیتواند خالی باشد")]
    [MinLength(8, ErrorMessage = "فیلد {0} باید حداقل {1} کاراکتر باشد.")]
    [MaxLength(200, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر باشد")]
    public string Password { get; set; }

    /// <summary>
    /// Change in every update
    /// </summary>
    [Required, MaxLength(50)]
    public string IdentityCode { get; set; }

    /// <summary>
    /// Email Activation code.
    /// Change when Security Items edited.
    /// </summary>
    [Required, MaxLength(50)]
    public string ActiveCode { get; set; }

    #endregion
}


public partial class User
{
    #region Relations

    public ICollection<UserRole> UserRoles { get; set; }

    #endregion

    #region Methods

    public override string ToString() => UserName;

    #endregion
}