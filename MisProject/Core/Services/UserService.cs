using ClientLibraries.Converters;
using Core.Security;

namespace Core.Services;

public class UserService : IUserService
{
    private readonly MisDbContext _db;

    public UserService(MisDbContext db)
    {
        _db = db;
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

    public async Task<DbResponse<User, RegisterError>> RegisterUser(RegisterDTO registerDto)
    {
        var user = new User
        {
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            UserName = registerDto.UserName,
            Email = registerDto.Email,
            Password = PasswordHelper.EncodePasswordMd5(registerDto.Password),
        };

        return await AddUser(user);
    }
}