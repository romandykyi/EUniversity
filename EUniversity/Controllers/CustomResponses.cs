using System.Diagnostics;

namespace EUniversity.Controllers;

public static class CustomResponses
{
    public static object NotFound(string message, HttpContext httpContext)
    {
        var result = new
        {
            type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
            title = "Resource not found",
            status = StatusCodes.Status404NotFound,
            traceId = Activity.Current?.Id ?? httpContext.TraceIdentifier,
            message
        };
        return result;
    }
}
