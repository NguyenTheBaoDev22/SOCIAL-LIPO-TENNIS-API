using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Attributes
{
    /// <summary>
    /// Bộ lọc cho phép truyền metadata xuống middleware
    /// </summary>
    public class AuthorizePermissionFilter : IAuthorizationFilter
    {
        private readonly string _permission;

        public AuthorizePermissionFilter(string permission)
        {
            _permission = permission;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // Gắn vào HttpContext.Items để middleware đọc
            context.HttpContext.Items["RequiredPermission"] = _permission;
        }
    }
}
