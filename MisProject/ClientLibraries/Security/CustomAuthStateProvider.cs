using System.Net.Http.Headers;
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


public class CustomAuthorizationHandler : DelegatingHandler
{
    private readonly ILocalStorageService _localStorageService;

    public CustomAuthorizationHandler(ILocalStorageService localStorageService)
    {
        //injecting local storage service
        _localStorageService = localStorageService;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        //getting token from the localstorage
        var jwtToken = await _localStorageService.GetItemAsync<string>("jwt_token");

        //adding the token in authorization header
        if (jwtToken != null)
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

        //sending the request
        return await base.SendAsync(request, cancellationToken);
    }
}