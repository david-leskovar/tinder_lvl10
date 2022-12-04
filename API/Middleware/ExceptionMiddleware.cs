using API.Errors;
using System.Globalization;
using System.Net;
using System.Text.Json;

namespace Middleware.Example;

public class RequestCultureMiddleware
{
    private readonly RequestDelegate next;
    private readonly ILogger<RequestCultureMiddleware> logger;
    private readonly IHostEnvironment env;

    public RequestCultureMiddleware(RequestDelegate next,ILogger<RequestCultureMiddleware> logger,IHostEnvironment env)
    {
        this.next = next;
        this.logger = logger;
        this.env = env;
    }

    public async Task InvokeAsync(HttpContext context) {
        try
        {

            await next(context);

        }
        catch (Exception ex)
        {
            Console.WriteLine(" ka daj js sm tole");
            logger.LogError(ex, ex.Message);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = env.IsDevelopment() ? new ApiException(context.Response.StatusCode, ex.Message, ex.StackTrace?.ToString()) : new ApiException(context.Response.StatusCode, "Internal Server Error");
            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

            var json = JsonSerializer.Serialize(response, options);
            await context.Response.WriteAsync(json);


        }


    }

   
}



/*


try
        {

            await _next(context);

        }
        catch (Exception ex)
{
    Console.WriteLine(" ka daj js sm tole");
    _logger.LogError(ex, ex.Message);
    context.Response.ContentType = "application/json";
    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

    var response = _env.IsDevelopment() ? new ApiException(context.Response.StatusCode, ex.Message, ex.StackTrace?.ToString()) : new ApiException(context.Response.StatusCode, "Internal Server Error");
    var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    var json = JsonSerializer.Serialize(response, options);
    await context.Response.WriteAsync(json);


}
*/