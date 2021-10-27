using Microsoft.AspNetCore.Mvc;

namespace MisProject.Server.Controllers;

[Route("/Api/V1/[controller]")]
public class UserController : Controller
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// Register user
    /// </summary>
    /// <param name="dto">RegisterDTO for user</param>
    /// <remarks>
    /// Sample request:
    ///     Post /Api/V1/User
    ///     {
    ///         "FirstName": "my first name",
    ///         "LastName": "my last name",
    ///         "UserName": "myusername",
    ///         "Email": "MySampleEmail@test.com",
    ///         "Password": "password",
    ///         "RepeatPassword": "password"
    ///     }
    /// </remarks>
    /// <returns>A DbUserResponses</returns>
    /// <response code="200">Returns the DbResponse of new created user</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<DbResponse<User, RegisterError>>> Register(RegisterDTO dto)
    {
        var result = await _userService.RegisterUser(dto);
        return Ok(result);
    }
}