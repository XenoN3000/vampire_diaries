using System.Text;
using API.Helpers;
using API.Services;
using Domain;
using Infrastructure.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Persistence;

namespace API.Extensions.Services;

public static class IdentityServiceExtensions
{
    public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
    {
        var env = Environment.GetEnvironmentVariable(Konstants.Env.Name);

        services.AddIdentityCore<AppUser>()
            .AddEntityFrameworkStores<DataContext>();

        var tokenKey = configuration[Konstants.TokenKey];
        if (string.IsNullOrEmpty(tokenKey))
        {
            throw new Exception("TokenKey is missing from configuration. Ensure it is set in appsettings.json or environment variables.");
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                };
            });


        services.AddAuthorization(
            options => { options.AddPolicy(Konstants.IsDiaryOwner, policy => policy.Requirements.Add(new IsOwnerRequirement())); });
        services.AddTransient<IAuthorizationHandler, IsOwnerRequirementHandler>();
        services.AddScoped<TokenService>();


        return services;
    }
}