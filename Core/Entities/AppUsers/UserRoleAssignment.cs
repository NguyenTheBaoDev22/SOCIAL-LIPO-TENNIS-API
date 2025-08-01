namespace Core.Entities.AppUsers
{
    public class UserRoleAssignment : Audit
    {
        public Guid UserId { get; set; }
        public User User { get; set; } = default!;

        public Guid? TenantId { get; set; } // ✅ Cho phép null nếu role là hệ thống (Admin)

        public Guid? MerchantId { get; set; } // ✅ Cho phép null nếu không gắn Merchant cụ thể
        public Merchant? Merchant { get; set; }

        public Guid? MerchantBranchId { get; set; }
        public MerchantBranch? MerchantBranch { get; set; }

        public Guid RoleId { get; set; }
        public Role Role { get; set; } = default!;
    }
}
