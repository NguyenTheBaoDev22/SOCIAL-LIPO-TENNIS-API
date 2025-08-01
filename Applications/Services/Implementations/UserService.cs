using Applications.Interfaces.Repositories;
using Applications.Services.Interfaces;
using Core.Entities.AppUsers;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Exceptions;
using System.Diagnostics;

namespace Applications.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IBaseRepository<User> _userRepository;
        private readonly IBaseRepository<UserRoleAssignment> _userRoleRepository;
        private readonly IPasswordHasher _passwordHasher;

        public UserService(
            IBaseRepository<User> userRepository,
            IBaseRepository<UserRoleAssignment> userRoleRepository,
            IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<User?> GetByUsernameOrEmailAsync(string input)
        {
            return await _userRepository
                .AsQueryable()
                .FirstOrDefaultAsync(u => u.Username == input || u.Email == input);
        }

        public async Task<User?> GetByIdAsync(Guid userId)
        {
            return await _userRepository.GetByIdAsync(userId);
        }

        public async Task<bool> ExistsByUsernameOrEmailAsync(string input)
        {
            return await _userRepository
                .AsQueryable()
                .AnyAsync(u => u.Username == input || u.Email == input);
        }

        public async Task<bool> ExistsByPhoneOrEmailAsync(string phone, string? email = null)
        {
            return await _userRepository
                .AsQueryable()
                .AnyAsync(u =>
                    u.PhoneNumber == phone ||
                    (!string.IsNullOrEmpty(email) && u.Email == email));
        }

        public async Task<User> CreateUserAsync(User user, string passwordPlainText, bool isVerified = false, bool isActive = true)
        {
            var traceId = Activity.Current?.Id ?? Guid.NewGuid().ToString();

            if (await ExistsByUsernameOrEmailAsync(user.Username))
                throw new BusinessException("Username or email already exists", "03EX", traceId);

            // Corrected to call HashPassword with only the plain password string
            user.PasswordHash = _passwordHasher.HashPassword(passwordPlainText);

            user.IsVerified = isVerified;
            user.IsActive = isActive;

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();
            return user;
        }


        public async Task AssignRoleAsync(User user, Guid roleId, Guid tenantId, Guid merchantId, Guid? merchantBranchId = null)
        {
            var assignment = new UserRoleAssignment
            {
                UserId = user.Id,
                TenantId = tenantId,
                MerchantId = merchantId,
                MerchantBranchId = merchantBranchId,
                RoleId = roleId
            };

            await _userRoleRepository.AddAsync(assignment);
            await _userRoleRepository.SaveChangesAsync();
        }
    }
}
