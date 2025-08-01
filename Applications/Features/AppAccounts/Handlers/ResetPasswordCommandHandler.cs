using Applications.Features.AppAccounts.Commands;
using Applications.Interfaces.Repositories;
using Core.Interfaces;
using MediatR;
using Shared.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Features.AppAccounts.Handlers
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, BaseResponse<bool>>
    {
        private readonly IUserRepository _userRepo;
        private readonly IPasswordHasher _passwordHasher;

        public async Task<BaseResponse<bool>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepo.FindByPhoneAsync(request.PhoneNumber);
            if (user == null)
                return BaseResponse<bool>.Error("Không tìm thấy tài khoản.");

            user.PasswordHash = _passwordHasher.HashPassword(request.NewPassword);
            await _userRepo.UpdateAsync(user, cancellationToken);

            return BaseResponse<bool>.Success(true, "Đặt lại mật khẩu thành công.");
        }
    }

}
