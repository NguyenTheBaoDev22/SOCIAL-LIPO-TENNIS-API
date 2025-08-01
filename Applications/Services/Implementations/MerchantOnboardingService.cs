using Applications.Interfaces.Repositories;
using Core.Entities.AppUsers;
using Core.Entities;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Applications.Services.Interfaces;
using Shared.Utils;
using Shared.Options;
using Microsoft.Extensions.Options;
using Applications.Interfaces.Services;

namespace Applications.Services.Implementations
{
    //public class MerchantOnboardingService : IMerchantOnboardingService
    //{
    //    private readonly IUnitOfWork _uow;
    //    private readonly IJwtService _jwtService;
    //    private readonly INotificationService _notificationService;
    //    private readonly JwtOptions _jwtOptions;

    //    public MerchantOnboardingService(
    //        IUnitOfWork uow,
    //        IJwtService jwtService,
    //        INotificationService notificationService,
    //        IOptions<JwtOptions> jwtOptions)
    //    {
    //        _uow = uow;
    //        _jwtService = jwtService;
    //        _notificationService = notificationService;
    //        _jwtOptions = jwtOptions.Value;
    //    }

    //    public async Task<(User user, string password)> CreateOwnerAccountAsync(Merchant merchant, CancellationToken cancellationToken)
    //    {
    //        var username = merchant.PrimaryPhone;
    //        var existing = await _uow.UserRepositories.AsQueryable()
    //            .FirstOrDefaultAsync(x => x.Username == username, cancellationToken);

    //        if (existing != null)
    //            throw new Exception("User already exists");

    //        var password = PasswordUtils.GenerateRandomPassword();
    //        var user = new User
    //        {
    //            Username = username,
    //            PhoneNumber = merchant.PrimaryPhone,
    //            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
    //            IsActive = true
    //        };

    //        await _uow.UserRepositories.AddAsync(user, cancellationToken);
    //        await _uow.SaveChangesAsync(cancellationToken);

    //        var role = await _uow.RoleRepositories.AsQueryable()
    //            .FirstOrDefaultAsync(r => r.Code == "Owner", cancellationToken)
    //            ?? throw new Exception("Owner role not found");

    //        await _uow.UserRoleAssignmentRepositories.AddAsync(new UserRoleAssignment
    //        {
    //            UserId = user.Id,
    //            RoleId = role.Id,
    //            MerchantId = merchant.Id,
    //            TenantId = merchant.Id
    //        }, cancellationToken);

    //        await _uow.UserMerchantRepositories.AddAsync(new UserMerchant
    //        {
    //            UserId = user.Id,
    //            MerchantId = merchant.Id,
    //            RoleId = role.Id
    //        }, cancellationToken);

    //        await _uow.SaveChangesAsync(cancellationToken);

    //        // Tạo JWT token chứa userId, dùng để đặt lại mật khẩu
    //        var token = _jwtService.GenerateToken(new Dictionary<string, object>
    //        {
    //            { "userId", user.Id },
    //            { "username", user.Username },
    //            { "purpose", "reset-password" }
    //        }, expiresInMinutes: _jwtOptions.ResetPasswordTokenExpiryMinutes);

    //        var resetLink = $"https://uat-shop.zenpay.com.vn/new-password?token={token}";

    //        // Gửi link qua notification service
    //        var message = $"Chào bạn, tài khoản quản lý của bạn đã được tạo.\nVui lòng đặt mật khẩu mới tại: {resetLink}";

    //        if (!string.IsNullOrEmpty(user.PhoneNumber))
    //        {
    //            await _notificationService.SendSmsAsync(user.PhoneNumber, message);
    //        }
    //        else if (!string.IsNullOrEmpty(user.Email))
    //        {
    //            await _notificationService.SendEmailAsync(user.Email, "Kích hoạt tài khoản quản lý ZenShop", message);
    //        }

    //        return (user, password); // password ở đây là tạm, nên không cần dùng nếu đã yêu cầu user đặt lại mật khẩu
    //    }
    //}
}
