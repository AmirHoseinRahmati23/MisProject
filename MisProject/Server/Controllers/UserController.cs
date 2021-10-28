using FluentValidation.Results;
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
    /// <response code="400">Returns if DtoValidationField</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DbResponse<RegisterDTO, RegisterError>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationResult))]
    public async Task<ActionResult<DbResponse<RegisterDTO, RegisterError>>> Register([FromBody] RegisterDTO dto)
    {
        var dtoFluentValidator = new RegisterDtoFluentValidator();
        var validationResult = await dtoFluentValidator.ValidateAsync(dto);

        if (!validationResult.IsValid)
            return BadRequest(validationResult);

        var result = await _userService.RegisterUser(dto);

        if (result.Value != null)
            SecurityIgnoredMethod<RegisterDTO>.RemoveUnSecureParameters(result.Value);

        return Ok(result);
    }
}