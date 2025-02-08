using API.Extensions;
using API.Extensions.Middleware;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace API;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.ApplyServices(builder.Configuration);
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

        var app = builder.Build();
        
        app.AddMiddlewares();

        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        try
        {
            var context = services.GetRequiredService<DataContext>();
            await context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred while migrating the database.");
            throw;
        }

        await app.RunAsync();
    }
}