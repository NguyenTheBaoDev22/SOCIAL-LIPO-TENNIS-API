using Applications.Features.Users.DTOs;
using Applications.Interfaces.Repositories;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Features.Users.Queries
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, BaseResponse<UserDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GetUserByIdQueryHandler> _logger;

        public GetUserByIdQueryHandler(IUnitOfWork unitOfWork, ILogger<GetUserByIdQueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<BaseResponse<UserDto>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("🔍 Fetching UserId {UserId}", request.UserId);

            var user = await _unitOfWork.UserRepositories.GetByIdWithRolesAsync(request.UserId, cancellationToken);
            if (user == null)
            {
                _logger.LogWarning("❌ User not found: {UserId}", request.UserId);
                return BaseResponse<UserDto>.Error("User not found", "User_NotFound");
            }

            var dto = user.Adapt<UserDto>();
            return BaseResponse<UserDto>.Success(dto);
        }
    }
}
