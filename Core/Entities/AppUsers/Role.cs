using System.ComponentModel.DataAnnotations;

namespace Core.Entities.AppUsers
{
    public class Role : Audit
    {
        [Required, MaxLength(50)]
        public string Code { get; set; } = default!;  // Eg: admin, staff, manager

        [MaxLength(100)]
        public string Name { get; set; } = default!;
        // Thêm thuộc tính TenantId để hỗ trợ multi-tenant
        public Guid? TenantId { get; set; }  // Chỉ định TenantId cho mỗi Role
        public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }
}
