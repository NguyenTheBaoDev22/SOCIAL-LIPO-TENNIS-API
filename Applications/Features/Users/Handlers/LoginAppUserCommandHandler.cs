using Applications.Features.Users.Commands;
using Applications.Interfaces.Repositories;
using Core.Entities.AppUsers;
using Core.Interfaces;
using MediatR;
using Shared.DTOs;
using Shared.Results;

namespace Applications.Features.Users.Handlers
{
    public class LoginAppUserCommandHandler : IRequestHandler<LoginAppUserCommand, BaseResponse<TokenResult>>
    {
        private readonly IPasswordHasher _passwordHasher;
        private readonly IAppUserJwtTokenGenerator _tokenGenerator;
        private readonly IUserRepository _userRepository;

        public LoginAppUserCommandHandler(
            IPasswordHasher passwordHasher,
            IAppUserJwtTokenGenerator tokenGenerator,
            IUserRepository userRepository)
        {
            _passwordHasher = passwordHasher;
            _tokenGenerator = tokenGenerator;
            _userRepository = userRepository;
        }

        public async Task<BaseResponse<TokenResult>> Handle(LoginAppUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByUsernameAsync(request.UsernameOrPhone);

            if (user == null || !_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
            {
                return BaseResponse<TokenResult>.Error(
                    message: "Tên đăng nhập hoặc mật khẩu không chính xác.",
                    code: "INVALID_CREDENTIALS"
                );
            }

            var roles = user.UserRoleAssignments
                .Select(ura => ura.Role.Name)
                .Distinct()
                .ToList();

            var permissions = user.UserRoleAssignments
                .SelectMany(ura => ura.Role.RolePermissions.Select(rp => rp.Permission.Code.ToString()))
                .Distinct()
                .ToList();

            var payload = new AppUserTokenPayload
            {
                UserId = user.Id,
                Username = user.Username,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                TenantId = user.TenantId,
                MerchantId = user.MerchantId,
                MerchantBranchId = user.MerchantBranchId,
                Roles = roles,
                Permissions = permissions
            };

            var tokenString = _tokenGenerator.GenerateToken(payload);
            var expiresIn = 60 * 60 * 24; // 1 day
            var result = new TokenResult
            {
                AccessToken = tokenString,
                TokenType = "Bearer",
                ExpiresIn = expiresIn,
                Expiration = DateTime.UtcNow.AddSeconds(expiresIn)
            };

            return BaseResponse<TokenResult>.Success(
                data: result,
                message: "Đăng nhập thành công.",
                code: "00"
            );
        }
    }
}
