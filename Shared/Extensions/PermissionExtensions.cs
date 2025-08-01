using Shared.Authorization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Extensions
{
    public static class PermissionExtensions
    {
        /// <summary>
        /// Lấy mô tả (DescriptionAttribute) từ PermissionCode enum.
        /// </summary>
        public static string GetDescription(this PermissionCode code)
        {
            var field = typeof(PermissionCode).GetField(code.ToString());
            var attr = field?.GetCustomAttribute<DescriptionAttribute>();
            return attr?.Description ?? code.ToString();
        }
        ////Cách sử dụng trong controller, log, hoặc UI:
        //var permission = PermissionCode.ExportInvoice;
        //string description = permission.GetDescription();

        //Console.WriteLine(description); // => "Xuất hóa đơn"
    }
}
