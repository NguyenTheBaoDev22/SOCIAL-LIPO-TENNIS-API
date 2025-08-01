using Applications.DTOs;
using Applications.Features.AppAccounts.Commands;
using Applications.Interfaces.Repositories;
using Applications.Interfaces.Services;
using Core.Entities;
using Core.Entities.AppUsers;
using Core.Enumerables;
using MediatR;
using Serilog;
using Shared.Helpers;
using Shared.Results;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Applications.Features.AppAccounts.Handlers;

/// <summary>
/// Xử lý gửi OTP cho chức năng quên mật khẩu.
/// </summary>
public class RequestForgotPasswordOtpCommandHandler : IRequestHandler<RequestForgotPasswordOtpCommand, BaseResponse<bool>>
{
    private readonly IMBFSmsBranchnameService _smsService;
    private readonly IOtpCodeLogRepository _otpRepo;
    private readonly IOtpRequestCounterRepository _otpCounterRepo;
    private readonly IUserRepository _userRepo;
    private readonly IUnitOfWork _unitOfWork;

    public RequestForgotPasswordOtpCommandHandler(
        IMBFSmsBranchnameService smsService,
        IOtpCodeLogRepository otpRepo,
        IOtpRequestCounterRepository otpCounterRepo,
        IUserRepository userRepo,
        IUnitOfWork unitOfWork)
    {
        _smsService = smsService;
        _otpRepo = otpRepo;
        _otpCounterRepo = otpCounterRepo;
        _userRepo = userRepo;
        _unitOfWork = unitOfWork;
    }

    public async Task<BaseResponse<bool>> Handle(RequestForgotPasswordOtpCommand request, CancellationToken cancellationToken)
    {
        var phone = request.PhoneNumber.Trim();
        var now = DateTime.UtcNow;
        const int maxDailyOtp = 3;

        Log.Information("🔐 [ForgotPassword] Bắt đầu xử lý yêu cầu gửi OTP cho số điện thoại: {Phone}", phone);

        // 1. Kiểm tra user
        var user = await _userRepo.FindByPhoneAsync(phone, cancellationToken);
        if (user == null)
        {
            Log.Warning("❌ [ForgotPassword] Không tìm thấy user với số điện thoại: {Phone}", phone);
            return BaseResponse<bool>.Error($"Không tìm thấy tài khoản tương ứng với số điện thoại {phone}.");
        }

        // 2. Kiểm tra bộ đếm OTP
        var counter = await _otpCounterRepo.GetAsync(phone, OtpPurpose.ForgotPassword, cancellationToken);
        if (counter != null && counter.Count >= maxDailyOtp && counter.LastResetAt > now.AddHours(-24))
        {
            Log.Warning("⛔ [ForgotPassword] Vượt giới hạn gửi OTP. Phone: {Phone}, Count: {Count}, LastResetAt: {LastResetAt}", phone, counter.Count, counter.LastResetAt);
            return BaseResponse<bool>.Error("Bạn đã vượt quá số lần gửi OTP trong 24h. Vui lòng thử lại sau.");
        }

        // 3. Sinh OTP
        var otp = RandomHelper.GenerateNumericCode(6);
        var expireAt = now.AddMinutes(5);

        var otpLog = new OtpCodeLog
        {
            Id = Guid.NewGuid(),
            PhoneNumber = phone,
            OtpCode = otp,
            Purpose = OtpPurpose.ForgotPassword,
            ExpireAt = expireAt,
            IsVerified = false,
            CreatedAt = now
        };
        await _otpRepo.AddAsync(otpLog, cancellationToken);
        Log.Information("📝 [ForgotPassword] Ghi log OTP: {Otp}, ExpireAt: {ExpireAt}", otp, expireAt);

        // 4. Gửi SMS
        var smsRequest = new AccountCreationSMSOtp
        {
            PhoneNumber = phone,
            OtpCode = otp,
            ExpirationInMinutes = "5"
        };

        var smsResult = await _smsService.SendAccountRegistrationNonUnicodeSMSOtpAsync(smsRequest);
        if (!smsResult.IsSuccess)
        {
            Log.Error("📛 [ForgotPassword] Gửi OTP thất bại. Phone: {Phone}, Lý do: {Message}", phone, smsResult.Message);
            return BaseResponse<bool>.Error("Gửi OTP thất bại, vui lòng thử lại sau.");
        }

        Log.Information("📨 [ForgotPassword] Đã gửi OTP {Otp} đến {Phone}", otp, phone);

        // 5. Cập nhật bộ đếm gửi OTP
        if (counter == null || counter.LastResetAt <= now.AddHours(-24))
        {
            var newCounter = new OtpRequestCounter
            {
                Id = Guid.NewGuid(), // 👈 Đảm bảo có Id mới
                PhoneNumber = phone,
                Purpose = OtpPurpose.ForgotPassword,
                Count = 1,
                LastResetAt = now
            };

            await _otpCounterRepo.AddOrUpdateAsync(newCounter, cancellationToken);
            Log.Information("🆕 [ForgotPassword] Tạo mới bộ đếm OTP cho {Phone}", phone);
        }
        else
        {
            counter.Count++;
            // 👇 Kiểm tra kỹ counter.Id để tránh lỗi như log bạn gặp
            if (counter.Id == Guid.Empty)
            {
                Log.Error("❗ [ForgotPassword] Bộ đếm OTP có Id rỗng, không thể cập nhật. Phone: {Phone}", phone);
                return BaseResponse<bool>.Error("Không thể cập nhật bộ đếm OTP. Vui lòng thử lại sau.");
            }

            await _otpCounterRepo.UpdateAsync(counter, cancellationToken);
            Log.Information("🔁 [ForgotPassword] Tăng bộ đếm OTP: {Phone} => {Count}", phone, counter.Count);
        }

        // 6. Lưu thay đổi
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        Log.Information("💾 [ForgotPassword] Đã lưu log OTP và bộ đếm vào DB cho {Phone}", phone);

        return BaseResponse<bool>.Success(true, "Đã gửi mã OTP thành công.");
    }


}

