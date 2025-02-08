using System.Net;
using System.Text.Json;
using Application.Core;

namespace API.Middleware;

public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            if (ex.InnerException != null)
            {
                logger.LogError(ex.InnerException, ex.InnerException.Message);
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            // var respone = new AppException(context.Response.StatusCode, ex.Message, ex.StackTrace);

            var respone = env.IsDevelopment()
                ? new AppException(context.Response.StatusCode, ex.Message, ex.StackTrace)
                : new AppException(context.Response.StatusCode, "Internal Server Error");


            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

            var json = JsonSerializer.Serialize(respone, options);

            await context.Response.WriteAsync(json);
        }
    }
}