using Newtonsoft.Json;
using System.Net;

namespace Api.Utils;

public class ExceptionHandler
{
    private readonly RequestDelegate _next;

    public ExceptionHandler(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HangleExceptionAsync(context, ex);
        }
    }

    private Task HangleExceptionAsync(HttpContext context, Exception ex)
    {
        // Log

        string result = JsonConvert.SerializeObject(Envelope.Error(ex.Message));
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        return context.Response.WriteAsync(result);
    }

}
