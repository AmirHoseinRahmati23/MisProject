using System.Security.Claims;
using ClientLibraries.HttpCallers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace ClientLibraries.Security;

public class PermissionPolicyProvider : IAuthorizationPolicyProvider
{
    const string POLICY_PREFIX = "RequiredPermission_";
    private DefaultAuthorizationPolicyProvider FallbackPolicyProvider { get; }

    public PermissionPolicyProvider(IOptions<AuthorizationOptions> options)
    {
        FallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
    }

    public async Task<AuthorizationPolicy> GetDefaultPolicyAsync() => await FallbackPolicyProvider.GetDefaultPolicyAsync();

    public async Task<AuthorizationPolicy?> GetFallbackPolicyAsync() => await FallbackPolicyProvider.GetFallbackPolicyAsync();

    public async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        if (policyName.StartsWith(POLICY_PREFIX, StringComparison.OrdinalIgnoreCase) &&
            int.TryParse(policyName.Substring(POLICY_PREFIX.Length), out var permId))
        {
            var policy = new AuthorizationPolicyBuilder();
            policy.AddRequirements(new PermissionRequirement(permId));
            return await Task.FromResult(policy.Build());
        }

        return await FallbackPolicyProvider.GetPolicyAsync(policyName);
    }
}

public class PermissionRequirement : IAuthorizationRequirement
{
    public int PermissionId { get; private set; }

    public PermissionRequirement(int permissionId) => PermissionId = permissionId;
}

public class PermissionAuthorizeAttribute : AuthorizeAttribute
{
    const string POLICY_PREFIX = "RequiredPermission_";

    public PermissionAuthorizeAttribute(int permissionId) => PermissionId = permissionId;

    public int PermissionId
    {
        get => int.TryParse(Policy?.Substring(POLICY_PREFIX.Length), out var id) ? id : 0;
        set => Policy = $"{POLICY_PREFIX}{value}";
    }
}

public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    private readonly IUserCaller _userCaller;

    public PermissionAuthorizationHandler(IUserCaller userCaller)
    {
        _userCaller = userCaller;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        if (context.User.Identity is { IsAuthenticated: true, Name: { } })
        {
            var result = await _userCaller.CheckPermission(requirement.PermissionId);
            if (result)
                context.Succeed(requirement);
        }
    }
}