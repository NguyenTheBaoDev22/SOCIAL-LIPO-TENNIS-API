using Applications.Features.Users.DTOs;
using MediatR;
using Shared.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Features.Users.Queries
{
    public class GetUserListQuery : IRequest<BaseResponse<PaginatedResult<UserDto>>>
    {
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public string? SearchKeyword { get; set; }
        public Guid? RoleId { get; set; }
        public Guid? TenantId { get; set; }
        public Guid? MerchantId { get; set; }
    }
}
