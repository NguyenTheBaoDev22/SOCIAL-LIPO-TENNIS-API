using Shared.Authorization;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities.AppUsers
{
    public class Permission : Audit
    {
        [Required, MaxLength(100)]
        public PermissionCode Code { get; set; }  // Eg: create_user, update_branch

        [MaxLength(200)]
        public string Description { get; set; } = default!;
    }
}
