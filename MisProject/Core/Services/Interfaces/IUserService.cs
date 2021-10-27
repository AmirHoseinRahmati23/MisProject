namespace Core.Services.Interfaces;

public interface IUserService
{
    Task<bool> IsEmailExists(string email);

    Task<bool> IsUserNameExists(string userName);

    Task<bool> IsUserExists(int userId);

    Task<User?> GetUser(int userId);

    Task<User?> GetUser(string userName);

    /// <summary>
    /// Note: Password should be hashed
    /// </summary>
    Task<DbResponse<User, RegisterError>> AddUser(User user);

    Task<DbResponse<User, RegisterError>> RegisterUser(RegisterDTO registerDto);
}