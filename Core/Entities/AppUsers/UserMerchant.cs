using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.AppUsers
{
    public class UserMerchant : Audit
    {
        [ForeignKey("User")]
        public Guid UserId { get; set; }
        public User User { get; set; } = default!;

        [ForeignKey("Merchant")]
        public Guid MerchantId { get; set; }
        public Merchant Merchant { get; set; } = default!;

        [ForeignKey("MerchantBranch")]
        public Guid? MerchantBranchId { get; set; }
        public MerchantBranch? MerchantBranch { get; set; }

        [ForeignKey("Role")]
        public Guid RoleId { get; set; }
        public Role Role { get; set; } = default!;
        // Thêm thuộc tính IsPrimary để đánh dấu chi nhánh quan trọng nhất
        public bool IsPrimary { get; set; } = false; // Mặc định là false, chỉ set true khi cần
    }
}
