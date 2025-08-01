using Applications.Features.MerchantBranches.Commands;
using Applications.Interfaces.Repositories;
using Applications.Interfaces.Services;
using Applications.Services.Interfaces;
using Core.Entities;
using Core.Entities.AppUsers;
using Core.Enumerables;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Shared.Constants;
using Shared.Results;
using Serilog;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using Shared.Interfaces;
using Amazon.Runtime.Internal.Util;

namespace Applications.Features.MerchantBranches.Handlers
{
    public class ActivateMerchantBranchHandler : IRequestHandler<ActivateMerchantBranchCommand, BaseResponse<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;  // Unit of Work to manage transactions across repositories
        private readonly IUserService _userService;  // Service to manage user-related actions
        private readonly ITokenService _tokenService;  // Service to handle token generation (e.g., password reset token)
        private readonly INotificationService _notificationService;  // Service to send notifications (e.g., email)
        private readonly ICurrentUserService _currentUserService;  // Service to get current user information
        private readonly AppSettings _appSettings;  // Configuration settings like Frontend URL

        public ActivateMerchantBranchHandler(
            IUnitOfWork unitOfWork,
            IUserService userService,
            ITokenService tokenService,
            INotificationService notificationService,
            ICurrentUserService currentUserService,  // Inject CurrentUserService
            IOptions<AppSettings> appSettings)
        {
            _unitOfWork = unitOfWork;
            _userService = userService;
            _tokenService = tokenService;
            _notificationService = notificationService;
            _currentUserService = currentUserService;
            _appSettings = appSettings.Value;
        }

        public async Task<BaseResponse<bool>> Handle(ActivateMerchantBranchCommand request, CancellationToken cancellationToken)
        {
            var traceId = Activity.Current?.Id ?? Guid.NewGuid().ToString();  // Ensure traceId for logging

            // Step 1: Retrieve the Merchant Branch by ID
            Log.Information("[{TraceId}] ⏹️ Retrieving Merchant Branch with ID {MerchantBranchId}", traceId, request.MerchantBranchId);
            var branch = await _unitOfWork.MerchantBranchRepositories.GetByIdAsync(request.MerchantBranchId);
            if (branch == null)
            {
                Log.Error("[{TraceId}] Branch with ID {MerchantBranchId} not found", traceId, request.MerchantBranchId);
                return BaseResponse<bool>.Error("Branch not found", "0120");  // Return error if the branch is not found
            }

            // Step 2: Retrieve the Merchant associated with the branch
            Log.Information("[{TraceId}] ⏹️ Retrieving Merchant with ID {MerchantId}", traceId, branch.MerchantId);
            var merchant = await _unitOfWork.MerchantRepositories.GetByIdAsync(branch.MerchantId);
            if (merchant == null)
            {
                Log.Error("[{TraceId}] Merchant with ID {MerchantId} not found", traceId, branch.MerchantId);
                return BaseResponse<bool>.Error("Merchant not found", "0107");  // Return error if the merchant is not found
            }

            // Step 3: Activate the Merchant Branch and Merchant
            Log.Information("[{TraceId}] ⏹️ Activating Merchant Branch and Merchant", traceId);
            branch.Status = EBranchStatus.Active;  // Set the branch status to Active
            merchant.IsActive = true;  // Set the merchant as active

            // Step 4: Check if a user already exists for the merchant
            Log.Information("[{TraceId}] ⏹️ Checking if user already exists for Merchant {MerchantName}", traceId, merchant.MerchantName);
            var existingUser = await _unitOfWork.UserRepositories.GetQueryable()
                .FirstOrDefaultAsync(u => u.Username == merchant.PrimaryPhone || u.Email == merchant.PrimaryEmail);

            if (existingUser == null) // If no user exists, create one
            {
                Log.Information("[{TraceId}] ⏹️ No existing user found for Merchant {MerchantName}, creating new user", traceId, merchant.MerchantName);

                var user = new User
                {
                    Username = merchant.PrimaryPhone,  // Use the merchant's primary phone number as the username
                    PhoneNumber = merchant.PrimaryPhone,
                    Email = merchant.PrimaryEmail,
                    IsVerified = false,  // User is initially not verified
                    IsActive = true,  // User is active by default
                    MerchantId = merchant.Id,
                    MerchantBranchId = branch.Id,
                    TenantId = _currentUserService.TenantId // Use TenantId from CurrentUserService
                };

                // Step 5: Add the new user to the repository
                await _unitOfWork.UserRepositories.AddAsync(user);
                Log.Information("[{TraceId}] ⏹️ User created successfully for Merchant {MerchantName}", traceId, merchant.MerchantName);

                // Step 6: Assign a default role to the user (MerchantOwner role)
                var role = await _unitOfWork.RoleRepositories.GetQueryable()
                    .FirstOrDefaultAsync(r => r.Code == "MerchantOwner");  // Default role for the merchant owner

                if (role != null)
                {
                    Log.Information("[{TraceId}] ⏹️ Assigning role {RoleName} to User {UserName}", traceId, role.Name, user.Username);

                    // Assign the role to the user
                    await _unitOfWork.UserRoleAssignmentRepositories.AddAsync(new UserRoleAssignment
                    {
                        UserId = user.Id,
                        MerchantId = merchant.Id,
                        MerchantBranchId = branch.Id,
                        RoleId = role.Id,
                        TenantId = _currentUserService.TenantId??Guid.Empty  // Gán TenantId từ CurrentUserService nếu có
                    });
                }

                // Step 7: Generate a password reset token for the new user
                var token = _tokenService.GenerateResetPasswordToken(user.Id);
                var resetPasswordUrl = $"{_appSettings.FrontendUrlConfig.BaseUrl}/reset-password?token={token}";

                // Step 8: Send the password reset email to the user
                await _notificationService.SendResetPasswordEmail(user.Email, resetPasswordUrl);
                Log.Information("[{TraceId}] ⏹️ Password reset email sent to {Email}", traceId, user.Email);
            }

            // Step 9: Commit all changes to the database
            Log.Information("[{TraceId}] ⏹️ Committing changes to database", traceId);
            await _unitOfWork.SaveChangesAsync();  // Thay vì CommitAsync

            // Step 10: Return success response indicating the merchant branch was activated and user was created
            Log.Information("[{TraceId}] ✅ Merchant branch activated successfully and user created", traceId);
            return BaseResponse<bool>.Success(true, "Merchant branch activated successfully and user created.");
        }
    }

}
