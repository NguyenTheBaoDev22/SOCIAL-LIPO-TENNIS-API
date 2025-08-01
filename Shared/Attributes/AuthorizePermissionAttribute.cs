using Microsoft.AspNetCore.Mvc;
using Shared.Authorization;
using System;

namespace Shared.Attributes
{
    /// <summary>
    /// Gắn vào controller hoặc action để yêu cầu permission cụ thể.
    /// Có thể dùng dạng string hoặc enum PermissionCode.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
    public class AuthorizePermissionAttribute : TypeFilterAttribute
    {
        /// <summary>
        /// Sử dụng dạng chuỗi: [AuthorizePermission("ApproveMerchant")]
        /// </summary>
        /// <param name="permission"></param>
        public AuthorizePermissionAttribute(string permission)
            : base(typeof(AuthorizePermissionFilter))
        {
            Arguments = new object[] { permission };
        }

        /// <summary>
        /// Sử dụng dạng Enum: [AuthorizePermission(PermissionCode.ApproveMerchant)]
        /// Giúp tránh lỗi chính tả.
        /// </summary>
        /// <param name="permissionCode"></param>
        public AuthorizePermissionAttribute(PermissionCode permissionCode)
            : base(typeof(AuthorizePermissionFilter))
        {
            Arguments = new object[] { permissionCode.ToString() };
        }
    }
}
