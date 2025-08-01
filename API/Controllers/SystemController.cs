using Applications.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Attributes;
using Shared.Authorization;
using Shared.Helpers;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystemController : ControllerBase
    {
        [HttpGet("permissions")]
        [Authorize]
        [AuthorizePermission(PermissionCode.ApproveMerchant)]
        public IActionResult GetAllPermissions()
        {
            var permissions = Enum.GetValues(typeof(PermissionCode))
                .Cast<PermissionCode>()
                .Select(p => new PermissionDto
                {
                    Code = p.ToString(),
                    Description = EnumHelper.GetDescription(p)
                });

            return Ok(permissions);
        }
    }
}
