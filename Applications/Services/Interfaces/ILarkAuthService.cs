using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Services.Interfaces
{
    public interface ILarkAuthService
    {
        /// <summary>
        /// Tạo URL ủy quyền Larksuite.
        /// </summary>
        /// <param name="state">Một chuỗi duy nhất để bảo vệ CSRF.</param>
        /// <returns>URL ủy quyền hoàn chỉnh.</returns>
        string GenerateAuthorizationUrl(string state);
    }
}
