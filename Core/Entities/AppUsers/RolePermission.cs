using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.AppUsers
{
    public class RolePermission : Audit
    {
        [ForeignKey("Role")]
        public Guid RoleId { get; set; }
        public Role Role { get; set; } = default!;

        [ForeignKey("Permission")]
        public Guid PermissionId { get; set; }
        public Permission Permission { get; set; } = default!;
    }
}
