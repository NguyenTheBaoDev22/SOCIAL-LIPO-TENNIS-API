using Microsoft.AspNetCore.Builder;

namespace Shared.Middleware
{
    public static class PermissionMiddlewareExtensions
    {
        public static IApplicationBuilder UsePermissionAuthorization(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthorizeByPermissionMiddleware>();
        }
    }
}
