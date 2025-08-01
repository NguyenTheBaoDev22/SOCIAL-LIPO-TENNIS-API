using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Reflection;
using System.Security;

namespace Shared.Authorization
{
    public enum PermissionMatchMode
    {
        And,
        Or
    }

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
    public class RequirePermissionAttribute : Attribute, IAuthorizationFilter
    {
        public PermissionCode[] RequiredPermissions { get; }
        public PermissionMatchMode MatchMode { get; }

        public RequirePermissionAttribute(PermissionCode permission, PermissionMatchMode matchMode = PermissionMatchMode.Or)
        {
            RequiredPermissions = new[] { permission };
            MatchMode = matchMode;
        }

        public RequirePermissionAttribute(PermissionCode[] permissions, PermissionMatchMode matchMode = PermissionMatchMode.Or)
        {
            RequiredPermissions = permissions;
            MatchMode = matchMode;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;

            var userPermissions = user?.Claims
                .Where(c => c.Type == "permission")
                .Select(c => c.Value)
                .ToHashSet(StringComparer.OrdinalIgnoreCase)
                ?? new HashSet<string>();

            bool isAuthorized = MatchMode switch
            {
                PermissionMatchMode.Or => RequiredPermissions.Any(p => userPermissions.Contains(p.ToString())),
                PermissionMatchMode.And => RequiredPermissions.All(p => userPermissions.Contains(p.ToString())),
                _ => false
            };

            if (!isAuthorized)
            {
                var descriptions = RequiredPermissions.Select(p => $"{p} ({GetPermissionDescription(p)})");
                var message = MatchMode == PermissionMatchMode.And
                    ? $"Missing all required permissions: {string.Join(", ", descriptions)}"
                    : $"Missing at least one of: {string.Join(", ", descriptions)}";

                context.Result = new ContentResult
                {
                    StatusCode = StatusCodes.Status403Forbidden,
                    ContentType = "application/json",
                    Content = System.Text.Json.JsonSerializer.Serialize(new
                    {
                        code = "403",
                        message = message,
                        traceId = context.HttpContext.TraceIdentifier
                    })
                };
            }
        }

        private string GetPermissionDescription(PermissionCode code)
        {
            var field = typeof(PermissionCode).GetField(code.ToString());
            var attr = field?.GetCustomAttribute<System.ComponentModel.DescriptionAttribute>();
            return attr?.Description ?? code.ToString();
        }
    }
}
