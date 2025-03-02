using System.Reflection;
using API.Helpers;
using Application.Core;
using Application.Interfaces;
using Infrastructure.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Persistence;
using Environment = System.Environment;

namespace API.Extensions.Services;

public static class ServiceExtensions
{

    public static IServiceCollection ConfigureSwaggerGen(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Name = "Authorization",
                Description = "Enter 'Bearer {token}'"
            });
            
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    []
                }
                
            });

        });

        return services;
    }
    
    public static IServiceCollection ConfigureControllerServices(this IServiceCollection services)
    {
        services.AddControllers(options =>
        {
            var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
            options.Filters.Add(new AuthorizeFilter(policy));
        });
        return services;
    }

    public static IServiceCollection ConfigureHttpContextAccessor(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<IUserAccessor, UserAccessor>();

        return services;
    }

    public static IServiceCollection ConfigureCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(Konstants.CorsPolicy,
                builder =>
                    builder
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .WithOrigins("http://localhost:3000"));
        });

        return services;
    }
    
    public static IServiceCollection AddMediatorConfig(this IServiceCollection services)
    {
        // services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(Application.Tasks.Handlers.List.Handler).Assembly));

        return services;
    }

    public static IServiceCollection AddAutoMapperConfig(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MappingProfiles).Assembly);
        return services;
    }

    public static IServiceCollection ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<DataContext>(opt =>
        {
            var env = Environment.GetEnvironmentVariable(Konstants.Env.Name);

            string connStr;

            if (env == Konstants.Environment.Type.Development.ToString())
            {
                connStr = configuration.GetConnectionString(Konstants.DefaultConnection);
                opt.UseSqlite(connStr);
            }
            else
            {
                var connUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

                // Parse connection URL to connection string for Npgsql
                connUrl = connUrl!.Split("://")[1];
                var pgUserPass = connUrl.Split("@")[0];
                var pgHostPortDb = connUrl.Split("@")[1];
                var pgHostPort = pgHostPortDb.Split("/")[0];
                var pgDb = pgHostPortDb.Split("/")[1];
                var pgUser = pgUserPass.Split(":")[0];
                var pgPass = pgUserPass.Split(":")[1];
                var pgHost = pgHostPort.Split(":")[0];
                var pgPort = pgHostPort.Split(":")[1];


                connStr = $"Server={pgHost};Port={pgPort};User Id={pgUser};Password={pgPass};Database={pgDb}; Trust Server Certificate=true";
                opt.UseNpgsql(connStr);
            }

        });
        return services;
    }
}