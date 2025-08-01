using MediatR;
using Shared.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Features.AppAccounts.Commands
{

    /// <summary>
    /// Command thiết lập mật khẩu lần đầu cho tài khoản sau khi admin duyệt.
    /// </summary>
    public class SetupPasswordCommand : IRequest<BaseResponse<bool>>
    {
        /// <summary>
        /// Token được gửi qua email để xác nhận danh tính.
        /// </summary>
        public string Token { get; set; } = default!;

        /// <summary>
        /// Mật khẩu mới mà người dùng muốn thiết lập.
        /// </summary>
        public string NewPassword { get; set; } = default!;
    }
}
