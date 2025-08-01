using Applications.Interfaces.Services;
using Applications.Interfaces;
using Applications.Services.Interfaces;
using Core.Entities.AppUsers;
using Core.Entities;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Applications.Interfaces.Repositories;
using Shared.Interfaces.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Applications.Services.Implementations
{
    public class MerchantService : IMerchantService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;
        private readonly INotificationService _notificationService;
        private readonly IAppSettings _appSettings;

        public MerchantService(
            IUnitOfWork unitOfWork,
            ITokenService tokenService,
            INotificationService notificationService,
            IAppSettings appSettings)
        {
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
            _notificationService = notificationService;
            _appSettings = appSettings;
        }

        public async Task CreateUserForActivatedMerchantBranchAsync(MerchantBranch branch, CancellationToken cancellationToken)
        {
            var merchant = branch.Merchant;
            var existingUser = await _unitOfWork.UserRepositories
                .GetQueryable(u => u.PhoneNumber == merchant.PrimaryPhone)
                .FirstOrDefaultAsync(cancellationToken);

            if (existingUser != null)
            {
                Log.Information("👤 User already exists | Phone: {Phone}", existingUser.PhoneNumber);
                return;
            }

            var newUser = new User
            {
                Id = Guid.NewGuid(),
                PhoneNumber = merchant.PrimaryPhone,
                Email = merchant.PrimaryEmail,
                IsActive = true,
                IsVerified = false,
                CreatedAt = DateTime.UtcNow
            };
            await _unitOfWork.UserRepositories.AddAsync(newUser);

            await _unitOfWork.UserMerchantRepositories.AddAsync(new UserMerchant
            {
                UserId = newUser.Id,
                MerchantId = merchant.Id,
                MerchantBranchId = branch.Id,
                IsPrimary = true
            });

            var defaultRole = await _unitOfWork.RoleRepositories
                .GetQueryable(r => r.Name == "MerchantOwner" && r.TenantId == merchant.TenantId)
                .FirstOrDefaultAsync(cancellationToken);

            if (defaultRole != null)
            {
                await _unitOfWork.UserRoleAssignmentRepositories.AddAsync(new UserRoleAssignment
                {
                    UserId = newUser.Id,
                    RoleId = defaultRole?.Id ?? Guid.Empty,  // Nếu defaultRole.Id là null, dùng Guid.Empty
                    TenantId = merchant.TenantId,  // Nếu merchant.TenantId là null, dùng Guid.Empty
                });
            }

            var token = _tokenService.GenerateResetPasswordToken(newUser.Id);
            var resetLink = $"{_appSettings.FrontendUrlConfig.BaseUrl}/reset-password?token={token}";

            await _notificationService.SendEmailAsync(newUser.Email, "Kích hoạt tài khoản", $@"
            Xin chào {merchant.MerchantName},<br/>
            Địa điểm kinh doanh của bạn đã được kích hoạt.<br/> 
            Vui lòng nhấn vào <a href='{resetLink}'>đây</a> để tạo mật khẩu và đăng nhập hệ thống.
        ");

            Log.Information("✅ Created user and sent password setup link | Email: {Email}", newUser.Email);
        }
    }

}
