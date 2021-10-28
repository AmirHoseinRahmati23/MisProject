using System.Net.Http.Json;
using ClientLibraries.DTOs;

namespace ClientLibraries.HttpCallers;

public interface IUserCaller
{
    /// <summary>
    /// Status 200: Cast to DbResponse<RegisterDTO, RegisterError>, Status 400: Cast to ValidationResult
    /// </summary>
    Task<HttpResponseMessage> Register(RegisterDTO dto);
}

public class UserCaller : IUserCaller
{
    private readonly HttpClient _client;
    private string _controllerAddress = "/Api/V1/User";


    public UserCaller(HttpClient client)
    {
        _client = client;
    }

    /// <inheritdoc/>
    public async Task<HttpResponseMessage> Register(RegisterDTO dto)
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
}