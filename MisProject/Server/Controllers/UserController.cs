﻿using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace MisProject.Server.Controllers;

[Route("/Api/V1/[controller]")]
public class UserController : Controller
{
    private readonly IUserService _userService;
    private readonly IJwtHelper _jwtHelper;

    public UserController(IUserService userService, IJwtHelper jwtHelper)
    {
        _userService = userService;
        _jwtHelper = jwtHelper;
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

    /// <summary>
    /// Get user by JWT Token
    /// </summary>
    /// <param name="jwtToken">Jwt token generated by Login method.</param>
    /// <remarks>
    /// Sample request:
    ///     Get /Api/V1/User?jwtToken=Value
    /// </remarks>
    /// <returns>A Application user for authenticationStateProvider</returns>
    /// <response code="200">Returns the application user if jwtToken is valid</response>
    /// <response code="400">if jwt token isn't valid</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApplicationUser))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApplicationUser?>> GetUserByJwt(string jwtToken)
    {
        var user = await _jwtHelper.GetUserByJWT(jwtToken);

        if (user != null)
        {
            return Ok(new ApplicationUser
            {
                UserName = user.UserName,
                Email = user.Email,
                UserId = user.UserId
            });
        }

        return BadRequest();
    }
}