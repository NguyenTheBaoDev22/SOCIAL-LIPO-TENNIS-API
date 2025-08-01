using Core.Entities.AppUsers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Features.Users.DTOs
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = default!;
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }

        public bool IsActive { get; set; }
        public bool IsVerified { get; set; }

        public DateTime? LastLoginAt { get; set; }

        // Multi-tenant fields
        public Guid? TenantId { get; set; }
        public Guid? MerchantId { get; set; }
        public Guid? MerchantBranchId { get; set; }

        // Danh sách role theo từng phạm vi
        public List<UserRoleDto> Roles { get; set; } = new();
    }
    public class UserRoleDto
    {
        public Guid RoleId { get; set; }
        public string RoleName { get; set; } = default!;
        public Guid? TenantId { get; set; }
        public Guid? MerchantId { get; set; }
        public Guid? MerchantBranchId { get; set; }
    }
}
