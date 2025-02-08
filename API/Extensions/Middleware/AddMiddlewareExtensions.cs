using API.Helpers;

namespace API.Extensions.Middleware;

public static class AddMiddlewareExtensions
{
    public static WebApplication AddMiddlewares(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        else
        {
            app.UseHttpsRedirection();
        }
        
        app.UseCors(Konstants.CorsPolicy);
        
        // app.UseAuthorization();
        app.MapControllers();
        
        
        return app;
    } 
}