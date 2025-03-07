namespace API.Extensions.Services;

public static class ApplyServicesExtensions
{
    public static IServiceCollection ApplyServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureControllerServices();
        services.AddEndpointsApiExplorer();
        services.ConfigureSwaggerGen();
        services.ConfigureDatabase(configuration);
        services.ConfigureCors();
        services.AddMediatorConfig();
        services.AddAutoMapperConfig();
        services.ConfigureHttpContextAccessor();
        services.AddIdentityServices(configuration);
        
        return services;
    }
}