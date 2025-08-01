using Applications.Features.AppAccounts.Commands;
using Applications.Interfaces.Repositories;
using Applications.Services.Interfaces;
using Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Shared.Results;

namespace Applications.Features.AppAccounts.Handlers;

/// <summary>
/// Handler xử lý lệnh thiết lập mật khẩu lần đầu từ token.
/// </summary>
public class SetupPasswordCommandHandler : IRequestHandler<SetupPasswordCommand, BaseResponse<bool>>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;

    public SetupPasswordCommandHandler(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
    }

    public async Task<BaseResponse<bool>> Handle(SetupPasswordCommand request, CancellationToken cancellationToken)
    {
        Log.Information("🔐 [SetupPassword] Bắt đầu thiết lập mật khẩu với token: {Token}", request.Token);

        // ✅ Bước 1: Tìm user dựa vào token
        var user = await _userRepository.AsQueryable()
            .FirstOrDefaultAsync(u => u.SetPasswordToken == request.Token, cancellationToken);

        if (user == null)
        {
            Log.Warning("❌ [SetupPassword] Token không hợp lệ hoặc đã hết hạn: {Token}", request.Token);
            return BaseResponse<bool>.Error( "Liên kết không hợp lệ hoặc đã hết hạn.", ErrorCodes.Session_TokenError);
        }

        // ✅ Bước 2: Kiểm tra thời hạn token
        if (user.SetPasswordTokenExpiry == null || user.SetPasswordTokenExpiry < DateTime.UtcNow)
        {
            Log.Warning("⏰ [SetupPassword] Token đã hết hạn cho UserId: {UserId}", user.Id);
            return BaseResponse<bool>.Error( "Liên kết đã hết hạn. Vui lòng yêu cầu lại.", ErrorCodes.Session_Expired);
        }

        // ✅ Bước 3: Cập nhật mật khẩu và trạng thái xác thực
        user.PasswordHash = _passwordHasher.HashPassword(request.NewPassword);
        user.SetPasswordToken = null;
        user.SetPasswordTokenExpiry = null;
        user.IsVerified = true;
        user.UpdatedAt = DateTime.UtcNow;

        await _userRepository.UpdateAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        Log.Information("✅ [SetupPassword] Thiết lập mật khẩu thành công cho UserId: {UserId}", user.Id);

        return BaseResponse<bool>.Success(true, "Thiết lập mật khẩu thành công.");
    }
}
