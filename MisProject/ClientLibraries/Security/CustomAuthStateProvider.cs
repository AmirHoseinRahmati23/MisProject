using System.Security.Claims;
using Blazored.LocalStorage;
using ClientLibraries.DTOs;
using ClientLibraries.HttpCallers;
using Microsoft.AspNetCore.Components.Authorization;

namespace ClientLibraries.Security;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private readonly ILocalStorageService _localStorageService;
    private readonly IUserCaller _userCaller;

    public CustomAuthStateProvider(ILocalStorageService localStorageService, IUserCaller userCaller)
    {
        _localStorageService = localStorageService;
        _userCaller = userCaller;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var currentUser = await GetUserByJwt();

        if (currentUser != null)
        {
            //create a claims
            var claimEmailAddress = new Claim(ClaimTypes.Name, currentUser.Email);
            var claimNameIdentifier = new Claim(ClaimTypes.NameIdentifier, currentUser.UserId.ToString());
            var claimName = new Claim(ClaimTypes.Name, currentUser.UserName);

            //create claimsIdentity
            var claimsIdentity = new ClaimsIdentity(new[] { claimEmailAddress, claimNameIdentifier, claimName }, "serverAuth");

            //create claimsPrincipal
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            return new AuthenticationState(claimsPrincipal);
        }
        else
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
    }

    private async Task<ApplicationUser?> GetUserByJwt()
    {
        //pulling the token from localStorage
        var jwtToken = await _localStorageService.GetItemAsStringAsync("jwt_token");
        if (jwtToken == null) return null;

        return await _userCaller.GetUserByJwt(jwtToken);
    }

}