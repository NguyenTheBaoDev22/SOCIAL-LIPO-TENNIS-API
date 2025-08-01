namespace Shared.DTOs
{
    public class AppUserTokenPayload
    {
        public Guid UserId { get; set; }
        public string? Username { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }

        public Guid? TenantId { get; set; }
        public Guid? MerchantId { get; set; }
        public Guid? MerchantBranchId { get; set; }

        public List<string>? Roles { get; set; }
        public List<string>? Permissions { get; set; }
    }
   
}
