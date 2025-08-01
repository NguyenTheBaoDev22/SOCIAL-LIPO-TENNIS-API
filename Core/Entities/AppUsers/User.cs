using System.ComponentModel.DataAnnotations;

namespace Core.Entities.AppUsers
{
    public class User : Audit
    {
        [Required, MaxLength(100)]
        public string Username { get; set; } = default!;

        public string? PasswordHash { get; set; } = default!;

        [MaxLength(15)]
        public string? PhoneNumber { get; set; }
        [EmailAddress]
        public string? Email { get; set; }

        public bool IsActive { get; set; } = true;
        public bool IsVerified { get; set; } = false;
        public string? RefreshTokenHash { get; set; }
        public DateTime? RefreshTokenExpiry { get; set; }

        public DateTime? LastLoginAt { get; set; }
        // Multi-tenant fields
        public Guid? TenantId { get; set; }
        public Guid? MerchantId { get; set; }
        public Guid? MerchantBranchId { get; set; }
        public ICollection<UserRoleAssignment> UserRoleAssignments { get; set; } = new List<UserRoleAssignment>();
        public DateTime? SetPasswordTokenExpiry { get; set; }
        public string? SetPasswordToken { get; set; }
    }

}
