﻿using Ocelot.Authorization;
using Ocelot.Infrastructure.Claims.Parser;
using Ocelot.Responses;
using System.Security.Claims;

namespace Mango.GatewaySolution.Extensions
{
    public class CustomScopeAuthorizer : IScopesAuthorizer
    {
        private readonly IClaimsParser _claimsParser;
        private readonly string _scope = "scope";

        public CustomScopeAuthorizer(IClaimsParser claimsParser)
        {
            _claimsParser = claimsParser;
        }

        public Response<bool> Authorize(ClaimsPrincipal claimsPrincipal, List<string> routeAllowedScopes)
        {
            if (routeAllowedScopes == null || routeAllowedScopes.Count == 0)
            {
                return new OkResponse<bool>(true);
            }

            var values = _claimsParser.GetValuesByClaimType(claimsPrincipal.Claims, _scope);

            if (values.IsError)
            {
                return new ErrorResponse<bool>(values.Errors);
            }

            var userScopes = values.Data[0].Split(" ");

            var matchesScopes = routeAllowedScopes.Intersect(userScopes).ToList();

            if (matchesScopes.Count == 0)
            {
                return new ErrorResponse<bool>(
                    new ScopeNotAuthorizedError($"no one user scope: '{string.Join(",", userScopes)}' match with some allowed scope: '{string.Join(",", routeAllowedScopes)}'"));
            }

            return new OkResponse<bool>(true);
        }
    }
}
