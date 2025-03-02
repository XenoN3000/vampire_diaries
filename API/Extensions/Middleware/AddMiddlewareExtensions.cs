using API.Helpers;
using API.Middleware;

namespace API.Extensions.Middleware;

public static class AddMiddlewareExtensions
{
    public static WebApplication AddMiddlewares(this WebApplication app)
    {
        
        app.UseMiddleware<ExceptionMiddleware>();


        app.UseSwagger();
        app.UseSwaggerUI();
        
        if (app.Environment.IsDevelopment())
        {
            // app.UseSwagger();
            // app.UseSwaggerUI();
        }
        else
        {
            
           
            
            app.Use(async (context, next) =>
            {
                context.Response.Headers.Append("Strict-Transport-Security","max-age=31536000");
                await next.Invoke();
            });
        }
        
        app.UseHttpsRedirection();

        app.UseCors(Konstants.CorsPolicy);

        app.UseAuthentication();
        app.UseAuthorization();
        
        app.MapControllers();
        
        
        return app;
    } 
}