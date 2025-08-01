using MediatR;
using Shared.Results;
using System.ComponentModel.DataAnnotations;

namespace Applications.Features.Users.Commands
{
    public class AssignRoleToUserCommand : IRequest<BaseResponse<string>>
    {
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public Guid RoleId { get; set; }
        [Required]
        public Guid TenantId { get; set; }
        [Required]
        public Guid MerchantId { get; set; }
        public Guid? MerchantBranchId { get; set; }
    }
}
