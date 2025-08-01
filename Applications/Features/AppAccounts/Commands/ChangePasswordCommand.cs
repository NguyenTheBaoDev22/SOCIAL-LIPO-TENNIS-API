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
    /// Command đổi mật khẩu cho user đã đăng nhập.
    /// </summary>
    public class ChangePasswordCommand : IRequest<BaseResponse<bool>>
    {
        public string CurrentPassword { get; set; } = default!;
        public string NewPassword { get; set; } = default!;
    }
}
