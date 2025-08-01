using Applications.Features.AppAccounts.Commands;
using Applications.Interfaces.Repositories;
using Core.Enumerables;
using Core.Interfaces;
using MediatR;
using Shared.Results;

public class VerifyOtpCommandHandler : IRequestHandler<VerifyOtpCommand, BaseResponse<string>>
{
    private readonly IOtpCodeLogRepository _otpRepo;
    private readonly IUserRepository _userRepo;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAppUserJwtTokenGenerator _jwtTokenGenerator;

    public VerifyOtpCommandHandler(
        IOtpCodeLogRepository otpRepo,
        IUserRepository userRepo,
        IUnitOfWork unitOfWork,
        IAppUserJwtTokenGenerator jwtTokenGenerator)
    {
        _otpRepo = otpRepo;
        _userRepo = userRepo;
        _unitOfWork = unitOfWork;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<BaseResponse<string>> Handle(VerifyOtpCommand request, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;

        // 1. Tìm OTP chưa xác minh
        var otpLog = await _otpRepo.GetLatestUnverifiedOtpAsync(
            request.PhoneNumber, request.OtpCode, OtpPurpose.ForgotPassword, cancellationToken);

        if (otpLog == null || otpLog.ExpireAt < now)
        {
            return BaseResponse<string>.Error("Mã OTP không hợp lệ hoặc đã hết hạn.");
        }

        // 2. Đánh dấu đã xác minh
        otpLog.IsVerified = true;
        await _otpRepo.UpdateAsync(otpLog, cancellationToken);

        // 3. Tìm user theo số điện thoại
        var user = await _userRepo.FindByPhoneAsync(request.PhoneNumber, cancellationToken);
        if (user == null)
        {
            return BaseResponse<string>.Error("Không tìm thấy tài khoản tương ứng.");
        }

        // 4. Sinh token tạo lại mật khẩu
        var token = _jwtTokenGenerator.GeneratePasswordSetupToken(user.Id, user.Email ?? "", user.PhoneNumber!);
        user.SetPasswordToken = token;
        user.SetPasswordTokenExpiry = now.AddHours(1); // hoặc 15-30 phút tùy chính sách

        await _userRepo.UpdateAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // 5. Trả token để FE redirect user đến trang tạo mật khẩu
        return BaseResponse<string>.Success(token, "Xác minh OTP thành công. Token tạo mật khẩu đã được cấp.");
    }
}
