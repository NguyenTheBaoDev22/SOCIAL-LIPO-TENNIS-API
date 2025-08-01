using Applications.Features.Users.DTOs;
using AutoMapper;
using Core.Entities.AppUsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.MappingProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<User, UserDto>()
           .ForMember(dest => dest.Roles, opt => opt.MapFrom(src =>
               src.UserRoleAssignments.Select(ura => new UserRoleDto
               {
                   RoleId = ura.RoleId,
                   RoleName = ura.Role != null ? ura.Role.Name : string.Empty,
                   TenantId = ura.TenantId,
                   MerchantId = ura.MerchantId,
                   MerchantBranchId = ura.MerchantBranchId
               })
           ));
        }

    }
}
