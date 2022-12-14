using Mango.GatewaySolution.Extensions;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using Ocelot.Authorization;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IScopesAuthorizer, CustomScopeAuthorizer>();

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        //options.Authority = "https://localhost:7088/";
        options.Authority = "https://localhost:7287";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false
        };
    });
builder.Services.AddOcelot();

var app = builder.Build();

await app.UseOcelot();

app.Run();
