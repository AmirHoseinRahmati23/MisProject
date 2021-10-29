using System.Net.Http.Json;
using ClientLibraries.DTOs;

namespace ClientLibraries.HttpCallers;

public interface IUserCaller
{
    /// <summary>
    /// Status 200: Cast to DbResponse<RegisterDto, RegisterError>, Status 400: Cast to ValidationResult
    /// </summary>
    Task<HttpResponseMessage> Register(RegisterDto dto);

    /// <summary>
    /// Return ApplicationUser? in each status
    /// </summary>
    Task<ApplicationUser?> GetUserByJwt(string jwtToken);

    /// <summary>
    /// Status 200: Cast to DbResponse<string, LoginError>, Status 400: Cast to ValidationResult
    /// </summary>
    Task<HttpResponseMessage> Login(LoginDto dto);

    /// <summary>
    /// Check if user have permission or not.
    /// </summary>
    Task<bool> CheckPermission(int permissionId);
}

public class UserCaller : IUserCaller
{
    private readonly HttpClient _client;
    private readonly string _controllerAddress = "/Api/V1/User";

    public UserCaller(HttpClient client)
    {
        _client = client;
    }

    /// <inheritdoc />
    public async Task<HttpResponseMessage> Register(RegisterDto dto)
    {
        try
        {
            var result = await _client.PostAsJsonAsync(_controllerAddress, dto);
            return result;
        }
        catch
        {
            return new HttpResponseMessage(0);
        }
    }

    /// <inheritdoc/>
    public async Task<ApplicationUser?> GetUserByJwt(string jwtToken)
    {
        try
        {
            var result = await _client.GetAsync($"{_controllerAddress}?jwtToken={jwtToken}");

            if (result.IsSuccessStatusCode)
            {
                var applicationUser = await result.Content.ReadFromJsonAsync<ApplicationUser?>();
                return applicationUser;
            }
        }
        catch
        {
            // ignored
        }

        return null;
    }

    /// <inheritdoc />
    public async Task<HttpResponseMessage> Login(LoginDto dto)
    {
        try
        {
            var result = await _client.PostAsJsonAsync(_controllerAddress + "/Login", dto);
            return result;
        }
        catch
        {
            return new HttpResponseMessage(0);
        }
    }

    /// <inheritdoc />
    public async Task<bool> CheckPermission(int permissionId)
    {
        return await Task.FromResult(permissionId == 2);
    }
}