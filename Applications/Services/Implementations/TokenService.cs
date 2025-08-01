using Applications.Services.Interfaces;
using Core.Entities.AppUsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Services.Implementations
{
    public class TokenService : ITokenService
    {
        // Phương thức tạo token reset mật khẩu
        public string GenerateResetPasswordToken(Guid userId)
        {
            // Mã hóa đơn giản, trong production nên dùng JWT có expiry
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        }

        // Phương thức xác thực token reset mật khẩu
        public bool ValidateResetPasswordToken(string token, out Guid userId)
        {
            try
            {
                // Giải mã token và lấy ra ID người dùng (có thể dùng JWT hoặc cách khác tuỳ nhu cầu)
                byte[] bytes = Convert.FromBase64String(token);
                userId = new Guid(bytes);

                // Giả sử rằng ID của người dùng là hợp lệ nếu không có exception nào xảy ra
                return true;
            }
            catch
            {
                // Nếu giải mã token không thành công, trả về false và gán userId bằng Guid.Empty
                userId = Guid.Empty;
                return false;
            }
        }
    }
}
