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
    Task<DbResponses<User, RegisterError>> AddUser(User user);

    Task<DbResponses<User, RegisterError>> RegisterUser(RegisterDTO registerDto);
}