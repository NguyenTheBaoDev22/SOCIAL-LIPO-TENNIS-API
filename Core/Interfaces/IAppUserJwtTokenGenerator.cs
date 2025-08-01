using Shared.DTOs;

namespace Core.Interfaces
{
    public interface IAppUserJwtTokenGenerator
    {
        string GenerateToken(
            AppUserTokenPayload appUserTokenPayload, Guid? tenantId, Guid? merchantId, Guid? branchId);
        string GenerateToken(AppUserTokenPayload payload);
        string GeneratePasswordSetupToken(Guid userId, string email, string phoneNumber);
    }
}
