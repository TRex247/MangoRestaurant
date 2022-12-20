using Microsoft.AspNetCore.Authorization;

namespace Mango.Extensions
{
    public static class ScopeAuthorization
    {
        public static AuthorizationPolicyBuilder RequireScope(
        this AuthorizationPolicyBuilder authorizationPolicyBuilder,
        params string[] requiredScopes)
        {
            authorizationPolicyBuilder.RequireScope((IEnumerable<string>)requiredScopes);
            return authorizationPolicyBuilder;
        }

        public static AuthorizationPolicyBuilder RequireScope(
            this AuthorizationPolicyBuilder authorizationPolicyBuilder,
            IEnumerable<string> requiredScopes)
        {
            authorizationPolicyBuilder.AddRequirements(new ScopeAuthorizationRequirement(requiredScopes));
            return authorizationPolicyBuilder;
        }
    }
}