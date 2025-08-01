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
    public class ToggleUserStatusCommandHandler : IRequestHandler<ToggleUserStatusCommand, BaseResponse<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ToggleUserStatusCommandHandler> _logger;

        public ToggleUserStatusCommandHandler(IUnitOfWork unitOfWork, ILogger<ToggleUserStatusCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<BaseResponse<bool>> Handle(ToggleUserStatusCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.UserRepositories.GetByIdAsync(request.UserId, cancellationToken);
            if (user == null)
            {
                _logger.LogWarning("❌ User not found: {UserId}", request.UserId);
                return BaseResponse<bool>.Error("User not found", "User_NotFound");
            }

            user.IsActive = request.IsActive;
            _unitOfWork.UserRepositories.Update(user);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("✅ Updated user status: {UserId}, Active = {IsActive}", user.Id, user.IsActive);

            return BaseResponse<bool>.Success(true, "User status updated", "00");
        }
    }
}
