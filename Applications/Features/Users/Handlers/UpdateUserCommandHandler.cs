using Applications.Features.Users.Commands;
using Applications.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Features.Users.Handlers
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, BaseResponse<string>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UpdateUserCommandHandler> _logger;

        public UpdateUserCommandHandler(IUnitOfWork unitOfWork, ILogger<UpdateUserCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<BaseResponse<string>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("🔄 Updating UserId {UserId}", request.UserId);

            var user = await _unitOfWork.UserRepositories.GetByIdAsync(request.UserId, cancellationToken);
            if (user == null)
            {
                _logger.LogWarning("❌ User not found: {UserId}", request.UserId);
                return BaseResponse<string>.Error("User not found", "User_NotFound");
            }

            // Cập nhật thông tin cơ bản
            user.Username = request.Username;
            user.PhoneNumber = request.PhoneNumber;
            if (request.IsActive.HasValue)
                user.IsActive = request.IsActive.Value;

            if (!string.IsNullOrWhiteSpace(request.NewPassword))
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);

            user.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.UserRepositories.UpdateAsync(user, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("✅ User updated: {UserId}", user.Id);

            return BaseResponse<string>.Success(user.Id.ToString(), "User updated successfully");
        }
    }
}
