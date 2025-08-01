using Applications.Features.Merchants.Commands;
using Applications.Interfaces.Repositories;
using Applications.Services.Interfaces;
using Applications.Features.LarksuiteIntegrations.DTOs;
using Core.Entities.AppUsers;
using Core.Entities.Partners;
using Core.Enumerables;
using Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Shared.Constants;
using Shared.Interfaces;
using Shared.Results;
using System;
using System.Threading;
using System.Threading.Tasks;
using Applications.Features.Merchants.Commands.Applications.Features.Merchants.Commands;

namespace Applications.Features.Merchants.Handlers
{
    /// <summary>
    /// Xử lý phê duyệt merchant: tạo user, gán quyền, cập nhật trạng thái và gửi link tạo mật khẩu.
    /// </summary>
    public class ApproveMerchantCommandHandler : IRequestHandler<ApproveMerchantCommand, BaseResponse<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;
        private readonly IAppUserJwtTokenGenerator _jwtTokenGenerator;
        private readonly ILarkEmailService _emailService;

        public ApproveMerchantCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser,
            IAppUserJwtTokenGenerator jwtTokenGenerator,
            ILarkEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
            _jwtTokenGenerator = jwtTokenGenerator;
            _emailService = emailService;
        }

        public async Task<BaseResponse<bool>> Handle(ApproveMerchantCommand request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.ExecuteInTransactionAsync(async () =>
            {
                var traceId = _unitOfWork is ITraceable trace ? trace.TraceId : Guid.NewGuid().ToString();

                var branch = await _unitOfWork.MerchantBranchRepositories.GetWithMerchantByIdAsync(request.MerchantBranchId);
                if (branch == null)
                {
                    Log.Warning("[{TraceId}] ❌ Branch not found: {BranchId}", traceId, request.MerchantBranchId);
                    return BaseResponse<bool>.Error("Branch not found", ErrorCodes.MerchantBranch_NotFound);
                }

                if (branch.Status == EBranchStatus.Active && branch.VerificationStatus == VerificationStatusConstants.Approved)
                {
                    Log.Information("[{TraceId}] ℹ️ Branch already approved: {BranchId}", traceId, branch.Id);
                    return BaseResponse<bool>.Success(true, "Branch already approved");
                }

                var username = branch.Merchant.PrimaryPhone;
                var primaryEmail = branch.Merchant.PrimaryEmail;

                if (string.IsNullOrWhiteSpace(primaryEmail))
                {
                    Log.Warning("[{TraceId}] ⚠️ Merchant {MerchantId} missing primary email", traceId, branch.Merchant.Id);
                    return BaseResponse<bool>.Error("Merchant is missing primary email", ErrorCodes.Merchant_MissingPrimaryEmail);
                }

                var existingUser = await _unitOfWork.UserRepositories.AsQueryable()
                    .FirstOrDefaultAsync(x => x.Username == username, cancellationToken);
                if (existingUser != null)
                {
                    Log.Warning("[{TraceId}] ❌ User already exists for phone: {Phone}", traceId, username);
                    return BaseResponse<bool>.Error("Phone number is already registered", ErrorCodes.User_AlreadyExists);
                }

                var existingEmail = await _unitOfWork.UserRepositories.AsQueryable()
                    .AnyAsync(x => x.Email == primaryEmail, cancellationToken);
                if (existingEmail)
                {
                    Log.Warning("[{TraceId}] ❌ Email already exists: {Email}", traceId, primaryEmail);
                    return BaseResponse<bool>.Error("Email is already registered", ErrorCodes.User_AlreadyExists);
                }

                var user = new User
                {
                    Username = username,
                    PhoneNumber = username,
                    Email = primaryEmail,
                    IsActive = true
                };
                await _unitOfWork.UserRepositories.AddAsync(user, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                var role = await _unitOfWork.RoleRepositories.AsQueryable()
                    .FirstOrDefaultAsync(r => r.Code == "Owner", cancellationToken);
                if (role == null)
                {
                    Log.Error("[{TraceId}] ❌ Owner role not found", traceId);
                    return BaseResponse<bool>.Error("Owner role not found", ErrorCodes.Role_NotFound);
                }

                await _unitOfWork.UserRoleAssignmentRepositories.AddAsync(new UserRoleAssignment
                {
                    UserId = user.Id,
                    RoleId = role.Id,
                    MerchantId = branch.Merchant.Id,
                    TenantId = branch.Merchant.Id
                }, cancellationToken);

                await _unitOfWork.UserMerchantRepositories.AddAsync(new UserMerchant
                {
                    UserId = user.Id,
                    MerchantId = branch.Merchant.Id,
                    RoleId = role.Id
                }, cancellationToken);

                branch.Status = EBranchStatus.Active;
                branch.VerificationStatus = VerificationStatusConstants.Approved;
                branch.UpdatedAt = DateTime.UtcNow;
                await _unitOfWork.MerchantBranchRepositories.UpdateAsync(branch, cancellationToken);

                branch.Merchant.IsActive = true;
                branch.Merchant.VerifiedAt = DateTime.UtcNow;
                branch.Merchant.VerifiedBy = _currentUser.UserId?.ToString();
                branch.Merchant.UpdatedAt = DateTime.UtcNow;
                branch.Merchant.UpdatedBy = _currentUser.UserId;
                await _unitOfWork.MerchantRepositories.UpdateAsync(branch.Merchant, cancellationToken);

                var token = _jwtTokenGenerator.GeneratePasswordSetupToken(user.Id, primaryEmail!, username);
                user.SetPasswordToken = token;
                user.SetPasswordTokenExpiry = DateTime.UtcNow.AddHours(24);
                await _unitOfWork.UserRepositories.UpdateAsync(user, cancellationToken);

                var link = $"https://uat-shop.zenpay.com.vn/new-password?token={token}";

                var emailBody = $"""
                <p>Xin chào,</p>
                <p>Tài khoản của bạn tại ZenPay đã được tạo thành công.</p>
                <p>Vui lòng <a href=\"{link}\">bấm vào đây</a> để thiết lập mật khẩu và bắt đầu sử dụng hệ thống.</p>
                """;

                var emailDto = new LarkEmailDto
                {
                    FromEmail = "noreply@zenpay.vn",
                    RecipientEmail = primaryEmail!,
                    Subject = "Kích hoạt tài khoản ZenPay",
                    Body = emailBody
                };

                //var accessToken = await _unitOfWork.LarkSuiteTokenRepositories.GetValidAccessTokenAsync();
                //var sendResult = await _emailService.SendEmailAsync(emailDto, accessToken, cancellationToken);

                //if (!sendResult.Succeeded)
                //{
                //    Log.Warning("[{TraceId}] ❌ Failed to send password setup email to {Email}", traceId, primaryEmail);
                //}

                Log.Information("[{TraceId}] ✅ Merchant approved. User created: {UserId}, link: {Link}", traceId, user.Id, link);

                return BaseResponse<bool>.Success(true, "Merchant approved and password setup link generated");
            }, cancellationToken);
        }
    }
}
