using Applications.Features.AppAccounts.Commands;
using Applications.Interfaces.Repositories;
using Core.Interfaces;
using MediatR;
using Serilog;
using Shared.Interfaces;
using Shared.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Features.AppAccounts.Handlers
{
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, BaseResponse<bool>>
    {
        private readonly IUserRepository _userRepository;
        private readonly ICurrentUserService _currentUser;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUnitOfWork _unitOfWork;

        public ChangePasswordCommandHandler(
            IUserRepository userRepository,
            ICurrentUserService currentUser,
            IPasswordHasher passwordHasher,
            IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _currentUser = currentUser;
            _passwordHasher = passwordHasher;
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponse<bool>> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;
            if (userId == null)
                return BaseResponse<bool>.Error("Không xác định được người dùng.", ErrorCodes.User_NotFound);

            var user = await _userRepository.GetByIdAsync(userId.Value, cancellationToken);
            if (user == null)
                return BaseResponse<bool>.Error("Tài khoản không tồn tại.", ErrorCodes.User_NotFound);

            // Kiểm tra mật khẩu cũ
            if (!_passwordHasher.VerifyPassword(request.CurrentPassword, user.PasswordHash))
            {
                Log.Warning("❌ [ChangePassword] Mật khẩu cũ không chính xác cho userId {UserId}", user.Id);
                return BaseResponse<bool>.Error("Mật khẩu cũ không đúng.", ErrorCodes.User_InvalidPassword);
            }

            user.PasswordHash = _passwordHasher.HashPassword(request.NewPassword);
            user.UpdatedAt = DateTime.UtcNow;

            await _userRepository.UpdateAsync(user, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            Log.Information("🔐 [ChangePassword] User {UserId} đã đổi mật khẩu thành công.", user.Id);

            return BaseResponse<bool>.Success(true, "Đổi mật khẩu thành công.");
        }
    }
}
