using Mango.Services.OpenId.Data;
using Mango.Services.OpenId.Models;
using OpenIddict.Abstractions;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Mango.Services.OpenId
{
    public class Worker : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        public Worker(IServiceProvider serviceProvider)
            => _serviceProvider = serviceProvider;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();

            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            await context.Database.EnsureCreatedAsync();

            var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();

            if (await manager.FindByClientIdAsync("mango") == null)
            {
                await manager.CreateAsync(new OpenIddictApplicationDescriptor
                {
                    ClientId = "mango",
                    ClientSecret = "secret".Sha256(),
                    ConsentType = ConsentTypes.Explicit,
                    DisplayName = "Mango web application",
                    PostLogoutRedirectUris =
                {
                    new Uri("https://localhost:7252/signout-callback-oidc")
                },
                    RedirectUris =
                {
                    new Uri("https://localhost:7252/signin-oidc")
                },
                    Permissions =
                {
                    Permissions.Endpoints.Authorization,
                    Permissions.Endpoints.Logout,
                    Permissions.Endpoints.Token,
                    Permissions.GrantTypes.AuthorizationCode,
                    Permissions.GrantTypes.RefreshToken,
                    Permissions.ResponseTypes.Code,
                    Permissions.Scopes.Email,
                    Permissions.Scopes.Profile,
                    Permissions.Scopes.Roles,
                    Permissions.Prefixes.Scope + "mango"
                }
                });
            }

            if (await manager.FindByClientIdAsync("client") == null)
            {
                await manager.CreateAsync(new OpenIddictApplicationDescriptor
                {
                    ClientId = "client",
                    ClientSecret = "secret".Sha256(),
                    ConsentType = ConsentTypes.Explicit,
                    DisplayName = "Client",
                    PostLogoutRedirectUris =
                {
                    new Uri("https://localhost:7252/signout-callback-oidc")
                },
                    RedirectUris =
                {
                    new Uri("https://localhost:7252/signin-oidc")
                },
                    Permissions =
                {
                    Permissions.Endpoints.Authorization,
                    Permissions.Endpoints.Logout,
                    Permissions.Endpoints.Token,
                    Permissions.GrantTypes.ClientCredentials,
                    Permissions.GrantTypes.RefreshToken,
                    Permissions.ResponseTypes.Code,
                    Permissions.Scopes.Profile,
                    Permissions.Prefixes.Scope + "mango",
                    Permissions.Prefixes.Scope + "read",
                    Permissions.Prefixes.Scope + "write"
                },
                    Requirements =
                {
                    Requirements.Features.ProofKeyForCodeExchange
                }
                });
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
