using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Core.Security;

public interface IJwtHelper
{
    string GenerateJwtToken(ApplicationUser user);

    Task<User?> GetUserByJwt(string jwtToken);
}

public class JwtHelper : IJwtHelper
{
    private readonly IConfiguration _configuration;
    private readonly IUserService _userService;
    private readonly ILogger<JwtHelper> _logger;

    public JwtHelper(IConfiguration configuration, IUserService userService, ILogger<JwtHelper> logger)
    {
        _configuration = configuration;
        _userService = userService;
        _logger = logger;
    }

    public string GenerateJwtToken(ApplicationUser user)
    {
        //getting the secret key
        var secretKey = _configuration["JWTSettings:SecretKey"];
        var key = Encoding.ASCII.GetBytes(secretKey);

        //create claims
        var claimEmail = new Claim(ClaimTypes.Email, user.Email);
        var claimNameIdentifier = new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString());
        var claimUserName = new Claim(ClaimTypes.Name, user.UserName);
        var claimFullName = new Claim(ClaimTypes.GivenName, user.FullName);

        //create claimsIdentity
        var claimsIdentity = new ClaimsIdentity(new[] { claimEmail, claimUserName, claimNameIdentifier, claimFullName }, "serverAuth");

        // generate token that is valid for 14 days
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = claimsIdentity,
            Expires = DateTime.UtcNow.AddDays(14),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        //creating a token handler
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        _logger.LogInformation($"Jwt created for user {user.UserName}({user.UserId})");

        //returning the token back
        return tokenHandler.WriteToken(token);
    }

    public async Task<User?> GetUserByJwt(string jwtToken)
    {
        try
        {
            //getting the secret key
            var secretKey = _configuration["JWTSettings:SecretKey"];
            var key = Encoding.ASCII.GetBytes(secretKey);

            //preparing the validation parameters
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false
            };
            var tokenHandler = new JwtSecurityTokenHandler();

            //validating the token
            var principle = tokenHandler.ValidateToken(jwtToken, tokenValidationParameters, out var securityToken);
            var jwtSecurityToken = (JwtSecurityToken)securityToken;

            if (jwtSecurityToken.ValidTo > DateTime.Now
                && jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                //returning the user if found
                var userId = principle.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (userId != null)
                {
                    var user = await _userService.GetUser(int.Parse(userId));
                    if (user != null)
                    {
                        _logger.LogInformation($"Jwt Authorized for user {user.UserName}({user.UserId})");
                    }

                    return user;
                }
            }
        }
        catch (Exception ex)
        {
            //logging the error and returning null
            _logger.LogError($"Exception: {ex}");
        }

        //returning null if token is not validated
        return null;
    }

}