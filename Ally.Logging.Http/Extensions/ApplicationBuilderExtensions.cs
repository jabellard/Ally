using Microsoft.AspNetCore.Builder;

namespace Ally.Logging.Http.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestLoggingMiddleware>();
        }
    }
}