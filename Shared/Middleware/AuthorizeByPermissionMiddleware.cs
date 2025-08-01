using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Shared.Authorization;
using System.Text.Json;

namespace Shared.Middleware
{
    /// <summary>
    /// Middleware kiểm tra quyền truy cập theo permission từ JWT claims.
    /// </summary>
    public class AuthorizeByPermissionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AuthorizeByPermissionMiddleware> _logger;

        public AuthorizeByPermissionMiddleware(RequestDelegate next, ILogger<AuthorizeByPermissionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Alternative 2: Sử dụng Attribute trên Controller/Action thay vì endpoint metadata
            // Kiểm tra nếu user chưa authenticate
            if (!context.User.Identity.IsAuthenticated)
            {
                await _next(context);
                return;
            }

            // Lấy route values để xác định controller/action
            var routeData = context.GetRouteData();
            if (routeData == null)
            {
                await _next(context);
                return;
            }

            // Thay vì dùng endpoint metadata, có thể dùng approach khác
            // Ví dụ: Check permissions dựa trên route pattern hoặc controller/action
            var controller = routeData.Values["controller"]?.ToString();
            var action = routeData.Values["action"]?.ToString();

            // Bạn có thể define permissions mapping theo controller/action
            var requiredPermissions = GetRequiredPermissions(controller, action);

            if (!requiredPermissions.Any())
            {
                await _next(context);
                return;
            }

            var claims = context.User.Claims;
            var permissions = claims.Where(c => c.Type == "permission").Select(c => c.Value).ToList();

            foreach (var permission in requiredPermissions)
            {
                if (!permissions.Contains(permission))
                {
                    _logger.LogWarning("🚫 Permission denied: Missing '{Permission}'", permission);
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    context.Response.ContentType = "application/json";

                    var response = new
                    {
                        code = "403",
                        message = $"Permission '{permission}' is required.",
                        traceId = context.TraceIdentifier
                    };

                    await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(response));
                    return;
                }
            }

            await _next(context);
        }

        private List<string> GetRequiredPermissions(string controller, string action)
        {
            // Implement logic to return required permissions based on controller/action
            // Hoặc bạn có thể dùng một dictionary mapping
            var permissionMap = new Dictionary<string, List<string>>
            {
                { "Users.Get", new List<string> { "user.view" } },
                { "Users.Post", new List<string> { "user.create" } },
                { "Users.Put", new List<string> { "user.edit" } },
                { "Users.Delete", new List<string> { "user.delete" } },
                // Add more mappings as needed
            };

            var key = $"{controller}.{action}";
            return permissionMap.ContainsKey(key) ? permissionMap[key] : new List<string>();
        }
    }
}