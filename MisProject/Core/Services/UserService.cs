using ClientLibraries.Converters;
using Core.Security;
using Microsoft.Extensions.Configuration;

namespace Core.Services;

public class UserService : IUserService
{
    private readonly MisDbContext _db;
    private readonly IConfiguration _configuration;

    public UserService(MisDbContext db, IConfiguration configuration)
    {
        _db = db;
        _configuration = configuration;
    }

    public async Task<bool> IsEmailExists(string email)
        => await _db.Users.AnyAsync(x => x.FixedEmail == email.ToFixedText());

    public async Task<bool> IsUserNameExists(string userName)
        => await _db.Users.AnyAsync(x => x.FixedUserName == userName.ToFixedText());

    public async Task<bool> IsUserExists(int userId)
        => await _db.Users.AnyAsync(x => x.UserId == userId);

    public async Task<User?> GetUser(int userId)
        => await _db.Users.FirstOrDefaultAsync(x => x.UserId == userId);

    public async Task<User?> GetUser(string userName)
        => await _db.Users.FirstOrDefaultAsync(x => x.FixedUserName == userName.ToFixedText());

    /// <inheritdoc/>
    public async Task<DbResponse<User, RegisterError>> AddUser(User user)
    {
        var result = new DbResponse<User, RegisterError>();

        if (await IsUserNameExists(user.UserName))
            result.AddError(RegisterError.UserNameExists, RegisterError.UserNameExists.ToErrorText());
        if (await IsEmailExists(user.Email))
            result.AddError(RegisterError.EmailExists, RegisterError.EmailExists.ToErrorText());

        if (result.Success)
        {
            user.ActiveCode = NameGenerator.GenerateUniqueCode();
            user.IdentityCode = NameGenerator.GenerateUniqueCode();

            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();

            result.Value = user;
        }

        return result;
    }

    public async Task<DbResponse<RegisterDTO, RegisterError>> RegisterUser(RegisterDTO registerDto)
    {
        var user = new User
        {
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            UserName = registerDto.UserName,
            Email = registerDto.Email,
            Password = PasswordHelper.EncodePasswordMd5(registerDto.Password),
        };

        var result = await AddUser(user);

        var returnValue = new DbResponse<RegisterDTO, RegisterError>
        {
            Success = result.Success,
            Errors = result.Errors
        };

        if (result.Success && result.Value != null)
        {
            returnValue.Value = new()
            {
                UserName = result.Value.UserName,
                Email = result.Value.Email,
                FirstName = result.Value.FirstName,
                LastName = result.Value.LastName,
            };
        }

        return returnValue;
    }

    public async Task<DbResponse<ApplicationUser, LoginError>> LoginUser(LoginDTO loginDto)
    {
        var result = new DbResponse<ApplicationUser, LoginError>();

        var hashedPassword = PasswordHelper.EncodePasswordMd5(loginDto.Password);
        var user = await _db.Users.SingleOrDefaultAsync(p =>
            p.FixedUserName == loginDto.UserName.ToFixedText() && p.Password == hashedPassword);

        if (user == null)
        {
            result.AddError(LoginError.UserPasswordWrong, LoginError.UserPasswordWrong.ToErrorText());
            return result;
        }

        if (bool.Parse(_configuration["IdentitySettings:EmailActivationRequired"]))
            result.AddError(LoginError.EmailActivationRequired, LoginError.EmailActivationRequired.ToErrorText());
        if (bool.Parse(_configuration["IdentitySettings:PhoneActivationRequired"]))
            result.AddError(LoginError.PhoneActivationRequired, LoginError.PhoneActivationRequired.ToErrorText());

        if (result.Success)
            result.Value = new ApplicationUser(user.UserId, user.UserName, user.Email, $"{user.FirstName} {user.LastName}");

        return result;
    }
}