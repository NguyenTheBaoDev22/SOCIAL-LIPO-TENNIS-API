using Applications.Features.Users.Commands;
using Applications.Interfaces.Repositories;
using Core.Entities.AppUsers;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Results;

namespace Applications.Features.Users.Handlers
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, BaseResponse<Guid>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CreateUserCommandHandler> _logger;

        public CreateUserCommandHandler(IUnitOfWork unitOfWork, ILogger<CreateUserCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<BaseResponse<Guid>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("🧑‍💻 Creating user: {Username} - {PhoneNumber}", request.Username, request.PhoneNumber);

            var exists = await _unitOfWork.UserRepositories.ExistsByUsernameAsync(request.Username);
            if (exists)
            {
                _logger.LogWarning("⚠️ User already exists with username/phone: {Username}/{PhoneNumber}", request.Username, request.PhoneNumber);
                return BaseResponse<Guid>.Error("User already exists", "User_Exists");
            }

            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = request.Username,
                PhoneNumber = request.PhoneNumber,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
            };

            await _unitOfWork.UserRepositories.AddAsync(user, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("✅ User created: {UserId}", user.Id);
            return BaseResponse<Guid>.Success(user.Id, "User created successfully");
        }
    }
}
